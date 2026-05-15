using System;

namespace Homework4
{ 
class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1 - Чётные, нечётные, уникальные");
            Console.WriteLine("2 - Меньше заданного параметра");
            Console.WriteLine("3 - Поиск последовательности из 3 чисел");
            Console.WriteLine("4 - Минимум и максимум в двумерном массиве");
            Console.WriteLine("5 - Количество слов в предложении");
            Console.WriteLine("6 - Перевернуть каждое слово");
            Console.WriteLine("7 - Количество гласных букв");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            if (choice == "1")
                Task1();
            else if (choice == "2")
                Task2();
            else if (choice == "3")
                Task3();
            else if (choice == "4")
                Task4();
            else if (choice == "5")
                Task5();
            else if (choice == "6")
                Task6();
            else if (choice == "7")
                Task7();
            else if (choice == "0")
                break;
            else
                Console.WriteLine("Неверный ввод");
        }
    }

    //Задание 1 Чётные, нечётные, уникальные элементы
    static void Task1()
    {
        int[] arr = { 1, 2, 3, 2, 4, 5, 4, 6, 7, 6 };

        Console.Write("Массив: ");
        PrintArr(arr);

        int evenCount = 0;
        int oddCount = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] % 2 == 0)
                evenCount++;
            else
                oddCount++;
        }

        int uniqueCount = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            bool isUnique = true;
            for (int j = 0; j < arr.Length; j++)
            {
                if (i != j && arr[i] == arr[j])
                {
                    isUnique = false;
                    break;
                }
            }
            if (isUnique)
                uniqueCount++;
        }

        Console.WriteLine("Чётных:    " + evenCount);
        Console.WriteLine("Нечётных:  " + oddCount);
        Console.WriteLine("Уникальных: " + uniqueCount);
    }

    //задание 2 Количество значений меньше заданного числа
    static void Task2()
    {
        int[] arr = { 3, 7, 1, 9, 4, 7, 2, 8, 5 };

        Console.Write("Массив: ");
        PrintArr(arr);

        Console.Write("Введите число: ");
        int target = int.Parse(Console.ReadLine());

        int count = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] < target)
                count++;
        }

        Console.WriteLine("Значений меньше " + target + ": " + count);
    }

    //задание 3 Сколько раз встречается последовательность из трёх введённых чисел

    static void Task3()
    {
        int[] arr = { 7, 6, 5, 3, 4, 7, 6, 5, 8, 7, 6, 5 };

        Console.Write("Массив: ");
        PrintArr(arr);

        Console.Write("Введите первое число: ");
        int a = int.Parse(Console.ReadLine());

        Console.Write("Введите второе число: ");
        int b = int.Parse(Console.ReadLine());

        Console.Write("Введите третье число: ");
        int c = int.Parse(Console.ReadLine());

        int count = 0;

        for (int i = 0; i < arr.Length - 2; i++)
        {
            if (arr[i] == a && arr[i + 1] == b && arr[i + 2] == c)
                count++;
        }

        Console.WriteLine("Последовательность " + a + " " + b + " " + c + " встречается: " + count + " раз(а)");
    }

    //задание 4: Минимум и максимум в двумерном массиве
    static void Task4()
    {
        int[,] arr = {
            {  5, 12,  3 },
            { 18,  7,  1 },
            {  9,  4, 15 }
        };

        Console.WriteLine("Двумерный массив:");
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
                Console.Write(arr[i, j] + " ");
            Console.WriteLine();
        }

        int min = arr[0, 0];
        int max = arr[0, 0];

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                if (arr[i, j] < min)
                    min = arr[i, j];

                if (arr[i, j] > max)
                    max = arr[i, j];
            }
        }

        Console.WriteLine("Минимум:  " + min);
        Console.WriteLine("Максимум: " + max);
    }

    //задание 5 количество слов в предложении
    static void Task5()
    {
        Console.Write("Введите предложение: ");
        string sentence = Console.ReadLine();

        string[] words = sentence.Split(' ');

        int count = 0;
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i] != "")
                count++;
        }

        Console.WriteLine("Количество слов: " + count);
    }

    //задание 6 Перевернуть каждое слово в предложении
    static void Task6()
    {
        Console.Write("Введите предложение: ");
        string sentence = Console.ReadLine();

        string[] words = sentence.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            char[] letters = words[i].ToCharArray();

            int left = 0;
            int right = letters.Length - 1;

            while (left < right)
            {
                char temp = letters[left];
                letters[left] = letters[right];
                letters[right] = temp;
                left++;
                right--;
            }

            words[i] = new string(letters);
        }

        string result = "";
        for (int i = 0; i < words.Length; i++)
        {
            if (i == 0)
                result = words[i];
            else
                result = result + " " + words[i];
        }

        Console.WriteLine("После переворота: " + result);
    }

    //задание 7 Количество гласных букв в предложении
    static void Task7()
    {
        Console.Write("Введите предложение: ");
        string sentence = Console.ReadLine();

        string vowels = "аеёиоуыэюяАЕЁИОУЫЭЮЯaeiouAEIOU";

        int count = 0;

        for (int i = 0; i < sentence.Length; i++)
        {
            for (int j = 0; j < vowels.Length; j++)
            {
                if (sentence[i] == vowels[j])
                {
                    count++;
                    break;
                }
            }
        }

        Console.WriteLine("Количество гласных: " + count);
    }

    static void PrintArr(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
            Console.Write(arr[i] + " ");
        Console.WriteLine();
    }
}
}