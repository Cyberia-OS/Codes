using System;
using System.Windows;

namespace MonitorApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            txtШирина.Text = SystemParameters.PrimaryScreenWidth.ToString();
            txtВысота.Text = SystemParameters.PrimaryScreenHeight.ToString();
        }

        private void Рассчитать_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(txtШирина.Text, out double ширина) ||
                !double.TryParse(txtВысота.Text, out double высота))
            {
                MessageBox.Show("Введите корректные числа!");
                return;
            }

            //диагональ в пикселях по теореме Пифагора
            double диагональПикс = Math.Sqrt(ширина * ширина + высота * высота);

            //пиксели в дюймы
            double диагональДюймов = диагональПикс / 96;

            txtРезультат.Text = $"Диагональ: {диагональДюймов:F1} дюймов";
        }
    }
}