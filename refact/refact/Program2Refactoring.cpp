// Задание 2 программа 2 после рефакторинга
// Применённые методы рефакторинга
// Выделение метода каждая операция  отдельная функция
// Замена условного оператора switch вместо цепочки if-else
// Выделение локальной переменной: понятные имена
// Устранение дублирования цикл возведения в степень вынесен в функцию

#include <iostream>

using namespace std;

//Функция: сложение
double add(double a, double b)
{
    return a + b;
}

//Функция: вычитание
double subtract(double a, double b)
{
    return a - b;
}

//функция: умножение (работает и с отрицательными числами)
double multiply(double a, double b)
{
    return a * b;
}

//Функция: деление с проверкой на ноль
bool divide(double a, double b, double& result)
{
    if (b == 0)
        return false;
    result = a / b;
    return true;
}

//Функция: возведение числа в целую степень (положительную и отрицательную)
double power(double base, int exponent)
{
    double result = 1.0;
    int absExp = exponent;
    if (absExp < 0) absExp = -absExp;

    for (int i = 0; i < absExp; i++)
        result *= base;

    if (exponent < 0)
        result = 1.0 / result;

    return result;
}

//Функция: вывод комментария об умножении отрицательных чисел
void printMultiplyComment(double a, double b)
{
    if (a < 0 && b < 0)
        cout << "Оба числа отрицательные — результат положительный." << endl;
    else if (a < 0 || b < 0)
        cout << "Одно из чисел отрицательное — результат отрицательный." << endl;
}

//Функция: вывод меню операций
void printMenu()
{
    cout << endl << "Выберите операцию:" << endl;
    cout << "1 - Сложение (+)" << endl;
    cout << "2 - Вычитание (-)" << endl;
    cout << "3 - Умножение (*)" << endl;
    cout << "4 - Деление (/)" << endl;
    cout << "5 - Возведение в степень (a^b)" << endl;
    cout << "Ваш выбор: ";
}

int main()
{
    double a, b;
    int choice;
    double result;

    cout << "Калькулятор" << endl;
    cout << "Введите первое число: ";
    cin >> a;
    cout << "Введите второе число: ";
    cin >> b;

    printMenu();
    cin >> choice;

    switch (choice)
    {
    case 1:
        cout << "Результат: " << add(a, b) << endl;
        break;

    case 2:
        cout << "Результат: " << subtract(a, b) << endl;
        break;

    case 3:
        result = multiply(a, b);
        cout << "Результат: " << result << endl;
        printMultiplyComment(a, b);
        break;

    case 4:
        if (!divide(a, b, result))
            cout << "Ошибка: деление на ноль!" << endl;
        else
            cout << "Результат: " << result << endl;
        break;

    case 5:
        result = power(a, (int)b);
        cout << a << " ^ " << (int)b << " = " << result << endl;
        break;

    default:
        cout << "Неверный выбор операции!" << endl;
        break;
    }

    return 0;
}