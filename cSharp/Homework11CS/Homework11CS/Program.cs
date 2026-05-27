using System;

namespace Homework11CS
{
    //задание 1
    class Human
    {
        protected string name = "";
        protected int age;
        protected string gender = "";

        public void SetName(string value) { name = value; }
        public string GetName() { return name; }

        public void SetAge(int value) { age = value; }
        public int GetAge() { return age; }

        public void SetGender(string value) { gender = value; }
        public string GetGender() { return gender; }

        public virtual void Input()
        {
            Console.Write("Имя: ");
            name = Console.ReadLine() ?? "";
            Console.Write("Возраст: ");
            int.TryParse(Console.ReadLine(), out age);
            Console.Write("Пол: ");
            gender = Console.ReadLine() ?? "";
        }

        public virtual void Print()
        {
            Console.WriteLine($"Имя: {name}, Возраст: {age}, Пол: {gender}");
        }
    }

    class Builder : Human
    {
        protected string trade = "";
        protected int experience;

        public void SetTrade(string value) { trade = value; }
        public string GetTrade() { return trade; }

        public void SetExperience(int value) { experience = value; }
        public int GetExperience() { return experience; }

        public override void Input()
        {
            base.Input();
            Console.Write("Специальность (каменщик/электрик): ");
            trade = Console.ReadLine() ?? "";
            Console.Write("Стаж (лет): ");
            int.TryParse(Console.ReadLine(), out experience);
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Специальность: {trade}, Стаж: {experience} лет");
        }
    }

    class Sailor : Human
    {
        protected string rank = "";
        protected int seaYears;

        public void SetRank(string value) { rank = value; }
        public string GetRank() { return rank; }

        public void SetSeaYears(int value) { seaYears = value; }
        public int GetSeaYears() { return seaYears; }

        public override void Input()
        {
            base.Input();
            Console.Write("Звание: ");
            rank = Console.ReadLine() ?? "";
            Console.Write("Морских лет: ");
            int.TryParse(Console.ReadLine(), out seaYears);
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Звание: {rank}, Морской стаж: {seaYears} лет");
        }
    }

    class Pilot : Human
    {
        protected string aircraft = "";
        protected int hours;

        public void SetAircraft(string value) { aircraft = value; }
        public string GetAircraft() { return aircraft; }

        public void SetHours(int value) { hours = value; }
        public int GetHours() { return hours; }

        public override void Input()
        {
            base.Input();
            Console.Write("Тип самолёта: ");
            aircraft = Console.ReadLine() ?? "";
            Console.Write("Налёт (часов): ");
            int.TryParse(Console.ReadLine(), out hours);
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Самолёт: {aircraft}, Налёт: {hours} ч");
        }
    }

    //задание 2
    class Passport
    {
        protected string number = "";
        protected string fullName = "";
        protected DateTime birthDate;
        protected string birthPlace = "";
        protected DateTime issueDate;
        protected string issuedBy = "";
        protected string country = "";   // поле, которое было в коде А и нужно по условию

        public void SetNumber(string value) { number = value; }
        public string GetNumber() { return number; }

        public void SetFullName(string value) { fullName = value; }
        public string GetFullName() { return fullName; }

        public void SetBirthDate(DateTime value) { birthDate = value; }
        public DateTime GetBirthDate() { return birthDate; }

        public void SetBirthPlace(string value) { birthPlace = value; }
        public string GetBirthPlace() { return birthPlace; }

        public void SetIssueDate(DateTime value) { issueDate = value; }
        public DateTime GetIssueDate() { return issueDate; }

        public void SetIssuedBy(string value) { issuedBy = value; }
        public string GetIssuedBy() { return issuedBy; }

        public void SetCountry(string value) { country = value; }
        public string GetCountry() { return country; }

        public virtual void Input()
        {
            Console.Write("Серия и номер: ");
            number = Console.ReadLine() ?? "";
            Console.Write("ФИО: ");
            fullName = Console.ReadLine() ?? "";
            Console.Write("Дата рождения (гггг-мм-дд): ");
            DateTime.TryParse(Console.ReadLine(), out birthDate);
            Console.Write("Место рождения: ");
            birthPlace = Console.ReadLine() ?? "";
            Console.Write("Гражданство: ");
            country = Console.ReadLine() ?? "";
            Console.Write("Дата выдачи (гггг-мм-дд): ");
            DateTime.TryParse(Console.ReadLine(), out issueDate);
            Console.Write("Кем выдан: ");
            issuedBy = Console.ReadLine() ?? "";
        }

        public virtual void Print()
        {
            Console.WriteLine($"Паспорт {number}");
            Console.WriteLine($"{fullName}, род. {birthDate:dd.MM.yyyy}");
            Console.WriteLine($"Место: {birthPlace}, Страна: {country}");
            Console.WriteLine($"Выдан: {issueDate:dd.MM.yyyy}, {issuedBy}");
        }
    }

    class ForeignPassport : Passport
    {
        protected string foreignNumber = "";
        protected string[] visas = new string[10];
        protected int visaCount;

        public void SetForeignNumber(string value) { foreignNumber = value; }
        public string GetForeignNumber() { return foreignNumber; }

        public void AddVisa(string visa)
        {
            if (visaCount < visas.Length)
                visas[visaCount++] = visa;
        }

