#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QSqlDatabase>
#include <QVector>
#include <QTimer>
#include <QLabel>
#include <QSettings>
#include "models.h"

namespace Ui { class MainWindow; }

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private:
    void openDatabase();
    void createTables();

    void addEmployee(Employee &emp);
    void updateEmployee(const Employee &emp);
    void deleteEmployee(int employeeId);
    QVector<Employee> getAllEmployees(const QString &searchText = QString());
    Employee getEmployeeById(int employeeId);

    void saveTimesheet(const Timesheet &ts);
    Timesheet getTimesheet(int employeeId, const QString &period);

    void saveSalaryRecord(const SalaryRecord &rec);

    void saveTeachingLoad(const TeachingLoad &tl);
    TeachingLoad getTeachingLoad(int employeeId, const QString &period);

    void refreshEmployeeTable(const QString &searchText = QString());
    void fillEmployeeComboBoxes();
    int selectedEmployeeId() const;

private slots:
    void onAddEmployee();
    void onEditEmployee();
    void onDeleteEmployee();

    void onSaveTimesheet();
    void onSaveTeachingLoad();

    void onCalculateSalary();
    void onSaveSalary();

    void onGenerateReport();
    void onExportCsv();

    void onTabChanged(int index);
    void onHistoryEmployeeChanged(int index);

private:
    void refreshHistoryTable(int employeeId);
    QVector<SalaryRow> fetchSalaryRows(const QString &whereClause, const QString &bindValue,
                                       bool joinEmployeeName);

    Ui::MainWindow *ui;
    QSqlDatabase db;

    bool salaryCalculated = false;
    SalaryRecord lastSalaryRecord;

    QTimer *sessionTimer = nullptr;
    int sessionSeconds = 0;
    QLabel *statusLabel = nullptr;
};

#endif // MAINWINDOW_H