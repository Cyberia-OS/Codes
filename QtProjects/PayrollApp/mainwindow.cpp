#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "employeedialog.h"

#include <QSqlQuery>
#include <QSqlError>
#include <QMessageBox>
#include <QTableWidgetItem>
#include <QFile>
#include <QTextStream>
#include <QFileDialog>
#include <QDebug>
#include <QStatusBar>
#include <QFont>
#include <QHeaderView>
#include <QApplication>
#include <QSettings>
#include <QDate>

static SalaryRecord calculateSalary(const SalaryInput &in)
{
    SalaryRecord rec;
    rec.employeeId = in.employeeId;
    rec.period = in.period;
    rec.bonus = in.bonus;
    rec.incentive = in.incentive;
    rec.academicAllowance = in.teachingAllowance;
    rec.degreeAllowance = in.degreeAllowance;
    rec.titleAllowance = in.titleAllowance;
    rec.checkingAllowance = in.checkingAllowance;

    rec.accrued = in.normDays > 0 ? (in.baseSalary / in.normDays) * in.workedDays : 0.0;

    double taxable = rec.accrued + rec.bonus + rec.incentive
                   + rec.academicAllowance + rec.degreeAllowance
                   + rec.titleAllowance + rec.checkingAllowance;

    rec.ndfl       = taxable * 0.13;
    rec.unionFee   = taxable * 0.01;
    rec.totalToPay = taxable - rec.ndfl - rec.unionFee;
    rec.insuranceFee = taxable * 0.30;

    return rec;
}


MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent), ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    setWindowIcon(QIcon(":/app_icon.png"));

    openDatabase();

    connect(ui->searchEdit, &QLineEdit::textChanged, this, &MainWindow::refreshEmployeeTable);
    connect(ui->addEmployeeButton, &QPushButton::clicked, this, &MainWindow::onAddEmployee);
    connect(ui->editEmployeeButton, &QPushButton::clicked, this, &MainWindow::onEditEmployee);
    connect(ui->deleteEmployeeButton,&QPushButton::clicked, this, &MainWindow::onDeleteEmployee);

    connect(ui->saveTimesheetButton, &QPushButton::clicked, this, &MainWindow::onSaveTimesheet);
    connect(ui->saveTeachingLoadButton, &QPushButton::clicked, this, &MainWindow::onSaveTeachingLoad);

    auto updateTeachingTotal = [this]() {
        double allowance = ui->lecturesSpin->value()  * ui->lectureRateSpin->value()
                         + ui->seminarsSpin->value()  * ui->seminarRateSpin->value()
                         + ui->examsSpin->value()     * 250.0;
        double hours = ui->lecturesSpin->value() + ui->seminarsSpin->value() + ui->examsSpin->value();
        ui->teachingTotalLabel->setText(
            QString("Итого: %1 ч. → %2 руб.").arg(hours, 0, 'f', 1).arg(allowance, 0, 'f', 2));
    };
    connect(ui->lecturesSpin,    QOverload<double>::of(&QDoubleSpinBox::valueChanged), this, updateTeachingTotal);
    connect(ui->seminarsSpin,    QOverload<double>::of(&QDoubleSpinBox::valueChanged), this, updateTeachingTotal);
    connect(ui->examsSpin,       QOverload<double>::of(&QDoubleSpinBox::valueChanged), this, updateTeachingTotal);
    connect(ui->lectureRateSpin, QOverload<double>::of(&QDoubleSpinBox::valueChanged), this, updateTeachingTotal);
    connect(ui->seminarRateSpin, QOverload<double>::of(&QDoubleSpinBox::valueChanged), this, updateTeachingTotal);

    connect(ui->calcSalaryButton, &QPushButton::clicked, this, &MainWindow::onCalculateSalary);
    connect(ui->saveSalaryButton, &QPushButton::clicked, this, &MainWindow::onSaveSalary);

    connect(ui->generateReportButton,&QPushButton::clicked, this, &MainWindow::onGenerateReport);
    connect(ui->exportCsvButton, &QPushButton::clicked, this, &MainWindow::onExportCsv);

    connect(ui->tabWidget, &QTabWidget::currentChanged, this, &MainWindow::onTabChanged);
    connect(ui->historyEmployeeCombo, QOverload<int>::of(&QComboBox::currentIndexChanged),
            this, &MainWindow::onHistoryEmployeeChanged);

    refreshEmployeeTable();
    fillEmployeeComboBoxes();


    statusLabel = new QLabel(this);
    statusLabel->setContentsMargins(0, 0, 8, 0);
    statusBar()->addPermanentWidget(statusLabel);

    auto updateSessionStatus = [this]() {
        ++sessionSeconds;
        int h = sessionSeconds / 3600;
        int m = (sessionSeconds % 3600) / 60;
        int s = sessionSeconds % 60;

        QSqlQuery q;
        q.exec("SELECT COUNT(*) FROM employees");
        int empCount = (q.next()) ? q.value(0).toInt() : 0;

        statusLabel->setText(
            QString("Пользователь: admin  |  Сотрудников: %1  |  Сессия: %2:%3:%4")
                .arg(empCount)
                .arg(h, 2, 10, QChar('0'))
                .arg(m, 2, 10, QChar('0'))
                .arg(s, 2, 10, QChar('0'))
            );
    };

    sessionTimer = new QTimer(this);
    connect(sessionTimer, &QTimer::timeout, this, updateSessionStatus);
    sessionTimer->start(1000);
    updateSessionStatus();
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::openDatabase()
{
    db = QSqlDatabase::addDatabase("QSQLITE");
    db.setDatabaseName("university_payroll.db");
    if (!db.open())
        qDebug() << "DB open error:" << db.lastError().text();
    createTables();
}

