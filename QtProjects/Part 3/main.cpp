#include <QCoreApplication>
#include <list>
#include <iostream>
#include <algorithm>

using namespace std;

void printList(const list<int>& l) {
    // Использование итератора
    list<int>::const_iterator it;
    for (it = l.begin(); it != l.end(); ++it) {
        cout << *it << " ";
    }
    cout << endl;
}

int main(int argc, char *argv[]) {
    QCoreApplication a(argc, argv);
    setlocale(LC_ALL, "Russian");

    // 1. Создаем два связных списка
    list<int> listA = {1, 2, 3, 4, 5};
    list<int> listB = {10, 20, 30};

    cout << "Список А до замены: ";
    printList(listA);
    cout << "Список Б: ";
    printList(listB);

    // 2. Реализуем замену (копирование listB в listA)
    cout << "\n--- Выполняется замена списка А на Б ---\n";
    listA = listB;

    cout << "Список А после замены: ";
    printList(listA);

    cout << "\nНажмите Enter, чтобы выйти...";
    cin.get();
    return 0;
}
