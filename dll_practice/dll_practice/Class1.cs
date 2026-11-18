using System;

namespace my_dll
{
    public class info
    {
        public static void show(string text)
        {
            Console.WriteLine("сообщение: " + text);
        }
    }

    public class year_helper
    {
        public static bool is_leap(int year)
        {
            return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
        }
    }

    public class numbers
    {
        public static int max(int a, int b, int c)
        {
            return Math.Max(a, Math.Max(b, c));
        }

        public static int min(int a, int b, int c)
        {
            return Math.Min(a, Math.Min(b, c));
        }

        public static int sum(int a, int b, int c)
        {
            return a + b + c;
        }
    }

    public class calculator
    {
        public static long factorial(int n)
        {
            long res = 1;
            for (int i = 2; i <= n; i++)
            {
                res *= i;
            }
            return res;
        }

        public static bool is_prime(int n)
        {
            if (n < 2) return false;
            for (int i = 2; i < n; i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        public static bool is_even(int n)
        {
            return n % 2 == 0;
        }

        public static bool is_odd(int n)
        {
            return n % 2 != 0;
        }
    }

    public class fraction
    {
        public int num;
        public int den;

        public fraction(int n, int d)
        {
            num = n;
            den = d == 0 ? 1 : d;
        }

        public void show()
        {
            Console.WriteLine(num + "/" + den);
        }

        public fraction add(fraction other)
        {
            int new_num = num * other.den + other.num * den;
            int new_den = den * other.den;
            return new fraction(new_num, new_den);
        }

        public fraction multiply(fraction other)
        {
            return new fraction(num * other.num, den * other.den);
        }
    }
}