void MainWindow::createTables()
{
    QSqlQuery query;

    if (!query.exec(
            "CREATE TABLE IF NOT EXISTS employees ("
            "  id INTEGER PRIMARY KEY AUTOINCREMENT,"
            "  fullName TEXT NOT NULL,"
            "  position TEXT,"
            "  department TEXT,"
            "  baseSalary REAL DEFAULT 0"
            ")"
            )) qDebug() << "createTables employees:" << query.lastError().text();

    if (!query.exec(
            "CREATE TABLE IF NOT EXISTS timesheets ("
            "  id INTEGER PRIMARY KEY AUTOINCREMENT,"
            "  employeeId INTEGER NOT NULL,"
            "  period TEXT NOT NULL,"
            "  workedDays INTEGER DEFAULT 0,"
            "  normDays INTEGER DEFAULT 21,"
            "  UNIQUE(employeeId, period)"
            ")"
            )) qDebug() << "createTables timesheets:" << query.lastError().text();

    if (!query.exec(
            "CREATE TABLE IF NOT EXISTS salary_records ("
            "  id INTEGER PRIMARY KEY AUTOINCREMENT,"
            "  employeeId INTEGER NOT NULL,"
            "  period TEXT NOT NULL,"
            "  accrued REAL DEFAULT 0,"
            "  bonus REAL DEFAULT 0,"
            "  incentive REAL DEFAULT 0,"
            "  academicAllowance REAL DEFAULT 0,"
            "  degreeAllowance REAL DEFAULT 0,"
            "  titleAllowance REAL DEFAULT 0,"
            "  checkingAllowance REAL DEFAULT 0,"
            "  ndfl REAL DEFAULT 0,"
            "  unionFee REAL DEFAULT 0,"
            "  insuranceFee REAL DEFAULT 0,"
            "  totalToPay REAL DEFAULT 0,"
            "  UNIQUE(employeeId, period)"
            ")"
            )) qDebug() << "createTables salary_records:" << query.lastError().text();

    if (!query.exec(
            "CREATE TABLE IF NOT EXISTS teaching_load ("
            "  id INTEGER PRIMARY KEY AUTOINCREMENT,"
            "  employeeId INTEGER NOT NULL,"
            "  period TEXT NOT NULL,"
            "  lectures REAL DEFAULT 0,"
            "  seminars REAL DEFAULT 0,"
            "  exams REAL DEFAULT 0,"
            "  lectureRate REAL DEFAULT 300,"
            "  seminarRate REAL DEFAULT 200,"
            "  examRate REAL DEFAULT 250,"
            "  UNIQUE(employeeId, period)"
            ")"
            )) qDebug() << "createTables teaching_load:" << query.lastError().text();

    auto addColumnIfMissing = [&](const QString &table, const QString &column, const QString &def) {
        QSqlQuery check;
        check.exec(QString("PRAGMA table_info(%1)").arg(table));
        while (check.next()) {
            if (check.value(1).toString() == column)
                return;
        }
        QSqlQuery alter;
        if (!alter.exec(QString("ALTER TABLE %1 ADD COLUMN %2 %3").arg(table, column, def)))
            qDebug() << "migration" << column << ":" << alter.lastError().text();
    };

    struct ColumnMigration { QString table; QString column; QString definition; };
    const QVector<ColumnMigration> migrations = {
        { "teaching_load",  "lectureRate", "REAL DEFAULT 300" },
        { "teaching_load",  "seminarRate", "REAL DEFAULT 200" },
        { "teaching_load",  "examRate",  "REAL DEFAULT 250" },
        { "salary_records", "academicAllowance", "REAL DEFAULT 0" },
        { "salary_records", "degreeAllowance", "REAL DEFAULT 0" },
        { "salary_records", "titleAllowance", "REAL DEFAULT 0" },
        { "salary_records", "checkingAllowance", "REAL DEFAULT 0" },
        { "salary_records", "insuranceFee", "REAL DEFAULT 0" },
    };
    for (const ColumnMigration &m : migrations)
        addColumnIfMissing(m.table, m.column, m.definition);
}

