#ifndef LOGINDIALOG_H
#define LOGINDIALOG_H

#include <QDialog>

namespace Ui { class LoginDialog; }

class LoginDialog : public QDialog
{
    Q_OBJECT

public:
    explicit LoginDialog(QWidget *parent = nullptr);
    ~LoginDialog();

private slots:
    void onLoginClicked();

private:
    Ui::LoginDialog *ui;
    static constexpr const char* VALID_LOGIN    = "admin";
    static constexpr const char* VALID_PASSWORD = "1234";
};

#endif // LOGINDIALOG_H
