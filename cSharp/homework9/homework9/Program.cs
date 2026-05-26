using System;

namespace Russia
{
    class Moscow
    {
        public string Name { get; } = "Москва";
        public string Country { get; } = "Россия";
        public long Population { get; } = 12_600_000;

        public void Print()
        {
            Console.WriteLine($"  {Country} -> {Name}: {Population:N0} чел.");
        }
    }
}

namespace USA
{
    class WashingtonDC
    {
        public string Name { get; } = "Вашингтон";
        public string Country { get; } = "США";
        public long Population { get; } = 689_545;

        public void Print()
        {
            Console.WriteLine($"  {Country} -> {Name}: {Population:N0} чел.");
        }
    }
}

namespace France
{
    class Paris
    {
        public string Name { get; } = "Париж";
        public string Country { get; } = "Франция";
        public long Population { get; } = 2_161_000;

        public void Print()
        {
            Console.WriteLine($"  {Country} -> {Name}: {Population:N0} чел.");
        }
    }
}

namespace Homework9
{
    enum ArticleType { Food, Electronics, Clothing, Other }
    enum ClientType { Regular, Silver, Gold, VIP }
    enum PayType { Cash, Card, BankTransfer, Online }

    struct Article
    {
        public int Code;
        public string Name;
        public decimal Price;
        public ArticleType Type;

        public Article(int code, string name, decimal price, ArticleType type)
        {
            Code = code;
            Name = name;
            Price = price;
            Type = type;
        }

        public void Print()
        {
            Console.WriteLine($"  [{Code}] {Name} | {Price:C} | {Type}");
        }
    }

    struct Client
    {
        public int Code;
        public string FullName;
        public string Address;
        public string Phone;
        public int OrderCount;
        public decimal TotalSum;
        public ClientType Type;

        public Client(int code, string fullName, string address, string phone, ClientType type)
        {
            Code = code;
            FullName = fullName;
            Address = address;
            Phone = phone;
            OrderCount = 0;
            TotalSum = 0;
            Type = type;
        }

        public void Print()
        {
            Console.WriteLine($"  [{Code}] {FullName}");
            Console.WriteLine($"       Адрес: {Address} | Тел: {Phone}");
            Console.WriteLine($"       Тип: {Type} | Заказов: {OrderCount} | Сумма: {TotalSum:C}");
        }
    }

    struct RequestItem
    {
        public Article Article;
        public int Quantity;

        public RequestItem(Article article, int quantity)
        {
            Article = article;
            Quantity = quantity;
        }

        public void Print()
        {
            Console.WriteLine($"    {Article.Name} x{Quantity} = {Article.Price * Quantity:C}");
        }
    }

    struct Request
    {
        public int Code;
        public Client Client;
        public DateTime OrderDate;
        public RequestItem[] Items;
        public PayType PaymentType;

        public decimal TotalSum
        {
            get
            {
                if (Items == null) return 0;
                decimal sum = 0;
                foreach (RequestItem item in Items)
                    sum += item.Article.Price * item.Quantity;
                return sum;
            }
        }

        public Request(int code, Client client, DateTime date, RequestItem[] items, PayType payType)
        {
            Code = code;
            Client = client;
            OrderDate = date;
            Items = items;
            PaymentType = payType;
        }

        public void Print()
        {
            Console.WriteLine($"  Заказ #{Code} от {OrderDate:dd.MM.yyyy}");
            Console.WriteLine($"  Клиент: {Client.FullName} | Оплата: {PaymentType}");
            Console.WriteLine("  Товары:");
            if (Items != null)
                foreach (RequestItem item in Items)
                    item.Print();
            Console.WriteLine($"  Итого: {TotalSum:C}");
        }
    }

    class Student
    {
        private string _lastName;
        private string _firstName;
        private string _middleName;
        private string _group;
        private int _age;
        private int[][] _grades;

        private static string[] SubjectNames =
            { "Программирование", "Администрирование", "Дизайн" };

        public Student(string lastName, string firstName, string middleName, string group, int age)
        {
            _lastName = lastName;
            _firstName = firstName;
            _middleName = middleName;
            _group = group;
            _age = age;
            _grades = new int[][] { new int[0], new int[0], new int[0] };
        }

