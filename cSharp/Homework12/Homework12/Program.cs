using System;

//пользовательские исключения
class BadAmountEx : Exception
{
    public BadAmountEx(string message) : base(message) { }
}

class NoFundsEx : Exception
{
    public NoFundsEx(string message) : base(message) { }
}

//класс счета
class Account
{
    private static int counter = 0;
    public string Number { get; private set; }
    public decimal Balance { get; private set; }

    public Account()
    {
        counter++;
        Number = $"ACCT-{counter:D4}";
        Balance = 0;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new BadAmountEx("Сумма пополнения должна быть положительной");
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new BadAmountEx("Сумма снятия должна быть положительной");
        if (amount > Balance)
            throw new NoFundsEx("Недостаточно средств на счете");
        Balance -= amount;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("задание 1 Безопасное деление");
        Task1();

        Console.WriteLine("\nзадание 2 Банковский счёт");
        Task2();

        Console.WriteLine("\nпрограмма завершена");
    }

    static void Task1()
    {
        decimal[] results = new decimal[3];
        int index = 0;

        while (index < 3)
        {
            try
            {
                Console.Write("Введите первое число: ");
                int a = int.Parse(Console.ReadLine());

                Console.Write("Введите второе число: ");
                int b = int.Parse(Console.ReadLine());

                decimal result = (decimal)a / b;
                results[index] = result;
                index++;
                Console.WriteLine($"Результат: {result}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: введите целое число");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Ошибка: деление на ноль невозможно");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Массив результатов заполнен, дальнейшие вычисления невозможны");
                break;
            }
            finally
            {
                Console.WriteLine("Попытка выполнения операции завершена");
            }

            if (index < 3)
            {
                Console.Write("Хотите продолжить? (да/нет): ");
                if (Console.ReadLine()?.ToLower() != "да")
                    break;
            }
        }

        Console.WriteLine("\nУспешные результаты:");
        for (int i = 0; i < index; i++)
            Console.WriteLine($"{i + 1}: {results[i]}");
    }

    static void Task2()
    {
        Account acc = new Account();
        Console.WriteLine($"Номер счета: {acc.Number}");

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nДоступные операции:");
            Console.WriteLine("1 - Пополнить");
            Console.WriteLine("2 - Снять");
            Console.WriteLine("3 - Показать баланс");
            Console.WriteLine("4 - Выйти");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    try
                    {
                        Console.Write("Введите сумму пополнения: ");
                        decimal amount = decimal.Parse(Console.ReadLine());
                        acc.Deposit(amount);
                        Console.WriteLine("Счёт успешно пополнен");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Ошибка: введите число");
                    }
                    catch (BadAmountEx ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
                    }
                    finally
                    {
                        Console.WriteLine($"Текущий баланс: {acc.Balance:F2}");
                    }
                    break;

                case "2":
                    try
                    {
                        Console.Write("Введите сумму снятия: ");
                        decimal amount = decimal.Parse(Console.ReadLine());
                        acc.Withdraw(amount);
                        Console.WriteLine("Снятие выполнено");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Ошибка: введите число");
                    }
                    catch (BadAmountEx ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    catch (NoFundsEx ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
                    }
                    finally
                    {
                        Console.WriteLine($"Текущий баланс: {acc.Balance:F2}");
                    }
                    break;

                case "3":
                    Console.WriteLine($"Текущий баланс: {acc.Balance:F2}");
                    break;

                case "4":
                    exit = true;
                    Console.WriteLine("Программа завершена.");
                    break;

                default:
                    Console.WriteLine("Неверный выбор, попробуйте снова");
                    break;
            }
        }
    }
}