using System;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    //импорт функций
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    //перегрузка SendMessage для системных команд 
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    //перегрузка SendMessage для передачи текста
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

    const uint MB_OK = 0x00000000;
    const uint MB_YESNO = 0x00000004;
    const uint MB_ICONQUESTION = 0x00000020;
    const int IDYES = 6;

    const uint WM_SETTEXT = 0x000C;
    const uint WM_CLOSE = 0x0010;

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Задание 1: Вывод 'Hello, World!'");
            Console.WriteLine("2. Задание 2: Игра 'Угадай число'");
            Console.WriteLine("3. Задание 3: Закрыть окно Блокнота");
            Console.WriteLine("4. Задание 4: Время в заголовке Блокнота");
            Console.WriteLine("0. Выход");
            Console.Write("\nВыберите номер задания: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Task1();
                    break;
                case "2":
                    Task2();
                    break;
                case "3":
                    Task3();
                    break;
                case "4":
                    Task4();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void Task1()
    {
        MessageBox(IntPtr.Zero, "Hello, World!", "Задание 1", MB_OK);
    }

    static void Task2()
    {
        bool playAgain = true;

        while (playAgain)
        {
            MessageBox(IntPtr.Zero, "Загадайте число от 0 до 100 и нажмите ОК.", "Угадай число", MB_OK);

            int min = 0;
            int max = 100;
            bool guessed = false;

            while (min <= max && !guessed)
            {
                int mid = (min + max) / 2;
                int answer = MessageBox(IntPtr.Zero, $"Ваше число {mid}?", "Попытка", MB_YESNO | MB_ICONQUESTION);

                if (answer == IDYES)
                {
                    MessageBox(IntPtr.Zero, $"Ура! Я угадал число {mid}!", "Победа", MB_OK);
                    guessed = true;
                }
                else
                {
                    int isGreater = MessageBox(IntPtr.Zero, $"Ваше число больше {mid}?", "Подсказка", MB_YESNO | MB_ICONQUESTION);
                    if (isGreater == IDYES)
                        min = mid + 1;
                    else
                        max = mid - 1;
                }
            }

            if (!guessed)
            {
                MessageBox(IntPtr.Zero, "Вы где-то ошиблись в ответах!", "Ошибка", MB_OK);
            }

            int restart = MessageBox(IntPtr.Zero, "Хотите сыграть еще раз?", "Новая игра", MB_YESNO | MB_ICONQUESTION);
            playAgain = (restart == IDYES);
        }
    }

    static void Task3()
    {
        IntPtr notepadHandle = FindWindow("Notepad", null);

        if (notepadHandle != IntPtr.Zero)
        {
            SendMessage(notepadHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            Console.WriteLine("Команда закрытия отправлена Блокноту");
        }
        else
        {
            Console.WriteLine("Окно Блокнота не найдено! Сначала запустите Блокнот");
        }

        Console.WriteLine("Нажмите любую клавишу для возврата в меню");
        Console.ReadKey();
    }

    static void Task4()
    {
        Console.WriteLine("Обновление заголовка Блокнота запущено");
        Console.WriteLine("Нажмите любую клавишу в этом окне, чтобы остановить и вернуться в меню\n");

        while (!Console.KeyAvailable)
        {
            IntPtr notepadHandle = FindWindow("Notepad", null);

            if (notepadHandle != IntPtr.Zero)
            {
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                SendMessage(notepadHandle, WM_SETTEXT, IntPtr.Zero, $"Текущее время: {currentTime}");
                Console.Write($"\rОбновлено: {currentTime} ");
            }
            else
            {
                Console.Write("\rОжидание запуска Блокнота...");
            }

            Thread.Sleep(1000);
        }

        Console.ReadKey(true); 
    }
}