void MainWindow::addEmployee(Employee &emp)
{
    QSqlQuery query;
    query.prepare("INSERT INTO employees (fullName, position, department, baseSalary) "
                  "VALUES (:fullName, :position, :department, :baseSalary)");
    query.bindValue(":fullName", emp.fullName);
    query.bindValue(":position", emp.position);
    query.bindValue(":department", emp.department);
    query.bindValue(":baseSalary", emp.baseSalary);
    if (!query.exec())
        qDebug() << "addEmployee:" << query.lastError().text();
    emp.id = query.lastInsertId().toInt();
}

void MainWindow::updateEmployee(const Employee &emp)
{
    QSqlQuery query;
    query.prepare("UPDATE employees SET fullName=:fullName, position=:position, "
                  "department=:department, baseSalary=:baseSalary WHERE id=:id");
    query.bindValue(":fullName", emp.fullName);
    query.bindValue(":position", emp.position);
    query.bindValue(":department", emp.department);
    query.bindValue(":baseSalary", emp.baseSalary);
    query.bindValue(":id", emp.id);
    if (!query.exec())
        qDebug() << "updateEmployee:" << query.lastError().text();
}

void MainWindow::deleteEmployee(int employeeId)
{
    QSqlQuery query;

    query.prepare("DELETE FROM timesheets WHERE employeeId=:id");
    query.bindValue(":id", employeeId);
    if (!query.exec()) qDebug() << "deleteEmployee timesheets:" << query.lastError().text();

    query.prepare("DELETE FROM salary_records WHERE employeeId=:id");
    query.bindValue(":id", employeeId);
    if (!query.exec()) qDebug() << "deleteEmployee salary_records:" << query.lastError().text();

    query.prepare("DELETE FROM employees WHERE id=:id");
    query.bindValue(":id", employeeId);
    if (!query.exec()) qDebug() << "deleteEmployee employees:" << query.lastError().text();
}

QVector<Employee> MainWindow::getAllEmployees(const QString &searchText)
{
    QVector<Employee> result;
    QSqlQuery query;

    if (searchText.isEmpty()) {
        query.exec("SELECT id, fullName, position, department, baseSalary FROM employees ORDER BY fullName");
    } else {
        query.prepare("SELECT id, fullName, position, department, baseSalary FROM employees "
                      "WHERE fullName LIKE :search ORDER BY fullName");
        query.bindValue(":search", "%" + searchText + "%");
        query.exec();
    }

    while (query.next()) {
        Employee emp;
        emp.id = query.value(0).toInt();
        emp.fullName = query.value(1).toString();
        emp.position = query.value(2).toString();
        emp.department = query.value(3).toString();
        emp.baseSalary = query.value(4).toDouble();
        result.append(emp);
    }

    return result;
}

Employee MainWindow::getEmployeeById(int employeeId)
{
    Employee emp;
    QSqlQuery query;
    query.prepare("SELECT id, fullName, position, department, baseSalary FROM employees WHERE id=:id");
    query.bindValue(":id", employeeId);

    if (query.exec() && query.next()) {
        emp.id = query.value(0).toInt();
        emp.fullName = query.value(1).toString();
        emp.position = query.value(2).toString();
        emp.department = query.value(3).toString();
        emp.baseSalary = query.value(4).toDouble();
    }

    return emp;
}

void MainWindow::saveTimesheet(const Timesheet &ts)
{
    QSqlQuery query;
    query.prepare("INSERT OR REPLACE INTO timesheets (employeeId, period, workedDays, normDays) "
                  "VALUES (:employeeId, :period, :workedDays, :normDays)");
    query.bindValue(":employeeId", ts.employeeId);
    query.bindValue(":period", ts.period);
    query.bindValue(":workedDays", ts.workedDays);
    query.bindValue(":normDays", ts.normDays);
    if (!query.exec()) qDebug() << "saveTimesheet:" << query.lastError().text();
}

Timesheet MainWindow::getTimesheet(int employeeId, const QString &period)
{
    Timesheet ts;
    QSqlQuery query;
    query.prepare("SELECT id, employeeId, period, workedDays, normDays FROM timesheets "
                  "WHERE employeeId=:employeeId AND period=:period");
    query.bindValue(":employeeId", employeeId);
    query.bindValue(":period", period);

    if (query.exec() && query.next()) {
        ts.id = query.value(0).toInt();
        ts.employeeId = query.value(1).toInt();
        ts.period = query.value(2).toString();
        ts.workedDays = query.value(3).toInt();
        ts.normDays = query.value(4).toInt();
    }

    return ts;
}

