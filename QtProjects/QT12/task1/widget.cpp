#include "widget.h"
#include "ui_widget.h"

Widget::Widget(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::Widget)
{
    ui->setupUi(this);

    //массив пользователей
    fillUsers();

    ui->EnterButton->setEnabled(false);

    QRegularExpression loginExpresion("[a-zA-Z0-9]{3,10}");
    ui->loginEdit->setValidator(new QRegularExpressionValidator(loginExpresion, this));

    //пароль ровно 4 цифры
    QRegularExpression passwordExpresion("[0-9]{4}");
    ui->passwordEdit->setValidator(new QRegularExpressionValidator(passwordExpresion, this));

    ui->passwordEdit->setEchoMode(QLineEdit::Password);

    connect(ui->loginEdit, SIGNAL(textEdited(QString)), this, SLOT(on_loginEdit_textChanged(QString)));
    connect(ui->passwordEdit, SIGNAL(textEdited(QString)), this, SLOT(on_passwordEdit_textChanged(QString)));
}

Widget::~Widget()
{
    delete ui;
}

//заполнение массива из 10 пользователей тестовыми данными
void Widget::fillUsers()
{
    users[0].login = "user1"; users[0].password = "1111";
    users[1].login = "user2"; users[1].password = "2222";
    users[2].login = "user3"; users[2].password = "3333";
    users[3].login = "user4"; users[3].password = "4444";
    users[4].login = "user5"; users[4].password = "5555";
    users[5].login = "user6"; users[5].password = "6666";
    users[6].login = "user7"; users[6].password = "7777";
    users[7].login = "user8"; users[7].password = "8888";
    users[8].login = "user9"; users[8].password = "9999";
    users[9].login = "admin"; users[9].password = "0000";
}

void Widget::on_loginEdit_textChanged(const QString &)
{
    ui->EnterButton->setEnabled(ui->loginEdit->hasAcceptableInput() &&
                                 ui->passwordEdit->hasAcceptableInput());
}

void Widget::on_passwordEdit_textChanged(const QString &)
{
    ui->EnterButton->setEnabled(ui->loginEdit->hasAcceptableInput() &&
                                 ui->passwordEdit->hasAcceptableInput());
}

//проверка введенных данных по массиву
void Widget::on_EnterButton_clicked()
{
    QString login = ui->loginEdit->text();
    QString password = ui->passwordEdit->text();

    bool found = false;

    for (int i = 0; i < 10; i++)
    {
        if (users[i].login == login && users[i].password == password)
        {
            found = true;
            break;
        }
    }

    if (found)
        QMessageBox::information(this, "Сообщение", "Доступ разрешен");
    else
        QMessageBox::warning(this, "Сообщение", "Неверный логин или пароль");
}
