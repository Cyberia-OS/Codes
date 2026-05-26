using System;
using System.Collections.Generic;

namespace Homework10
{
    //задание 1 геометрические фигуры
    abstract class Shape
    {
        public abstract double Area();
        public abstract double Perimeter();
        public abstract string Name { get; }

        public virtual void Print()  
        {
            Console.WriteLine($"{Name}: площадь = {Area():F2}, периметр = {Perimeter():F2}");
        }
    }

    class Triangle : Shape
    {
        private double a, b, c;
        public Triangle(double a, double b, double c) { this.a = a; this.b = b; this.c = c; }
        public override double Area()
        {
            double p = Perimeter() / 2;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
        public override double Perimeter() => a + b + c;
        public override string Name => "Треугольник";
    }

    class Square : Shape
    {
        private double side;
        public Square(double side) { this.side = side; }
        public override double Area() => side * side;
        public override double Perimeter() => 4 * side;
        public override string Name => "Квадрат";
    }

    class Rectangle : Shape
    {
        private double width, height;
        public Rectangle(double width, double height) { this.width = width; this.height = height; }
        public override double Area() => width * height;
        public override double Perimeter() => 2 * (width + height);
        public override string Name => "Прямоугольник";
    }

    class Rhombus : Shape
    {
        private double side, d1, d2;
        public Rhombus(double side, double d1, double d2) { this.side = side; this.d1 = d1; this.d2 = d2; }
        public override double Area() => (d1 * d2) / 2;
        public override double Perimeter() => 4 * side;
        public override string Name => "Ромб";
    }

    class Parallelogram : Shape
    {
        private double b, side, h;
        public Parallelogram(double b, double side, double h) { this.b = b; this.side = side; this.h = h; }
        public override double Area() => b * h;
        public override double Perimeter() => 2 * (b + side);
        public override string Name => "Параллелограмм";
    }

    class Trapezoid : Shape
    {
        private double a, b, c, d, h;
        public Trapezoid(double a, double b, double c, double d, double h)
        {
            this.a = a; this.b = b; this.c = c; this.d = d; this.h = h;
        }
        public override double Area() => (a + b) * h / 2;
        public override double Perimeter() => a + b + c + d;
        public override string Name => "Трапеция";
    }

    class Circle : Shape
    {
        private double r;
        public Circle(double r) { this.r = r; }
        public override double Area() => Math.PI * r * r;
        public override double Perimeter() => 2 * Math.PI * r;
        public override string Name => "Круг";
    }

    class Ellipse : Shape
    {
        private double semiMajor, semiMinor;
        public Ellipse(double semiMajor, double semiMinor) { this.semiMajor = semiMajor; this.semiMinor = semiMinor; }
        public override double Area() => Math.PI * semiMajor * semiMinor;
        public override double Perimeter()
        {
            double a = semiMajor, b = semiMinor;
            double h = Math.Pow((a - b) / (a + b), 2);
            return Math.PI * (a + b) * (1 + 3 * h / (10 + Math.Sqrt(4 - 3 * h)));
        }
        public override string Name => "Эллипс";
    }

    class CompoundShape
    {
        private List<Shape> shapes = new List<Shape>();
        public void Add(Shape s) => shapes.Add(s);
        public double TotalArea()
        {
            double sum = 0;
            foreach (var s in shapes) sum += s.Area();
            return sum;
        }
        public void Print()
        {
            Console.WriteLine($"Составная фигура (фигур: {shapes.Count})");
            foreach (var s in shapes) s.Print();
            Console.WriteLine($"Общая площадь: {TotalArea():F2}");
        }
    }

    //задание 2 иерархия товаров
    enum FlowType { Arrival, Sold, WrittenOff, Transferred }

    abstract class Product
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product(int code, string name, decimal price, int quantity)
        {
            Code = code; Name = name; Price = price; Quantity = quantity;
        }

        public abstract void Print();
    }