void MainWindow::saveSalaryRecord(const SalaryRecord &rec)
{
    QSqlQuery query;
    query.prepare("INSERT OR REPLACE INTO salary_records "
                  "(employeeId, period, accrued, bonus, incentive, "
                  " academicAllowance, degreeAllowance, titleAllowance, checkingAllowance, "
                  " ndfl, unionFee, insuranceFee, totalToPay) "
                  "VALUES (:employeeId, :period, :accrued, :bonus, :incentive, "
                  " :academicAllowance, :degreeAllowance, :titleAllowance, :checkingAllowance, "
                  " :ndfl, :unionFee, :insuranceFee, :totalToPay)");
    query.bindValue(":employeeId", rec.employeeId);
    query.bindValue(":period", rec.period);
    query.bindValue(":accrued", rec.accrued);
    query.bindValue(":bonus", rec.bonus);
    query.bindValue(":incentive",  rec.incentive);
    query.bindValue(":academicAllowance", rec.academicAllowance);
    query.bindValue(":degreeAllowance", rec.degreeAllowance);
    query.bindValue(":titleAllowance", rec.titleAllowance);
    query.bindValue(":checkingAllowance", rec.checkingAllowance);
    query.bindValue(":ndfl", rec.ndfl);
    query.bindValue(":unionFee", rec.unionFee);
    query.bindValue(":insuranceFee", rec.insuranceFee);
    query.bindValue(":totalToPay", rec.totalToPay);
    if (!query.exec()) qDebug() << "saveSalaryRecord:" << query.lastError().text();
}

void MainWindow::refreshEmployeeTable(const QString &searchText)
{
    QVector<Employee> employees = getAllEmployees(searchText);

    ui->employeeTable->setRowCount(employees.size());

    for (int row = 0; row < employees.size(); ++row) {
        const Employee &emp = employees[row];
        ui->employeeTable->setItem(row, 0, new QTableWidgetItem(QString::number(emp.id)));
        ui->employeeTable->setItem(row, 1, new QTableWidgetItem(emp.fullName));
        ui->employeeTable->setItem(row, 2, new QTableWidgetItem(emp.position));
        ui->employeeTable->setItem(row, 3, new QTableWidgetItem(emp.department));
        ui->employeeTable->setItem(row, 4, new QTableWidgetItem(
                                               QString::number(emp.baseSalary, 'f', 2)));
    }
    ui->employeeTable->resizeColumnsToContents();

    setWindowTitle("Учёт заработной платы");
}

int MainWindow::selectedEmployeeId() const
{
    int row = ui->employeeTable->currentRow();
    if (row < 0) return -1;
    return ui->employeeTable->item(row, 0)->text().toInt();
}

void MainWindow::onAddEmployee()
{
    EmployeeDialog dialog(Employee{}, this);
    if (dialog.exec() != QDialog::Accepted) return;

    Employee newEmployee = dialog.getEmployee();
    addEmployee(newEmployee);

    refreshEmployeeTable(ui->searchEdit->text());
    fillEmployeeComboBoxes();
}

void MainWindow::onEditEmployee()
{
    int employeeId = selectedEmployeeId();
    if (employeeId == -1) {
        QMessageBox::information(this, "Редактирование", "Сначала выберите сотрудника в таблице.");
        return;
    }

    Employee existing = getEmployeeById(employeeId);
    EmployeeDialog dialog(existing, this);
    if (dialog.exec() != QDialog::Accepted) return;

    Employee updated = dialog.getEmployee();
    updated.id = employeeId;
    updateEmployee(updated);

    refreshEmployeeTable(ui->searchEdit->text());
    fillEmployeeComboBoxes();
}

void MainWindow::onDeleteEmployee()
{
    int employeeId = selectedEmployeeId();
    if (employeeId == -1) {
        QMessageBox::information(this, "Удаление", "Сначала выберите сотрудника в таблице.");
        return;
    }

    int answer = QMessageBox::question(this, "Подтверждение",
                                       "Удалить выбранного сотрудника?\n"
                                       "Вместе с ним удалятся его табели и начисления.");
    if (answer != QMessageBox::Yes) return;

    deleteEmployee(employeeId);
    refreshEmployeeTable(ui->searchEdit->text());
    fillEmployeeComboBoxes();
}

void MainWindow::fillEmployeeComboBoxes()
{
    ui->timesheetEmployeeCombo->clear();
    ui->salaryEmployeeCombo->clear();
    ui->historyEmployeeCombo->clear();

    for (const Employee &emp : getAllEmployees()) {
        ui->timesheetEmployeeCombo->addItem(emp.fullName, emp.id);
        ui->salaryEmployeeCombo->addItem(emp.fullName, emp.id);
        ui->historyEmployeeCombo->addItem(emp.fullName, emp.id);
    }
}

void MainWindow::onSaveTimesheet()
{
    if (ui->timesheetEmployeeCombo->currentIndex() < 0) {
        QMessageBox::warning(this, "Ошибка", "Выберите сотрудника из списка.");
        return;
    }

    int workedDays = ui->workedDaysSpin->value();
    int normDays   = ui->normDaysSpin->value();
    if (workedDays > normDays) {
        QMessageBox::warning(this, "Ошибка", "Отработанные дни не могут быть больше нормы дней.");
        return;
    }

    Timesheet ts;
    ts.employeeId = ui->timesheetEmployeeCombo->currentData().toInt();
    ts.period     = ui->timesheetPeriodEdit->date().toString("yyyy-MM");
    ts.workedDays = workedDays;
    ts.normDays   = normDays;

    saveTimesheet(ts);
    QMessageBox::information(this, "Готово", "Табель успешно сохранён.");
}

