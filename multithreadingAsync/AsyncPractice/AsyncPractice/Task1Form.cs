using System;
using System.Threading;
using System.Windows.Forms;

namespace AsyncPractice
{
    public partial class Form1 : Form
    {
        private bool isRunning = false;
        private Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();

            var priorities = Enum.GetValues(typeof(ThreadPriority));
            cmbNumPrio.DataSource = priorities.Clone();
            cmbLetPrio.DataSource = priorities.Clone();
            cmbSymPrio.DataSource = priorities.Clone();
        }

        // Кнопка Старт
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isRunning) return;
            isRunning = true;

            Thread t1 = new Thread(GenerateNumbers)
            {
                Priority = (ThreadPriority)cmbNumPrio.SelectedItem,
                IsBackground = true
            };
            Thread t2 = new Thread(GenerateLetters)
            {
                Priority = (ThreadPriority)cmbLetPrio.SelectedItem,
                IsBackground = true
            };
            Thread t3 = new Thread(GenerateSymbols)
            {
                Priority = (ThreadPriority)cmbSymPrio.SelectedItem,
                IsBackground = true
            };

            t1.Start();
            t2.Start();
            t3.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isRunning = false;
        }

        private void GenerateNumbers()
        {
            while (isRunning)
            {
                string val = rnd.Next(0, 10).ToString();
                Invoke(new Action(() => txtNumbers.AppendText(val + " ")));
                Thread.Sleep(300);
            }
        }

        private void GenerateLetters()
        {
            while (isRunning)
            {
                char val = (char)rnd.Next('A', 'Z' + 1);
                Invoke(new Action(() => txtLetters.AppendText(val + " ")));
                Thread.Sleep(300);
            }
        }

        private void GenerateSymbols()
        {
            char[] syms = { '!', '@', '#', '$', '%', '&', '*' };
            while (isRunning)
            {
                char val = syms[rnd.Next(syms.Length)];
                Invoke(new Action(() => txtSymbols.AppendText(val + " ")));
                Thread.Sleep(300);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
