#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QSet>
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
    void onAnalyzeClicked();

private:
    QString setToString(const QSet<QString>& s);
    Ui::MainWindow *ui;

    QSet<QString> allProducts;
    QList<QSet<QString>> stores;
    QStringList storeNames;
};

#endif // MAINWINDOW_H