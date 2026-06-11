#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    ui->comboBox->addItem("Чётные столбцы  (индексы 0, 2, 4)");
    ui->comboBox->addItem("Нечётные столбцы (индексы 1, 3, 5)");

    rows = 4;
    cols = 5;

        for (int i = 0; i < rows; i++) {
            QVector<int> row;
            for (int j = 0; j < cols; j++) {
                row.append((i + 1) * (j + 1));
            }
            matrix.append(row);
        }

    displayMatrix();
}

void MainWindow::displayMatrix()
{
    QString header = "      ";
    for (int j = 0; j < cols; j++) {
        header += QString("  [%1] ").arg(j);
    }
    ui->textEdit->append(header);
    ui->textEdit->append(QString("-").repeated(46));

    for (int i = 0; i < rows; i++) {
        QString str = QString("[%1]  ").arg(i);
        for (int j = 0; j < cols; j++) {
            str += QString("%1 | ").arg(matrix[i][j], 4);
        }
        ui->textEdit->append(str);
    }
}

void MainWindow::on_pushButton_clicked()
{
    int startCol = ui->comboBox->currentIndex();
    int sum = 0;

    for (int i = 0; i < rows; i++)
        for (int j = startCol; j < cols; j += 2)
            sum += matrix[i][j];

    QString label = (startCol == 0) ? "Сумма чётных столбцов"
                                    : "Сумма нечётных столбцов";
    ui->label_result->setText(label + ":  " + QString::number(sum));
}

MainWindow::~MainWindow()
{
    delete ui;
}
