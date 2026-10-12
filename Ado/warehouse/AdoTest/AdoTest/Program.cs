using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WarehouseApp
{
    class Program
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["WarehouseString"].ConnectionString;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (!TestConnection())
            {
                Console.WriteLine("Работа программы невозможна из-за ошибки подключения.");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("УПРАВЛЕНИЕ СКЛАДОМ");
                Console.WriteLine("1. Показать всю информацию о товарах (Задание 3)");
                Console.WriteLine("2. Показать все типы товаров");
                Console.WriteLine("3. Показать всех поставщиков");
                Console.WriteLine("4. Товар с МАКСИМАЛЬНЫМ количеством");
                Console.WriteLine("5. Товар с МИНИМАЛЬНЫМ количеством");
                Console.WriteLine("6. Товар с МИНИМАЛЬНОЙ себестоимостью");
                Console.WriteLine("7. Товар с МАКСИМАЛЬНОЙ себестоимостью");
                Console.WriteLine("8. Показать товары заданной категории (Задание 4)");
                Console.WriteLine("9. Показать товары заданного поставщика");
                Console.WriteLine("10. Показать самый старый товар на складе");
                Console.WriteLine("11. Среднее количество товаров по каждому типу");
                Console.WriteLine("0. Выход");
                Console.Write("\nВыберите пункт меню: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAllProducts(); break;
                    case "2": ShowAllTypes(); break;
                    case "3": ShowAllSuppliers(); break;
                    case "4": ShowProductByAggregate("SELECT TOP 1 * FROM Products ORDER BY Quantity DESC", "Максимальное количество"); break;
                    case "5": ShowProductByAggregate("SELECT TOP 1 * FROM Products ORDER BY Quantity ASC", "Минимальное количество"); break;
                    case "6": ShowProductByAggregate("SELECT TOP 1 * FROM Products ORDER BY CostPrice ASC", "Минимальная себестоимость"); break;
                    case "7": ShowProductByAggregate("SELECT TOP 1 * FROM Products ORDER BY CostPrice DESC", "Максимальная себестоимость"); break;
                    case "8": ShowProductsByCategory(); break;
                    case "9": ShowProductsBySupplier(); break;
                    case "10": ShowProductByAggregate("SELECT TOP 1 * FROM Products ORDER BY DeliveryDate ASC", "Самый старый товар"); break;
                    case "11": ShowAverageQuantityByType(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный пункт меню."); break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        // Задание 2: Метод тестирования подключения
        private static bool TestConnection()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // Пытаемся подключиться
                    Console.WriteLine("[УСПЕХ]: Подключение к базе данных 'Склад' выполнено успешно!");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ОШИБКА]: Не удалось подключиться к базе данных! Текст ошибки:\n{ex.Message}");
                    return false;
                }
            }
        }

        // Задание 3: Отображение всей информации о товаре (с JOIN таблиц типов и поставщиков)
        private static void ShowAllProducts()
        {
            string query = @"SELECT p.Id, p.Title, t.Name AS Type, s.Name AS Supplier, p.Quantity, p.CostPrice, p.DeliveryDate 
                             FROM Products p
                             JOIN ProductTypes t ON p.TypeId = t.Id
                             JOIN Suppliers s ON p.SupplierId = s.Id";

            ExecuteAndPrintTable(query, "СПИСОК ВСЕХ ТОВАРОВ НА СКЛАДЕ");
        }

        private static void ShowAllTypes() => ExecuteAndPrintTable("SELECT * FROM ProductTypes", "ТИПЫ ТОВАРОВ");

        private static void ShowAllSuppliers() => ExecuteAndPrintTable("SELECT * FROM Suppliers", "ПОСТАВЩИКИ");

        // Задание 3 и 4: Вывод одного товара по условию (макс/мин/старый)
        private static void ShowProductByAggregate(string query, string label)
        {
            Console.WriteLine($"{label.ToUpper()}");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            Console.WriteLine($"Товар: {rdr["Title"]} | Кол-во: {rdr["Quantity"]} | Цена: {rdr["CostPrice"]} | Дата: {Convert.ToDateTime(rdr["DeliveryDate"]).ToShortDateString()}");
                        }
                        else Console.WriteLine("База данных пуста.");
                    }
                }
                catch (Exception ex) { Console.WriteLine("Ошибка: " + ex.Message); }
            }
        }

        // Задание 4: Показать товары заданной категории (фильтрация)
        private static void ShowProductsByCategory()
        {
            Console.Write("Введите название категории (типа товара): ");
            string category = Console.ReadLine();

            string query = @"SELECT p.Title, p.Quantity, p.CostPrice FROM Products p
                             JOIN ProductTypes t ON p.TypeId = t.Id WHERE t.Name = @param";

            ExecuteFilterQuery(query, category, $"ТОВАРЫ КАТЕГОРИИ: {category}");
        }

        // Задание 4: Показать товары заданного поставщика (фильтрация)
        private static void ShowProductsBySupplier()
        {
            Console.Write("Введите название поставщика: ");
            string supplier = Console.ReadLine();

            string query = @"SELECT p.Title, p.Quantity, p.CostPrice FROM Products p
                             JOIN Suppliers s ON p.SupplierId = s.Id WHERE s.Name = @param";

            ExecuteFilterQuery(query, supplier, $"ТОВАРЫ ОТ ПОСТАВЩИКА: {supplier}");
        }

        // Задание 4: Среднее количество товаров по каждому типу (Группировка AVG)
        private static void ShowAverageQuantityByType()
        {
            string query = @"SELECT t.Name AS [Тип товара], AVG(p.Quantity) AS [Среднее количество]
                             FROM Products p
                             JOIN ProductTypes t ON p.TypeId = t.Id
                             GROUP BY t.Name";

            ExecuteAndPrintTable(query, "СРЕДНЕЕ КОЛИЧЕСТВО ПО ТИПАМ");
        }

        // Вспомогательный метод для автоматического вывода любой SQL-таблицы в консоль
        private static void ExecuteAndPrintTable(string query, string title)
        {
            Console.WriteLine($"{title}");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        for (int i = 0; i < rdr.FieldCount; i++) Console.Write(rdr.GetName(i) + "\t│ ");
                        Console.WriteLine("\n" + new string('-', 80));

                        while (rdr.Read())
                        {
                            for (int i = 0; i < rdr.FieldCount; i++) Console.Write(rdr[i].ToString() + "\t│ ");
                            Console.WriteLine();
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine("Ошибка: " + ex.Message); }
            }
        }

        // Вспомогательный метод для выполнения безопасных параметризованных запросов
        private static void ExecuteFilterQuery(string query, string parameterValue, string title)
        {
            Console.WriteLine($"{title}");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@param", parameterValue);
                try
                {
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.HasRows) { Console.WriteLine("Ничего не найдено."); return; }
                        while (rdr.Read())
                        {
                            Console.WriteLine($"Товар: {rdr["Title"]} | Количество: {rdr["Quantity"]} | Себестоимость: {rdr["CostPrice"]}");
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine("Ошибка: " + ex.Message); }
            }
        }
    }
}
