#include "MainWindow.h"
#include "ui_MainWindow.h"

Widget::Widget(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::Widget)
{
    ui->setupUi(this);
}

Widget::~Widget()
{
    delete ui;
}

void Widget::on_ReplaceButton_clicked()
{
    QString digitNames[9] = {
        "один", "два", "три", "четыре", "пять",
        "шесть", "семь", "восемь", "девять"
    };

    QString text = ui->sourceEdit->toPlainText();

    //одна цифра от 1 до 9
    QRegularExpression rx("[1-9]");

    int pos = 0;
    QRegularExpressionMatch foundMatch = rx.match(text, pos);

    while (foundMatch.hasMatch())
    {
        QString foundDigit = foundMatch.captured(0); //найденная цифра
        int digitValue = foundDigit.toInt();
        QString replacement = digitNames[digitValue - 1];

        int matchPos = foundMatch.capturedStart(0);

        //замена найденной цифры на её текстовое название
        text.replace(matchPos, foundDigit.length(), replacement);

        //сдвигаем позицию поиска на длину вставленной строки
        pos = matchPos + replacement.length();

        foundMatch = rx.match(text, pos);
    }

    ui->resultEdit->setPlainText(text);
}
