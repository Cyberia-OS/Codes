#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QRandomGenerator>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    fillMatrix();
    showMatrix();
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::fillMatrix()
{
    int rows = 3, cols = 3;
    matrix.resize(rows);
    for (int i = 0; i < rows; i++) {
        matrix[i].resize(cols);
        for (int j = 0; j < cols; j++) {
            matrix[i][j] = QRandomGenerator::global()->bounded(40);
        }
    }
}

void MainWindow::showMatrix()
{
    ui->textEdit->clear();
    for (int i = 0; i < matrix.size(); i++) {
        QString line;
        for (int j = 0; j < matrix[i].size(); j++) {
            line += QString::number(matrix[i][j]) + "\t";
        }
        ui->textEdit->append(line);
    }
}

int MainWindow::sumColumns(bool even)
{
    int sum = 0;
    int rows = matrix.size();
    int cols = matrix[0].size();
    for (int j = 0; j < cols; j++) {
        if ( (even && j % 2 == 0) || (!even && j % 2 == 1) ) {
            for (int i = 0; i < rows; i++) {
                sum += matrix[i][j];
            }
        }
    }
    return sum;
}

void MainWindow::on_pushButton_clicked()
{
    bool even = (ui->comboBox->currentIndex() == 0);
    int result = sumColumns(even);
    QString type = even ? "чётных" : "нечётных";
    ui->labelResult->setText(QString("Сумма %1 столбцов: %2").arg(type).arg(result));
}