#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QMap>
#include <QVector>
#include <QPair>
#include <QString>

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void on_comboCategory_currentIndexChanged(int index);
    void on_btnAddDish_clicked();
    void on_btnRemoveDish_clicked();
    void on_btnPlaceOrder_clicked();

private:
    Ui::MainWindow *ui;

    QMap<QString, QVector<QPair<QString, double>>> menu;

    QVector<QPair<QString, double>> orderItems;

    double totalPrice;

    void fillMenuData();
    void updateMenuList();
    void updateOrderList();
};
#endif // MAINWINDOW_H
