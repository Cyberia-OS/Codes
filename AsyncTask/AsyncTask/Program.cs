using System;
using System.Threading;

class Program
{
    //флаг для остановки фоновых потоков при выходе из задания в общее меню
    static volatile bool stopBackground = false;

    //делегаты для модели apm
    delegate double HeavyMathDelegate(int n);
    delegate void TrajectoryDelegate();

    //переменные для задания 3
    static int ticketsLeft = 100;

    //переменные для задания 4
    static int fuelLevel = 0;

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("практикум: многопоточность и асинхронность");
            Console.WriteLine("задание 1: диспетчер задач (thread vs threadpool)");
            Console.WriteLine("задание 2: асинхронный калькулятор (модель apm)");
            Console.WriteLine("задание 3: касса кинотеатра (синхронизация)");
            Console.WriteLine("задание 4: космический запуск (комплексное)");
            Console.WriteLine("0. выход");
            Console.Write("выберите задание: ");

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

    //задание 1 диспетчер задач
    static void Task1()
    {
        Console.Clear();
        stopBackground = false;

        Thread timeThread = new Thread(() =>
        {
            while (!stopBackground)
            {
                Console.WriteLine($"время: {DateTime.Now:HH:mm:ss}");
                Thread.Sleep(1000);
            }
        });
        timeThread.IsBackground = true;
        timeThread.Start();

        for (int i = 1; i <= 3; i++)
        {
            ThreadPool.QueueUserWorkItem(PoolTask1, i);
        }

        Thread.Sleep(5000);
        stopBackground = true;

        Console.WriteLine("главный поток завершил работу. нажмите любую клавишу");
        Console.ReadKey();
    }

    static void PoolTask1(object state)
    {
        int taskId = (int)state;
        Console.WriteLine($"задача №{taskId} выполняется в потоке {Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(2000);
    }

    //задание 2 асинхронный калькулятор
    static void Task2()
    {
        Console.Clear();
        HeavyMathDelegate mathDel = CalculateHeavyMath;

        mathDel.BeginInvoke(25, new AsyncCallback(CalculationFinished), mathDel);

        Console.WriteLine("расчет запущен. главный поток свободен.");
        Console.WriteLine("ожидайте результат (3 сек), затем нажмите любую клавишу для выхода в меню");
        Console.ReadKey();
    }

    static double CalculateHeavyMath(int n)
    {
        Thread.Sleep(3000);
        return Math.Sqrt(n);
    }

    static void CalculationFinished(IAsyncResult ar)
    {
        HeavyMathDelegate del = (HeavyMathDelegate)ar.AsyncState;
        double result = del.EndInvoke(ar);
        Console.WriteLine($"результат вычислений: {result}");
    }

    //задание 3 касса кинотеатра
    static void Task3()
    {
        Console.Clear();
        Console.WriteLine("1 - продажа без синхронизации (билеты уйдут в минус)");
        Console.WriteLine("2 - продажа с использованием interlocked");
        Console.Write("выберите режим: ");
        string mode = Console.ReadLine();

        ticketsLeft = 100;
        Thread[] cashiers = new Thread[5];

        for (int i = 0; i < 5; i++)
        {
            int cashierId = i + 1;
            if (mode == "1")
                cashiers[i] = new Thread(() => SellUnsafe(cashierId));
            else
                cashiers[i] = new Thread(() => SellSafe(cashierId));

            cashiers[i].Start();
        }

        foreach (Thread t in cashiers)
        {
            t.Join();
        }

        Console.WriteLine("все кассиры закончили работу. нажмите любую клавишу");
        Console.ReadKey();
    }

    static void SellUnsafe(int cashierId)
    {
        while (ticketsLeft > 0)
        {
            Thread.Sleep(10);
            ticketsLeft--;
            Console.WriteLine($"кассир {cashierId} продал билет. осталось: {ticketsLeft}");
        }
    }

    static void SellSafe(int cashierId)
    {
        while (true)
        {
            //атомарно уменьшаем счетчик на 1 и получаем новое значение
            int remaining = Interlocked.Decrement(ref ticketsLeft);
            if (remaining >= 0)
            {
                Thread.Sleep(10);
                Console.WriteLine($"кассир {cashierId} продал билет. осталось: {remaining}");
            }
            else
            {
                //если билетов не было возвращаем счетчик обратно к 0 и выходим
                Interlocked.Increment(ref ticketsLeft);
                break;
            }
        }
    }

    //задание 4 космический запуск
    static void Task4()
    {
        Console.Clear();
        stopBackground = false;
        fuelLevel = 0;

        //мониторинг 
        Thread telemetryThread = new Thread(() =>
        {
            while (!stopBackground)
            {
                Console.WriteLine("телеметрия в норме...");
                Thread.Sleep(1000);
            }
        });
        telemetryThread.IsBackground = true;
        telemetryThread.Start();

        //асинхронный расчет траектории 
        TrajectoryDelegate trajDel = CalculateTrajectory;
        trajDel.BeginInvoke(new AsyncCallback(TrajectoryFinished), trajDel);

        //топливные насосы (threadpool + interlocked)
        ThreadPool.QueueUserWorkItem(PumpFuel);

        //главный поток ждет 5 секунд и выводит итог
        Thread.Sleep(5000);
        stopBackground = true;

        int finalFuel = Interlocked.CompareExchange(ref fuelLevel, 0, 0);
        Console.WriteLine($"итоговый уровень топлива: {finalFuel}");
        Console.WriteLine("программа завершена. нажмите любую клавишу для возврата в меню...");
        Console.ReadKey();
    }

    static void CalculateTrajectory()
    {
        Thread.Sleep(4000);
    }

    static void TrajectoryFinished(IAsyncResult ar)
    {
        TrajectoryDelegate del = (TrajectoryDelegate)ar.AsyncState;
        del.EndInvoke(ar);
        Console.WriteLine("траектория рассчитана! к запуску готов");
    }

    static void PumpFuel(object state)
    {
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(500);
            Interlocked.Add(ref fuelLevel, 10);
        }
    }
}
