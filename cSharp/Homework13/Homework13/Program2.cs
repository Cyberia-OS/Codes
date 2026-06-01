using System;

namespace Homework13
{
    internal class Program2
    {
        static void Main()
        {
            Person person = new Person();
            person.PropertyChanged += (s, e) => Console.WriteLine($"Свойство '{e.PropertyName}' изменено.");

            person.Name = "Мария";
            person.Age = 30;
            person.Age = 30;   
            person.Name = "Анна";
        }
    }

    //аргументы события
    public class PropertyEventArgs : EventArgs
    {
        public string PropertyName { get; }
        public PropertyEventArgs(string name) => PropertyName = name;
    }

    //делегат
    public delegate void PropertyEventHandler(object sender, PropertyEventArgs e);

    //интерфейс
    interface IPropertyChanged
    {
        event PropertyEventHandler PropertyChanged;
    }

    //класс с уведомлениями об изменениях
    public class Person : IPropertyChanged
    {
        private string name;
        private int age;

        public event PropertyEventHandler PropertyChanged;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyEventArgs(nameof(Name)));
            }
        }

        public int Age
        {
            get => age;
            set
            {
                age = value;
                PropertyChanged?.Invoke(this, new PropertyEventArgs(nameof(Age)));
            }
        }
    }
}