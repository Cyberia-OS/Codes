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
    bool ok1, ok2;
    int a = ui->lineEdit_A->text().toInt(&ok1);
    int b = ui->lineEdit_B->text().toInt(&ok2);

    if (!ok1 || !ok2) {
        QMessageBox::warning(this, "Ошибка", "Введите целые числа!");
        return;
    }

    if (a == b) {
        a = 0;
        b = 0;
        ui->label_result->setText("A = 0\nB = 0");
    } else {
        int sum = a + b;
        a = sum;
        b = sum;
        ui->label_result->setText("A = " + QString::number(a) + "\n" +
                                  "B = " + QString::number(b));
    }
}
