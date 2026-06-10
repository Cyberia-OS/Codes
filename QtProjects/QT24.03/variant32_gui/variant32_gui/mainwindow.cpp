#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <cstdlib>
#include <ctime>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    srand(time(0));

    for (int i = 0; i < 8; i++) {
        list.append(rand() % 50);
    }

    ui->radioButton->setChecked(true);
    ui->lineEdit->setEnabled(true);

    displayList();
}

//вывод списка через итератор
void MainWindow::displayList()
{
    ui->textEdit->clear();
    if (list.isEmpty()) {
        ui->textEdit->append("Список пуст");
        return;
    }
    QList<int>::iterator it = list.begin();
    while (it != list.end()) {
        ui->textEdit->append(QString::number(*it) + " ");
        it++;
    }
}

void MainWindow::on_radioButton_clicked()
{
    ui->lineEdit->setEnabled(true);
    ui->lineEdit->clear();
}

void MainWindow::on_radioButton_2_clicked()
{
    ui->lineEdit->setEnabled(false);
    ui->lineEdit->clear();
}

void MainWindow::on_pushButton_clicked()
{
    if (ui->radioButton->isChecked()) {
        //добавить элемент в конец списка
        bool ok;
        int num = ui->lineEdit->text().toInt(&ok);
        if (!ok) {
            return;
        }
        list.append(num);
        ui->lineEdit->clear();

    } else if (ui->radioButton_2->isChecked()) {
        //удалить элемент с начала списка
        if (list.isEmpty()) {
            ui->textEdit->clear();
            ui->textEdit->append("Список пуст");
            return;
        }
        list.removeFirst();
    }

    displayList();
}

MainWindow::~MainWindow()
{
    delete ui;
}
