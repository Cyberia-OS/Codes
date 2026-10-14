using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace AdoNetLab2
{
    class Program
    {
        static string connectionString =
            ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== ЗАПУСК ПРАКТИЧЕСКОГО ЗАНЯТИЯ: ОТСОЕДИНЁННЫЙ РЕЖИМ ===\n");

            Task1_ManualDataTable();
            Task2_DataSetFill();
            Task3_AutoUpdate();
            Task4_CustomUpdate();
            Task5_RowStateOrder();
            Task6_TableMappings();

            Console.WriteLine("\nВсе задания выполнены. Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void PrintTable(DataTable table, string title = "")
        {
            if (!string.IsNullOrEmpty(title))
                Console.WriteLine($"\n=== {title} ===");

            foreach (DataColumn col in table.Columns)
                Console.Write(col.ColumnName.PadRight(15));
            Console.WriteLine();
            Console.WriteLine(new string('-', table.Columns.Count * 15));

            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    Console.WriteLine("[Строка удалена локально (Deleted)]");
                    continue;
                }

                foreach (DataColumn col in table.Columns)
                {
                    object val = row[col];
                    string s = val == DBNull.Value ? "NULL" : val.ToString();
                    Console.Write(s.PadRight(15));
                }
                Console.WriteLine();
            }
        }

        //задание 1. datatable вручную
        static void Task1_ManualDataTable()
        {
            Console.WriteLine("\n########## ЗАДАНИЕ 1. DataTable вручную ##########");

            DataTable table = new DataTable("Authors");

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("LastName", typeof(string));

            DataRow row1 = table.NewRow();
            row1["Id"] = 1;
            row1["FirstName"] = "Isaac";
            row1["LastName"] = "Asimov";
            table.Rows.Add(row1);

            DataRow row2 = table.NewRow();
            row2["Id"] = 2;
            row2["FirstName"] = "Ray";
            row2["LastName"] = "Bradbury";
            table.Rows.Add(row2);

            table.Rows.Add(3, "Clifford", "Simak");
            table.Rows.Add(4, "Roger", "Zelazny");

            DataRow demoRow = table.Rows[0];
            Console.WriteLine($"\n[Чтение] row[0] = {demoRow[0]}, " +
                              $"row[\"FirstName\"] = {demoRow["FirstName"]}");

            PrintTable(table, "Authors (manual)");
        }

        //задание 2. dataset + sqldataadapter 
        static void Task2_DataSetFill()
        {
            Console.WriteLine("\n########## ЗАДАНИЕ 2. DataSet + SqlDataAdapter ##########");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Books", conn);
                DataSet ds = new DataSet();

                da.Fill(ds, "Books");

                PrintTable(ds.Tables["Books"], "Books из БД (Task 2)");
            }
        }

        //задание 3. update через sqlcommandbuilder + проблемный эксперимент
        static void Task3_AutoUpdate()
        {
            Console.WriteLine("\n########## ЗАДАНИЕ 3. SqlCommandBuilder ##########");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Books", conn);
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();

                da.Fill(ds, "Books");
                DataTable table = ds.Tables["Books"];

                if (table.Rows.Count > 0)
                {
                    Console.WriteLine($"\nЦена первой книги ДО изменений: {table.Rows[0]["Price"]}");
                    table.Rows[0]["Price"] = 999;
                    Console.WriteLine($"Цена первой книги ПОСЛЕ локального изменения: {table.Rows[0]["Price"]}");

                    Console.WriteLine("\n--- Сгенерированные SqlCommandBuilder команды ---");
                    Console.WriteLine("INSERT: " + cmdBuilder.GetInsertCommand().CommandText);
                    Console.WriteLine("UPDATE: " + cmdBuilder.GetUpdateCommand().CommandText);
                    Console.WriteLine("DELETE: " + cmdBuilder.GetDeleteCommand().CommandText);

                    da.Update(ds, "Books");

                    DataSet dsVerify = new DataSet();
                    da.Fill(dsVerify, "Books");
                    PrintTable(dsVerify.Tables["Books"], "Данные в БД после синхронизации (Task 3)");
                }

                Console.WriteLine("\n--- Проблемный эксперимент: SELECT без Id ---");
                try
                {
                    SqlDataAdapter badDa = new SqlDataAdapter(
                        "SELECT Title, Price, Pages FROM Books", conn);
                    SqlCommandBuilder badBuilder = new SqlCommandBuilder(badDa);

                    DataSet badDs = new DataSet();
                    badDa.Fill(badDs, "Books");

                    if (badDs.Tables["Books"].Rows.Count > 0)
                    {
                        badDs.Tables["Books"].Rows[0]["Price"] = 1234;
                        badDa.Update(badDs, "Books");
                    }
                    Console.WriteLine("Эксперимент неожиданно завершился без ошибки.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка (ожидаемая): " + ex.GetType().Name);
                    Console.WriteLine("Сообщение: " + ex.Message);
                }
            }
        }

        //задание 4. пользовательский updatecommand
        static void Task4_CustomUpdate()
        {
            Console.WriteLine("\n########## ЗАДАНИЕ 4. Пользовательский UpdateCommand ##########");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Books", conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "Books");

                SqlCommand updateCmd = new SqlCommand(
                    "UPDATE Books SET Price = @pPrice WHERE Id = @pId", conn);

                updateCmd.Parameters.Add(new SqlParameter("@pPrice", SqlDbType.Int));
                updateCmd.Parameters["@pPrice"].SourceVersion = DataRowVersion.Current;
                updateCmd.Parameters["@pPrice"].SourceColumn = "Price";

                updateCmd.Parameters.Add(new SqlParameter("@pId", SqlDbType.Int));
                updateCmd.Parameters["@pId"].SourceVersion = DataRowVersion.Original;
                updateCmd.Parameters["@pId"].SourceColumn = "Id";

                da.UpdateCommand = updateCmd;

                DataTable table = ds.Tables["Books"];
                if (table.Rows.Count > 1)
                {
                    table.Rows[0]["Price"] = 888;
                    table.Rows[1]["Price"] = 777;

                    da.Update(ds, "Books");

                    DataSet dsVerify = new DataSet();
                    da.Fill(dsVerify, "Books");
                    PrintTable(dsVerify.Tables["Books"],
                        "После пользовательского UpdateCommand (Task 4)");
                }
            }
        }

        //задание 5. управление порядком синхронизации (dataviewrowstate)
        static void Task5_RowStateOrder()
        {
            Console.WriteLine("\n########## ЗАДАНИЕ 5. Порядок синхронизации ##########");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Books", conn);
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds, "Books");

                DataTable books = ds.Tables["Books"];

                if (books.Rows.Count > 1)
                {
                    //1) удаляем
                    books.Rows[0].Delete();
                    //2) изменяем
                    books.Rows[1]["Price"] = 777;
                    //3) добавляем
                    DataRow newRow = books.NewRow();
                    newRow["AuthorId"] = 1;
                    newRow["Title"] = "New Book";
                    newRow["Price"] = 500;
                    newRow["Pages"] = 100;
                    books.Rows.Add(newRow);

                    Console.WriteLine("\n--- Текущие RowState строк ---");
                    foreach (DataRow row in books.Rows)
                    {
                        string title = row.RowState == DataRowState.Deleted
                            ? "[Deleted]"
                            : row["Title"].ToString();
                        Console.WriteLine($"{title,-20} -> {row.RowState}");
                    }

                    da.Update(books.Select(null, null, DataViewRowState.Deleted));
                    da.Update(books.Select(null, null, DataViewRowState.ModifiedCurrent));
                    da.Update(books.Select(null, null, DataViewRowState.Added));

                    Console.WriteLine("\nСинхронизация Deleted -> Modified -> Added выполнена.");

                    //перечитываем из бд
                    DataSet dsVerify = new DataSet();
                    da.Fill(dsVerify, "Books");
                    PrintTable(dsVerify.Tables["Books"], "БД после Task 5");
                }
            }
        }

        //задание 6. tablemappings
        static void Task6_TableMappings()
        {
            Console.WriteLine("\n########## ЗАДАНИЕ 6. TableMappings ##########");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Authors; SELECT * FROM Books;";

                //6.1 без tablemappings
                Console.WriteLine("\n--- Без TableMappings ---");
                SqlDataAdapter da1 = new SqlDataAdapter(sql, conn);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);

                foreach (DataTable t in ds1.Tables)
                    Console.WriteLine($"Имя таблицы по умолчанию: {t.TableName}");

                //6.2 с tablemappings
                Console.WriteLine("\n--- С TableMappings ---");
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();

                DataTableMapping dtmAuthors = da.TableMappings.Add("Table", "Authors");
                dtmAuthors.ColumnMappings.Add("Id", "Num");
                dtmAuthors.ColumnMappings.Add("FirstName", "Name");
                dtmAuthors.ColumnMappings.Add("LastName", "Surname");

                da.TableMappings.Add("Table1", "Books");

                da.Fill(ds);

                PrintTable(ds.Tables["Authors"],
                    "Authors (переименованные колонки)");
                PrintTable(ds.Tables["Books"],
                    "Books (переименованная таблица)");
            }
        }
    }
}