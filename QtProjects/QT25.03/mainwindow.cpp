#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QStack>
#include <QQueue>
#include <QRandomGenerator>

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

//задание 1
void MainWindow::on_btnStack_clicked()
{
    QStack<int> stk;

    // Заполняем стек 10 случайными числами от -20 до 20
    for (int i = 0; i < 10; i++) {
        int num = QRandomGenerator::global()->bounded(-20, 21);
        stk.push(num);
    }

    QString text = "Исходный стек:\n";
    for (int i = 0; i < stk.size(); i++) {
        text += QString::number(stk[i]) + " ";
    }
    text += "\n\n";

    //замена положительные 1, отрицательные -1, 0
    for (int i = 0; i < stk.size(); i++) {
        if (stk[i] > 0)
            stk[i] = 1;
        else if (stk[i] < 0)
            stk[i] = -1;
    }

    text += "После замены:\n";
    for (int i = 0; i < stk.size(); i++) {
        text += QString::number(stk[i]) + " ";
    }

    ui->stackOutput->setText(text);
}

//задание 2 QQueue
void MainWindow::on_btnQueue_clicked()
{
    QQueue<int> q;

    // Заполняем очередь 10 случайными числами от -10 до 20
    for (int i = 0; i < 10; i++) {
        int num = QRandomGenerator::global()->bounded(-10, 21);
        q.enqueue(num);
    }

    QString text = "Исходная очередь:\n";
    for (int i = 0; i < q.size(); i++) {
        text += QString::number(q[i]) + " ";
    }
    text += "\n\n";

    int maxVal = q[0];
    for (int i = 1; i < q.size(); i++) {
        if (q[i] > maxVal)
            maxVal = q[i];
    }

    text += "Максимальный элемент: " + QString::number(maxVal) + "\n\n";

    //увеличиваем каждый элемент на maxVal
    for (int i = 0; i < q.size(); i++) {
        q[i] = q[i] + maxVal;
    }

    text += "После увеличения на max:\n";
    for (int i = 0; i < q.size(); i++) {
        text += QString::number(q[i]) + " ";
    }

    ui->queueOutput->setText(text);
}
