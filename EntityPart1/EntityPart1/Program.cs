using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MailingList
{
    class Program
    {
        static SqlConnection conn;
        static bool isConnected;
        const string DbConn =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MailingListDb;Integrated Security=True";

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n1. подключиться к бд");
                Console.WriteLine("2. отключиться");
                Console.WriteLine("3. тесты (задание 1)");
                Console.WriteLine("4. отчёты (задания 2-5)");
                Console.WriteLine("0. выход");
                Console.Write("выбор: ");

                switch (Console.ReadLine())
                {
                    case "1": Connect(); 
                        break;
                    case "2": Disconnect(); 
                        break;
                    case "3": RunTests(); 
                        break;
                    case "4": ReportsMenu(); 
                        break;
                    case "0": Disconnect();
                        return;
                }
            }
        }

        static void Connect()
        {
            if (isConnected) { Console.WriteLine("уже подключено"); return; }
            try
            {
                conn = new SqlConnection(DbConn);
                conn.Open();
                isConnected = true;
                Console.WriteLine("успешное подключение");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка: {ex.Message}");
                isConnected = false;
            }
        }

        static void Disconnect()
        {
            if (!isConnected) return;
            conn.Close(); conn.Dispose(); isConnected = false;
            Console.WriteLine("отключено");
        }

        static void RunTests()
        {
            if (!isConnected) { Console.WriteLine("сначала подключитесь"); return; }
            int passed = 0, failed = 0;
            void Assert(bool ok, string name)
            {
                if (ok) { Console.WriteLine($"  PASS: {name}"); passed++; }
                else { Console.WriteLine($"  FAIL: {name}"); failed++; }
            }

            Console.WriteLine("\nтесты:");
            Assert(ScalarInt("SELECT COUNT(*) FROM vw_SectionPopularity") == 4, "vw_SectionPopularity");
            Assert(ScalarInt("SELECT COUNT(*) FROM vw_SectionPromoCount") == 4, "vw_SectionPromoCount");
            Assert(ScalarInt("SELECT dbo.fn_DaysUntilPromoEnd(DATEADD(DAY,5,CAST(GETDATE() AS DATE)))") == 5, "fn_DaysUntilPromoEnd");

            try { new SqlCommand("EXEC sp_Top3Popular", conn).ExecuteNonQuery(); Assert(true, "sp_Top3Popular"); }
            catch { Assert(false, "sp_Top3Popular"); }
            try { new SqlCommand("EXEC sp_Top3ByGender", conn).ExecuteNonQuery(); Assert(true, "sp_Top3ByGender"); }
            catch { Assert(false, "sp_Top3ByGender"); }

            var before = ScalarInt("SELECT COUNT(*) FROM ArchiveLog");
            Execute(@"INSERT INTO PromoArchive(OriginalProductId,ProductName,SectionName,PromoEndDate)
                      VALUES (999,N'test',N'test',CAST(GETDATE() AS DATE))");
            var after = ScalarInt("SELECT COUNT(*) FROM ArchiveLog");
            Assert(after == before + 1, "триггер trg_LogArchive");
            Execute("DELETE FROM PromoArchive WHERE OriginalProductId = 999");

            Console.WriteLine($"\nитог: {passed} passed, {failed} failed");
        }

        static void ReportsMenu()
        {
            if (!isConnected) { Console.WriteLine("сначала подключитесь"); return; }
            var map = new Dictionary<string, string> {
                {"1","sp_Top3Popular"},{"2","sp_MostPopular"},{"3","sp_Top3Unpopular"},{"4","sp_MostUnpopular"},
                {"5","sp_Top3ByPromo"},{"6","sp_MostPromo"},{"7","sp_Top3ByLeastPromo"},{"8","sp_LeastPromo"},
                {"9","sp_PromosEndingIn3Days"},{"10","sp_ExpiredPromos"},{"11","sp_ArchiveExpiredPromos"},
                {"12","sp_AvgAgeBySection"},{"13","sp_AvgAgeByCity"},{"14","sp_AvgAgeByCountry"},
                {"15","sp_MostPopularByGender"},{"16","sp_Top3ByGender"},
                {"17","sp_CustomerCountByGender"},{"18","sp_CustomerCountByGenderCountry"}
            };

            while (true)
            {
                foreach (var kv in map) Console.WriteLine($"{kv.Key}. {kv.Value}");
                Console.WriteLine("0. назад");
                Console.Write("выбор: ");
                var input = Console.ReadLine();
                if (input == "0") return;
                if (map.ContainsKey(input)) RunSP(map[input]);
                else Console.WriteLine("неверный выбор");
            }
        }

        static void RunSP(string name)
        {
            try
            {
                using (var cmd = new SqlCommand("EXEC " + name, conn))
                using (var r = cmd.ExecuteReader())
                {
                    var cols = new List<string>();
                    for (int i = 0; i < r.FieldCount; i++) cols.Add(r.GetName(i));
                    Console.WriteLine(string.Join(" | ", cols));
                    int rows = 0;
                    while (r.Read())
                    {
                        var vals = new List<string>();
                        for (int i = 0; i < r.FieldCount; i++)
                        {
                            var v = r.IsDBNull(i) ? "" : r.GetValue(i);
                            vals.Add(v is double d ? d.ToString("F2") : v.ToString());
                        }
                        Console.WriteLine(string.Join(" | ", vals));
                        rows++;
                    }
                    if (rows == 0) Console.WriteLine("(нет данных)");
                }
            }
            catch (Exception ex) { Console.WriteLine($"ошибка: {ex.Message}"); }
        }

        static void Execute(string sql) { using (var cmd = new SqlCommand(sql, conn)) cmd.ExecuteNonQuery(); }
        static int ScalarInt(string sql) { using (var cmd = new SqlCommand(sql, conn)) return Convert.ToInt32(cmd.ExecuteScalar()); }
    }
}