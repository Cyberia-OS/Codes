#ifndef MATRIXCOLS_H
#define MATRIXCOLS_H

#include <QMainWindow>
#include <QVector>

QT_BEGIN_NAMESPACE
namespace Ui { class MatrixCols; }
QT_END_NAMESPACE

class MatrixCols : public QMainWindow
{
    Q_OBJECT

public:
    explicit MatrixCols(QWidget *parent = nullptr);
    ~MatrixCols();

private slots:
    void on_pushButton_clicked();

private:
    Ui::MatrixCols *ui;
    QVector<QVector<int>> matrix;   // двумерный вектор (матрица)

    void fillMatrix();
    void displayMatrix();
};

#endif // MATRIXCOLS_H
