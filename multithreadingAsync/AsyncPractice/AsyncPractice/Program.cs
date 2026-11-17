using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncPractice
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());  
            Application.Run(new Task2Form()); 
            Application.Run(new Task4Form()); 
            Application.Run(new Task6Form()); 
            Application.Run(new Task7Form()); 
        }
    }
}
