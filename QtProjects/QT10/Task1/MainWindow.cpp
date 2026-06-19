#include "MainWindow.h"
#include "ui_MainWindow.h"
#include <QList>
#include <QTime>
#include <algorithm>
#include <cstdlib>
#include <ctime>

Dialog::Dialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::Dialog)
{
    ui->setupUi(this);
    setWindowTitle("Задание 1 - Случайная перестановка чисел");
}

Dialog::~Dialog()
{
    delete ui;
}

void Dialog::on_btnGenerate_clicked()
{
    //создаём список чисел от 1 до 20
    QList<int> list;
    for (int i = 1; i <= 20; i++)
        list << i;

    //сортируем список
    std::sort(list.begin(), list.end());

    QString inputStr = "";
    for (int i = 0; i < list.size(); i++)
    {
        if (i > 0) inputStr += ", ";
        inputStr += QString::number(list[i]);
    }
    ui->lblInput->setText("Входные данные: " + inputStr);

    //перемешиваем 20 раз меняем два случайных элемента местами
    srand(time(0));
    for (int i = 0; i < 20; i++)
    {
        int a = rand() % 20; //случайный a
        int b = rand() % 20; //случайный b
        int tmp = list[a];
        list[a] = list[b];
        list[b] = tmp;
    }

    QString resultStr = "";
    for (int i = 0; i < list.size(); i++)
    {
        if (i > 0) resultStr += ", ";
        resultStr += QString::number(list[i]);
    }
    ui->lblResult->setText("Результат: " + resultStr);
}
