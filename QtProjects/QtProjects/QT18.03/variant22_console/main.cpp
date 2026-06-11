#include <QCoreApplication>
#include <QVector>
#include <QTextStream>
#include <QDebug>

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    QTextStream in(stdin);

    qDebug() << "Введите размер массива N:";
    int n = in.readLine().toInt();

    QVector<int> vec;
    qDebug() << "Введите" << n << "целых чисел:";
    for (int i = 0; i < n; i++) {
        int num = in.readLine().toInt();
        vec.append(num);
    }

    qDebug() << "Исходный массив:" << vec;

    QString evenStr = "Элементы с чётными индексами: ";
    for (int i = 0; i < vec.size(); i += 2) {
        evenStr += QString::number(vec[i]) + " ";
    }
    qDebug().noquote() << evenStr;

    QString oddStr = "Элементы с нечётными индексами: ";
    for (int i = 1; i < vec.size(); i += 2) {
        oddStr += QString::number(vec[i]) + " ";
    }
    qDebug().noquote() << oddStr;

    return 0;
}
