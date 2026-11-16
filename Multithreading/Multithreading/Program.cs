using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static int counter = 0;
    private static object locker = new object();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("многопоточность в .net");
            Console.WriteLine("задача 1: базовое управление потоками и метод join");
            Console.WriteLine("задача 2: фоновые потоки и таймер");
            Console.WriteLine("задача 3: борьба с гонкой данных");
            Console.WriteLine("задача 4: игра успел, не успел");
            Console.WriteLine("0. выход");
            Console.Write("выберите задачу: ");

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
                case "0": 
                    return;
            }
        }
    }

    //задача 1
    static void Task1()
    {
        Console.Clear();
        Thread t1 = new Thread(new ParameterizedThreadStart(PrintSteps));
        Thread t2 = new Thread(new ParameterizedThreadStart(PrintSteps));

        t1.Start("поток а");
        t2.Start("поток б");

        t1.Join();
        t2.Join();

        Console.WriteLine("все потоки завершили работу!");
        Console.WriteLine("нажмите любую клавишу для возврата в меню");
        Console.ReadKey();
    }

    static void PrintSteps(object nameObj)
    {
        string name = (string)nameObj;
        for (int i = 1; i <= 5; i++)
        {
            Console.WriteLine($"[{name}] шаг: {i}");
            Thread.Sleep(500);
        }
    }

    //задача 2
    static void Task2()
    {
        Console.Clear();
        Console.WriteLine("нажмите enter для выхода");

        Timer timer = new Timer(PrintTime, null, 1000, 2000);

        Thread bgThread = new Thread(BackgroundWork);
        bgThread.IsBackground = true;
        bgThread.Start();

        Console.ReadLine();
        timer.Dispose();
    }

    static void PrintTime(object state)
    {
        Console.WriteLine(DateTime.Now);
    }

    static void BackgroundWork()
    {
        while (true)
        {
            Console.WriteLine("фоновый поток работает");
            Thread.Sleep(300);
        }
    }

    //задача 3
    static void Task3()
    {
        Console.Clear();
        Console.WriteLine("1 - без блокировки (гонка данных)");
        Console.WriteLine("2 - с блокировкой lock");
        Console.Write("выберите режим: ");
        string mode = Console.ReadLine();

        counter = 0;
        Thread[] threads = new Thread[5];

        for (int i = 0; i < 5; i++)
        {
            if (mode == "1")
                threads[i] = new Thread(IncrementUnsafe);
            else
                threads[i] = new Thread(IncrementSafe);

            threads[i].Start();
        }

        for (int i = 0; i < 5; i++)
        {
            threads[i].Join();
        }

        Console.WriteLine($"итоговое значение counter: {counter}");
        Console.WriteLine("нажмите любую клавишу для возврата в меню");
        Console.ReadKey();
    }

    static void IncrementUnsafe()
    {
        for (int i = 0; i < 100000; i++)
        {
            counter++;
        }
    }

    static void IncrementSafe()
    {
        for (int i = 0; i < 100000; i++)
        {
            lock (locker)
            {
                counter++;
            }
        }
    }

    //задача 4
    static void Task4()
    {
        Console.Clear();
        Console.WriteLine("приготовьтесь");

        while (Console.KeyAvailable) Console.ReadKey(true);

        Random rnd = new Random();
        int delay = rnd.Next(2000, 5000);
        Thread.Sleep(delay);

        bool reacted = false;

        Thread timeoutThread = new Thread(() =>
        {
            Thread.Sleep(3000);
            if (!reacted)
            {
                Console.ResetColor();
                Console.WriteLine("\nвы не успели слишком долго");
                Environment.Exit(0);
            }
        });
        timeoutThread.IsBackground = true;
        timeoutThread.Start();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("жми!");
        Console.ResetColor();

        Stopwatch sw = Stopwatch.StartNew();
        Console.ReadKey(true);
        sw.Stop();
        reacted = true;

        Console.WriteLine($"ваше время реакции: {sw.ElapsedMilliseconds} миллисекунд");
        Console.WriteLine("нажмите любую клавишу для возврата в меню");
        Console.ReadKey();
    }
}