        public string[] GetVisas() { return visas; }
        public int GetVisaCount() { return visaCount; }

        public override void Input()
        {
            base.Input();
            Console.Write("Номер загранпаспорта: ");
            foreignNumber = Console.ReadLine() ?? "";
            Console.Write("Количество виз: ");
            int count;
            int.TryParse(Console.ReadLine(), out count);
            for (int i = 0; i < count; i++)
            {
                Console.Write($"Виза {i + 1}: ");
                AddVisa(Console.ReadLine() ?? "");
            }
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Загранпаспорт: {foreignNumber}");
            Console.WriteLine("Визы:");
            for (int i = 0; i < visaCount; i++)
                Console.WriteLine($"  - {visas[i]}");
        }
    }

    //задание 3
    class Animal
    {
        protected string name;
        protected string habitat;

        public Animal(string name, string habitat)
        {
            this.name = name;
            this.habitat = habitat;
        }

        public string GetName() { return name; }
        public string GetHabitat() { return habitat; }

        public virtual void Print()
        {
            Console.WriteLine($"{name} обитает в {habitat}");
        }
    }

    class Tiger : Animal
    {
        protected string stripeColor;

        public Tiger(string name, string habitat, string stripeColor) : base(name, habitat)
        {
            this.stripeColor = stripeColor;
        }

        public string GetStripeColor() { return stripeColor; }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Это тигр с полосами цвета: {stripeColor}");
        }
    }

    class Crocodile : Animal
    {
        protected double length;

        public Crocodile(string name, string habitat, double length) : base(name, habitat)
        {
            this.length = length;
        }

        public double GetLength() { return length; }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Крокодил длиной {length} м");
        }
    }

    class Kangaroo : Animal
    {
        protected double jumpHeight;

        public Kangaroo(string name, string habitat, double jumpHeight) : base(name, habitat)
        {
            this.jumpHeight = jumpHeight;
        }

        public double GetJumpHeight() { return jumpHeight; }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Кенгуру прыгает на {jumpHeight} м в высоту");
        }
    }

    //задание 4
    abstract class Figure
    {
        public abstract double GetArea();
    }

    class Rectangle : Figure
    {
        protected double width, height;

        public Rectangle(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        public override double GetArea() => width * height;
    }

    class Circle : Figure
    {
        protected double radius;

        public Circle(double radius)
        {
            this.radius = radius;
        }

        public override double GetArea() => Math.PI * radius * radius;
    }

    class RightTriangle : Figure
    {
        protected double legA, legB;

        public RightTriangle(double a, double b)
        {
            legA = a;
            legB = b;
        }

        public override double GetArea() => 0.5 * legA * legB;
    }

    class Trapezoid : Figure
    {
        protected double baseA, baseB, height;

        public Trapezoid(double a, double b, double h)
        {
            baseA = a;
            baseB = b;
            height = h;
        }

        public override double GetArea() => 0.5 * (baseA + baseB) * height;
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n1 - Профессии (Строитель, Моряк, Пилот)");
                Console.WriteLine("2 - Паспорт и загранпаспорт");
                Console.WriteLine("3 - Животные (Тигр, Крокодил, Кенгуру)");
                Console.WriteLine("4 - Геометрические фигуры");
                Console.WriteLine("0 - Выход");
                Console.Write("Введите: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

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
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод"); 
                        break;
                }
            }
        }

        static void Task1()
        {
            Builder builder = new Builder();
            Sailor sailor = new Sailor();
            Pilot pilot = new Pilot();

            Console.WriteLine("Строитель");
            builder.Input();
            Console.WriteLine("\nМоряк");
            sailor.Input();
            Console.WriteLine("\nПилот");
            pilot.Input();

            Console.WriteLine("\nРезультат");
            builder.Print();
            Console.WriteLine();
            sailor.Print();
            Console.WriteLine();
            pilot.Print();
        }

        static void Task2()
        {
            Console.WriteLine("Внутренний паспорт");
            Passport passport = new Passport();
            passport.Input();

            Console.WriteLine("\nЗаграничный паспорт");
            ForeignPassport foreign = new ForeignPassport();
            foreign.Input();

            Console.WriteLine("\nРезультат");
            passport.Print();
            Console.WriteLine();
            foreign.Print();
        }

        static void Task3()
        {
            Tiger tiger = new Tiger("Амур", "Уссурийская тайга", "чёрный/оранжевый");
            Crocodile croc = new Crocodile("Нильский", "Реки Африки", 4.5);
            Kangaroo kangaroo = new Kangaroo("Рыжий", "Австралия", 2.8);

            Console.WriteLine("Животные:");
            tiger.Print();
            croc.Print();
            kangaroo.Print();
        }

        static void Task4()
        {
            Figure[] figures = new Figure[]
            {
                new Rectangle(5, 10),
                new Circle(7),
                new RightTriangle(3, 4),
                new Trapezoid(6, 10, 4)
            };

            string[] names = { "Прямоугольник", "Круг", "Прямоугольный треугольник", "Трапеция" };

            for (int i = 0; i < figures.Length; i++)
            {
                Console.WriteLine($"{names[i]}: площадь = {figures[i].GetArea():F2}");
            }
        }
    }
}