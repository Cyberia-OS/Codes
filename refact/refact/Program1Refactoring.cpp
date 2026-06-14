//задание 2 Программа 1 после рефакторинга
//применённые методы рефакторинга:
//Выделение метода логика разбора строки вынесена в функции
//Выделение локальной переменной понятные имена переменных
//странение дублирования кода 

#include <iostream>
#include <string>

using namespace std;

//функция проверяет является ли символ цифрой
bool isDigitChar(char c)
{
    return (c >= '0' && c <= '9');
}

//фуункция проверяет является ли символ буквой
bool isLetterChar(char c)
{
    return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
}

//функция извлекает целое число из строки 
int extractNumber(const string& input, bool& isNegative)
{
    string digitStr = "";
    isNegative = false;

    for (int i = 0; i < (int)input.length(); i++)
    {
        if (input[i] == '-' && i == 0)
            isNegative = true;
        else if (isDigitChar(input[i]))
            digitStr += input[i];
    }

    int number = 0;
    for (int i = 0; i < (int)digitStr.length(); i++)
        number = number * 10 + (digitStr[i] - '0');

    if (isNegative)
        number = -number;

    return number;
}

//функция определяет наличие цифр и букв в строке
void analyzeInput(const string& input, bool& hasDigit, bool& hasLetter)
{
    hasDigit = false;
    hasLetter = false;

    for (int i = 0; i < (int)input.length(); i++)
    {
        if (isDigitChar(input[i]))
            hasDigit = true;
        else if (isLetterChar(input[i]))
            hasLetter = true;
    }
}

//функция выводит информацию о числе и его квадрат
void printNumberInfo(int number)
{
    if (number > 0)
        cout << "Вы ввели положительное число: " << number << endl;
    else if (number < 0)
        cout << "Вы ввели отрицательное число: " << number << endl;
    else
        cout << "Вы ввели ноль." << endl;

    cout << "Квадрат числа: " << number * number << endl;
}

int main()
{
    string input;
    cout << "Введите значение: ";
    cin >> input;

    bool hasDigit, hasLetter, isNegative;
    analyzeInput(input, hasDigit, hasLetter);

    if (!hasDigit && !hasLetter)
    {
        cout << "Вы ввели специальный символ." << endl;
        return 0;
    }

    if (!hasDigit)
    {
        cout << "Вы ввели буквы (не число)." << endl;
        return 0;
    }

    int number = extractNumber(input, isNegative);

    if (hasLetter)
        cout << "Вы ввели: " << number << " (только номер)" << endl;

    printNumberInfo(number);

    return 0;
}