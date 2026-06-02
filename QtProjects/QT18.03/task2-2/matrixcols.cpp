#include "matrixcols.h"
#include "ui_matrixcols.h"   // автогенерируется Qt из matrixcols.ui
#include <QFont>
#include <cstdlib>
#include <ctime>

MatrixCols::MatrixCols(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MatrixCols)
{
    ui->setupUi(this);  // применяет разметку из .ui файла

    ui->textEdit->setReadOnly(true);
    ui->textEdit->setFont(QFont("Courier New", 11));

    fillMatrix();
    displayMatrix();
}

MatrixCols::~MatrixCols()
{
    delete ui;
}

// Заполнение матрицы случайными числами от 1 до 20
void MatrixCols::fillMatrix()
{
    srand(static_cast<unsigned>(time(nullptr)));
    matrix.clear();
    for (int i = 0; i < 4; i++) {
        QVector<int> row;
        for (int j = 0; j < 5; j++)
            row.append(rand() % 20 + 1);
        matrix.append(row);
    }
}

// Вывод матрицы в textEdit с заголовком столбцов
void MatrixCols::displayMatrix()
{
    ui->textEdit->clear();

    // Шапка: номера столбцов
    QString header = "      ";
    for (int j = 0; j < matrix[0].size(); j++)
        header += QString(" [%1]").arg(j);
    ui->textEdit->append(header);

    // Строки матрицы (каждое число в поле шириной 4)
    for (int i = 0; i < matrix.size(); i++) {
        QString str = QString("[%1]  ").arg(i);
        for (int j = 0; j < matrix[i].size(); j++)
            str += QString("%1").arg(matrix[i][j], 4);
        ui->textEdit->append(str);
    }
}

// Обработчик кнопки — вычисляет суммы выбранных столбцов
void MatrixCols::on_pushButton_clicked()
{
    // currentIndex() = 0 → чётные столбцы (j = 0, 2, 4, ...)
    // currentIndex() = 1 → нечётные столбцы (j = 1, 3, ...)
    int choice   = ui->comboBox->currentIndex();
    int startCol = choice;  // индекс совпадает с нужным стартом

    QString result = (choice == 0)
        ? "Суммы чётных столбцов (0, 2, 4):\n"
        : "Суммы нечётных столбцов (1, 3):\n";

    for (int j = startCol; j < matrix[0].size(); j += 2) {
        int colSum = 0;
        for (int i = 0; i < matrix.size(); i++)
            colSum += matrix[i][j];
        result += QString("  Столбец [%1]: сумма = %2\n").arg(j).arg(colSum);
    }

    ui->labelResult->setText(result);
}
