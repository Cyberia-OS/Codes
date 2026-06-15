#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    connect(ui->btnCatalog,     &QPushButton::clicked, this, &MainWindow::openCatalog);
    connect(ui->btnOrder,       &QPushButton::clicked, this, &MainWindow::openOrder);
    connect(ui->btnCatalogBack, &QPushButton::clicked, this, &MainWindow::openMain);
    connect(ui->btnConfirm,     &QPushButton::clicked, this, &MainWindow::openMain);

    ui->pages->setCurrentIndex(0);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::openCatalog()
{
    ui->pages->setCurrentIndex(1);
    setWindowTitle("Каталог товаров");
}

void MainWindow::openOrder()
{
    ui->pages->setCurrentIndex(2);
    setWindowTitle("Заказ");
}

void MainWindow::openMain()
{
    ui->pages->setCurrentIndex(0);
    setWindowTitle("Интернет магазин");
}
