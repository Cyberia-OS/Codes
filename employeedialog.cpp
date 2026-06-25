#include "employeedialog.h"
#include "ui_employeedialog.h"
#include <QMessageBox>
#include <QPushButton>

EmployeeDialog::EmployeeDialog(const Employee &existing, QWidget *parent)
    : QDialog(parent), ui(new Ui::EmployeeDialog), employee(existing)
{
    ui->setupUi(this);

    ui->buttonBox->button(QDialogButtonBox::Save)->setText("Сохранить");
    ui->buttonBox->button(QDialogButtonBox::Cancel)->setText("Отмена");

    bool isEdit = (existing.id != -1);
    setWindowTitle(isEdit ? "Редактировать сотрудника" : "Добавить сотрудника");

    connect(ui->buttonBox, &QDialogButtonBox::accepted, this, [this]() {
        onSaveClicked();
    });

    if (isEdit) {
        ui->fullNameEdit->setText(existing.fullName);
        ui->positionEdit->setText(existing.position);
        ui->departmentEdit->setText(existing.department);
        ui->salarySpin->setValue(existing.baseSalary);
    }
}

EmployeeDialog::~EmployeeDialog()
{
    delete ui;
}

void EmployeeDialog::onSaveClicked()
{
    if (ui->fullNameEdit->text().trimmed().isEmpty()) {
        QMessageBox::warning(this, "Ошибка ввода", "Поле «ФИО» не может быть пустым.");
        return;
    }
    if (ui->salarySpin->value() <= 0.0) {
        QMessageBox::warning(this, "Ошибка ввода", "Оклад должен быть больше нуля.");
        return;
    }

    employee.fullName = ui->fullNameEdit->text().trimmed();
    employee.position = ui->positionEdit->text().trimmed();
    employee.department = ui->departmentEdit->text().trimmed();
    employee.baseSalary = ui->salarySpin->value();

    accept();
}

Employee EmployeeDialog::getEmployee() const
{
    return employee;
}
