#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QMessageBox>
#include <QDateTime>

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
    , totalPrice(0.0)
{
    ui->setupUi(this);

    fillMenuData();

    ui->comboCategory->addItems(menu.keys());

    updateMenuList();
    updateOrderList();

    ui->dateTimeEdit->setDateTime(QDateTime::currentDateTime());
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::fillMenuData()
{
    menu["Закуски"] = {
        {"Брускетта с томатами", 250.0},
        {"Сырная тарелка", 450.0},
        {"Карпаччо из говядины", 380.0}
    };

    menu["Супы"] = {
        {"Борщ", 280.0},
        {"Крем-суп из тыквы", 260.0},
        {"Уха", 320.0}
    };

    menu["Горячие блюда"] = {
        {"Стейк из лосося", 650.0},
        {"Паста Карбонара", 420.0},
        {"Куриное филе гриль", 380.0},
        {"Бифштекс с овощами", 590.0}
    };

    menu["Десерты"] = {
        {"Тирамису", 280.0},
        {"Чизкейк", 260.0},
        {"Шоколадный фондан", 320.0}
    };

    menu["Напитки"] = {
        {"Чай", 120.0},
        {"Кофе", 150.0},
        {"Морс", 130.0},
        {"Лимонад", 140.0}
    };
}

void MainWindow::updateMenuList()
{
    ui->listMenu->clear();

    QString category = ui->comboCategory->currentText();
    if (!menu.contains(category))
        return;

    const auto &dishes = menu[category];
    for (const auto &dish : dishes) {
        QString text = QString("%1 - %2 руб.").arg(dish.first).arg(dish.second, 0, 'f', 0);
        ui->listMenu->addItem(text);
    }
}

//обновляет список заказанных блюд и итоговую сумму
void MainWindow::updateOrderList()
{
    ui->listOrder->clear();
    totalPrice = 0.0;

    for (const auto &item : orderItems) {
        QString text = QString("%1 - %2 руб.").arg(item.first).arg(item.second, 0, 'f', 0);
        ui->listOrder->addItem(text);
        totalPrice += item.second;
    }

    ui->labelTotal->setText(QString("Итого: %1 руб.").arg(totalPrice, 0, 'f', 0));
}

//смена категории в выпадающем списке
void MainWindow::on_comboCategory_currentIndexChanged(int index)
{
    Q_UNUSED(index);
    updateMenuList();
}

//добавление выбранного блюда из меню в заказ
void MainWindow::on_btnAddDish_clicked()
{
    int row = ui->listMenu->currentRow();
    if (row < 0) {
        QMessageBox::information(this, "Внимание", "Выберите блюдо из меню.");
        return;
    }

    QString category = ui->comboCategory->currentText();
    QPair<QString, double> dish = menu[category][row];

    orderItems.append(dish);
    updateOrderList();
}

//удаление выбранного блюда из заказа
void MainWindow::on_btnRemoveDish_clicked()
{
    int row = ui->listOrder->currentRow();
    if (row < 0) {
        QMessageBox::information(this, "Внимание", "Выберите блюдо в заказе для удаления.");
        return;
    }

    orderItems.removeAt(row);
    updateOrderList();
}

//оформление заказа и бронирование столика
void MainWindow::on_btnPlaceOrder_clicked()
{
    QString name = ui->editName->text().trimmed();
    int guests = ui->spinGuests->value();
    QDateTime dateTime = ui->dateTimeEdit->dateTime();

    if (name.isEmpty()) {
        QMessageBox::warning(this, "Ошибка", "Введите имя для бронирования столика.");
        return;
    }

    if (orderItems.isEmpty()) {
        QMessageBox::warning(this, "Ошибка", "Добавьте хотя бы одно блюдо в заказ.");
        return;
    }

    QString message;
    message += QString("Имя: %1\n").arg(name);
    message += QString("Количество гостей: %1\n").arg(guests);
    message += QString("Дата и время: %1\n\n").arg(dateTime.toString("dd.MM.yyyy hh:mm"));
    message += "Заказанные блюда:\n";

    for (const auto &item : orderItems) {
        message += QString("- %1 (%2 руб.)\n").arg(item.first).arg(item.second, 0, 'f', 0);
    }

    message += QString("\nИтоговая стоимость: %1 руб.").arg(totalPrice, 0, 'f', 0);

    QMessageBox::information(this, "Столик забронирован", message);

    orderItems.clear();
    updateOrderList();
    ui->editName->clear();
    ui->spinGuests->setValue(1);
}
