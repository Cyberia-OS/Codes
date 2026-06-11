using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        double firstNumber = 0; 
        string operation = ""; 
        bool newNumber = true;  

        public MainWindow()
        {
            InitializeComponent();
        }

        //нажали цифру 0-9
        private void Digit_Click(object sender, RoutedEventArgs e)
        {
            string digit = (sender as Button).Content.ToString();

            if (newNumber)
            {
                txtCurrent.Text = digit;
                newNumber = false;
            }
            else
            {
                if (txtCurrent.Text == "0")
                    txtCurrent.Text = digit;
                else
                    txtCurrent.Text += digit;
            }
        }

        //точка
        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            //точку только если её ещё нет
            if (!txtCurrent.Text.Contains("."))
                txtCurrent.Text += ".";
        }

        //операции
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            firstNumber = double.Parse(txtCurrent.Text);
            operation = (sender as Button).Content.ToString();
            txtHistory.Text = firstNumber + " " + operation;
            newNumber = true;
        }

        //=
        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            if (operation == "") return;

            double secondNumber = double.Parse(txtCurrent.Text);
            double result = 0;

            switch (operation)
            {
                case "+": result = firstNumber + secondNumber; break;
                case "-": result = firstNumber - secondNumber; break;
                case "*": result = firstNumber * secondNumber; break;
                case "/":
                    if (secondNumber == 0)
                    {
                        MessageBox.Show("На ноль делить нельзя!");
                        return;
                    }
                    result = firstNumber / secondNumber;
                    break;
            }

            txtHistory.Text = firstNumber + " " + operation + " " + secondNumber + " =";
            txtCurrent.Text = result.ToString();
            operation = "";
            newNumber = true;
        }

        //CE очищаем только текущее число
        private void CE_Click(object sender, RoutedEventArgs e)
        {
            txtCurrent.Text = "0";
            newNumber = true;
        }

        //очищаем всё
        private void C_Click(object sender, RoutedEventArgs e)
        {
            txtCurrent.Text = "0";
            txtHistory.Text = "";
            firstNumber = 0;
            operation = "";
            newNumber = true;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (txtCurrent.Text.Length > 1)
                txtCurrent.Text = txtCurrent.Text.Remove(txtCurrent.Text.Length - 1);
            else
                txtCurrent.Text = "0";
        }
    }
}