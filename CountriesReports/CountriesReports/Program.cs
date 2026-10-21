using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace CountriesReports
{
    class Program
    {
        static SqlConnection conn;
        static bool isConnected;

        const string DbConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CountriesDb;Integrated Security=True";
        const string MasterConnectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n1. подключиться к бд");
                Console.WriteLine("2. отключиться от бд");
                Console.WriteLine("3. отчёты");
                Console.WriteLine("0. выход");
                Console.Write("выбор: ");

                switch (Console.ReadLine())
                {
                    case "1": Connect(); break;
                    case "2": Disconnect(); break;
                    case "3": ReportsMenu(); break;
                    case "0": Disconnect(); return;
                    default: Console.WriteLine("неверный выбор"); break;
                }
            }
        }

        //задание 1: подключение / отключение
        static void Connect()
        {
            if (isConnected) { Console.WriteLine("уже подключено"); return; }
            try
            {
                CreateDatabaseIfNotExists();
                conn = new SqlConnection(DbConnectionString);
                conn.Open();
                isConnected = true;
                EnsureSchemaAndSeed();
                Console.WriteLine("успешное подключение к бд");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка подключения: {ex.Message}");
                isConnected = false;
            }
        }

        static void Disconnect()
        {
            if (!isConnected) { Console.WriteLine("нет активного подключения"); return; }
            conn.Close();
            conn.Dispose();
            isConnected = false;
            Console.WriteLine("отключено от бд");
        }

        static void CreateDatabaseIfNotExists()
        {
            using (var masterConn = new SqlConnection(MasterConnectionString))
            {
                masterConn.Open();
                using (var cmd = new SqlCommand("IF DB_ID('CountriesDb') IS NULL CREATE DATABASE CountriesDb", masterConn))
                    cmd.ExecuteNonQuery();
            }
        }

        static void EnsureSchemaAndSeed()
        {
            Execute(@"
                IF OBJECT_ID('dbo.Countries', 'U') IS NULL
                CREATE TABLE Countries (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Population BIGINT NOT NULL,
                    Area FLOAT NOT NULL,
                    PartOfWorld NVARCHAR(100) NOT NULL
                );");

            Execute(@"
                IF OBJECT_ID('dbo.Cities', 'U') IS NULL
                CREATE TABLE Cities (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    CountryId INT NOT NULL,
                    Name NVARCHAR(100) NOT NULL,
                    Population BIGINT NOT NULL,
                    IsCapital BIT NOT NULL DEFAULT 0,
                    CONSTRAINT FK_Cities_Countries FOREIGN KEY (CountryId) REFERENCES Countries(Id)
                );");

            Execute(@"
                IF NOT EXISTS (SELECT 1 FROM Countries)
                INSERT INTO Countries (Name, Population, Area, PartOfWorld) VALUES
                (N'Россия', 146000000, 17098246, N'Европа'),
                (N'Германия', 83000000, 357022, N'Европа'),
                (N'Франция', 67000000, 551695, N'Европа'),
                (N'Китай', 1412000000, 9596961, N'Азия'),
                (N'Индия', 1400000000, 3287263, N'Азия'),
                (N'Египет', 104000000, 1002450, N'Африка'),
                (N'Нигерия', 206000000, 923768, N'Африка'),
                (N'США', 331000000, 9833517, N'Северная Америка'),
                (N'Бразилия', 214000000, 8515767, N'Южная Америка'),
                (N'Австралия', 26000000, 7692024, N'Австралия и Океания');");

            Execute(@"
                IF NOT EXISTS (SELECT 1 FROM Cities)
                INSERT INTO Cities (CountryId, Name, Population, IsCapital) VALUES
                (1, N'Москва', 12500000, 1), (1, N'Санкт-Петербург', 5400000, 0),
                (2, N'Берлин', 3700000, 1), (2, N'Мюнхен', 1500000, 0),
                (3, N'Париж', 2100000, 1),
                (4, N'Пекин', 21500000, 1), (4, N'Шанхай', 24900000, 0),
                (5, N'Нью-Дели', 32000000, 1), (5, N'Мумбаи', 20400000, 0),
                (6, N'Каир', 9500000, 1), (6, N'Александрия', 5200000, 0),
                (7, N'Абуджа', 3600000, 1), (7, N'Лагос', 15000000, 0),
                (8, N'Вашингтон', 700000, 1), (8, N'Нью-Йорк', 8400000, 0),
                (9, N'Бразилиа', 3000000, 1), (9, N'Рио-де-Жанейро', 6700000, 0),
                (10, N'Канберра', 400000, 1), (10, N'Сидней', 5300000, 0);");

            Console.WriteLine("бд инициализирована демо-данными");
        }

        static void ReportsMenu()
        {
            if (!isConnected) { Console.WriteLine("сначала подключитесь к бд"); return; }

            while (true)
            {
                Console.WriteLine("\n1. полная информация о странах");
                Console.WriteLine("2. частичная информация о странах");
                Console.WriteLine("3. информация о конкретной стране");
                Console.WriteLine("4. города конкретной страны");
                Console.WriteLine("5. страны на указанную букву");
                Console.WriteLine("6. столицы на указанную букву");
                Console.WriteLine("7. топ-3 столиц с наименьшим населением");
                Console.WriteLine("8. топ-3 стран с наименьшим населением");
                Console.WriteLine("9. среднее население столиц по частям света");
                Console.WriteLine("10. топ-3 стран по каждой части света (наименьшее)");
                Console.WriteLine("11. топ-3 стран по каждой части света (наибольшее)");
                Console.WriteLine("12. среднее население городов в конкретной стране");
                Console.WriteLine("13. город с наименьшим населением в стране");
                Console.WriteLine("0. назад");
                Console.Write("выбор: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        OutputReport("полная информация о странах",
                        ReadReport("SELECT * FROM Countries ORDER BY Name")); break;
                    case "2": ReportPartialCountries(); break;
                    case "3": ReportSpecificCountry(); break;
                    case "4": ReportCitiesOfCountry(); break;
                    case "5": ReportStartingWith(country: true); break;
                    case "6": ReportStartingWith(country: false); break;
                    case "7": ReportTop3SmallestCapitals(); break;
                    case "8":
                        OutputReport("топ-3 стран с наименьшим населением",
                        ReadReport("SELECT TOP 3 Name, Population FROM Countries ORDER BY Population ASC")); break;
                    case "9":
                        OutputReport("среднее население столиц по частям света",
                        ReadReport(@"SELECT co.PartOfWorld, AVG(CAST(ci.Population AS FLOAT)) AS AvgPop
                                    FROM Cities ci JOIN Countries co ON ci.CountryId = co.Id
                                    WHERE ci.IsCapital = 1
                                    GROUP BY co.PartOfWorld ORDER BY co.PartOfWorld")); break;
                    case "10": ReportTop3ByPart(ascending: true); break;
                    case "11": ReportTop3ByPart(ascending: false); break;
                    case "12": ReportAvgCityPop(); break;
                    case "13": ReportSmallestCity(); break;
                    case "0": return;
                    default: Console.WriteLine("неверный выбор"); break;
                }
            }
        }

        //задание 2.2: частичная информация
        static void ReportPartialCountries()
        {
            string[] cols = { "Name", "Population", "Area", "PartOfWorld" };
            string[] labels = { "название", "население", "площадь", "часть света" };
            Console.WriteLine("какие поля показать? (номера через запятую)");
            for (int i = 0; i < cols.Length; i++) Console.WriteLine($"{i + 1}. {labels[i]}");

            var selected = (Console.ReadLine() ?? "")
                .Split(',')
                .Select(s => s.Trim())
                .Where(s => int.TryParse(s, out int n) && n >= 1 && n <= cols.Length)
                .Select(s => cols[int.Parse(s) - 1])
                .Distinct()
                .ToList();

            if (selected.Count == 0) { Console.WriteLine("ничего не выбрано"); return; }

            var lines = ReadReport($"SELECT {string.Join(", ", selected)} FROM Countries ORDER BY Name");
            OutputReport("частичная информация о странах", lines);
        }

        //задание 2.3: конкретная страна
        static void ReportSpecificCountry()
        {
            Console.Write("название страны: ");
            var name = Console.ReadLine();
            var lines = ReadReport("SELECT * FROM Countries WHERE Name = @n",
                new SqlParameter("@n", name ?? ""));
            if (lines.Count <= 2) lines.Add($"страна '{name}' не найдена");
            OutputReport($"информация о '{name}'", lines);
        }

        //задание 2.4: города страны
        static void ReportCitiesOfCountry()
        {
            Console.Write("название страны: ");
            var name = Console.ReadLine();
            var lines = ReadReport(@"
                SELECT ci.Name AS город, ci.Population AS население, ci.IsCapital AS столица
                FROM Cities ci JOIN Countries co ON ci.CountryId = co.Id
                WHERE co.Name = @n
                ORDER BY ci.Population DESC", new SqlParameter("@n", name ?? ""));
            if (lines.Count <= 2) lines.Add($"для страны '{name}' нет городов");
            OutputReport($"города страны '{name}'", lines);
        }

        //задание 3.1 / 3.2: страны или столицы на букву
        static void ReportStartingWith(bool country)
        {
            Console.Write("первая буква: ");
            var letter = Console.ReadLine() ?? "";

            string sql = country
                ? "SELECT Id, Name, PartOfWorld FROM Countries WHERE Name LIKE @p ORDER BY Name"
                : @"SELECT ci.Name AS столица, co.Name AS страна
                    FROM Cities ci JOIN Countries co ON ci.CountryId = co.Id
                    WHERE ci.IsCapital = 1 AND ci.Name LIKE @p ORDER BY ci.Name";

            var lines = ReadReport(sql, new SqlParameter("@p", letter + "%"));
            if (lines.Count <= 2) lines.Add($"не найдено на '{letter}'");
            OutputReport(country ? $"страны на букву '{letter}'" : $"столицы на букву '{letter}'", lines);
        }

        //задание 3.3: топ-3 столиц с наименьшим населением
        static void ReportTop3SmallestCapitals()
        {
            OutputReport("топ-3 столиц с наименьшим населением", ReadReport(@"
                SELECT TOP 3 ci.Name AS столица, co.Name AS страна, ci.Population AS население
                FROM Cities ci JOIN Countries co ON ci.CountryId = co.Id
                WHERE ci.IsCapital = 1
                ORDER BY ci.Population ASC"));
        }

        //задание 4.1 / 4.2: топ-3 стран по каждой части света
        static void ReportTop3ByPart(bool ascending)
        {
            var lines = new List<string>();
            foreach (var part in GetPartsOfWorld())
            {
                lines.Add($"\n{part}");
                lines.AddRange(ReadReport(
                    $"SELECT TOP 3 Name, Population FROM Countries WHERE PartOfWorld = @p " +
                    $"ORDER BY Population {(ascending ? "ASC" : "DESC")}",
                    new SqlParameter("@p", part)));
            }
            OutputReport($"топ-3 стран по частям света ({(ascending ? "наименьшее" : "наибольшее")} население)", lines);
        }

        //задание 4.3: среднее население городов в стране
        static void ReportAvgCityPop()
        {
            Console.Write("название страны: ");
            var name = Console.ReadLine();
            var lines = ReadReport(@"
                SELECT co.Name AS страна, AVG(CAST(ci.Population AS FLOAT)) AS среднее
                FROM Cities ci JOIN Countries co ON ci.CountryId = co.Id
                WHERE co.Name = @n
                GROUP BY co.Name", new SqlParameter("@n", name ?? ""));
            if (lines.Count <= 2) lines.Add($"нет данных по стране '{name}'");
            OutputReport($"среднее население городов в '{name}'", lines);
        }

        //задание 4.4: город с наименьшим населением в стране
        static void ReportSmallestCity()
        {
            Console.Write("название страны: ");
            var name = Console.ReadLine();
            var lines = ReadReport(@"
                SELECT TOP 1 ci.Name AS город, ci.Population AS население, co.Name AS страна
                FROM Cities ci JOIN Countries co ON ci.CountryId = co.Id
                WHERE co.Name = @n
                ORDER BY ci.Population ASC", new SqlParameter("@n", name ?? ""));
            if (lines.Count <= 2) lines.Add($"нет данных по стране '{name}'");
            OutputReport($"город с наименьшим населением в '{name}'", lines);
        }

        //читает любой select и возвращает строки с заголовком
        static List<string> ReadReport(string sql, params SqlParameter[] prms)
        {
            var lines = new List<string>();
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (prms != null && prms.Length > 0) cmd.Parameters.AddRange(prms);
                using (var r = cmd.ExecuteReader())
                {
                    var cols = new List<string>();
                    for (int i = 0; i < r.FieldCount; i++) cols.Add(r.GetName(i));
                    lines.Add(string.Join(" | ", cols));
                    while (r.Read())
                    {
                        var vals = new List<string>();
                        for (int i = 0; i < r.FieldCount; i++)
                            vals.Add(r.IsDBNull(i) ? "" : r.GetValue(i).ToString());
                        lines.Add(string.Join(" | ", vals));
                    }
                }
            }
            return lines;
        }

        static List<string> GetPartsOfWorld()
        {
            var parts = new List<string>();
            using (var r = new SqlCommand("SELECT DISTINCT PartOfWorld FROM Countries ORDER BY PartOfWorld", conn).ExecuteReader())
                while (r.Read()) parts.Add(r.GetString(0));
            return parts;
        }

        static void OutputReport(string title, List<string> lines)
        {
            Console.WriteLine($"\n{title}");
            Console.Write("1. на экран\n2. в файл\nвыбор: ");
            if (Console.ReadLine() == "2")
            {
                var fileName = $"report_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                File.WriteAllText(fileName, title + "\n\n" + string.Join("\n", lines), Encoding.UTF8);
                Console.WriteLine($"сохранено в файл '{fileName}'");
            }
            else
            {
                foreach (var l in lines) Console.WriteLine(l);
            }
        }

        static void Execute(string sql)
        {
            using (var cmd = new SqlCommand(sql, conn))
                cmd.ExecuteNonQuery();
        }
    }
}