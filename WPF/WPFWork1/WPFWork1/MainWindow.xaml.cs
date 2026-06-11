using System.IO;
using System.Text;
using System.Windows;
using System.Xml;

namespace XmlOrdersApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Сохранить_Click(object sender, RoutedEventArgs e)
        {
            //Проверяем что поля не пустые
            if (txtId.Text == "" || txtClient.Text == "" || txtProduct.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            //создаём XML файл
            XmlTextWriter writer = new XmlTextWriter("orders.xml", Encoding.UTF8);

            writer.WriteStartDocument();

            writer.WriteStartElement("Заказы");   

            writer.WriteStartElement("Заказ");   
            writer.WriteAttributeString("номер", txtId.Text);
            writer.WriteAttributeString("клиент", txtClient.Text);

            writer.WriteStartElement("Товар"); 
            writer.WriteAttributeString("название", txtProduct.Text);
            writer.WriteEndElement();          

            writer.WriteEndElement();             

            writer.WriteEndElement();               
            writer.WriteEndDocument();

            writer.Close();

            txtВывод.Text = "Файл сохранён!";
        }

        //"Загрузить"
        private void Загрузить_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("orders.xml"))
            {
                MessageBox.Show("Сначала сохраните данные!");
                return;
            }

            string результат = "";

            //через XmlDocument
            результат += "XmlDocument\n";

            XmlDocument doc = new XmlDocument();
            doc.Load("orders.xml");

            XmlNode заказ = doc.GetElementsByTagName("Заказ")[0];
            результат += "Заказ " + заказ.Attributes["номер"].Value + "\n";
            результат += "Клиент: " + заказ.Attributes["клиент"].Value + "\n";

            XmlNode товар = doc.GetElementsByTagName("Товар")[0];
            результат += "Товар: " + товар.Attributes["название"].Value + "\n\n";

            //через XmlTextReader
            результат += "XmlTextReader\n";

            XmlTextReader reader = new XmlTextReader("orders.xml");

            while (reader.Read())
            {
                if (reader.Name == "Заказ")
                    результат += "Заказ " + reader.GetAttribute("номер") +
                                 ", Клиент: " + reader.GetAttribute("клиент") + "\n";

                if (reader.Name == "Товар")
                    результат += "Товар: " + reader.GetAttribute("название") + "\n";
            }

            reader.Close();

            txtВывод.Text = результат;
        }
    }
}