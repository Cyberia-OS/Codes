using System;
using System.Windows.Forms;

namespace Task2Anketa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxCity.Items.AddRange(new object[]
            {
                "Москва",
                "Санкт-Петербург",
                "Казань",
                "Новосибирск"
            });

            checkedListBoxHobby.Items.AddRange(new object[]
            {
                "Спорт",
                "Музыка",
                "Книги",
                "Путешествия",
                "Программирование"
            });

            numericUpDownAge.Minimum = 1;
            numericUpDownAge.Maximum = 100;
            numericUpDownAge.Value = 18;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string result = "";

            result += "Город: " + comboBoxCity.Text + "\r\n";
            result += "Возраст: " + numericUpDownAge.Value + "\r\n";
            result += "Увлечения:\r\n";

            if (checkedListBoxHobby.CheckedItems.Count == 0)
            {
                result += "(ничего не выбрано)\r\n";
            }
            else
            {
                for (int i = 0; i < checkedListBoxHobby.CheckedItems.Count; i++)
                {
                    result += " - " + checkedListBoxHobby.CheckedItems[i] + "\r\n";
                }
            }

            textBoxResult.Text = result;
        }
    }
}
