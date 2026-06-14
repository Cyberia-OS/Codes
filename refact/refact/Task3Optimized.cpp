// Задание 3 Оптимизация 
// Задача: вычислить сумму квадратов чётных чисел от 1 до N
// Что оптимизировано:
//1. Устранено дублирование: count и sum вычисляются в одном цикле
//2. Добавлена проверка всех вводимых переменных
//3. Логика разбита на функции 
//4. Цикл стартует сразу с первого чётного числа
//5. Добавлена защита от деления на ноль
//6. Мемоизация: квадрат вычисляется один раз и сохраняется в переменную

#include <iostream>

using namespace std;

// Функция: проверка корректности ввода N
bool isValidInput(int n)
{
    if (n <= 0)
    {
        cout << "Ошибка: N должно быть натуральным числом (больше 0)" << endl;
        return false;
    }
    return true;
}

// Функция вычисляет сумму квадратов чётных чисел от 1 до n
// Оптимизация шаг цикла = 2, нет лишней проверки чётности
long long sumOfEvenSquares(int n)
{
    long long sum = 0;

    for (int i = 2; i <= n; i += 2)
    {
        long long square = (long long)i * i; 
        sum += square;
    }

    return sum;
}

// Функция считает количество чётных чисел от 1 до n 
int countEvenNumbers(int n)
{
    return n / 2;  //Оптимизация простая формула вместо цикла
}

// Функция вычисляет и выводит среднее значение с защитой от деления на ноль
void printAverage(long long sum, int count)
{
    if (count == 0)
    {
        cout << "Нет чётных чисел — среднее не определено." << endl;
        return;
    }
    double average = (double)sum / count;
    cout << "Среднее значение квадратов: " << average << endl;
}

//выводит итоговые результаты
void printResults(int n, long long sum, int count)
{
    cout << "Результаты" << endl;
    cout << "Сумма квадратов чётных чисел от 1 до " << n << " = " << sum << endl;
    cout << "Количество чётных чисел: " << count << endl;
    printAverage(sum, count);
}

int main()
{
    int n;

    cout << "Сумма квадратов чётных чисел" << endl;
    cout << "Введите N (натуральное число): ";
    cin >> n;

    //проверка корректности ввода
    if (!isValidInput(n))
        return 1;

    long long sum = sumOfEvenSquares(n);
    int count = countEvenNumbers(n);

    printResults(n, sum, count);

    return 0;
}