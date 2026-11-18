using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("выберите задание (1-6):");
        Console.WriteLine("1. приложение в единственном экземпляре");
        Console.WriteLine("2. защита общего счетчика мьютексом");
        Console.WriteLine("3. ограничение количества потоков");
        Console.WriteLine("4. очередь производитель-потребитель");
        Console.WriteLine("5. стартовый сигнал для группы потоков");
        Console.WriteLine("6. поочередный вывод двух потоков");
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
            case "6": Task6();
                break;
            default: Console.WriteLine("неверный выбор"); 
                break;
        }
    }

    // задание 1
    static void Task1()
    {
        Mutex mutex = null;

        try
        {
            mutex = Mutex.OpenExisting("MY_MUTEX");
            Console.WriteLine("приложение уже запущено!");
            return;
        }
        catch (WaitHandleCannotBeOpenedException)
        {
            mutex = new Mutex(true, "MY_MUTEX");
        }

        Console.WriteLine("приложение работает. нажмите enter для выхода...");
        Console.ReadLine();

        mutex.ReleaseMutex();
        mutex.Dispose();
    }

    // задание 2
    class MutexCounter
    {
        public int count = 0;
        private Mutex mutex = new Mutex();

        public void Increment()
        {
            for (int i = 0; i < 1000000; i++)
            {
                mutex.WaitOne();
                count++;
                mutex.ReleaseMutex();
            }
        }
    }

    static void Task2()
    {
        MutexCounter counter = new MutexCounter();
        List<Thread> threads = new List<Thread>();

        for (int i = 0; i < 5; i++)
        {
            Thread t = new Thread(counter.Increment);
            threads.Add(t);
            t.Start();
        }

        foreach (Thread t in threads)
        {
            t.Join();
        }

        Console.WriteLine("итоговое значение: " + counter.count);
    }

    // задание 3
    static Semaphore sem3 = new Semaphore(3, 3);
    static int done3 = 0;

    static void Task3()
    {
        for (int i = 1; i <= 6; i++)
        {
            ThreadPool.QueueUserWorkItem(Worker3, i);
        }

        while (done3 < 6)
        {
            Thread.Sleep(100);
        }
    }

    static void Worker3(object state)
    {
        int id = (int)state;

        while (true)
        {
            if (sem3.WaitOne(500))
            {
                try
                {
                    Console.WriteLine("задача " + id + " (поток " + Thread.CurrentThread.ManagedThreadId + ") работает");
                    Thread.Sleep(2000);
                }
                finally
                {
                    Console.WriteLine("задача " + id + " освободила семафор");
                    sem3.Release();
                }
                break;
            }
            else
            {
                Console.WriteLine("задача " + id + " ждет (таймаут 500 мс)");
            }
        }

        Interlocked.Increment(ref done3);
    }

    // задание 4
    static Queue<int> queue = new Queue<int>();
    static object locker = new object();
    static Semaphore empty = new Semaphore(5, 5);
    static Semaphore full = new Semaphore(0, 5);

    static void Task4()
    {
        Thread prod = new Thread(Producer);
        Thread cons1 = new Thread(() => Consumer(1));
        Thread cons2 = new Thread(() => Consumer(2));

        prod.Start();
        cons1.Start();
        cons2.Start();

        prod.Join();
        cons1.Join();
        cons2.Join();
    }

    static void Producer()
    {
        for (int i = 1; i <= 10; i++)
        {
            empty.WaitOne();
            lock (locker)
            {
                queue.Enqueue(i);
                Console.WriteLine("добавлено: " + i + " (в очереди: " + queue.Count + ")");
            }
            full.Release();
            Thread.Sleep(100);
        }

        for (int i = 0; i < 2; i++)
        {
            empty.WaitOne();
            lock (locker)
            {
                queue.Enqueue(-1);
            }
            full.Release();
        }
    }

    static void Consumer(int id)
    {
        while (true)
        {
            full.WaitOne();
            int item;
            lock (locker)
            {
                item = queue.Dequeue();
                if (item != -1)
                {
                    Console.WriteLine("потребитель " + id + " забрал: " + item);
                }
            }
            empty.Release();

            if (item == -1) 
                break;
            Thread.Sleep(250);
        }
    }

    // задание 5
    static ManualResetEvent startEvent = new ManualResetEvent(false);

    static void Task5()
    {
        List<Thread> threads = new List<Thread>();

        for (int i = 0; i < 10; i++)
        {
            int id = i + 1;
            Thread t = new Thread(() => Worker5(id));
            threads.Add(t);
            t.Start();
        }

        Thread.Sleep(2000);
        Console.WriteLine("сигнал set отправлен");
        startEvent.Set();

        foreach (Thread t in threads)
        {
            t.Join();
        }

        Console.WriteLine("вызываем reset и запускаем поток 11");
        startEvent.Reset();

        Thread t11 = new Thread(() => Worker5(11));
        t11.Start();

        Thread.Sleep(1500);
        Console.WriteLine("поток 11 ждал reset, отправляем set снова");
        startEvent.Set();
        t11.Join();
    }

    static void Worker5(int id)
    {
        startEvent.WaitOne();
        Console.WriteLine("поток " + id + " стартовал");
    }

    // задание 6
    static AutoResetEvent event1 = new AutoResetEvent(true);
    static AutoResetEvent event2 = new AutoResetEvent(false);

    static void Task6()
    {
        Thread t1 = new Thread(PrintOdd);
        Thread t2 = new Thread(PrintEven);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();
    }

    static void PrintOdd()
    {
        for (int i = 1; i <= 9; i += 2)
        {
            event1.WaitOne();
            Console.WriteLine(i);
            event2.Set();
        }
    }

    static void PrintEven()
    {
        for (int i = 2; i <= 10; i += 2)
        {
            event2.WaitOne();
            Console.WriteLine(i);
            event1.Set();
        }
    }
}
