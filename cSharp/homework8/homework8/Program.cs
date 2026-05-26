using System;

namespace homework8
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("  Выберите задание:");
                Console.WriteLine("  1 - Квадрат из символов");
                Console.WriteLine("  2 - Проверка числа-палиндрома");
                Console.WriteLine("  3 - Фильтрация массива");
                Console.WriteLine("  4 - Класс Веб-сайт");
                Console.WriteLine("  5 - Класс Журнал");
                Console.WriteLine("  0 - Выход");
                Console.Write("Ваш выбор: ");

                switch (Console.ReadLine())
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
                    case "0": return;
                    default: Console.WriteLine("Неверный пункт меню");
                        break;
                }
            }
        }

        //задание 1 Квадрат из символов
        static void DrawSquare(int size, char symbol)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    Console.Write(symbol + " ");
                Console.WriteLine();
            }
        }

        static void Task1()
        {
            Console.Write("Введите длину стороны квадрата: ");
            int size = int.Parse(Console.ReadLine());

            Console.Write("Введите символ: ");
            char symbol = Console.ReadLine()[0];

            Console.WriteLine();
            DrawSquare(size, symbol);
        }

        //Задание 2Число-палиндром
        static bool IsPalindrome(int number)
        {
            string s = Math.Abs(number).ToString();
            int left = 0;
            int right = s.Length - 1;

            while (left < right)
            {
                if (s[left] != s[right])
                    return false;
                left++;
                right--;
            }

            return true;
        }

        static void Task2()
        {
            Console.Write("Введите число: ");
            int number = int.Parse(Console.ReadLine());

            if (IsPalindrome(number))
                Console.WriteLine($"{number} — палиндром.");
            else
                Console.WriteLine($"{number} — не палиндром.");
        }

        //Задание 3
        static int[] FilterArray(int[] original, int[] filter)
        {
            //сколько элементов останется
            int count = 0;
            foreach (int item in original)
            {
                bool found = false;
                foreach (int f in filter)
                    if (item == f) { found = true; break; }
                if (!found) count++;
            }

            int[] result = new int[count];
            int index = 0;
            foreach (int item in original)
            {
                bool found = false;
                foreach (int f in filter)
                    if (item == f) { found = true; break; }
                if (!found) result[index++] = item;
            }

            return result;
        }

        static int[] ReadArray(string prompt)
        {
            Console.Write(prompt);
            string[] parts = Console.ReadLine().Split(' ');
            int[] arr = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
                arr[i] = int.Parse(parts[i]);
            return arr;
        }

        static void Task3()
        {
            int[] original = ReadArray("Введите оригинальный массив: ");
            int[] filter = ReadArray("Введите массив для фильтрации: ");

            int[] result = FilterArray(original, filter);

            Console.Write("Результат: ");
            Console.WriteLine(string.Join(" ", result));
        }

        //Задание 4
        static void Task4()
        {
            Website site = new Website();
            site.Input();
            site.Print();
        }

        //Задание 5
        static void Task5()
        {
            Magazine magazine = new Magazine();
            magazine.Input();
            magazine.Print();
        }
    }

    //класс Веб-сайт

    class Website
    {
        private string name;
        private string url;
        private string description;
        private string ip;

        public void Input()
        {
            Console.Write("Название сайта: ");
            name = Console.ReadLine();

            Console.Write("Путь (URL): ");
            url = Console.ReadLine();

            Console.Write("Описание: ");
            description = Console.ReadLine();

            Console.Write("IP-адрес: ");
            ip = Console.ReadLine();
        }

        public void Print()
        {
            Console.WriteLine("\nИнформация о сайте");
            Console.WriteLine($"Название:  {name}");
            Console.WriteLine($"URL:       {url}");
            Console.WriteLine($"Описание:  {description}");
            Console.WriteLine($"IP-адрес:  {ip}");
        }

        public string GetName() { return name; }
        public void SetName(string v) { name = v; }

        public string GetUrl() { return url; }
        public void SetUrl(string v) { url = v; }

        public string GetDescription() { return description; }
        public void SetDescription(string v) { description = v; }

        public string GetIp() { return ip; }
        public void SetIp(string v) { ip = v; }
    }

    //Класс Журнал
    class Magazine
    {
        private string name;
        private int foundedYear;
        private string description;
        private string phone;
        private string email;

        public void Input()
        {
            Console.Write("Название журнала: ");
            name = Console.ReadLine();

            Console.Write("Год основания: ");
            foundedYear = int.Parse(Console.ReadLine());

            Console.Write("Описание: ");
            description = Console.ReadLine();

            Console.Write("Контактный телефон: ");
            phone = Console.ReadLine();

            Console.Write("Контактный e-mail: ");
            email = Console.ReadLine();
        }

        public void Print()
        {
            Console.WriteLine("\nИнформация о журнале");
            Console.WriteLine($"Название:      {name}");
            Console.WriteLine($"Год основания: {foundedYear}");
            Console.WriteLine($"Описание:      {description}");
            Console.WriteLine($"Телефон:       {phone}");
            Console.WriteLine($"E-mail:        {email}");
        }

        public string GetName() { return name; }
        public void SetName(string v) { name = v; }

        public int GetFoundedYear() { return foundedYear; }
        public void SetFoundedYear(int v) { foundedYear = v; }

        public string GetDescription() { return description; }
        public void SetDescription(string v) { description = v; }

        public string GetPhone() { return phone; }
        public void SetPhone(string v) { phone = v; }

        public string GetEmail() { return email; }
        public void SetEmail(string v) { email = v; }
    }
}
