using System;
using System.Configuration; 
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StudentGradesApp
{
    class Program
    {
       
        static string currentConfigName = "SqlServerConnectionString";
        static string currentProvider = "";
        static string connectionString = "";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("System.Data.SQLite", System.Data.SQLite.SQLiteFactory.Instance);

            LoadConnectionString();

            while (true)
            {
                Console.WriteLine($"\nОценки студентов (Провайдер: {currentProvider})");
                Console.WriteLine("1. Подключиться к БД");
                Console.WriteLine("2. Показать все оценки");
                Console.WriteLine("3. Показать всех студентов");
                Console.WriteLine("4. Показать все предметы");
                Console.WriteLine("5. Добавить оценку");
                Console.WriteLine("6. Удалить оценку");
                Console.WriteLine("7. Сменить СУБД");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");

                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1": await TestConnectionAsync();
                            break;
                        case "2": await ShowGradesAsync();
                            break;
                        case "3": await ShowStudentsAsync();
                            break;
                        case "4": await ShowSubjectsAsync();
                            break;
                        case "5": await AddGradeAsync(); 
                            break;
                        case "6": await DeleteGradeAsync(); 
                            break;
                        case "7": SwitchProvider(); 
                            break;
                        case "0": return;
                        default: Console.WriteLine("Неверный выбор"); 
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ошибочка: " + ex.Message);
                }
            }
        }

        //смена СУБД во время работы программы
        static void SwitchProvider()
        {
            currentConfigName = currentConfigName == "SqlServerConnectionString"
                ? "SqliteConnectionString"
                : "SqlServerConnectionString";

            LoadConnectionString();
            Console.WriteLine($"СУБД изменена. Новый провайдер: {currentProvider}");
        }

        //задание 2 чтение параметров подключения из App.config
        static void LoadConnectionString()
        {
            var settings = ConfigurationManager.ConnectionStrings[currentConfigName];
            if (settings == null)
            {
                throw new Exception($"Строка подключения '{currentConfigName}' не найдена в App.config.");
            }

            connectionString = settings.ConnectionString;
            currentProvider = settings.ProviderName; 
        }

        //задание 1 создание универсального подключения через абстрактную фабрику
        static DbConnection CreateConnection()
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(currentProvider);
            DbConnection conn = factory.CreateConnection()!;
            conn.ConnectionString = connectionString;
            return conn;
        }

        //Задание 3 и 7 синхронное чтение данных
        static async Task RunWithTimerAsync(string sql, Action<DbDataReader> handler)
        {
            Stopwatch sw = Stopwatch.StartNew();

            await using DbConnection conn = CreateConnection();
            await conn.OpenAsync();

            await using DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            await using DbDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                handler(reader);

            sw.Stop();
            Console.WriteLine($"\nЗапрос выполнен за {sw.Elapsed.TotalSeconds:F3} сек.");
        }

        static async Task TestConnectionAsync()
        {
            await using DbConnection conn = CreateConnection();
            await conn.OpenAsync();
            Console.WriteLine("Успешное подключение к БД!");
        }

        static async Task ShowGradesAsync()
        {
            string sql = @"SELECT g.Id, s.FullName AS StudentName, sub.Name AS SubjectName,
                                  g.Grade, g.GradeDate
                           FROM Grades g
                           JOIN Students s ON s.Id = g.StudentId
                           JOIN Subjects sub ON sub.Id = g.SubjectId";

            await RunWithTimerAsync(sql, reader =>
            {
                Console.WriteLine($"{reader["Id"]} | {reader["StudentName"]} | " +
                                  $"{reader["SubjectName"]} | {reader["Grade"]} | {reader["GradeDate"]}");
            });
        }

        static async Task ShowStudentsAsync()
        {
            await RunWithTimerAsync("SELECT Id, FullName, GroupName FROM Students", reader =>
            {
                Console.WriteLine($"{reader["Id"]} | {reader["FullName"]} | {reader["GroupName"]}");
            });
        }

        static async Task ShowSubjectsAsync()
        {
            await RunWithTimerAsync("SELECT Id, Name FROM Subjects", reader =>
            {
                Console.WriteLine($"{reader["Id"]} | {reader["Name"]}");
            });
        }

        //задание 4 асинхронное добавление данных
        static async Task AddGradeAsync()
        {
            Console.Write("StudentId (число): ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            { Console.WriteLine("Должно быть число!"); return; }

            Console.Write("SubjectId (число): ");
            if (!int.TryParse(Console.ReadLine(), out int subjectId))
            { Console.WriteLine("Должно быть число!"); return; }

            Console.Write("Оценка (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out int grade))
            { Console.WriteLine("Должно быть число!"); return; }

            Stopwatch sw = Stopwatch.StartNew(); // Задание 7: Замер времени

            await using DbConnection conn = CreateConnection();
            await conn.OpenAsync();

            await using DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Grades (StudentId, SubjectId, Grade, GradeDate)
                                VALUES (@S, @Sub, @G, @D)";
            AddParam(cmd, "@S", studentId);
            AddParam(cmd, "@Sub", subjectId);
            AddParam(cmd, "@G", grade);
            AddParam(cmd, "@D", DateTime.Now.ToString("yyyy-MM-dd"));

            int rows = await cmd.ExecuteNonQueryAsync();
            sw.Stop();

            Console.WriteLine($"Добавлено строк: {rows}");
            Console.WriteLine($"Операция выполнена за {sw.Elapsed.TotalSeconds:F3} сек.");
        }

        //задание 5 асинхронное удаление данных
        static async Task DeleteGradeAsync()
        {
            Console.Write("Id оценки для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            { Console.WriteLine("Должно быть число!"); return; }

            Stopwatch sw = Stopwatch.StartNew(); // Задание 7: Замер времени

            await using DbConnection conn = CreateConnection();
            await conn.OpenAsync();

            await using DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Grades WHERE Id = @Id";
            AddParam(cmd, "@Id", id);

            int rows = await cmd.ExecuteNonQueryAsync();
            sw.Stop();

            Console.WriteLine($"Удалено строк: {rows}");
            Console.WriteLine($"Операция выполнена за {sw.Elapsed.TotalSeconds:F3} сек.");
        }

        static void AddParam(DbCommand cmd, string paramName, object value)
        {
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = value;
            cmd.Parameters.Add(param);
        }
    }
}
