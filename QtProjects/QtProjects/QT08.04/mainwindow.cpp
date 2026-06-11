#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QFile>
#include <QTextStream>
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

//Читать из файла f
void MainWindow::on_btnRead_clicked()
{
    QFile fileF("f.txt");

    if (!fileF.exists()) {
        QMessageBox::warning(this, "Ошибка", "Файл f.txt не найден");
        return;
    }

    if (!fileF.open(QIODevice::ReadOnly)) {
        QMessageBox::warning(this, "Ошибка", "Не удалось открыть файл f.txt для чтения");
        return;
    }

    QTextStream stream(&fileF);
    QString buffer = stream.readAll();
    ui->textF->setText(buffer);

    fileF.close();
}

//Обработать и записать в файл g
void MainWindow::on_btnWrite_clicked()
{
    //числа из текстового поля
    QStringList numbers = ui->textF->toPlainText().split(" ", Qt::SkipEmptyParts);

    if (numbers.isEmpty()) {
        QMessageBox::warning(this, "Ошибка", "Нет данных для обработки");
        return;
    }

    QList<int> nums;
    for (int i = 0; i < numbers.length(); i++) {
        nums.append(numbers[i].toInt());
    }

    QList<int> maxValues;
    int i = 0;
    while (i < nums.size()) {
        //Определяем конец текущей группы
        int groupEnd = i + 5;
        if (groupEnd > nums.size())
            groupEnd = nums.size();

        int maxVal = nums[i];
        for (int j = i + 1; j < groupEnd; j++) {
            if (nums[j] > maxVal)
                maxVal = nums[j];
        }

        maxValues.append(maxVal);
        i = groupEnd;
    }

    QFile fileG("g.txt");

    if (!fileG.open(QIODevice::WriteOnly)) {
        QMessageBox::warning(this, "Ошибка", "Не удалось открыть файл g.txt для записи");
        return;
    }

    QTextStream stream(&fileG);
    QString result = "";
    for (int k = 0; k < maxValues.size(); k++) {
        stream << QString::number(maxValues[k]) + " ";
        result += QString::number(maxValues[k]) + " ";
    }

    fileG.close();

    ui->textG->setText(result);
    QMessageBox::information(this, "Готово", "Результат записан в файл g.txt");
}
