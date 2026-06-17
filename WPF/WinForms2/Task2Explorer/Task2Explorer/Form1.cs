using System;
using System.IO;
using System.Windows.Forms;

namespace Task2Explorer
{
    public partial class Form1 : Form
    {
        //текущая открытая папка Пустая строка значит сейчас показан список дисков
        private string currentFolder = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowDrives();
        }

        //список доступных диско
        private void ShowDrives()
        {
            currentFolder = "";
            textBoxAddress.Text = "Мой компьютер";
            listBox1.Items.Clear();

            string[] drives = Environment.GetLogicalDrives();

            for (int i = 0; i < drives.Length; i++)
            {
                listBox1.Items.Add(drives[i]);
            }
        }

        //содержимое указанной папки
        private void ShowFolder(string path)
        {
            try
            {
                listBox1.Items.Clear();

                //сначала папки
                string[] folders = Directory.GetDirectories(path);
                for (int i = 0; i < folders.Length; i++)
                {
                    //оставляем только имя папки, без полного пути
                    string name = Path.GetFileName(folders[i]);
                    listBox1.Items.Add("[Папка] " + name);
                }

                //потом файлы
                string[] files = Directory.GetFiles(path);
                for (int i = 0; i < files.Length; i++)
                {
                    string name = Path.GetFileName(files[i]);
                    listBox1.Items.Add(name);
                }

                currentFolder = path;
                textBoxAddress.Text = path;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть папку: " + ex.Message);
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;

            string selected = listBox1.SelectedItem.ToString();

            if (currentFolder == "")
            {
                ShowFolder(selected);
                return;
            }

            //если выбрана папка заходим внутрь
            if (selected.StartsWith("[Папка] "))
            {
                string folderName = selected.Substring("[Папка] ".Length);
                string newPath = Path.Combine(currentFolder, folderName);
                ShowFolder(newPath);
            }
            //если выбран файл просто ничего не делаем 
        }

        //Назад к дискам
        private void btnDrives_Click(object sender, EventArgs e)
        {
            ShowDrives();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (currentFolder == "") return; // мы уже на уровне дисков

            DirectoryInfo info = Directory.GetParent(currentFolder);

            if (info == null)
                ShowDrives(); // выше корня диска - значит список дисков
            else
                ShowFolder(info.FullName);
        }

        //Выход
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Мой компьютер
        private void computerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDrives();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFolder == "")
                ShowDrives();
            else
                ShowFolder(currentFolder);
        }
    }
}