void MainWindow::onCalculateSalary()
{
    salaryCalculated = false;

    if (ui->salaryEmployeeCombo->currentIndex() < 0) {
        QMessageBox::warning(this, "Ошибка", "Выберите сотрудника из списка.");
        return;
    }

    int employeeId = ui->salaryEmployeeCombo->currentData().toInt();
    QString period = ui->salaryPeriodEdit->date().toString("yyyy-MM");

    Employee emp = getEmployeeById(employeeId);
    Timesheet ts = getTimesheet(employeeId, period);
    if (ts.id == -1) {
        QMessageBox::warning(this, "Нет данных",
                             "Для этого сотрудника нет табеля за выбранный месяц.\n"
                             "Сначала заполните табель на вкладке «Табель».");
        return;
    }

    TeachingLoad tl = getTeachingLoad(employeeId, period);
    double teachingAllowance = tl.teachingAllowance();

    static const double degreeAmounts[] = { 0.0, 900.0, 1500.0 };
    double degreeAllowance = degreeAmounts[qBound(0, ui->degreeCombo->currentIndex(), 2)];

    static const double titleAmounts[] = { 0.0, 900.0, 1500.0, 800.0, 500.0 };
    double titleAllowance = titleAmounts[qBound(0, ui->titleCombo->currentIndex(), 4)];

    double checkingAllowance = ui->checkingSpin->value();

    SalaryInput input;
    input.employeeId = employeeId;
    input.period = period;
    input.baseSalary = emp.baseSalary;
    input.workedDays = ts.workedDays;
    input.normDays = ts.normDays;
    input.bonus = ui->bonusSpin->value();
    input.incentive = ui->incentiveSpin->value();
    input.teachingAllowance = teachingAllowance;
    input.degreeAllowance = degreeAllowance;
    input.titleAllowance = titleAllowance;
    input.checkingAllowance = checkingAllowance;

    SalaryRecord rec = calculateSalary(input);

    double totalAccrued  = rec.accrued + rec.bonus + rec.incentive
                         + rec.academicAllowance + rec.degreeAllowance
                         + rec.titleAllowance + rec.checkingAllowance;
    double totalDeducted = rec.ndfl + rec.unionFee;

    QString periodFormatted = ui->salaryPeriodEdit->date().toString("MMMM yyyy");
    if (!periodFormatted.isEmpty())
        periodFormatted[0] = periodFormatted[0].toUpper();

    ui->payslipTitleLabel->setText(
        QString("Расчётный листок за %1").arg(periodFormatted));

    ui->payslipDaysLabel->setText(
        QString("Отработано: %1 дн. (норма %2 дн.)")
            .arg(ts.workedDays).arg(ts.normDays));

    ui->payslipDepartmentLabel->setText(
        QString("Подразделение/кафедра: %1")
            .arg(emp.department.isEmpty() ? "—" : emp.department));

    ui->payslipPositionLabel->setText(
        QString("Должность: %1")
            .arg(emp.position.isEmpty() ? "—" : emp.position));

    ui->payslipSalaryLabel->setText(
        QString("Ставка (оклад): %1 руб.%2")
            .arg(QString::number(emp.baseSalary, 'f', 2))
            .arg(teachingAllowance > 0.0
                 ? QString("   |   Нагрузка: %1 ч. (доплата %2 руб.)")
                       .arg(tl.lectures + tl.seminars + tl.exams, 0, 'f', 1)
                       .arg(teachingAllowance, 0, 'f', 2)
                 : QString()));

    struct PayRow {
        QString code;
        QString name;
        double accrued;
        double deducted;
    };

    QVector<PayRow> rows;
    rows << PayRow{ "0000000101", "Оклад (по табелю)",             rec.accrued,          0.0 };
    if (rec.degreeAllowance   > 0.0)
        rows << PayRow{ "0000000121", "Доплата за учёную степень",     rec.degreeAllowance,  0.0 };
    if (rec.titleAllowance    > 0.0)
        rows << PayRow{ "0000000122", "Доплата за звание/категорию",   rec.titleAllowance,   0.0 };
    if (rec.checkingAllowance > 0.0)
        rows << PayRow{ "0000000123", "Проверка работ",                rec.checkingAllowance,0.0 };
    if (rec.academicAllowance > 0.0)
        rows << PayRow{ "0000000120", "Доплата за учебную нагрузку",   rec.academicAllowance,0.0 };
    if (rec.bonus > 0.0)
        rows << PayRow{ "0000000110", "Премия",                        rec.bonus,            0.0 };
    if (rec.incentive > 0.0)
        rows << PayRow{ "0000000118", "Стимулирующая выплата",         rec.incentive,        0.0 };
    rows << PayRow{ "0000000301", "НДФЛ (13%)",                    0.0, rec.ndfl };
    rows << PayRow{ "0000000310", "Профсоюзный взнос (1%)",        0.0, rec.unionFee };
    rows << PayRow{ "",           "Итого",                         totalAccrued, totalDeducted };
    rows << PayRow{ "0000000400", "Страх. взносы 30% (работодатель)", rec.insuranceFee, 0.0 };

    auto makeItem = [](const QString &text, bool bold = false,
                       Qt::Alignment align = Qt::AlignVCenter | Qt::AlignLeft) {
        auto *item = new QTableWidgetItem(text);
        item->setTextAlignment(align);
        if (bold) {
            QFont f = item->font();
            f.setBold(true);
            item->setFont(f);
        }
        return item;
    };

    const Qt::Alignment numAlign = Qt::AlignVCenter | Qt::AlignRight;
    const int totalRow     = rows.size() - 2;
    const int insuranceRow = rows.size() - 1;

    ui->payslipTable->setRowCount(rows.size());
    for (int i = 0; i < rows.size(); ++i) {
        const PayRow &r = rows[i];
        bool isTotal     = (i == totalRow);
        bool isInsurance = (i == insuranceRow);

        ui->payslipTable->setItem(i, 0, makeItem(r.code, isTotal || isInsurance));
        ui->payslipTable->setItem(i, 1, makeItem(r.name, isTotal || isInsurance));

        QString accStr = (r.accrued  > 0.0) ? QString::number(r.accrued,  'f', 2) : "";
        QString dedStr = (r.deducted > 0.0) ? QString::number(r.deducted, 'f', 2) : "";
        if (isTotal) {
            accStr = QString::number(r.accrued,  'f', 2);
            dedStr = QString::number(r.deducted, 'f', 2);
        }

        ui->payslipTable->setItem(i, 2, makeItem(accStr, isTotal || isInsurance, numAlign));
        ui->payslipTable->setItem(i, 3, makeItem(dedStr, isTotal || isInsurance, numAlign));

        if (isTotal) {
            for (int c = 0; c < 4; ++c)
                if (auto *it = ui->payslipTable->item(i, c))
                    it->setBackground(QColor("#EBF4FF"));
        }
        if (isInsurance) {
            for (int c = 0; c < 4; ++c)
                if (auto *it = ui->payslipTable->item(i, c)) {
                    it->setBackground(QColor("#FEFCBF"));
                    QFont f = it->font();
                    f.setItalic(true);
                    it->setFont(f);
                }
        }
    }

    ui->payslipTable->resizeColumnsToContents();
    ui->payslipTable->horizontalHeader()->setSectionResizeMode(0, QHeaderView::ResizeToContents);
    ui->payslipTable->horizontalHeader()->setSectionResizeMode(1, QHeaderView::Stretch);
    ui->payslipTable->horizontalHeader()->setSectionResizeMode(2, QHeaderView::ResizeToContents);
    ui->payslipTable->horizontalHeader()->setSectionResizeMode(3, QHeaderView::ResizeToContents);

    ui->totalLabel->setText(QString::number(rec.totalToPay, 'f', 2) + " руб.");

    lastSalaryRecord = rec;
    salaryCalculated = true;
}

