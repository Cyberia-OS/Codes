using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsThreads
{
    public partial class Form1 : Form
    {
        int[] numbers = new int[10000];
        int maxVal, minVal;
        double avgVal;

        public Form1()
        {
            InitializeComponent();
        }

        // кнопка 1: задание 6
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            int start = int.Parse(textBox1.Text);
            int end = int.Parse(textBox2.Text);
            int count = int.Parse(textBox3.Text);

            for (int i = 0; i < count; i++)
            {
                int threadNum = i + 1;
                new Thread(() =>
                {
                    for (int j = start; j <= end; j++)
                    {
                        string text = $"поток {threadNum}: {j}";
                        listBox1.Invoke(new Action(() => listBox1.Items.Add(text)));
                    }
                }).Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = rnd.Next(1, 10000);
            }

            Thread tMax = new Thread(() =>
            {
                maxVal = numbers.Max();
                label4.Invoke(new Action(() => label4.Text = $"максимум: {maxVal}"));
            });

            Thread tMin = new Thread(() =>
            {
                minVal = numbers.Min();
                label5.Invoke(new Action(() => label5.Text = $"минимум: {minVal}"));
            });

            Thread tAvg = new Thread(() =>
            {
                avgVal = numbers.Average();
                label6.Invoke(new Action(() => label6.Text = $"среднее: {avgVal}"));
            });

            tMax.Start();
            tMin.Start();
            tAvg.Start();

            new Thread(() =>
            {
                tMax.Join();
                tMin.Join();
                tAvg.Join();

                using (StreamWriter sw = new StreamWriter("result.txt"))
                {
                    sw.WriteLine($"максимум: {maxVal}");
                    sw.WriteLine($"минимум: {minVal}");
                    sw.WriteLine($"среднее: {avgVal}");
                    sw.WriteLine(string.Join(", ", numbers));
                }

                MessageBox.Show("готово и сохранено в result.txt");
            }).Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }



        private void Form1_Load(object sender, EventArgs e) { }
    }
}
