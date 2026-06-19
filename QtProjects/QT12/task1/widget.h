#ifndef WIDGET_H
#define WIDGET_H

#include <QWidget>
#include <QString>
#include <QMessageBox>
#include <QRegularExpression>
#include <QRegularExpressionValidator>

namespace Ui {
class Widget;
}

//структура для хранения данных одного пользователя
struct User
{
    QString login;
    QString password;
};

class Widget : public QWidget
{
    Q_OBJECT

public:
    explicit Widget(QWidget *parent = 0);
    ~Widget();

private:
    Ui::Widget *ui;
    User users[10];
    void fillUsers();

private slots:
    void on_loginEdit_textChanged(const QString &arg1);
    void on_passwordEdit_textChanged(const QString &arg1);
    void on_EnterButton_clicked();
};

#endif // WIDGET_H