void MainWindow::onSaveSalary()
{
    int employeeId = ui->salaryEmployeeCombo->currentData().toInt();
    if (!salaryCalculated || lastSalaryRecord.employeeId != employeeId) {
        QMessageBox::information(this, "Сначала рассчитайте",
                                 "Нажмите «Рассчитать», прежде чем сохранять начисление.");
        return;
    }

    saveSalaryRecord(lastSalaryRecord);
    QMessageBox::information(this, "Готово", "Начисление зарплаты сохранено.");
}

QVector<SalaryRow> MainWindow::fetchSalaryRows(const QString &whereClause, const QString &bindValue,
                                               bool joinEmployeeName)
{
    QVector<SalaryRow> rows;

    QString labelColumn = joinEmployeeName ? "e.fullName" : "s.period";
    QString sql = QString(
        "SELECT %1, s.accrued, s.bonus, s.incentive, s.ndfl, s.unionFee, s.totalToPay "
        "FROM salary_records s ").arg(labelColumn);
    if (joinEmployeeName)
        sql += "JOIN employees e ON e.id = s.employeeId ";
    sql += "WHERE " + whereClause;
    sql += joinEmployeeName ? " ORDER BY e.fullName" : " ORDER BY s.period DESC";

    QSqlQuery query;
    query.prepare(sql);
    query.bindValue(":val", bindValue);
    if (!query.exec()) {
        qDebug() << "fetchSalaryRows:" << query.lastError().text();
        return rows;
    }

    while (query.next()) {
        SalaryRow r;
        QString rawLabel = query.value(0).toString();
        if (!joinEmployeeName) {
            QDate d = QDate::fromString(rawLabel, "yyyy-MM");
            rawLabel = d.isValid() ? d.toString("MM.yyyy") : rawLabel;
        }
        r.label      = rawLabel;
        r.accrued    = query.value(1).toDouble();
        r.bonus      = query.value(2).toDouble();
        r.incentive  = query.value(3).toDouble();
        r.ndfl       = query.value(4).toDouble();
        r.unionFee   = query.value(5).toDouble();
        r.totalToPay = query.value(6).toDouble();
        rows.append(r);
    }
    return rows;
}

