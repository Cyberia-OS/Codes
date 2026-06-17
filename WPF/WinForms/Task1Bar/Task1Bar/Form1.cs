using System;
using System.IO;
using System.Reflection.Emit;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Task1Bar
{
    public partial class Form1 : Form
    {
        //путь к файлу который будем читать
        private string filePath = "test.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Файл не найден: " + filePath);
                return;
            }

            //читаем все строки файла в массив
            string[] lines = File.ReadAllLines(filePath);

            progressBar1.Minimum = 0;
            progressBar1.Maximum = lines.Length;
            progressBar1.Step = 1;
            progressBar1.Value = 0;

            label1.Text = "Чтение файла";

            for (int i = 0; i < lines.Length; i++)
            {
                progressBar1.PerformStep();

                label1.Text = "Прочитано строк: " + progressBar1.Value + " из " + progressBar1.Maximum;

                this.Update();
                Thread.Sleep(50);
            }

            label1.Text = "Готово Прочитано строк: " + lines.Length;
        }
    }
}
