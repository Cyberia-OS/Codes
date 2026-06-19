#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "glwidget.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    connect(ui->slAmplitude, SIGNAL(valueChanged(int)), ui->widget, SLOT(changeAmplitude(int)));
    connect(ui->slFrequency, SIGNAL(valueChanged(int)), ui->widget, SLOT(changeFrequency(int)));
}

MainWindow::~MainWindow()
{
    delete ui;
}
