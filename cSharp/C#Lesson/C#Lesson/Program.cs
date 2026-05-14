using System;

namespace cLesson
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Меню заданий");
                Console.WriteLine("1. FizzBuzz");
                Console.WriteLine("2. Процент от числа");
                Console.WriteLine("3. Собрать число из четырёх цифр");
                Console.WriteLine("4. Обмен цифр в шестизначном числе");
                Console.WriteLine("5. Сезон и день недели по дате");
                Console.WriteLine("6. Конвертация температуры");
                Console.WriteLine("7. Чётные числа в диапазоне");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": Task1_FizzBuzz();
                        break;
                    case "2": Task2_Percent(); 
                        break;
                    case "3": Task3_FormNumber(); 
                        break;
                    case "4": Task4_SwapDigits();
                        break;
                    case "5": Task5_SeasonAndDay(); 
                        break;
                    case "6": Task6_TemperatureConversion();
                        break;
                    case "7": Task7_EvenNumbers();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод");
                        break;
                }

                Console.WriteLine();
            }
        }

        //FizzBuzz
        static void Task1_FizzBuzz()
        {
            Console.Write("Введите число от 1 до 100: ");
            int num = int.Parse(Console.ReadLine());

            if (num < 1 || num > 100)
            {
                Console.WriteLine("Ошибка: число должно быть от 1 до 100.");
                return;
            }

            if (num % 15 == 0)
                Console.WriteLine("Fizz Buzz");
            else if (num % 3 == 0)
                Console.WriteLine("Fizz");
            else if (num % 5 == 0)
                Console.WriteLine("Buzz");
            else
                Console.WriteLine(num);
        }

        //Процент от числа
        static void Task2_Percent()
        {
            Console.Write("Введите число: ");
            double number = double.Parse(Console.ReadLine());

            Console.Write("Введите процент: ");
            double percent = double.Parse(Console.ReadLine());

            double result = number * percent / 100;
            Console.WriteLine($"{percent}% от {number} = {result}");
        }

        //Собрать число из четырёх цифр
        static void Task3_FormNumber()
        {
            Console.Write("Введите 1-ю цифру: ");
            int d1 = int.Parse(Console.ReadLine());

            Console.Write("Введите 2-ю цифру: ");
            int d2 = int.Parse(Console.ReadLine());

            Console.Write("Введите 3-ю цифру: ");
            int d3 = int.Parse(Console.ReadLine());

            Console.Write("Введите 4-ю цифру: ");
            int d4 = int.Parse(Console.ReadLine());

            int number = d1 * 1000 + d2 * 100 + d3 * 10 + d4;
            Console.WriteLine($"Сформированное число: {number}");
        }

        //Обмен цифр в шестизначном числе
        static void Task4_SwapDigits()
        {
            Console.Write("Введите шестизначное число: ");
            string input = Console.ReadLine();

            if (input.Length != 6)
            {
                Console.WriteLine("Ошибка: число должно быть шестизначным.");
                return;
            }

            Console.Write("Введите номер первого разряда (1-6): ");
            int pos1 = int.Parse(Console.ReadLine());

            Console.Write("Введите номер второго разряда (1-6): ");
            int pos2 = int.Parse(Console.ReadLine());

            char[] chars = input.ToCharArray();
            char temp = chars[pos1 - 1];
            chars[pos1 - 1] = chars[pos2 - 1];
            chars[pos2 - 1] = temp;

            Console.WriteLine($"Результат: {new string(chars)}");
        }

        //Сезон и день недели по дате
        static void Task5_SeasonAndDay()
        {
            Console.Write("Введите дату (дд.мм.гггг): ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            string season;
            int month = date.Month;

            if (month >= 3 && month <= 5)
                season = "Spring";
            else if (month >= 6 && month <= 8)
                season = "Summer";
            else if (month >= 9 && month <= 11)
                season = "Fall";
            else
                season = "Winter";

            Console.WriteLine($"{season} {date.DayOfWeek}");
        }

        //Конвертация температуры
        static void Task6_TemperatureConversion()
        {
            Console.WriteLine("1. Фаренгейт -> Цельсий");
            Console.WriteLine("2. Цельсий -> Фаренгейт");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            Console.Write("Введите температуру: ");
            double temp = double.Parse(Console.ReadLine());

            if (choice == "1")
            {
                double result = (temp - 32) * 5 / 9;
                Console.WriteLine($"{temp} °F = {result:F1} °C");
            }
            else if (choice == "2")
            {
                double result = temp * 9 / 5 + 32;
                Console.WriteLine($"{temp} °C = {result:F1} °F");
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }

        //Чётные числа в диапазоне
        static void Task7_EvenNumbers()
        {
            Console.Write("Введите первое число: ");
            int a = int.Parse(Console.ReadLine());

            Console.Write("Введите второе число: ");
            int b = int.Parse(Console.ReadLine());

            int start = Math.Min(a, b);
            int end = Math.Max(a, b);

            Console.Write($"Чётные числа от {start} до {end}: ");
            for (int i = start; i <= end; i++)
            {
                if (i % 2 == 0)
                    Console.Write(i + " ");
            }
            Console.WriteLine();
        }
    }
}