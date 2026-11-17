using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncPractice
{
    public partial class Task7Form : Form
    {
        public Task7Form()
        {
            InitializeComponent();
        }

        private async void btnCalc_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtBase.Text, out double num) && double.TryParse(txtPower.Text, out double pow))
            {
                btnCalc.Enabled = false;
                lblResult.Text = "Вычисляем...";

                double result = await Task.Run(() => Math.Pow(num, pow));

                lblResult.Text = $"Результат: {result}";
                btnCalc.Enabled = true;
            }
            else
            {
                MessageBox.Show("Введите корректные числа (дробные через запятую)!");
            }
        }
    }
}
