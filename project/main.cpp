#include "task_operations.h"
#include <windows.h>
#include <iostream>

using namespace std;

int main() {
    SetConsoleCP(1251);
    SetConsoleOutputCP(1251);
    setlocale(LC_ALL, "RU");

    tasks.push_back(Task(nextId++, "Купить продукты", 2,
        "Молоко, хлеб, яйца", "20.12.2024", "10:00"));
    tasks.push_back(Task(nextId++, "Сделать ДЗ", 1,
        "Программа на C++", "15.12.2024", "18:00"));
    tasks.push_back(Task(nextId++, "Встреча с врачом", 1,
        "Кабинет 304", "12.12.2024", "14:00"));

    while (true) {
        cout << "\nсписок дел\n";
        cout << "1. Добавить задачу\n";
        cout << "2. Показать все\n";
        cout << "3. Редактировать\n";
        cout << "4. Удалить\n";
        cout << "5. Поиск\n";
        cout << "6. Сортировка\n";
        cout << "7. Фильтр по дате\n";
        cout << "8. Выход\n";
        cout << "Выбор: ";

        int choice; cin >> choice; clearInput();

        if (choice == 1) addTask();
        else if (choice == 2) showTasks();
        else if (choice == 3) editTask();
        else if (choice == 4) deleteTask();
        else if (choice == 5) searchTasks();
        else if (choice == 6) sortTasks();
        else if (choice == 7) filterByTime();
        else if (choice == 8) { cout << "Выход!\n"; break; }
    }
    return 0;
}