void MainWindow::onGenerateReport()
{
    QString period = ui->reportPeriodEdit->date().toString("yyyy-MM");

    QVector<SalaryRow> rows = fetchSalaryRows("s.period = :val", period, /*joinEmployeeName=*/true);

    if (rows.isEmpty()) {
        QMessageBox::information(this, "Отчёт", "За выбранный месяц начислений ещё нет.");
    }

    ui->reportTable->setRowCount(rows.size());
    double totalSum = 0.0;
    const double MROT = 22440.0;
    QColor warningColor(255, 255, 180);

    for (int row = 0; row < rows.size(); ++row) {
        const SalaryRow &r = rows[row];
        ui->reportTable->setItem(row, 0, new QTableWidgetItem(r.label));
        ui->reportTable->setItem(row, 1, new QTableWidgetItem(QString::number(r.accrued, 'f', 2)));
        ui->reportTable->setItem(row, 2, new QTableWidgetItem(QString::number(r.bonus, 'f', 2)));
        ui->reportTable->setItem(row, 3, new QTableWidgetItem(QString::number(r.incentive, 'f', 2)));
        ui->reportTable->setItem(row, 4, new QTableWidgetItem(QString::number(r.ndfl, 'f', 2)));
        ui->reportTable->setItem(row, 5, new QTableWidgetItem(QString::number(r.unionFee, 'f', 2)));
        ui->reportTable->setItem(row, 6, new QTableWidgetItem(QString::number(r.totalToPay, 'f', 2)));

        if (r.totalToPay < MROT) {
            for (int col = 0; col < ui->reportTable->columnCount(); ++col)
                ui->reportTable->item(row, col)->setBackground(warningColor);
        }

        totalSum += r.totalToPay;
    }

    ui->reportTable->resizeColumnsToContents();
    ui->reportTotalLabel->setText("Итого по университету: " +
                                  QString::number(totalSum, 'f', 2) + " руб.");
}

void MainWindow::onTabChanged(int index)
{
    if (index == 0)
        refreshEmployeeTable(ui->searchEdit->text());
    else if (index == 4) {
        int empId = ui->historyEmployeeCombo->currentData().toInt();
        if (empId > 0)
            refreshHistoryTable(empId);
    }
}

void MainWindow::onExportCsv()
{
    if (ui->reportTable->rowCount() == 0) {
        QMessageBox::information(this, "Экспорт", "Сначала сформируйте отчёт — таблица пуста.");
        return;
    }

    QString fileName = QFileDialog::getSaveFileName(
        this, "Сохранить отчёт как CSV",
        "report_" + ui->reportPeriodEdit->date().toString("yyyy-MM") + ".csv",
        "CSV файлы (*.csv)");
    if (fileName.isEmpty()) return;

    QFile file(fileName);
    if (!file.open(QIODevice::WriteOnly | QIODevice::Text)) {
        QMessageBox::warning(this, "Ошибка", "Не удалось создать файл.");
        return;
    }

    QTextStream out(&file);
    out.setEncoding(QStringConverter::Utf8);

    int columnCount = ui->reportTable->columnCount();
    QStringList headers;
    for (int col = 0; col < columnCount; ++col)
        headers << ui->reportTable->horizontalHeaderItem(col)->text();
    out << headers.join(";") << "\n";

    for (int row = 0; row < ui->reportTable->rowCount(); ++row) {
        QStringList cells;
        for (int col = 0; col < columnCount; ++col)
            cells << ui->reportTable->item(row, col)->text();
        out << cells.join(";") << "\n";
    }

    file.close();
    QMessageBox::information(this, "Готово", "Отчёт сохранён в файл:\n" + fileName);
}

