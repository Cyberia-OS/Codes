#include <QCoreApplication>
#include <QList>
#include <QDebug>
#include <cstdlib>
#include <ctime>

//вывод списка через итераторы
void printList(const QString &name, QList<int> &list)
{
    QString str = "";
    QList<int>::iterator it = list.begin();
    while (it != list.end()) {
        str += QString::number(*it) + " ";
        it++;
    }
    qDebug().noquote() << name + ": " + str;
}

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    srand(time(0));

    QList<int> list1, list2;

    for (int i = 0; i < 5; i++) {
        list1.append(rand() % 50);
    }

    //случайные числа от -25 до 24
    for (int i = 0; i < 5; i++) {
        list2.append(rand() % 50 - 25);
    }

    qDebug() << "до замены";
    printList("Список 1", list1);
    printList("Список 2", list2);

    list1 = list2;

    qDebug() << "после замены Списка 1 на Список 2";
    printList("Список 1", list1);
    printList("Список 2", list2);

    return 0;
}
