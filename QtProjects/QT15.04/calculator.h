#ifndef CALCULATOR_H
#define CALCULATOR_H

#include <QMainWindow>
#include <QStack>

QT_BEGIN_NAMESPACE
namespace Ui { class calculator; }
QT_END_NAMESPACE

class calculator : public QMainWindow
{
    Q_OBJECT

public:
    calculator(QWidget *parent = nullptr);
    ~calculator();

private slots:
    void on_btn0_clicked();
    void on_btn1_clicked();
    void on_btn2_clicked();
    void on_btn3_clicked();
    void on_btn4_clicked();
    void on_btn5_clicked();
    void on_btn6_clicked();
    void on_btn7_clicked();
    void on_btn8_clicked();
    void on_btn9_clicked();

    void on_btnPlus_clicked();
    void on_btnMinus_clicked();
    void on_btnMul_clicked();
    void on_btnDiv_clicked();

    void on_btnEqual_clicked();
    void on_btnClear_clicked();
    void on_btnDot_clicked();
    void on_btnSign_clicked();

private:
    Ui::calculator *ui;
    QStack<QString> stack;

    void calculate(QString sgn);
};

#endif // CALCULATOR_H
