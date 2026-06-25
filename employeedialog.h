#ifndef EMPLOYEEDIALOG_H
#define EMPLOYEEDIALOG_H

#include <QDialog>
#include "models.h"

namespace Ui { class EmployeeDialog; }

class EmployeeDialog : public QDialog
{
    Q_OBJECT

public:
    explicit EmployeeDialog(const Employee &existing = Employee(), QWidget *parent = nullptr);
    ~EmployeeDialog();

    Employee getEmployee() const;

private slots:
    void onSaveClicked();

private:
    Ui::EmployeeDialog *ui;
    Employee employee;
};

#endif // EMPLOYEEDIALOG_H