void MainWindow::refreshHistoryTable(int employeeId)
{
    QVector<SalaryRow> rows = fetchSalaryRows("employeeId = :val", QString::number(employeeId),
                                              /*joinEmployeeName=*/false);

    ui->historyTable->setRowCount(rows.size());
    for (int r = 0; r < rows.size(); ++r) {
        const SalaryRow &row = rows[r];
        ui->historyTable->setItem(r, 0, new QTableWidgetItem(row.label));
        ui->historyTable->setItem(r, 1, new QTableWidgetItem(QString::number(row.accrued, 'f', 2)));
        ui->historyTable->setItem(r, 2, new QTableWidgetItem(QString::number(row.bonus, 'f', 2)));
        ui->historyTable->setItem(r, 3, new QTableWidgetItem(QString::number(row.incentive, 'f', 2)));
        ui->historyTable->setItem(r, 4, new QTableWidgetItem(QString::number(row.ndfl, 'f', 2)));
        ui->historyTable->setItem(r, 5, new QTableWidgetItem(QString::number(row.unionFee, 'f', 2)));
        ui->historyTable->setItem(r, 6, new QTableWidgetItem(QString::number(row.totalToPay, 'f', 2)));
    }

    ui->historyTable->resizeColumnsToContents();
}

void MainWindow::saveTeachingLoad(const TeachingLoad &tl)
{
    QSqlQuery query;
    query.prepare("INSERT OR REPLACE INTO teaching_load "
                  "(employeeId, period, lectures, seminars, exams, lectureRate, seminarRate, examRate) "
                  "VALUES (:employeeId, :period, :lectures, :seminars, :exams, :lectureRate, :seminarRate, :examRate)");
    query.bindValue(":employeeId", tl.employeeId);
    query.bindValue(":period", tl.period);
    query.bindValue(":lectures", tl.lectures);
    query.bindValue(":seminars", tl.seminars);
    query.bindValue(":exams", tl.exams);
    query.bindValue(":lectureRate", tl.lectureRate);
    query.bindValue(":seminarRate", tl.seminarRate);
    query.bindValue(":examRate", TeachingLoad::EXAM_RATE);
    if (!query.exec()) qDebug() << "saveTeachingLoad:" << query.lastError().text();
}

TeachingLoad MainWindow::getTeachingLoad(int employeeId, const QString &period)
{
    TeachingLoad tl;
    QSqlQuery query;
    query.prepare("SELECT id, employeeId, period, lectures, seminars, exams, lectureRate, seminarRate "
                  "FROM teaching_load WHERE employeeId=:employeeId AND period=:period");
    query.bindValue(":employeeId", employeeId);
    query.bindValue(":period",     period);
    if (query.exec() && query.next()) {
        tl.id          = query.value(0).toInt();
        tl.employeeId  = query.value(1).toInt();
        tl.period      = query.value(2).toString();
        tl.lectures    = query.value(3).toDouble();
        tl.seminars    = query.value(4).toDouble();
        tl.exams       = query.value(5).toDouble();
        tl.lectureRate = query.value(6).toDouble();
        tl.seminarRate = query.value(7).toDouble();
    }
    return tl;
}

void MainWindow::onSaveTeachingLoad()
{
    if (ui->timesheetEmployeeCombo->currentIndex() < 0) {
        QMessageBox::warning(this, "Ошибка", "Выберите сотрудника из списка.");
        return;
    }

    double total = ui->lecturesSpin->value() + ui->seminarsSpin->value() + ui->examsSpin->value();
    if (total <= 0.0) {
        QMessageBox::warning(this, "Ошибка ввода", "Укажите хотя бы один вид нагрузки (часы > 0).");
        return;
    }

    TeachingLoad tl;
    tl.employeeId  = ui->timesheetEmployeeCombo->currentData().toInt();
    tl.period = ui->timesheetPeriodEdit->date().toString("yyyy-MM");
    tl.lectures = ui->lecturesSpin->value();
    tl.seminars = ui->seminarsSpin->value();
    tl.exams = ui->examsSpin->value();
    tl.lectureRate = ui->lectureRateSpin->value();
    tl.seminarRate = ui->seminarRateSpin->value();

    saveTeachingLoad(tl);

    double allowance = tl.teachingAllowance();
    QMessageBox::information(this, "Готово",
        QString("Учебная нагрузка сохранена.\n\n"
                "Лекции:       %1 ч. × %2 руб. = %3 руб.\n"
                "Семинары:     %4 ч. × %5 руб. = %6 руб.\n"
                "Зачёты/экз.:  %7 ч. × %8 руб. = %9 руб.\n\n"
                "Доплата за нагрузку: %10 руб.")
            .arg(tl.lectures, 0, 'f', 1).arg(tl.lectureRate, 0, 'f', 2)
            .arg(tl.lectures * tl.lectureRate, 0, 'f', 2)
            .arg(tl.seminars, 0, 'f', 1).arg(tl.seminarRate, 0, 'f', 2)
            .arg(tl.seminars * tl.seminarRate, 0, 'f', 2)
            .arg(tl.exams, 0, 'f', 1).arg(TeachingLoad::EXAM_RATE, 0, 'f', 2)
            .arg(tl.exams * TeachingLoad::EXAM_RATE, 0, 'f', 2)
            .arg(allowance, 0, 'f', 2));
}

void MainWindow::onHistoryEmployeeChanged(int /*index*/)
{
    int empId = ui->historyEmployeeCombo->currentData().toInt();
    if (empId > 0)
        refreshHistoryTable(empId);
}
