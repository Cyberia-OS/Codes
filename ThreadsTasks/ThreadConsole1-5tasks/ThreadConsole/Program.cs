using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace ConsoleTasks
{
    class Program
    {
        static object locker = new object();
        static int[] numbers = new int[10000];
        static int maxVal, minVal;
        static double avgVal;

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("выберите задание (1, 2, 3, 4 или 5 для выхода):");
                string choice = Console.ReadLine();

                if (choice == "1")
                    task1();
                else if (choice == "2") 
                    task2();
                else if (choice == "3")
                    task3();
                else if (choice == "4") 
                    task4And5();
                else if (choice == "5") 
                    break;
            }
        }

        //задание 1
        static void task1()
        {
            Thread t = new Thread(() =>
            {
                for (int i = 0; i <= 50; i++) Console.WriteLine(i);
            });
            t.Start();
            t.Join();
        }

        //задание 2
        static void task2()
        {
            Console.Write("начало: ");
            int start = int.Parse(Console.ReadLine());
            Console.Write("конец: ");
            int end = int.Parse(Console.ReadLine());

            Thread t = new Thread(() =>
            {
                for (int i = start; i <= end; i++) Console.WriteLine(i);
            });
            t.Start();
            t.Join();
        }

        //задание 3
        static void task3()
        {
            Console.Write("начало: ");
            int start = int.Parse(Console.ReadLine());
            Console.Write("конец: ");
            int end = int.Parse(Console.ReadLine());
            Console.Write("количество потоков: ");
            int count = int.Parse(Console.ReadLine());

            Thread[] threads = new Thread[count];
            for (int i = 0; i < count; i++)
            {
                int num = i + 1;
                threads[i] = new Thread(() =>
                {
                    for (int j = start; j <= end; j++)
                    {
                        lock (locker) Console.WriteLine($"поток {num}: {j}");
                    }
                });
                threads[i].Start();
            }

            foreach (var t in threads) t.Join();
        }

        //задания 4 и 5
        static void task4And5()
        {
            Random rnd = new Random();
            for (int i = 0; i < numbers.Length; i++) numbers[i] = rnd.Next(1, 10000);

            Thread tMax = new Thread(() => maxVal = numbers.Max());
            Thread tMin = new Thread(() => minVal = numbers.Min());
            Thread tAvg = new Thread(() => avgVal = numbers.Average());

            tMax.Start(); tMin.Start(); tAvg.Start();
            tMax.Join(); tMin.Join(); tAvg.Join();

            Console.WriteLine($"максимум: {maxVal}");
            Console.WriteLine($"минимум: {minVal}");
            Console.WriteLine($"среднее: {avgVal}");

            Thread tFile = new Thread(() =>
            {
                using (StreamWriter sw = new StreamWriter("result.txt"))
                {
                    sw.WriteLine($"максимум: {maxVal}");
                    sw.WriteLine($"минимум: {minVal}");
                    sw.WriteLine($"среднее: {avgVal}");
                    sw.WriteLine(string.Join(", ", numbers));
                }
                Console.WriteLine("записано в файл result.txt");
            });
            tFile.Start();
            tFile.Join();
        }
    }
}
