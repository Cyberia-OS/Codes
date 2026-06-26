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

struct TeachingLoad
{
    static constexpr double EXAM_RATE = 250.0;

    int id = -1;
    int employeeId = -1;
    QString period;
    double lectures = 0.0;
    double seminars = 0.0;
    double exams = 0.0;
    double lectureRate = 300.0;
    double seminarRate = 200.0;

    double teachingAllowance() const {
        return lectures * lectureRate + seminars * seminarRate + exams * EXAM_RATE;
    }
};

struct SalaryInput
{
    int employeeId = -1;
    QString period;

    double baseSalary = 0.0;
    int workedDays = 0;
    int normDays = 0;

    double bonus = 0.0;
    double incentive = 0.0;
    double teachingAllowance = 0.0;
    double degreeAllowance = 0.0;
    double titleAllowance = 0.0;
    double checkingAllowance = 0.0;
};

struct SalaryRecord
{
    int id = -1;
    int employeeId = -1;
    QString period;

    double accrued = 0.0;
    double bonus = 0.0;
    double incentive = 0.0;
    double academicAllowance = 0.0;
    double degreeAllowance = 0.0;
    double titleAllowance = 0.0;
    double checkingAllowance = 0.0;
    double ndfl = 0.0;
    double unionFee = 0.0;
    double insuranceFee = 0.0;
    double totalToPay = 0.0;
    double seniorityAllowance = 0.0;
};

struct SalaryRow
{
    QString label;
    double accrued = 0.0;
    double bonus = 0.0;
    double incentive = 0.0;
    double ndfl = 0.0;
    double unionFee = 0.0;
    double totalToPay = 0.0;
};

#endif // MODELS_H