using System;
using System.IO;  
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Console;

namespace HomeworkLesson10
{
    [Serializable]
    public class PaymentInvoice : ISerializable
    {
        public double DailyPayment { get; set; } //оплата за день
        public int Days { get; set; } //количество дней
        public double DailyPenalty { get; set; } //штраф за день задержки
        public int DelayDays { get; set; } //количество дней задержки

        //вычисляемые поля считаются из базовых
        public double TotalWithoutPenalty => DailyPayment * Days;
        public double Penalty => DailyPenalty * DelayDays;
        public double TotalAmount => TotalWithoutPenalty + Penalty;

        // Статическое свойство управляет сериализацией вычисляемых полей
        public static bool SerializeAllFields { get; set; } = true;

        public PaymentInvoice() { }

        public PaymentInvoice(double dailyPayment, int days, double dailyPenalty, int delayDays)
        {
            DailyPayment = dailyPayment;
            Days = days;
            DailyPenalty = dailyPenalty;
            DelayDays = delayDays;
        }

        //специальный конструктор для десериализации
        private PaymentInvoice(SerializationInfo info, StreamingContext context)
        {
            DailyPayment = info.GetDouble("DailyPayment");
            Days = info.GetInt32("Days");
            DailyPenalty = info.GetDouble("DailyPenalty");
            DelayDays = info.GetInt32("DelayDays");
        }

        //реализация ISerializable
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("DailyPayment", DailyPayment);
            info.AddValue("Days", Days);
            info.AddValue("DailyPenalty", DailyPenalty);
            info.AddValue("DelayDays", DelayDays);

            if (SerializeAllFields)
            {
                info.AddValue("TotalWithoutPenalty", TotalWithoutPenalty);
                info.AddValue("Penalty", Penalty);
                info.AddValue("TotalAmount", TotalAmount);
                WriteLine("  [Сериализация] Все поля (включая вычисляемые) сохранены.");
            }
            else
            {
                WriteLine("  [Сериализация] Только базовые поля сохранены.");
            }
        }

        public override string ToString()
        {
            return
                $"  Оплата за день:      {DailyPayment,10:F2} руб.\n" +
                $"  Количество дней:     {Days,10}\n" +
                $"  Штраф за день:       {DailyPenalty,10:F2} руб.\n" +
                $"  Дней задержки:       {DelayDays,10}\n" +
                $"  Сумма без штрафа:    {TotalWithoutPenalty,10:F2} руб.\n" +
                $"  Штраф:               {Penalty,10:F2} руб.\n" +
                $"  Итого к оплате:      {TotalAmount,10:F2} руб.";
        }
    }

    class Program
    {
        static void SerializeToFile(PaymentInvoice invoice, string filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = File.Create(filePath))
                formatter.Serialize(stream, invoice);
        }

        static PaymentInvoice DeserializeFromFile(string filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = File.OpenRead(filePath))
                return (PaymentInvoice)formatter.Deserialize(stream);
        }

        static void Main(string[] args)
        {
            PaymentInvoice invoice = new PaymentInvoice(200.0, 30, 15.0, 5);

            WriteLine("исходный счет");
            WriteLine(invoice);

            //SerializeAllFields = true
            WriteLine("\nSerializeAllFields = TRUE");
            PaymentInvoice.SerializeAllFields = true;
            try
            {
                WriteLine("\nСериализация:");
                SerializeToFile(invoice, "invoice_full.bin");

                WriteLine("Десериализация:");
                PaymentInvoice loaded = DeserializeFromFile("invoice_full.bin");
                WriteLine("\nСчёт после десериализации:");
                WriteLine(loaded);
            }
            catch (Exception ex) { WriteLine($"Ошибка: {ex.Message}"); }

            //SerializeAllFields = false
            WriteLine("\nSerializeAllFields = FALSE");
            PaymentInvoice.SerializeAllFields = false;
            try
            {
                WriteLine("\nСериализация:");
                SerializeToFile(invoice, "invoice_basic.bin");

                WriteLine("Десериализация:");
                PaymentInvoice loaded = DeserializeFromFile("invoice_basic.bin");
                WriteLine("\nСчёт после десериализации:");
                WriteLine(loaded);
            }
            catch (Exception ex) { WriteLine($"Ошибка: {ex.Message}"); }
        }
    }
}
