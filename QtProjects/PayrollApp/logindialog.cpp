#include "logindialog.h"
#include "ui_logindialog.h"

namespace {
constexpr const char* VALID_LOGIN    = "admin";
constexpr const char* VALID_PASSWORD = "1234";
}

LoginDialog::LoginDialog(QWidget *parent)
    : QDialog(parent), ui(new Ui::LoginDialog)
{
    ui->setupUi(this);

    setWindowIcon(QIcon(":/app_icon.png"));
    setFixedSize(500, 500);

    connect(ui->loginButton, &QPushButton::clicked, this, [this]() {
        onLoginClicked();
    });
}

LoginDialog::~LoginDialog()
{
    delete ui;
}

void LoginDialog::onLoginClicked()
{
    const QString login    = ui->loginEdit->text().trimmed();
    const QString password = ui->passwordEdit->text();

    if (login == QLatin1String(VALID_LOGIN) &&
        password == QLatin1String(VALID_PASSWORD))
    {
        accept();
    }
    else
    {
        ui->errorLabel->setVisible(true);
        ui->passwordEdit->clear();
        ui->passwordEdit->setFocus();
    }
}
