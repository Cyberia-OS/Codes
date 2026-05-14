using System;

class Homework
{
    static void Main(string[] args)
    {
        Console.WriteLine("Домашнее задание — Массивы ");
        Console.WriteLine();

        Task1();
        Console.WriteLine();

        Task2();
        Console.WriteLine();

        Task3();
        Console.WriteLine();

        Task4();
    }

    //задание 1 сжать массив — удалить все 0
    static void Task1()
    {
        Console.WriteLine("Задание 1: Удаление нулей из массива");

        int[] arr = { 1, 0, 3, 0, 0, 5, 7, 0, 9 };

        Console.Write("Исходный массив: ");
        foreach (int num in arr)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();

        int insertPos = 0;

        //сдвигаем ненулевые элементы в начало массива
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != 0)
            {
                arr[insertPos] = arr[i];
                insertPos++;
            }
        }

        for (int i = insertPos; i < arr.Length; i++)
        {
            arr[i] = -1;
        }

        Console.Write("Результат:       ");
        foreach (int num in arr)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
    }

    //задание 2 переставить элементы сначала отрицательные потом положительные 
    static void Task2()
    {
        Console.WriteLine("Задание 2: Отрицательные в начало");

        int[] arr = { 3, -1, 0, -5, 2, -3, 4, 0, -2 };

        Console.Write("Исходный массив: ");
        foreach (int num in arr)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();

        //два вспомогательных массива
        //один для отрицательных другой для положительных
        int[] negative = new int[arr.Length];
        int[] positive = new int[arr.Length];

        int negCount = 0; 
        int posCount = 0; 

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] < 0)
            {
                negative[negCount] = arr[i];
                negCount++;
            }
            else
            {
                positive[posCount] = arr[i];
                posCount++;
            }
        }

        //собираем результат сначала отрицательные потом положительные
        int[] result = new int[arr.Length];
        int index = 0;

        for (int i = 0; i < negCount; i++)
        {
            result[index] = negative[i];
            index++;
        }

        for (int i = 0; i < posCount; i++)
        {
            result[index] = positive[i];
            index++;
        }

        Console.Write("Результат:       ");
        foreach (int num in result)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
    }

    //задание 3 сколько раз введённое пользователем число встречается в массиве

    static void Task3()
    {
        Console.WriteLine("задание 3: Поиск числа в массиве");

        int[] arr = { 3, 1, 4, 1, 5, 9, 2, 6, 1, 3, 3 };

        Console.Write("Массив: ");
        foreach (int num in arr)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();

        Console.Write("Введите число для поиска: ");
        int target = int.Parse(Console.ReadLine());

        int count = 0;
        foreach (int num in arr)
        {
            if (num == target)
            {
                count++;
            }
        }

        Console.WriteLine("Число " + target + " встречается " + count + " раз(а).");
    }

    //задание 4 в двумерном массиве M на N поменять местами два заданных столбца
    static void Task4()
    {
        Console.WriteLine("задание 4 обмен столбцов в 2D массиве");

        int[,] arr = {
            {  1,  2,  3,  4 },
            {  5,  6,  7,  8 },
            {  9, 10, 11, 12 }
        };

        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1); 

        Console.WriteLine("Исходный массив:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(arr[i, j].ToString().PadLeft(4));
            }
            Console.WriteLine();
        }

        Console.Write("Введите номер первого столбца (0-" + (cols - 1) + "): ");
        int col1 = int.Parse(Console.ReadLine());

        Console.Write("Введите номер второго столбца (0-" + (cols - 1) + "): ");
        int col2 = int.Parse(Console.ReadLine());

        //смена столбцов местами
        for (int i = 0; i < rows; i++)
        {
            int temp = arr[i, col1];
            arr[i, col1] = arr[i, col2];
            arr[i, col2] = temp;
        }

        Console.WriteLine("Массив после обмена столбцов " + col1 + " и " + col2 + ":");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(arr[i, j].ToString().PadLeft(4));
            }
            Console.WriteLine();
        }
    }
}
