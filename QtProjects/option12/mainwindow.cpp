#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QMessageBox>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
}

MainWindow::~MainWindow()
{
    delete ui;
}

//Часть 1: Замена наименьшего из трёх чисел суммой двух других
void MainWindow::on_pushButton_Calc_clicked()
{
    bool ok1, ok2, ok3;
    int x = ui->lineEdit_X->text().toInt(&ok1);
    int y = ui->lineEdit_Y->text().toInt(&ok2);
    int z = ui->lineEdit_Z->text().toInt(&ok3);

    if (!ok1 || !ok2 || !ok3) {
        QMessageBox::information(this, "Ошибка", "Введите целые числа!");
        return;
    }

    int minVal = x;
    if (y < minVal) minVal = y;
    if (z < minVal) minVal = z;

    if (minVal == x) {
        x = y + z;
    } else if (minVal == y) {
        y = x + z;
    } else {
        z = x + y;
    }

    ui->label_Result->setText(QString("X = %1, Y = %2, Z = %3").arg(x).arg(y).arg(z));
}

//Часть 2: Преобразование A и B
void MainWindow::on_pushButton_Transform_clicked()
{
    bool ok1, ok2;
    int a = ui->lineEdit_A->text().toInt(&ok1);
    int b = ui->lineEdit_B->text().toInt(&ok2);

    if (!ok1 || !ok2) {
        QMessageBox::information(this, "Ошибка", "Введите целые числа!");
        return;
    }

    if (a != b) {
        int sum = a + b;
        a = sum;
        b = sum;
    } else {
        a = 0;
        b = 0;
    }

    ui->label_ResultA->setText(QString::number(a));
    ui->label_ResultB->setText(QString::number(b));
}