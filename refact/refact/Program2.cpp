//задание 1 - Программа 2 до рефакторинга
//описание: Простой калькулятор 
//с возведением в степень и умножением отрицательных чисел

#include <iostream>

using namespace std;

int main()
{
    double a, b;
    int choice;

    cout << "Введите первое число: ";
    cin >> a;
    cout << "Введите второе число: ";
    cin >> b;

    cout << "Выберите операцию:" << endl;
    cout << "1 - Сложение" << endl;
    cout << "2 - Вычитание" << endl;
    cout << "3 - Умножение" << endl;
    cout << "4 - Деление" << endl;
    cout << "5 - Возведение первого числа в степень второго" << endl;
    cout << "Ваш выбор: ";
    cin >> choice;

    if (choice == 1)
    {
        cout << "Результат: " << a + b << endl;
    }
    else if (choice == 2)
    {
        cout << "Результат: " << a - b << endl;
    }
    else if (choice == 3)
    {
        double result = a * b;

        cout << "Результат: " << result << endl;

        if (a < 0 && b < 0)
            cout << "Оба числа отрицательные, результат положительный" << endl;
        else if (a < 0 || b < 0)
            cout << "Одно из чисел отрицательное, результат отрицательный" << endl;
    }

    else if (choice == 4)
    {
        if (b == 0)
        {
            cout << "Ошибка: деление на ноль!" << endl;
        }
        else
        {
            cout << "Результат: " << a / b << endl;
        }
    }
    else if (choice == 5)
    {
        double result = 1;
        int power = (int)b;
        if (power >= 0)
        {
            for (int i = 0; i < power; i++)
            {
                result = result * a;
            }
            cout << a << " в степени " << power << " = " << result << endl;
        }
        else
        {
            for (int i = 0; i < -power; i++)
            {
                result = result * a;
            }
            cout << a << " в степени " << power << " = " << 1.0 / result << endl;
        }
    }
    else
    {
        cout << "Неверный выбор операции" << endl;
    }

    return 0;
}