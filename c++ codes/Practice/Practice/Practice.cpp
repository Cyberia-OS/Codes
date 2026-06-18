#include <iostream>
#include <string>
#include <cmath>
using namespace std;

//задание 1, до
void task1_before()
{
    string input;
    cout << "Введите строку: ";
    cin >> input;

    string digits = "";
    for (int i = 0; i < input.length(); i++)
    {
        if (input[i] >= '0' && input[i] <= '9')
            digits += input[i];
    }

    if (digits == "")
    {
        cout << "цифр не найдено" << endl;
        return;
    }

    int num = stoi(digits);
    cout << "число: " << num << endl;
    cout << "квадрат: " << num * num << endl;
}

//задание 1, после рефакторинга
string getDigits(const string& s)
{
    string res;
    for (char c : s)
        if (c >= '0' && c <= '9')
            res += c;
    return res;
}

void task1_after()
{
    string input;
    cout << "введите строку: ";
    cin >> input;

    string digits = getDigits(input);
    if (digits.empty())
    {
        cout << "цифр нет!" << endl;
        return;
    }

    int num = stoi(digits);
    cout << "Число: " << num << "\nКвадрат: " << num * num << endl;
}

//Задание 2, до
void task2_before()
{
    double a, b;
    char op;

    cout << "Введите первое число: ";
    cin >> a;
    cout << "Введите оператор (+, -, *, /, ^): ";
    cin >> op;
    cout << "Введите второе число: ";
    cin >> b;

    double result;
    if (op == '+')
        result = a + b;
    else if (op == '-')
        result = a - b;
    else if (op == '*')
        result = a * b;
    else if (op == '/')
    {
        if (b == 0)
        {
            cout << "ошибка: деление на ноль!" << endl;
            return;
        }
        result = a / b;
    }
    else if (op == '^')
        result = pow(a, b);
    else
    {
        cout << "неизвестный оператор!" << endl;
        return;
    }

    cout << "результат: " << result << endl;
}

//задание 2, после
double calc(double a, double b, char op)
{
    switch (op)
    {
    case '+': 
        return a + b;
    case '-':
        return a - b;
    case '*': 
        return a * b;
    case '/':
        if (b == 0) throw "деление на ноль!";
        return a / b;
    case '^': return pow(a, b);
    default: throw "Неизвестный оператор!";
    }
}

void task2_after()
{
    double a, b;
    char op;
    cout << "a = "; cin >> a;
    cout << "op: "; cin >> op;
    cout << "b = "; cin >> b;

    try
    {
        double res = calc(a, b, op);
        cout << "Результат: " << res << endl;
    }
    catch (const char* msg)
    {
        cout << "Ошибка: " << msg << endl;
    }
}

int main()
{
    setlocale(LC_ALL, "RU");
    int choice;
    do
    {;
        cout << "1. Задание 1 (до рефакторинга)\n";
        cout << "2. Задание 1 (после рефакторинга)\n";
        cout << "3. Задание 2 (до рефакторинга)\n";
        cout << "4. Задание 2 (после рефакторинга)\n";
        cout << "0. Выход\n";
        cout << "Ваш выбор: ";
        cin >> choice;

        switch (choice)
        {
        case 1: task1_before();
            break;
        case 2: task1_after(); 
            break;
        case 3: task2_before(); 
            break;
        case 4: task2_after(); 
            break;
        case 0: cout << "досвидания\n";
            break;
        default:
            cout << "неверный выбор!\n";
        }
    } while (choice != 0);

    return 0;
}