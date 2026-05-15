using System;

namespace Homework3
{
    class Program
    {
        static void Main()
        {
            string choice = "";

            while (choice != "0")
            {
                Console.WriteLine("1 - Веб-сайт");
                Console.WriteLine("2 - Журнал");
                Console.WriteLine("3 - Магазин");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите задание: ");

                choice = Console.ReadLine();
                Console.WriteLine();

                if (choice == "1")
                {
                    Website site = new Website();
                    site.Input();
                    Console.WriteLine("Данные сайта:");
                    site.Print();
                }
                else if (choice == "2")
                {
                    Journal journal = new Journal();
                    journal.Input();
                    Console.WriteLine("Данные журнала:");
                    journal.Print();
                }
                else if (choice == "3")
                {
                    Shop shop = new Shop();
                    shop.Input();
                    Console.WriteLine("Данные магазина:");
                    shop.Print();
                }
                else if (choice != "0")
                {
                    Console.WriteLine("Нет такого пункта, попробуйте снова");
                }
            }
        }
    }

    //класс Веб-сайт
    class Website
    {
        private string name;
        private string path;
        private string description;
        private string ip;

        public string GetName() { return name; }
        public void SetName(string n) { name = n; }

        public string GetPath() { return path; }
        public void SetPath(string p) { path = p; }

        public string GetDescription() { return description; }
        public void SetDescription(string d) { description = d; }

        public string GetIp() { return ip; }
        public void SetIp(string i) { ip = i; }

        public void Input()
        {
            Console.Write("Название сайта: ");
            name = Console.ReadLine();

            Console.Write("Путь к сайту: ");
            path = Console.ReadLine();

            Console.Write("Описание: ");
            description = Console.ReadLine();

            Console.Write("IP адрес: ");
            ip = Console.ReadLine();
        }

        public void Print()
        {
            Console.WriteLine("Название:  " + name);
            Console.WriteLine("Путь:      " + path);
            Console.WriteLine("Описание:  " + description);
            Console.WriteLine("IP адрес:  " + ip);
        }
    }

    //класс журнал
    class Journal
    {
        private string name;
        private int year;
        private string description;
        private string phone;
        private string email;

        public string GetName() { return name; }
        public void SetName(string n) { name = n; }

        public int GetYear() { return year; }
        public void SetYear(int y) { year = y; }

        public string GetDescription() { return description; }
        public void SetDescription(string d) { description = d; }

        public string GetPhone() { return phone; }
        public void SetPhone(string p) { phone = p; }

        public string GetEmail() { return email; }
        public void SetEmail(string e) { email = e; }

        public void Input()
        {
            Console.Write("Название журнала: ");
            name = Console.ReadLine();

            Console.Write("Год основания: ");
            int.TryParse(Console.ReadLine(), out year);

            Console.Write("Описание: ");
            description = Console.ReadLine();

            Console.Write("Телефон: ");
            phone = Console.ReadLine();

            Console.Write("E-mail: ");
            email = Console.ReadLine();
        }

        public void Print()
        {
            Console.WriteLine("Название:      " + name);
            Console.WriteLine("Год основания: " + year);
            Console.WriteLine("Описание:      " + description);
            Console.WriteLine("Телефон:       " + phone);
            Console.WriteLine("E-mail:        " + email);
        }
    }

    //класс магазин
    class Shop
    {
        private string name;
        private string address;
        private string description;
        private string phone;
        private string email;

        public string GetName() { return name; }
        public void SetName(string n) { name = n; }

        public string GetAddress() { return address; }
        public void SetAddress(string a) { address = a; }

        public string GetDescription() { return description; }
        public void SetDescription(string d) { description = d; }

        public string GetPhone() { return phone; }
        public void SetPhone(string p) { phone = p; }

        public string GetEmail() { return email; }
        public void SetEmail(string e) { email = e; }

        public void Input()
        {
            Console.Write("Название магазина: ");
            name = Console.ReadLine();

            Console.Write("Адрес: ");
            address = Console.ReadLine();

            Console.Write("Описание профиля: ");
            description = Console.ReadLine();

            Console.Write("Телефон: ");
            phone = Console.ReadLine();

            Console.Write("E-mail: ");
            email = Console.ReadLine();
        }

        public void Print()
        {
            Console.WriteLine("Название:  " + name);
            Console.WriteLine("Адрес:     " + address);
            Console.WriteLine("Описание:  " + description);
            Console.WriteLine("Телефон:   " + phone);
            Console.WriteLine("E-mail:    " + email);
        }
    }
}