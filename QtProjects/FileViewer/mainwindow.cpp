#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QFileDialog>
#include <QFile>
#include <QFileInfo>
#include <QTextStream>
#include <QMessageBox>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    this->setWindowTitle("Просмотр текстовых файлов");
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_btnOpen_clicked()
{
    QString fileName = QFileDialog::getOpenFileName(
        this,
        "Выберите текстовый файл",
        "",
        "Текстовые файлы (*.txt);;Все файлы (*.*)"
    );

    if (fileName.isEmpty())
        return;

    //чтение
    QFile file(fileName);
    if (!file.open(QIODevice::ReadOnly | QIODevice::Text)) {
        QMessageBox::warning(this, "Ошибка", "Не удалось открыть файл!");
        return;
    }

    QTextStream in(&file);
    QString content = in.readAll();
    file.close();

    ui->textContent->setPlainText(content);

    //получение информации о файле через QFileInfo
    QFileInfo info(fileName);

    QString details;
    details += "Имя файла: " + info.fileName() + "\n";
    details += "Путь: " + info.absoluteFilePath() + "\n";
    details += "Размер: " + QString::number(info.size()) + " байт\n";
    details += "Дата создания: " + info.birthTime().toString("dd.MM.yyyy hh:mm:ss") + "\n";
    details += "Дата изменения: " + info.lastModified().toString("dd.MM.yyyy hh:mm:ss") + "\n";

    //тип файла
    details += "Только для чтения: " + QString(info.isWritable() ? "Нет" : "Да") + "\n";
    details += "Скрытый файл: " + QString(info.isHidden() ? "Да" : "Нет") + "\n";

    ui->textInfo->setPlainText(details);
}
