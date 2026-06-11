#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QStringList>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    allProducts = {"Хлеб", "Молоко", "Масло", "Сыр", "Яйца",
                   "Кефир", "Сметана", "Йогурт", "Творог", "Колбаса"};

    storeNames = {"Магазин 1", "Магазин 2", "Магазин 3"};

    QSet<QString> store1 = {"Хлеб", "Молоко", "Масло", "Сыр", "Яйца", "Кефир"};
    QSet<QString> store2 = {"Хлеб", "Молоко", "Сыр", "Йогурт", "Колбаса"};
    QSet<QString> store3 = {"Хлеб", "Молоко", "Масло", "Сметана", "Творог"};

    stores = {store1, store2, store3};

    QString storesInfo;
    for (int i = 0; i < stores.size(); i++) {
        QStringList items = stores[i].values();
        items.sort();
        storesInfo += storeNames[i] + ": " + items.join(", ") + "\n";
    }
    ui->textEditStores->setText(storesInfo);

    connect(ui->btnAnalyze, &QPushButton::clicked, this, &MainWindow::onAnalyzeClicked);
}

MainWindow::~MainWindow()
{
    delete ui;
}

//множество QSet в строку для вывода
QString MainWindow::setToString(const QSet<QString>& s)
{
    QStringList list = s.values();
    list.sort();
    if (list.isEmpty()) {
        return "(пусто)";
    }
    return list.join(", ");
}

void MainWindow::onAnalyzeClicked()
{
    //товары во всех магазинах пересечение всех множеств
    QSet<QString> inAll = stores[0];
    for (int i = 1; i < stores.size(); i++) {
        inAll.intersect(stores[i]);
    }

    //товары в одном объединение всех множеств
    QSet<QString> inAny;
    for (int i = 0; i < stores.size(); i++) {
        inAny.unite(stores[i]);
    }

    //Товаров нет ни в одном все товары минус объединение
    QSet<QString> inNone = allProducts;
    inNone.subtract(inAny);

    ui->textEditInAll->setText(setToString(inAll));
    ui->textEditInAny->setText(setToString(inAny));
    ui->textEditInNone->setText(setToString(inNone));
}