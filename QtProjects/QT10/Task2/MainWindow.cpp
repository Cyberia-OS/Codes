#include "MainWindow.h"
#include "ui_MainWindow.h"
#include <QPainter>
#include <QTime>

Widget::Widget(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::Widget)
    , timer(new QTimer(this))
{
    ui->setupUi(this);
    setWindowTitle("Задание 2 - Секундная стрелка");
    connect(timer, &QTimer::timeout, this, &Widget::updateClock);
}

Widget::~Widget()
{
    delete ui;
}

void Widget::on_btnStart_clicked()
{
    timer->start(1000);
}

void Widget::on_btnStop_clicked()
{
    timer->stop();
}

void Widget::updateClock()
{
    update();
}

void Widget::paintEvent(QPaintEvent *)
{
    //циферблат
    const QPoint center(180, 180);
    const int radius = 150;
    const int markOffset = 15;
    const int handOffset = 20;

    QPainter painter(this);
    painter.setRenderHint(QPainter::Antialiasing);

    //окружность
    painter.setPen(QPen(Qt::black, 3));
    painter.drawEllipse(center, radius, radius);

    painter.save();
    painter.translate(center);
    painter.setPen(QPen(Qt::black, 5));
    for (int i = 0; i < 12; ++i) {
        painter.drawPoint(0, -(radius - markOffset));
        painter.rotate(30);
    }
    painter.restore();

    //сек стрелка
    QTime now = QTime::currentTime();
    int seconds = now.second();

    painter.save();
    painter.translate(center);
    painter.rotate(seconds * 6.0);
    painter.setPen(QPen(Qt::red, 2));
    painter.drawLine(0, 0, 0, -(radius - handOffset));
    painter.restore();

    //центральная точка
    painter.setPen(QPen(Qt::black, 8));
    painter.drawPoint(center);

    painter.setPen(Qt::black);
    painter.setFont(QFont("Arial", 14));
    painter.drawText(center.x() - 40, center.y() + radius + 25, now.toString("hh:mm:ss"));
}