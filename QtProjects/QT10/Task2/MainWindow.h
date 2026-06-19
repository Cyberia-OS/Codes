#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QWidget>
#include <QTimer>

QT_BEGIN_NAMESPACE
namespace Ui { class Widget; }
QT_END_NAMESPACE

class Widget : public QWidget
{
    Q_OBJECT

public:
    Widget(QWidget *parent = nullptr);
    ~Widget();

private slots:
    void on_btnStart_clicked();
    void on_btnStop_clicked();
    void updateClock();

protected:
    void paintEvent(QPaintEvent *event) override;

private:
    Ui::Widget *ui;
    QTimer *timer;
};

#endif // MAINWINDOW_H
