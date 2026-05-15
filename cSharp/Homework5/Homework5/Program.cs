using System;

namespace Homework5
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n1 - Товар (Article)");
                Console.WriteLine("2 - Клиент (Client)");
                Console.WriteLine("3 - Позиция заказа (RequestItem)");
                Console.WriteLine("4 - Заказ (Request)");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                if (choice == "1")
                    Task1();            
                else if (choice == "2")
                    Task2();                 
                else if (choice == "3")
                    Task3();              
                else if (choice == "4")
                    Task4();              
                else if (choice == "0")
                    break;             
                else
                    Console.WriteLine("Неверный ввод");
            }
        }

        //задание 1
        static void Task1()
        {
            Article item; 

            Console.Write("Код товара: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out item.Code))
            {
                Console.WriteLine("Ошибка! Код = 0");
                item.Code = 0;
            }

            Console.Write("Название товара: ");
            item.Name = Console.ReadLine();

            Console.Write("Цена товара: ");
            input = Console.ReadLine();
            if (!double.TryParse(input, out item.Price))
            {
                Console.WriteLine("Ошибка! Цена = 0");
                item.Price = 0;
            }

            Console.WriteLine("Тип товара (0 - Food, 1 - Electronics, 2 - Clothing, 3 - Other): ");
            input = Console.ReadLine();
            int typeInt;

            //проверяем что ввели целое число от 0 до 3
            if (!int.TryParse(input, out typeInt) || typeInt < 0 || typeInt > 3)
            {
                Console.WriteLine("Неверный тип. Установлен Other");
                item.Type = ArticleType.Other;
            }
            else
            {
                //приводим число к значению перечисления ArticleType
                item.Type = (ArticleType)typeInt;
            }

            Console.WriteLine("Данные товара:");
            item.Print();   // вызов метода Print структуры Article
        }

        //задание 2 ввод данных о клиенте
        static void Task2()
        {
            Client client;

            Console.Write("Код клиента: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out client.Code))
            {
                Console.WriteLine("Ошибка! Код = 0");
                client.Code = 0;
            }

            Console.Write("ФИО: ");
            client.FullName = Console.ReadLine();

            Console.Write("Адрес: ");
            client.Address = Console.ReadLine();

            Console.Write("Телефон: ");
            client.Phone = Console.ReadLine();

            Console.Write("Количество заказов: ");
            input = Console.ReadLine();
            if (!int.TryParse(input, out client.OrderCount))
            {
                Console.WriteLine("Ошибка! Количество = 0");
                client.OrderCount = 0;
            }

            Console.Write("Общая сумма заказов: ");
            input = Console.ReadLine();
            if (!double.TryParse(input, out client.TotalSum))
            {
                Console.WriteLine("Ошибка! Сумма = 0");
                client.TotalSum = 0;
            }

            Console.WriteLine("Тип клиента (0 - Regular, 1 - Silver, 2 - Gold, 3 - VIP): ");
            input = Console.ReadLine();
            int typeInt;

            if (!int.TryParse(input, out typeInt) || typeInt < 0 || typeInt > 3)
            {
                Console.WriteLine("Неверный тип. Установлен Regular");
                client.Type = ClientType.Regular;
            }
            else
            {
                client.Type = (ClientType)typeInt;
            }

            Console.WriteLine("Данные клиента:");
            client.Print();   
        }

        //задание 3
        static void Task3()
        {
            Article item;  

            Console.Write("Код товара: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out item.Code))
            {
                Console.WriteLine("Ошибка! Код = 0");
                item.Code = 0;
            }

            Console.Write("Название товара: ");
            item.Name = Console.ReadLine();

            Console.Write("Цена товара: ");
            input = Console.ReadLine();
            if (!double.TryParse(input, out item.Price))
            {
                Console.WriteLine("Ошибка! Цена = 0");
                item.Price = 0;
            }

            item.Type = ArticleType.Other;  

            RequestItem requestItem;    
            requestItem.Article = item;     

            Console.Write("Количество единиц: ");
            input = Console.ReadLine();
            if (!int.TryParse(input, out requestItem.Quantity))
            {
                Console.WriteLine("Ошибка Количество = 0");
                requestItem.Quantity = 0;
            }

            Console.WriteLine("Данные позиции заказа:");
            requestItem.Print();  
        }

        //задание 4 создание заказа 
        static void Task4()
        {
            Client client;
            client.Code = 1;
            client.FullName = "Иванов Иван Иванович";
            client.Address = "г. Москва, ул. Ленина, д. 5";
            client.Phone = "+7-999-123-45-67";
            client.OrderCount = 3;
            client.TotalSum = 15000;
            client.Type = ClientType.Gold;

            Article item1;
            item1.Code = 101;
            item1.Name = "Ноутбук";
            item1.Price = 50000;
            item1.Type = ArticleType.Electronics;

            Article item2;
            item2.Code = 102;
            item2.Name = "Мышь";
            item2.Price = 1500;
            item2.Type = ArticleType.Electronics;

            RequestItem pos1;
            pos1.Article = item1;
            pos1.Quantity = 1;

            RequestItem pos2;
            pos2.Article = item2;
            pos2.Quantity = 2;

            Request order;
            order.Code = 1001;
            order.Client = client;
            order.Date = "15.05.2026";
            order.Items = new RequestItem[] { pos1, pos2 }; 
            order.Pay = PayType.Card;                     

            Console.WriteLine("Данные заказа:");
            order.Print();   
        }
    }


    //перечисление возможных типов товара (задание 5)
    enum ArticleType
    {
        Food,       
        Electronics, 
        Clothing,     
        Other       
    }

    //перечисление категорий важности клиента (задание 6)
    enum ClientType
    {
        Regular,
        Silver,  
        Gold,     
        VIP     
    }

    //перечисление форм оплаты (задание 7)
    enum PayType
    {
        Cash,       
        Card,      
        BankTransfer
    }

    //товар
    struct Article
    {
        public int Code;        
        public string Name;     
        public double Price;       
        public ArticleType Type;   

        public void Print()
        {
            Console.WriteLine("Код:      " + Code);
            Console.WriteLine("Название: " + Name);
            Console.WriteLine("Цена:     " + Price);
            Console.WriteLine("Тип:      " + Type);
        }
    }

    //структура клиент
    struct Client
    {
        public int Code;           
        public string FullName;    
        public string Address;  
        public string Phone;    
        public int OrderCount;    
        public double TotalSum;  
        public ClientType Type;   

        public void Print()
        {
            Console.WriteLine("Код:            " + Code);
            Console.WriteLine("ФИО:            " + FullName);
            Console.WriteLine("Адрес:          " + Address);
            Console.WriteLine("Телефон:        " + Phone);
            Console.WriteLine("Кол-во заказов: " + OrderCount);
            Console.WriteLine("Общая сумма:    " + TotalSum);
            Console.WriteLine("Тип клиента:    " + Type);
        }
    }

    //структура Позиция заказа
    struct RequestItem
    {
        public Article Article;  
        public int Quantity;  

        public void Print()
        {
            Console.WriteLine("  Товар:      " + Article.Name);
            Console.WriteLine("  Цена:       " + Article.Price);
            Console.WriteLine("  Количество: " + Quantity);
        }
    }

    //структура заказ
    struct Request
    {
        public int Code;               
        public Client Client;          
        public string Date;             
        public RequestItem[] Items;     
        public PayType Pay;            

        //вычисляемое свойство TotalSum автоматически считает общую стоимость заказа сумма цен всех товаров с учётом количества
        public double TotalSum
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < Items.Length; i++)
                    sum += Items[i].Article.Price * Items[i].Quantity;
                return sum;
            }
        }

        public void Print()
        {
            Console.WriteLine("Код заказа:   " + Code);
            Console.WriteLine("Дата:         " + Date);
            Console.WriteLine("Форма оплаты: " + Pay);
            Console.WriteLine("Клиент:       " + Client.FullName);
            Console.WriteLine("Товары:");
            for (int i = 0; i < Items.Length; i++)
            {
                Console.WriteLine(" Позиция " + (i + 1) + " ---");
                Items[i].Print();
            }
            Console.WriteLine("Сумма заказа: " + TotalSum);
        }
    }
}