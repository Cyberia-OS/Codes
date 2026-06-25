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

static SalaryRecord calculateSalary(int employeeId, const QString &period,
                                    double baseSalary, int workedDays,
                                    int normDays, double bonus, double incentive)
{
    SalaryRecord rec;
    rec.employeeId = employeeId;
    rec.period = period;
    rec.bonus = bonus;
    rec.incentive = incentive;

    rec.accrued = normDays > 0 ? (baseSalary / normDays) * workedDays : 0.0;

    double taxable = rec.accrued + rec.bonus + rec.incentive;
    rec.ndfl = taxable * 0.13;
    rec.unionFee = taxable * 0.01;
    rec.totalToPay = taxable - rec.ndfl - rec.unionFee;

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

    connect(ui->calcSalaryButton, &QPushButton::clicked, this, &MainWindow::onCalculateSalary);
    connect(ui->saveSalaryButton, &QPushButton::clicked, this, &MainWindow::onSaveSalary);

    connect(ui->generateReportButton,&QPushButton::clicked, this, &MainWindow::onGenerateReport);
    connect(ui->exportCsvButton, &QPushButton::clicked, this, &MainWindow::onExportCsv);

    connect(ui->tabWidget, &QTabWidget::currentChanged, this, &MainWindow::onTabChanged);
    connect(ui->historyEmployeeCombo, QOverload<int>::of(&QComboBox::currentIndexChanged),
            this, &MainWindow::onHistoryEmployeeChanged);
    connect(ui->actionDarkTheme, &QAction::toggled, this, &MainWindow::onToggleDarkTheme);

    // Восстановить тему из прошлой сессии
    QSettings settings("MyOrg", "PayrollApp");
    bool darkTheme = settings.value("darkTheme", false).toBool();
    ui->actionDarkTheme->setChecked(darkTheme);
    applyTheme(darkTheme);

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
            "  ndfl REAL DEFAULT 0,"
            "  unionFee REAL DEFAULT 0,"
            "  totalToPay REAL DEFAULT 0,"
            "  UNIQUE(employeeId, period)"
            ")"
            )) qDebug() << "createTables salary_records:" << query.lastError().text();
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
                  "(employeeId, period, accrued, bonus, incentive, ndfl, unionFee, totalToPay) "
                  "VALUES (:employeeId, :period, :accrued, :bonus, :incentive, :ndfl, :unionFee, :totalToPay)");
    query.bindValue(":employeeId", rec.employeeId);
    query.bindValue(":period", rec.period);
    query.bindValue(":accrued", rec.accrued);
    query.bindValue(":bonus", rec.bonus);
    query.bindValue(":incentive", rec.incentive);
    query.bindValue(":ndfl", rec.ndfl);
    query.bindValue(":unionFee", rec.unionFee);
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

    SalaryRecord rec = calculateSalary(employeeId, period, emp.baseSalary,
                                       ts.workedDays, ts.normDays,
                                       ui->bonusSpin->value(),
                                       ui->incentiveSpin->value());

    double totalAccrued = rec.accrued + rec.bonus + rec.incentive;
    double totalDeducted = rec.ndfl + rec.unionFee;

    // --- Шапка расчётного листка (форма Т-51) ---
    QString periodFormatted = ui->salaryPeriodEdit->date().toString("MMMM yyyy");
    // Первая буква заглавная
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
        QString("Ставка (оклад): %1 руб.")
            .arg(QString::number(emp.baseSalary, 'f', 2)));

    // --- Таблица: Код | Начисление/удержание | Начислено | Удержано ---
    struct PayRow {
        QString code;
        QString name;
        double accrued;   // > 0 → показываем в «Начислено»
        double deducted;  // > 0 → показываем в «Удержано»
    };

    QVector<PayRow> rows = {
        { "0000000101", "Оклад (по табелю)",         rec.accrued,    0.0 },
        { "0000000110", "Премия",                    rec.bonus,      0.0 },
        { "0000000118", "Стимулирующая выплата",     rec.incentive,  0.0 },
        { "0000000301", "НДФЛ (13%)",                0.0,            rec.ndfl },
        { "0000000310", "Профсоюзный взнос (1%)",   0.0,            rec.unionFee },
        { "",           "Итого",                     totalAccrued,   totalDeducted },
    };

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

    ui->payslipTable->setRowCount(rows.size());
    for (int i = 0; i < rows.size(); ++i) {
        const PayRow &r = rows[i];
        bool isTotal = (i == rows.size() - 1);

        ui->payslipTable->setItem(i, 0, makeItem(r.code,  isTotal));
        ui->payslipTable->setItem(i, 1, makeItem(r.name,  isTotal));

        QString accStr = (r.accrued  > 0.0) ? QString::number(r.accrued,  'f', 2) : "";
        QString dedStr = (r.deducted > 0.0) ? QString::number(r.deducted, 'f', 2) : "";
        if (isTotal) { accStr = QString::number(r.accrued, 'f', 2); dedStr = QString::number(r.deducted, 'f', 2); }

        ui->payslipTable->setItem(i, 2, makeItem(accStr, isTotal, numAlign));
        ui->payslipTable->setItem(i, 3, makeItem(dedStr, isTotal, numAlign));

        if (isTotal) {
            for (int c = 0; c < 4; ++c) {
                if (auto *it = ui->payslipTable->item(i, c)) {
                    it->setBackground(QColor("#EBF4FF"));
                }
            }
        }
    }

    // Фиксируем ширину колонки «Код» и растягиваем «Начисление/удержание»
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

