using System;

namespace cLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Меню заданий");
                Console.WriteLine("1. Квадраты на прямоугольнике");
                Console.WriteLine("2. Вклад в банке");
                Console.WriteLine("3. Числа от A до B");
                Console.WriteLine("4. Число задом наперёд");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": Task1_Squares();
                        break;
                    case "2": Task2_Deposit(); 
                        break;
                    case "3": Task3_NumbersRange();
                        break;
                    case "4": Task4_ReverseNumber(); 
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }

                Console.WriteLine();
            }
        }

        //Квадраты на прямоугольнике
        static void Task1_Squares()
        {
            Console.Write("Введите сторону A прямоугольника: ");
            int a = int.Parse(Console.ReadLine());

            Console.Write("Введите сторону B прямоугольника: ");
            int b = int.Parse(Console.ReadLine());

            Console.Write("Введите сторону C квадрата: ");
            int c = int.Parse(Console.ReadLine());

            if (c > a || c > b)
            {
                Console.WriteLine("Невозможно разместить ни одного квадрата: сторона квадрата больше стороны прямоугольника");
                return;
            }

            int countByA = a / c;
            int countByB = b / c;
            int totalSquares = countByA * countByB;

            int usedArea = totalSquares * c * c;
            int freeArea = a * b - usedArea;

            Console.WriteLine($"Количество квадратов: {totalSquares}");
            Console.WriteLine($"Площадь незанятой части: {freeArea}");
        }

        //Вклад в банке
        static void Task2_Deposit()
        {
            Console.Write("Введите процент в месяц (0 < P < 25): ");
            double p = double.Parse(Console.ReadLine());

            if (p <= 0 || p >= 25)
            {
                Console.WriteLine("Ошибка: процент должен быть от 0 до 25");
                return;
            }

            double sum = 10000;
            int months = 0;

            while (sum <= 11000)
            {
                sum = sum + sum * p / 100;
                months++;
            }

            Console.WriteLine($"Количество месяцев: {months}");
            Console.WriteLine($"Итоговый вклад: {sum:F2} руб.");
        }

        //Числа от A до B
        static void Task3_NumbersRange()
        {
            Console.Write("Введите число A: ");
            int a = int.Parse(Console.ReadLine());

            Console.Write("Введите число B (B > A): ");
            int b = int.Parse(Console.ReadLine());

            if (a >= b)
            {
                Console.WriteLine("Ошибка: число A должно быть меньше числа B");
                return;
            }

            for (int i = a; i <= b; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    Console.Write(i + " ");
                }
                Console.WriteLine();
            }
        }

        //Число задом наперёд
        static void Task4_ReverseNumber()
        {
            Console.Write("Введите целое число больше 0: ");
            int n = int.Parse(Console.ReadLine());

            if (n <= 0)
            {
                Console.WriteLine("Ошибка: число должно быть больше 0");
                return;
            }

            int reversed = 0;
            int temp = n;

            while (temp > 0)
            {
                int digit = temp % 10;
                reversed = reversed * 10 + digit;
                temp = temp / 10;
            }

            Console.WriteLine($"Число задом наперёд: {reversed}");
        }
    }
}