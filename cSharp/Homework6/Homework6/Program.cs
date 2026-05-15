using System;

namespace Homework6
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine(" Выберите задание:");
                Console.WriteLine(" 1 - Строка цифр - int");
                Console.WriteLine(" 2 - Двоичная строка - int");
                Console.WriteLine(" 3 - Кредитная карточка");
                Console.WriteLine(" 4 - Вычислить выражение a*b*c");
                Console.WriteLine(" 0 - Выход");
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
                    case "0":
                        return;
                    default: Console.WriteLine("Неверный пункт меню");
                        break;
                }
            }
        }

        //задание 1
        static void Task1()
        {
            Console.Write("Введите строку цифр (0-9): ");
            string input = Console.ReadLine();

            try
            {
                int result = int.Parse(input);
                Console.WriteLine($"Результат: {result}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: строка содержит недопустимые символы");
            }
            catch (OverflowException)
            {
                Console.WriteLine($"Ошибка: число выходит за границы int [{int.MinValue} ; {int.MaxValue}].");
            }
        }

        //задание 2
        static void Task2()
        {
            Console.Write("Введите строку из 0 и 1: ");
            string input = Console.ReadLine();

            try
            {
                if (string.IsNullOrWhiteSpace(input))
                    throw new FormatException("Строка не должна быть пустой");

                foreach (char c in input)
                    if (c != '0' && c != '1')
                        throw new FormatException($"Недопустимый символ: '{c}'. Разрешены только 0 и 1");

                long longResult = 0;
                foreach (char c in input)
                {
                    longResult = longResult * 2 + (c - '0');
                    if (longResult > int.MaxValue)
                        throw new OverflowException("Число слишком большое для типа int");
                }

                Console.WriteLine($"Двоичное \"{input}\" = {(int)longResult} (десятичное)");
            }
            catch (FormatException ex) { Console.WriteLine($"Ошибка формата: {ex.Message}"); }
            catch (OverflowException ex) { Console.WriteLine($"Ошибка переполнения: {ex.Message}"); }
        }

        //задание 3
        static void Task3()
        {
            Console.Write("Номер карты (16 цифр): ");
            string number = Console.ReadLine();

            Console.Write("ФИО владельца: ");
            string owner = Console.ReadLine();

            Console.Write("CVC (3 цифры): ");
            string cvc = Console.ReadLine();

            Console.Write("Срок действия (MM/YY): ");
            string expiry = Console.ReadLine();

            try
            {
                CreditCard card = new CreditCard(number, owner, cvc, expiry);
                Console.WriteLine("Карта успешно создана: " + card);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        //задание 4
        static void Task4()
        {
            Console.Write("Введите выражение (например 3*2*4): ");
            string input = Console.ReadLine();

            try
            {
                if (string.IsNullOrWhiteSpace(input))
                    throw new FormatException("Выражение не должно быть пустым.");

                string[] parts = input.Split('*');

                if (parts.Length < 2)
                    throw new FormatException("Выражение должно содержать хотя бы один знак '*'.");

                long result = 1;
                foreach (string part in parts)
                {
                    if (!int.TryParse(part.Trim(), out int num))
                        throw new FormatException($"'{part.Trim()}' не является целым числом.");
                    result *= num;
                }

                Console.WriteLine($"Результат: {input} = {result}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Ошибка ввода: {ex.Message}");
            }
        }
    }

    //класс кредитная каточка

    class CreditCard
    {
        public string CardNumber { get; private set; }
        public string OwnerName { get; private set; }
        public string CVC { get; private set; }
        public string ExpiryDate { get; private set; }  

        public CreditCard(string cardNumber, string ownerName, string cvc, string expiryDate)
        {
            if (cardNumber == null || cardNumber.Length != 16)
                throw new ArgumentException("Номер карты должен содержать ровно 16 цифр");

            foreach (char c in cardNumber)
                if (!char.IsDigit(c))
                    throw new ArgumentException("Номер карты должен содержать только цифры");

            if (string.IsNullOrWhiteSpace(ownerName))
                throw new ArgumentException("ФИО владельца не может быть пустым");

            if (cvc == null || cvc.Length != 3)
                throw new ArgumentException("CVC должен содержать ровно 3 цифры");

            foreach (char c in cvc)
                if (!char.IsDigit(c))
                    throw new ArgumentException("CVC должен содержать только цифры");

            if (expiryDate == null || expiryDate.Length != 5 || expiryDate[2] != '/')
                throw new ArgumentException("Срок действия должен быть в формате MM/YY");

            int month = int.Parse(expiryDate.Substring(0, 2));
            if (month < 1 || month > 12)
                throw new ArgumentException("Месяц должен быть от 01 до 12");

            CardNumber = cardNumber;
            OwnerName = ownerName;
            CVC = cvc;
            ExpiryDate = expiryDate;
        }

        public override string ToString()
        {
            return $"**** **** **** {CardNumber.Substring(12)}, {OwnerName}, срок: {ExpiryDate}";
        }
    }
}