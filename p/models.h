#ifndef MODELS_H
#define MODELS_H

#include <QString>

struct Employee
{
    int id = -1;
    QString fullName;
    QString position;
    QString department;
    double baseSalary = 0.0;
};

struct Timesheet
{
    int id = -1;
    int employeeId = -1;
    QString period;
    int workedDays = 0;
    int normDays = 0;
};

struct SalaryRecord
{
    int id = -1;
    int employeeId = -1;
    QString period;

    double accrued = 0.0;
    double bonus = 0.0;
    double incentive = 0.0;
    double ndfl = 0.0;
    double unionFee = 0.0;
    double totalToPay = 0.0;
};

#endif // MODELS_H