        public string LastName { get { return _lastName; } set { _lastName = value; } }
        public string FirstName { get { return _firstName; } set { _firstName = value; } }
        public string Group { get { return _group; } set { _group = value; } }
        public int Age
        {
            get { return _age; }
            set { if (value > 0) _age = value; }
        }

        public void SetGrade(int subject, int grade)
        {
            Array.Resize(ref _grades[subject], _grades[subject].Length + 1);
            _grades[subject][_grades[subject].Length - 1] = grade;
        }

        public int[] GetGrades(int subject)
        {
            return _grades[subject];
        }

        public double GetAverage(int subject)
        {
            if (_grades[subject].Length == 0) return 0;
            int sum = 0;
            foreach (int g in _grades[subject])
                sum += g;
            return (double)sum / _grades[subject].Length;
        }

        public void Print()
        {
            Console.WriteLine($"  ФИО:    {_lastName} {_firstName} {_middleName}");
            Console.WriteLine($"  Группа: {_group} | Возраст: {_age}");
            for (int i = 0; i < 3; i++)
            {
                string gradesStr = _grades[i].Length > 0
                    ? string.Join(", ", _grades[i])
                    : "нет оценок";
                Console.WriteLine($"  {SubjectNames[i],-20}: {gradesStr,-20} | Ср. балл: {GetAverage(i):F2}");
            }
        }
    }

    struct CapitalInfo
    {
        public string Name;
        public long Population;
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n  1 - Структуры (Товар, Клиент, Заказ)");
                Console.WriteLine("  2 - Класс Студент");
                Console.WriteLine("  3 - Население столиц");
                Console.WriteLine("  0 - Выход");
                Console.Write("Ваш выбор: ");

                switch (Console.ReadLine())
                {
                    case "1": DemoStructures(); break;
                    case "2": DemoStudent(); break;
                    case "3": DemoCapitals(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный пункт меню."); break;
                }
            }
        }

        static void DemoStructures()
        {
            Article apple = new Article(1, "Яблоко", 15.99m, ArticleType.Food);
            Article phone = new Article(2, "Смартфон", 9999.00m, ArticleType.Electronics);
            Article tshirt = new Article(3, "Футболка", 499.00m, ArticleType.Clothing);

            Console.WriteLine("\nТовары");
            apple.Print();
            phone.Print();
            tshirt.Print();

            Client client = new Client(101, "Иванов Иван Иванович",
                                       "г. Москва, ул. Ленина 1",
                                       "+7-999-123-45-67", ClientType.Gold);
            Console.WriteLine("\nКлиент");
            client.Print();

            RequestItem[] items = { new RequestItem(apple, 5), new RequestItem(tshirt, 2) };
            Request order = new Request(1001, client, DateTime.Today, items, PayType.Card);
            Console.WriteLine("\nЗаказ");
            order.Print();
        }

        static void DemoStudent()
        {
            Student st = new Student("Петров", "Алексей", "Сергеевич", "21-ИТ", 20);

            st.SetGrade(0, 9); st.SetGrade(0, 8); st.SetGrade(0, 10);
            st.SetGrade(1, 7); st.SetGrade(1, 8);
            st.SetGrade(2, 6); st.SetGrade(2, 9);

            Console.WriteLine("\nСтудент");
            st.Print();
        }

        static void DemoCapitals()
        {
            Russia.Moscow moscow = new Russia.Moscow();
            USA.WashingtonDC washington = new USA.WashingtonDC();
            France.Paris paris = new France.Paris();

            Console.WriteLine("\nНаселение столиц");
            moscow.Print();
            washington.Print();
            paris.Print();

            CapitalInfo[] capitals = new CapitalInfo[]
            {
                new CapitalInfo { Name = moscow.Name, Population = moscow.Population },
                new CapitalInfo { Name = washington.Name, Population = washington.Population },
                new CapitalInfo { Name = paris.Name, Population = paris.Population }
            };

            for (int i = 0; i < capitals.Length - 1; i++)
            {
                for (int j = 0; j < capitals.Length - 1 - i; j++)
                {
                    if (capitals[j].Population < capitals[j + 1].Population)
                    {
                        CapitalInfo temp = capitals[j];
                        capitals[j] = capitals[j + 1];
                        capitals[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine("\nРейтинг по населению:");
            for (int i = 0; i < capitals.Length; i++)
                Console.WriteLine($"  {i + 1}. {capitals[i].Name} — {capitals[i].Population:N0} чел.");
        }
    }
}