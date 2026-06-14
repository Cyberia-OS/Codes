//Задание 3  Оптимизация
//Задача вычислить сумму квадратов чётных чисел от 1 до N
//Используются только элементарные конструкции циклы и тд
//без функций для демонстрации до оптимизации

#include <iostream>

using namespace std;

int main()
{
    int n;
    cout << "Введите N : ";
    cin >> n;

    // Нет проверки: является ли n положительным числом

    long long sum = 0;
    int i = 1;

    while (i <= n)
    {
        if (i % 2 == 0)
        {
            long long square = i * i;
            sum = sum + square;
        }
        i = i + 1;  //до оптимизации: i + 1 вместо i++
    }

    cout << "Сумма квадратов чётных чисел от 1 до " << n << " = " << sum << endl;

    //Повторное вычисление количества чётных чисел 
    int count = 0;
    int j = 1;
    while (j <= n)
    {
        if (j % 2 == 0)
        {
            count = count + 1;  //До оптимизации: count + 1 вместо count++
        }
        j = j + 1;
    }

    cout << "Количество чётных чисел: " << count << endl;
    cout << "Среднее значение квадратов: ";

    //Нет защиты от деления на ноль 
    double average = (double)sum / count;
    cout << average << endl;

    return 0;
}