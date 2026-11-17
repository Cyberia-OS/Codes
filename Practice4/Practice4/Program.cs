using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace SyncPractice
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - События (1000 чисел)");
                Console.WriteLine("2 - Критические секции (файл)");
                Console.WriteLine("3 - Критические секции (массив)");
                Console.WriteLine("0 - Выход");
                Console.Write("Выбор: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                if (choice == "1") 
                    Task1();
                else if (choice == "2")
                    Task2();
                else if (choice == "3")
                    Task3();
                else if (choice == "0")
                    break;

                Console.WriteLine("Нажмите Enter");
                Console.ReadLine();
            }
        }

        //задание 1 События 
        static void Task1()
        {
            int[] nums = new int[1000];
            ManualResetEvent ev = new ManualResetEvent(false);

            //генерирует числа и подает сигнал
            Thread tGen = new Thread(() =>
            {
                Random rnd = new Random();
                for (int i = 0; i < 1000; i++) nums[i] = rnd.Next(0, 5001);
                Console.WriteLine("Числа сгенерированы!");
                ev.Set(); 
            });

            //три потока ждут сигнала ev.WaitOne() и считают результаты
            Thread tMax = new Thread(() => { ev.WaitOne(); Console.WriteLine("Максимум: " + nums.Max()); });
            Thread tMin = new Thread(() => { ev.WaitOne(); Console.WriteLine("Минимум: " + nums.Min()); });
            Thread tAvg = new Thread(() => { ev.WaitOne(); Console.WriteLine("Среднее: " + nums.Average()); });

            tMax.Start(); tMin.Start(); tAvg.Start(); tGen.Start();
            tMax.Join(); tMin.Join(); tAvg.Join();
        }

        //задание 2 Критические секции 
        static void Task2()
        {
            object locker = new object();
            string path = "test.txt";
            File.WriteAllText(path, "Привет! Как дела? Отлично!");

            //Поток 1 считает предложения
            Thread t1 = new Thread(p =>
            {
                lock (locker)
                {
                    string text = File.ReadAllText((string)p);
                    int count = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    Console.WriteLine("Предложений в файле: " + count);
                }
            });

            //Поток 2 ждет и меняет '!' на '#'
            Thread t2 = new Thread(p =>
            {
                lock (locker)
                {
                    string text = File.ReadAllText((string)p);
                    File.WriteAllText((string)p, text.Replace('!', '#'));
                    Console.WriteLine("Новый текст: " + File.ReadAllText((string)p));
                }
            });

            t1.Start(path);
            Thread.Sleep(50);
            t2.Start(path);
            t1.Join(); t2.Join();
        }

        //зд 3 Критические секции (Массив)
        static void Task3()
        {
            object locker = new object();
            int[] arr = { 42, 7, 19, 3, 88, 15 };
            int target = 19;

            //поток 1: сортирует массив
            Thread t1 = new Thread(a =>
            {
                lock (locker)
                {
                    Array.Sort((int[])a);
                    Console.WriteLine("Отсортировано: " + string.Join(", ", (int[])a));
                }
            });

            //поток 2: ждет и ищет число
            Thread t2 = new Thread(num =>
            {
                lock (locker)
                {
                    bool found = Array.BinarySearch(arr, (int)num) >= 0;
                    Console.WriteLine($"Число {num} найдено: {found}");
                }
            });

            t1.Start(arr);
            Thread.Sleep(50);
            t2.Start(target);
            t1.Join(); t2.Join();
        }
    }
}
