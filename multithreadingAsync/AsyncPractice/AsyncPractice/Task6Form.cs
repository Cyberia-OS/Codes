using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncPractice
{
    public partial class Task6Form : Form
    {
        public Task6Form()
        {
            InitializeComponent();
        }

        private async void btnCalc_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtNumber.Text, out int number) && number >= 0)
            {
                btnCalc.Enabled = false;
                txtResult.Text = "Вычисляем...";

                BigInteger result = await Task.Run(() => CalculateFactorial(number));

                txtResult.Text = result.ToString();
                btnCalc.Enabled = true;
            }
            else
            {
                MessageBox.Show("Введите целое неотрицательное число!");
            }
        }

        private BigInteger CalculateFactorial(int n)
        {
            BigInteger res = 1;
            for (int i = 2; i <= n; i++)
            {
                res *= i;
            }
            return res;
        }
    }
}