    class HouseholdChemical : Product
    {
        public string HazardClass { get; set; }
        public HouseholdChemical(int code, string name, decimal price, int quantity, string hazardClass)
            : base(code, name, price, quantity) { HazardClass = hazardClass; }

        public override void Print()
        {
            Console.WriteLine($"[{Code}] {Name} (химия), цена: {Price:C}, остаток: {Quantity}, класс опасности: {HazardClass}");
        }
    }

    class FoodProduct : Product
    {
        public DateTime ExpiryDate { get; set; }
        public FoodProduct(int code, string name, decimal price, int quantity, DateTime expiryDate)
            : base(code, name, price, quantity) { ExpiryDate = expiryDate; }

        public override void Print()
        {
            Console.WriteLine($"[{Code}] {Name} (продукты), цена: {Price:C}, остаток: {Quantity}, годен до: {ExpiryDate:dd.MM.yyyy}");
        }
    }

    class ProductManager
    {
        private Product product;
        public ProductManager(Product p) => product = p;

        public void Arrival(int qty)
        {
            product.Quantity += qty;
            Console.WriteLine($"Поступило {qty} ед. '{product.Name}'. Остаток: {product.Quantity}");
        }

        public void Sold(int qty)
        {
            if (qty > product.Quantity) { Console.WriteLine("Недостаточно товара!"); return; }
            product.Quantity -= qty;
            Console.WriteLine($"Продано {qty} ед. '{product.Name}'. Остаток: {product.Quantity}");
        }

        public void WrittenOff(int qty)
        {
            if (qty > product.Quantity) { Console.WriteLine("Недостаточно товара!"); return; }
            product.Quantity -= qty;
            Console.WriteLine($"Списано {qty} ед. '{product.Name}'. Остаток: {product.Quantity}");
        }

        public void Transferred(int qty, string destination)
        {
            if (qty > product.Quantity) { Console.WriteLine("Недостаточно товара!"); return; }
            product.Quantity -= qty;
            Console.WriteLine($"Передано {qty} ед. '{product.Name}' в {destination}. Остаток: {product.Quantity}");
        }
    }

    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n1 - Геометрические фигуры");
                Console.WriteLine("2 - Управление товарами");
                Console.WriteLine("0 - Выход");
                Console.Write("Введите: ");
                string choice = Console.ReadLine();
                if (choice == "0") 
                    break;
                else if (choice == "1") DemoShapes();
                else if (choice == "2") DemoProducts();
                else Console.WriteLine("Неверный ввод");
            }
        }

        static void DemoShapes()
        {
            Shape[] shapes = {
                new Triangle(3, 4, 5),
                new Square(5),
                new Rhombus(5, 6, 8), 
                new Rectangle(4, 6),
                new Parallelogram(7, 5, 4),
                new Trapezoid(8, 5, 4, 4, 3.5), 
                new Circle(3),
                new Ellipse(5, 3)
            };

            Console.WriteLine("\nВсе фигуры:");
            foreach (var s in shapes) s.Print();

            var compound = new CompoundShape();
            compound.Add(new Square(4));
            compound.Add(new Circle(2));
            compound.Add(new Triangle(5, 5, 6));
            Console.WriteLine("\nСоставная фигура:");
            compound.Print();
        }

        static void DemoProducts()
        {
            var chem = new HouseholdChemical(1, "Отбеливатель", 89.99m, 50, "3");
            var food = new FoodProduct(2, "Молоко", 65.50m, 200, new DateTime(2026, 6, 1));

            Console.WriteLine("\nСписок товаров:");
            chem.Print();
            food.Print();

            var manager = new ProductManager(chem);
            Console.WriteLine("\nОперации с 'Отбеливатель':");
            manager.Arrival(20);
            manager.Sold(15);
            manager.WrittenOff(5);
            manager.Transferred(10, "Склад №2");
            chem.Print();
        }
    }
}