#include "task_operations.h"
#include <iostream>
#include <algorithm>
#include <iomanip>
#include <windows.h>

using namespace std;

vector<Task> tasks;
int nextId = 1;

Task::Task(int i, string t, int p, string d, string dt, string tm) 
    : id(i), title(t), priority(p), description(d), date(dt), time(tm), completed(false) {
}

void clearInput() {
    cin.clear();
    cin.ignore(1000, '\n');
}

void addTask() {
    cout << "\nДобавление задачи:\n";
    string title, desc, date, time;
    int priority;

    cout << "Название: "; getline(cin, title);
    cout << "Описание: "; getline(cin, desc);
    cout << "Приоритет (1-3): "; cin >> priority; clearInput();
    cout << "Дата (ДД.ММ.ГГГГ): "; getline(cin, date);
    cout << "Время (ЧЧ:ММ): "; getline(cin, time);

    tasks.push_back(Task(nextId++, title, priority, desc, date, time));
    cout << "Задача добавлена! ID: " << tasks.back().id << endl; 
}
void showTasks(bool showAll, string filter) {
    if (tasks.empty()) {
        cout << "Список пуст!\n";
        return;
    }

    cout << "\nСписок задач:\n";
    cout << "ID | Название | Приор. | Дата | Время | Статус\n";
    cout << "---------------------------------------------\n";

    for (auto& t : tasks) {
        if (!showAll && t.date != filter)
            continue;

        string status = t.completed ? "+" : "-";
        string prio = to_string(t.priority);

        cout << setw(2) << t.id << " | "
            << setw(20) << (t.title.length() > 20 ? t.title.substr(0, 17) + "..." : t.title) << " | "
            << setw(4) << prio << " | "
            << setw(10) << t.date << " | "
            << setw(5) << t.time << " | "
            << status << endl;
    }
}

void editTask() {  
    if (tasks.empty()) return;

    cout << "ID для редактирования: ";
    int id; cin >> id; clearInput();

    for (auto& t : tasks) {
        if (t.id == id) {
            cout << "1-Название, 2-Описание, 3-Приоритет, 4-Дата, 5-Время, 6-Статус: ";
            int opt; cin >> opt; clearInput();

            if (opt == 1) {
                cout << "Новое название: ";
                getline(cin, t.title);
            }
            else if (opt == 2) {
                cout << "Новое описание: ";
                getline(cin, t.description);
            }
            else if (opt == 3) {
                cout << "Новый приоритет (1-3): ";
                cin >> t.priority;
                clearInput();
            }
            else if (opt == 4) {
                cout << "Новая дата: ";
                getline(cin, t.date);
            }
            else if (opt == 5) {
                cout << "Новое время: ";
                getline(cin, t.time);
            }
            else if (opt == 6) {
                t.completed = !t.completed;
            }

            cout << "Задача обновлена!\n";
            return;
        }
    }
    cout << "Задача не найдена!\n";
}

void deleteTask() { 
    if (tasks.empty()) return;

    cout << "ID для удаления: ";
    int id; cin >> id; clearInput();

    for (int i = 0; i < tasks.size(); i++) {
        if (tasks[i].id == id) {
            tasks.erase(tasks.begin() + i);
            cout << "Задача удалена!\n";
            return;
        }
    }

    cout << "Задача не найдена!\n";
}

void searchTasks() { 
    if (tasks.empty()) return;

    cout << "1-По названию, 2-По приоритету, 3-По описанию, 4-По дате: ";
    int opt; cin >> opt; clearInput();

    string keyword;
    int prio;
    vector<Task*> results;

    if (opt == 1) {
        cout << "Ключевое слово: "; getline(cin, keyword);
        for (auto& t : tasks)
            if (t.title.find(keyword) != string::npos) results.push_back(&t);
    }
    else if (opt == 2) {
        cout << "Приоритет (1-3): "; cin >> prio; clearInput();
        for (auto& t : tasks)
            if (t.priority == prio) results.push_back(&t);
    }
    else if (opt == 3) {
        cout << "Текст в описании: "; getline(cin, keyword);
        for (auto& t : tasks)
            if (t.description.find(keyword) != string::npos) results.push_back(&t);
    }
    else if (opt == 4) {
        cout << "Дата (ДД.ММ.ГГГГ): "; getline(cin, keyword);
        for (auto& t : tasks)
            if (t.date == keyword) results.push_back(&t);
    }

    if (results.empty()) {
        cout << "Ничего не найдено!\n";
        return;
    }

    cout << "\nНайдено " << results.size() << " задач:\n";
    for (auto t : results) {
        cout << "ID:" << t->id << " \"" << t->title << "\" "
            << t->date << " " << t->time << endl;
    }
}
void sortTasks() {  
    if (tasks.empty()) return;

    cout << "1-По приоритету, 2-По дате: ";
    int opt; cin >> opt; clearInput();

    if (opt == 1) {
        sort(tasks.begin(), tasks.end(), compareByPriority);
        cout << "Отсортировано по приоритету!\n";
    }
    else if (opt == 2) {
        sort(tasks.begin(), tasks.end(), compareByDateTime);
        cout << "Отсортировано по дате!\n";
    }
    showTasks();
}

void filterByTime() { 
    if (tasks.empty()) return;

    cout << "1-На день, 2-На неделю, 3-На месяц: ";
    int opt; cin >> opt; clearInput();

    string filter;
    cout << "Введите дату (ДД.ММ.ГГГГ): ";
    getline(cin, filter);

    cout << "\nЗадачи на выбранный период:\n";
    showTasks(false, filter);
}

bool compareByPriority(Task& a, Task& b) {
    return a.priority < b.priority;
}3

bool compareByDateTime(Task& a, Task& b) {
    if (a.date != b.date) {
        return a.date < b.date;
    }
    return a.time < b.time;
}
