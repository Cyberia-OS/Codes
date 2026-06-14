//рефакторинг
//Стало 
//std::string (безопасно, пробелы)
//std::vector<int> (автоматическая память)
//struct Student с понятными именами: name, grades, average
//calculateAverage() принимает const&, не копирует
//среднее вычисляется один раз и хранится в структуре
//std::sort с лямбдой O(n log n) вместо пузырька
//код разбит на функции: readStudents, printTop, printAll
//проверки: n > 0, отсутствие оценок, топ ограничен реальным числом
//убраны std::accumulate, reserve, std::move 
//они полезны, но для наглядности заменены на простой цикл и прямое добавление.

#include <iostream>
#include <string>
#include <vector>
#include <algorithm> 

const int TOP_COUNT = 3;

struct Student
{
    std::string name;
    std::vector<int> grades;
    double average;
};

// простой подсчёт суммы 
double calculateAverage(const std::vector<int>& grades)
{
    if (grades.empty())
        return 0.0;
    int sum = 0;
    for (int g : grades)
        sum += g;
    return static_cast<double>(sum) / grades.size();
}

std::vector<Student> readStudents()
{
    int n;
    std::cout << "Введите количество студентов: ";
    std::cin >> n;

    if (n <= 0)
    {
        std::cout << "Количество студентов должно быть больше 0.\n";
        return {};
    }

    std::cin.ignore(); //очистить буфер после cin
    std::vector<Student> students;

    for (int i = 0; i < n; ++i)
    {
        Student s;
        std::cout << "Студент " << i + 1 << " — имя: ";
        std::getline(std::cin, s.name);

        int gradeCount;
        std::cout << "Количество оценок: ";
        std::cin >> gradeCount;

        if (gradeCount > 0)
        {
            std::cout << "Введите оценки: ";
            s.grades.resize(gradeCount);
            for (int j = 0; j < gradeCount; ++j)
                std::cin >> s.grades[j];
        }
        else
        {
            std::cout << "Оценок нет — средний балл будет 0.\n";
        }

        s.average = calculateAverage(s.grades);
        std::cin.ignore();
        students.push_back(s);// прямое копирование 
    }
    return students;
}

void sortByAverage(std::vector<Student>& students)
{
    std::sort(students.begin(), students.end(),
        [](const Student& a, const Student& b) {
            return a.average > b.average;
        });
}

void printTop(const std::vector<Student>& students)
{
    int top = std::min(TOP_COUNT, (int)students.size());
    std::cout << "\n=== Топ-" << top << " студентов ===\n";
    for (int i = 0; i < top; ++i)
    {
        std::cout << i + 1 << ". " << students[i].name
            << " — средний балл: " << students[i].average << '\n';
    }
}

void printAll(const std::vector<Student>& students)
{
    std::cout << "\n=== Все студенты (по убыванию балла) ===\n";
    for (size_t i = 0; i < students.size(); ++i)
    {
        std::cout << i + 1 << ". " << students[i].name
            << " — средний балл: " << students[i].average << '\n';
    }
}

int main()
{
    std::vector<Student> students = readStudents();
    if (students.empty())
    {
        std::cout << "Нет данных для обработки.\n";
        return 0;
    }

    sortByAverage(students);
    printTop(students);
    printAll(students);

    return 0;
}