void MainWindow::onGenerateReport()
{
    QString period = ui->reportPeriodEdit->date().toString("yyyy-MM");

    QSqlQuery query;
    query.prepare(
        "SELECT e.fullName, s.accrued, s.bonus, s.incentive, s.ndfl, s.unionFee, s.totalToPay "
        "FROM salary_records s "
        "JOIN employees e ON e.id = s.employeeId "
        "WHERE s.period = :period "
        "ORDER BY e.fullName"
        );
    query.bindValue(":period", period);

    if (!query.exec()) {
        qDebug() << "onGenerateReport:" << query.lastError().text();
        return;
    }

    struct Row { QString name; double accrued, bonus, incentive, ndfl, unionFee, totalToPay; };
    QVector<Row> rows;
    while (query.next()) {
        Row r;
        r.name = query.value(0).toString();
        r.accrued = query.value(1).toDouble();
        r.bonus = query.value(2).toDouble();
        r.incentive = query.value(3).toDouble();
        r.ndfl = query.value(4).toDouble();
        r.unionFee = query.value(5).toDouble();
        r.totalToPay = query.value(6).toDouble();
        rows.append(r);
    }

    if (rows.isEmpty()) {
        QMessageBox::information(this, "Отчёт", "За выбранный месяц начислений ещё нет.");
    }

    ui->reportTable->setRowCount(rows.size());
    double totalSum = 0.0;
    const double MROT = 22440.0;
    QColor warningColor(255, 255, 180);

    for (int row = 0; row < rows.size(); ++row) {
        const Row &r = rows[row];
        ui->reportTable->setItem(row, 0, new QTableWidgetItem(r.name));
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
    QSqlQuery q;
    q.prepare("SELECT period, accrued, bonus, incentive, ndfl, unionFee, totalToPay "
              "FROM salary_records WHERE employeeId=:id ORDER BY period DESC");
    q.bindValue(":id", employeeId);
    q.exec();

    QVector<QVector<QString>> rows;
    while (q.next()) {
        QVector<QString> row;
        // Форматируем период из "yyyy-MM" в "MM.yyyy"
        QString raw = q.value(0).toString();
        QDate d = QDate::fromString(raw, "yyyy-MM");
        row << (d.isValid() ? d.toString("MM.yyyy") : raw);
        for (int col = 1; col <= 6; ++col)
            row << QString::number(q.value(col).toDouble(), 'f', 2);
        rows.append(row);
    }

    ui->historyTable->setRowCount(rows.size());
    for (int r = 0; r < rows.size(); ++r)
        for (int c = 0; c < 7; ++c)
            ui->historyTable->setItem(r, c, new QTableWidgetItem(rows[r][c]));

    ui->historyTable->resizeColumnsToContents();
}

void MainWindow::onHistoryEmployeeChanged(int /*index*/)
{
    int empId = ui->historyEmployeeCombo->currentData().toInt();
    if (empId > 0)
        refreshHistoryTable(empId);
}

void MainWindow::applyTheme(bool dark)
{
    QSettings settings("MyOrg", "PayrollApp");
    settings.setValue("darkTheme", dark);

    if (dark) {
        qApp->setStyleSheet(
            "QMainWindow, QWidget {"
            "  background-color: #1A202C; color: #E2E8F0;"
            "  font-family: 'Segoe UI', sans-serif; font-size: 13px;"
            "}"
            "QTabWidget::pane { border: 1px solid #4A5568; background-color: #2D3748; border-radius: 4px; }"
            "QTabBar::tab { background-color: #2D3748; color: #A0AEC0; padding: 7px 18px;"
            "  border: 1px solid #4A5568; border-bottom: none;"
            "  border-top-left-radius: 4px; border-top-right-radius: 4px; margin-right: 2px; }"
            "QTabBar::tab:selected { background-color: #1A202C; color: #63B3ED; font-weight: bold; }"
            "QTabBar::tab:hover:!selected { background-color: #4A5568; }"
            "QPushButton { background-color: #2B6CB0; color: #FFFFFF; border: none;"
            "  border-radius: 4px; padding: 6px 14px; font-weight: bold; }"
            "QPushButton:hover { background-color: #3182CE; }"
            "QPushButton:pressed { background-color: #2C5282; }"
            "QLineEdit, QSpinBox, QDoubleSpinBox, QDateEdit, QComboBox {"
            "  background-color: #2D3748; border: 1px solid #4A5568;"
            "  border-radius: 4px; padding: 4px 8px; color: #E2E8F0; }"
            "QLineEdit:focus, QSpinBox:focus, QDoubleSpinBox:focus, QDateEdit:focus, QComboBox:focus {"
            "  border: 1px solid #63B3ED; }"
            "QTableWidget { background-color: #2D3748; border: 1px solid #4A5568;"
            "  gridline-color: #4A5568; border-radius: 4px; color: #E2E8F0; }"
            "QTableWidget::item:selected { background-color: #2B6CB0; color: #FFFFFF; }"
            "QHeaderView::section { background-color: #1A202C; color: #63B3ED; font-weight: bold;"
            "  border: none; border-bottom: 2px solid #2B6CB0; border-right: 1px solid #4A5568; padding: 5px 8px; }"
            "QLabel { color: #E2E8F0; }"
            "QMenuBar { background-color: #1A202C; color: #E2E8F0; }"
            "QMenuBar::item:selected { background-color: #2D3748; }"
            "QMenu { background-color: #2D3748; color: #E2E8F0; border: 1px solid #4A5568; }"
            "QMenu::item:selected { background-color: #2B6CB0; }"
            "QFrame { border-color: #4A5568; }"
        );
    } else {
        qApp->setStyleSheet(""); // вернуть исходную тему из .ui
    }
}

void MainWindow::onToggleDarkTheme(bool checked)
{
    applyTheme(checked);
}