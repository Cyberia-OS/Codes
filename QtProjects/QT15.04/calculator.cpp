#include "calculator.h"
#include "ui_calculator.h"

calculator::calculator(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::calculator)
{
    ui->setupUi(this);
}

calculator::~calculator()
{
    delete ui;
}

void calculator::on_btn0_clicked() { ui->lineEdit->insert("0"); }
void calculator::on_btn1_clicked() { ui->lineEdit->insert("1"); }
void calculator::on_btn2_clicked() { ui->lineEdit->insert("2"); }
void calculator::on_btn3_clicked() { ui->lineEdit->insert("3"); }
void calculator::on_btn4_clicked() { ui->lineEdit->insert("4"); }
void calculator::on_btn5_clicked() { ui->lineEdit->insert("5"); }
void calculator::on_btn6_clicked() { ui->lineEdit->insert("6"); }
void calculator::on_btn7_clicked() { ui->lineEdit->insert("7"); }
void calculator::on_btn8_clicked() { ui->lineEdit->insert("8"); }
void calculator::on_btn9_clicked() { ui->lineEdit->insert("9"); }

void calculator::on_btnPlus_clicked()  { calculate("+"); }
void calculator::on_btnMinus_clicked() { calculate("-"); }
void calculator::on_btnMul_clicked()   { calculate("*"); }
void calculator::on_btnDiv_clicked()   { calculate("/"); }

void calculator::on_btnClear_clicked()
{
    ui->lineEdit->clear();
    ui->lineEdit_2->clear();
    stack.clear();
}

void calculator::on_btnDot_clicked()
{
    if (ui->lineEdit->text().length() == 0 ||
        ui->lineEdit->text().at(ui->lineEdit->text().length()-1) == '-' ||
        ui->lineEdit->text().indexOf('.') != -1)
        return;
    ui->lineEdit->setText(ui->lineEdit->text() + '.');
}

void calculator::on_btnSign_clicked()
{
    if (ui->lineEdit->text().length() == 0 || ui->lineEdit->text().at(0) != '-')
        ui->lineEdit->setText("-" + ui->lineEdit->text());
    else
        ui->lineEdit->setText(ui->lineEdit->text().remove(0, 1));
}

void calculator::calculate(QString sgn)
{
    if (ui->lineEdit->text().length() == 0) { return; }

    if (stack.length() >= 2) {
        double val2 = ui->lineEdit->text().toDouble();
        QString sign = stack.pop();
        double val1 = stack.pop().toDouble();

        if (sign == "+") {
            stack.push(QString::number(val1 + val2));
        } else if (sign == "-") {
            stack.push(QString::number(val1 - val2));
        } else if (sign == "*") {
            stack.push(QString::number(val1 * val2));
        } else if (sign == "/") {
            if (val2 == 0)
                stack.push(QString::number(val1));
            else
                stack.push(QString::number(val1 / val2));
        }
        stack.push(sgn);
    } else {
        stack.push(ui->lineEdit->text());
        stack.push(sgn);
    }

    ui->lineEdit->clear();
    ui->lineEdit_2->setText(stack.toList().join(""));
}

void calculator::on_btnEqual_clicked()
{
    if (ui->lineEdit->text().length() != 0 && stack.length() == 2) {
        stack.push(ui->lineEdit->text());
    }
    if (stack.length() < 3) { return; }

    double val2 = stack.pop().toDouble();
    QString sign = stack.pop();
    double val1 = stack.pop().toDouble();

    if (sign == "+") {
        stack.push(QString::number(val1 + val2));
    } else if (sign == "-") {
        stack.push(QString::number(val1 - val2));
    } else if (sign == "*") {
        stack.push(QString::number(val1 * val2));
    } else if (sign == "/") {
        if (val2 == 0) {
            stack.push(QString::number(val1));
            stack.push(sign);
            ui->lineEdit->clear();
            return;
        }
        stack.push(QString::number(val1 / val2));
    }

    ui->lineEdit->setText(stack.pop());
    ui->lineEdit_2->setText(stack.toList().join(""));
}


