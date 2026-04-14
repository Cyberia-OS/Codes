#include <QCoreApplication>
#include <QVector>
#include <QDebug>
#include <QLocale>

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);
    setlocale(LC_ALL, "Russian");

    QTextStream in(stdin);

    qDebug() << "Введите размер массива N:";
    int N;
    in >> N;

    QVector<int> arr(N);
    qDebug() << "Введите" << N << "целых чисел:";
    for (int i = 0; i < N; ++i) {
        in >> arr[i];
    }

    qDebug() << "Элементы с чётными индексами (0,2,4...):";
    for (int i = 0; i < N; i += 2)
        qDebug() << arr[i];
    qDebug() << "";

    qDebug() << "Элементы с нечётными индексами (1,3,5...):";
    for (int i = 1; i < N; i += 2)
        qDebug() << arr[i];

    qDebug() << "\nНажмите Enter для выхода...";
    in.readLine();

    return 0;
}