#include <iostream>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;

// Структура задачи
struct Task
{
    string description;
    int priority;
    bool isCompleted;
};

void inputTask(Task& task)
{
    cout << "Описание задачи: ";
    cin.ignore();
    getline(cin, task.description);

    while (true)
    {
        cout << "Приоритет (1-5): ";
        cin >> task.priority;
        if (task.priority >= 1 && task.priority <= 5)
            break;
        cout << "Ошибка! Введите число от 1 до 5.\n";
        cin.clear();
        cin.ignore(10000, '\n');
    }
    task.isCompleted = false;
}

//Ввод всех задач
vector<Task> readTasks()
{
    int count;
    cout << "Количество задач: ";
    cin >> count;

    if (count <= 0)
    {
        cout << "Количество должно быть больше 0.\n";
        return {};
    }

    vector<Task> tasks;
    for (int i = 0; i < count; ++i)
    {
        cout << "\nЗадача " << i + 1 << ":\n";
        Task t;
        inputTask(t);
        tasks.push_back(t);
    }
    return tasks;
}

//Функция сравнения для сортировки по убыванию приоритета
bool compareByPriority(const Task& a, const Task& b)
{
    return a.priority > b.priority;
}

//Вывод топ-N задач
void printTopTasks(const vector<Task>& tasks, int topCount)
{
    if (tasks.empty())
    {
        cout << "Нет задач\n";
        return;
    }

    int limit = (topCount > (int)tasks.size()) ? (int)tasks.size() : topCount;

    cout << "\nТоп-" << limit << " задач\n";
    for (int i = 0; i < limit; ++i)
    {
        cout << i + 1 << ". "
            << tasks[i].description
            << " (приоритет " << tasks[i].priority << ")\n";
    }
}

int main()
{
    setlocale(LC_ALL, "RU");
    vector<Task> tasks = readTasks();
    if (tasks.empty())
        return 0;

    sort(tasks.begin(), tasks.end(), compareByPriority);
    printTopTasks(tasks, 3);
    return 0;
}