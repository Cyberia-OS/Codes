using System;
using my_dll;

namespace my_app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("задание 1");
            info.show("привет, это тест библиотеки");

            Console.WriteLine("\nзадание 2");
            int y = 2024;
            Console.WriteLine("год " + y + " високосный: " + year_helper.is_leap(y));

            Console.WriteLine("\nзадание 3");
            Console.WriteLine("максимум: " + numbers.max(5, 12, 3));
            Console.WriteLine("минимум: " + numbers.min(5, 12, 3));
            Console.WriteLine("сумма: " + numbers.sum(5, 12, 3));

            Console.WriteLine("\nзадание 4");
            int n = 5;
            Console.WriteLine("факториал: " + calculator.factorial(n));
            Console.WriteLine("простое: " + calculator.is_prime(n));
            Console.WriteLine("четное: " + calculator.is_even(n));
            Console.WriteLine("нечетное: " + calculator.is_odd(n));

            Console.WriteLine("\nзадание 5");
            fraction f1 = new fraction(1, 2);
            fraction f2 = new fraction(3, 4);
            Console.Write("первая дробь: ");
            f1.show();
            Console.Write("вторая дробь: ");
            f2.show();
            Console.Write("сложение: ");
            f1.add(f2).show();
            Console.Write("умножение: ");
            f1.multiply(f2).show();

            Console.ReadKey();
        }
    }
}
