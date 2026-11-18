using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("выберите задание (1-5):");
        Console.WriteLine("1. текущее время и дата тремя способами (task)");
        Console.WriteLine("2. простые числа от 0 до 1000");
        Console.WriteLine("3. простые числа в заданных границах и их количество");
        Console.WriteLine("4. минимум, максимум, среднее и сумма в массиве");
        Console.WriteLine("5. удаление дублей, сортировка и бинарный поиск (continuation tasks)");
        Console.Write("номер: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1": Task1();
                break;
            case "2": Task2(); 
                break;
            case "3": Task3(); 
                break;
            case "4": Task4(); 
                break;
            case "5": Task5();
                break;
            default: Console.WriteLine("неверный выбор");
                break;
        }
    }

    // задание 1
    static void Task1()
    {
        //через метод Start
        Task t1 = new Task(() =>
        {
            Console.WriteLine("способ 1 (start): " + DateTime.Now);
        });
        t1.Start();
        t1.Wait();

        //через Task.Factory.StartNew
        Task t2 = Task.Factory.StartNew(() =>
        {
            Console.WriteLine("способ 2 (factory.startnew): " + DateTime.Now);
        });
        t2.Wait();

        //через Task.Run
        Task t3 = Task.Run(() =>
        {
            Console.WriteLine("способ 3 (task.run): " + DateTime.Now);
        });
        t3.Wait();
    }

    //метод проверки на простое число
    static bool IsPrime(int n)
    {
        if (n < 2)
            return false;
        for (int i = 2; i * i <= n; i++)
        {
            if (n % i == 0)
                return false;
        }
        return true;
    }

    // задание 2
    static void Task2()
    {
        Task t = Task.Run(() =>
        {
            for (int i = 0; i <= 1000; i++)
            {
                if (IsPrime(i))
                {
                    Console.Write(i + " ");
                }
            }
            Console.WriteLine();
        });

        t.Wait();
        Console.WriteLine("поиск завершен.");
    }

    //задание 3
    static void Task3()
    {
        Console.Write("введите начало диапазона: ");
        int start = int.Parse(Console.ReadLine());

        Console.Write("введите конец диапазона: ");
        int end = int.Parse(Console.ReadLine());

        Task<int> t = Task.Run(() =>
        {
            int count = 0;
            for (int i = start; i <= end; i++)
            {
                if (IsPrime(i))
                {
                    Console.Write(i + " ");
                    count++;
                }
            }
            Console.WriteLine();
            return count;
        });

        t.Wait();
        Console.WriteLine("всего простых чисел: " + t.Result);
    }

    //задание 4
    static void Task4()
    {
        int[] arr = { 12, 5, 8, 21, 3, 17, 9, 4 };
        Console.WriteLine("исходный массив: " + string.Join(", ", arr));

        List<Task> tasks = new List<Task>();

        tasks.Add(Task.Run(() => Console.WriteLine("минимум: " + arr.Min())));
        tasks.Add(Task.Run(() => Console.WriteLine("максимум: " + arr.Max())));
        tasks.Add(Task.Run(() => Console.WriteLine("среднее: " + arr.Average())));
        tasks.Add(Task.Run(() => Console.WriteLine("сумма: " + arr.Sum())));

        Task.WaitAll(tasks.ToArray());
    }

    //задание 5
    static void Task5()
    {
        int[] arr = { 15, 4, 8, 15, 23, 4, 42, 8, 16 };
        int target = 16;

        Console.WriteLine("исходный массив: " + string.Join(", ", arr));

        //удаление дубликатов
        Task<int[]> removeDuplicates = Task.Run(() =>
        {
            int[] unique = arr.Distinct().ToArray();
            Console.WriteLine("без дублей: " + string.Join(", ", unique));
            return unique;
        });

        //сортировка (продолжение после шага 1)
        Task<int[]> sortTask = removeDuplicates.ContinueWith(prevTask =>
        {
            int[] sorted = prevTask.Result;
            Array.Sort(sorted);
            Console.WriteLine("отсортированный: " + string.Join(", ", sorted));
            return sorted;
        });

        //бинарный поиск 
        Task searchTask = sortTask.ContinueWith(prevTask =>
        {
            int[] sorted = prevTask.Result;
            int index = Array.BinarySearch(sorted, target);
            if (index >= 0)
            {
                Console.WriteLine("число " + target + " найдено на позиции " + index);
            }
            else
            {
                Console.WriteLine("число " + target + " не найдено");
            }
        });

        searchTask.Wait();
    }
}
