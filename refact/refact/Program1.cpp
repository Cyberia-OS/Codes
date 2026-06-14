//задание 1 Программа 1 до рефакторинга
//описание Программа считывает ввод пользователя и определяет:
// является ли ввод числом или буквой
// положительное или отрицательное число
// выводит квадрат числа

#include <iostream>
#include <string>

using namespace std;

int main()
{
    string input;
    cout << "Введите значение: ";
    cin >> input;

    //есть ли цифры в строке
    bool hasDigit = false;
    bool hasLetter = false;
    bool isNegative = false;
    string numStr = "";

    for (int i = 0; i < input.length(); i++)
    {
        if (input[i] == '-' && i == 0)
        {
            isNegative = true;
        }
        else if (input[i] >= '0' && input[i] <= '9')
        {
            hasDigit = true;
            numStr += input[i];
        }
        else
        {
            hasLetter = true;
        }
    }

    if (hasDigit && hasLetter)
    {
        int num = 0;
        for (int i = 0; i < numStr.length(); i++)
        {
            num = num * 10 + (numStr[i] - '0');
        }
        if (isNegative) num = -num;
        cout << "Вы ввели: " << num << " (только номер)" << endl;
        cout << "Квадрат числа: " << num * num << endl;
    }
    else if (hasDigit && !hasLetter)
    {
        int num = 0;

        for (int i = 0; i < numStr.length(); i++)
        {
            num = num * 10 + (numStr[i] - '0');
        }
        if (isNegative) num = -num;
        if (num > 0)
            cout << "Вы ввели положительное число: " << num << endl;
        else if (num < 0)
            cout << "Вы ввели отрицательное число: " << num << endl;
        else
            cout << "Вы ввели ноль: " << num << endl;
        cout << "Квадрат числа: " << num * num << endl;
    }
    else
    {
        cout << "Вы ввели буквы/символы (не число)" << endl;
    }

    return 0;
}