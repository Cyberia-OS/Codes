#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QMessageBox>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent), ui(new Ui::MainWindow)
{
    ui->setupUi(this);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_pushButton_clicked()
{
    bool ok1, ok2, ok3;
    double a = ui->lineEdit_A->text().toDouble(&ok1);
    double b = ui->lineEdit_B->text().toDouble(&ok2);
    double c = ui->lineEdit_C->text().toDouble(&ok3);

    if (!ok1 || !ok2 || !ok3) {
        QMessageBox::warning(this, "Ошибка", "Введите числа во все поля!");
        return;
    }

    double result;
    QString who;

    if (a <= b && a <= c) {
        who = "A";
        result = b + c;
    } else if (b <= c) {
        who = "B";
        result = a + c;
    } else {
        who = "C";
        result = a + b;
    }

    ui->label_result->setText("Наименьшее: " + who + "\n" +
                              who + " = " + QString::number(result));
}
