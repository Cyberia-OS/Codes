using System;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Task1TextEdit
{
    public partial class Form1 : Form
    {
        //путь к открытому файлу
        private string currentFilePath = "";

        public Form1()
        {
            InitializeComponent();
            UpdateTitle();
        }

        // Обновляем заголовок окна - в нём должен быть полный путь к файлу
        private void UpdateTitle()
        {
            if (currentFilePath == "")
                this.Text = "Текстовый редактор - (новый документ)";
            else
                this.Text = "Текстовый редактор - " + currentFilePath;
        }

        //Меню Новый
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            currentFilePath = "";
            UpdateTitle();
        }

        //Меню Открыть
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = File.OpenText(dlg.FileName);
                textBox1.Text = reader.ReadToEnd();
                reader.Close();

                currentFilePath = dlg.FileName;
                UpdateTitle();
            }
        }

        //Меню Сохранить
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //если файл еще не сохранялся то спрашиваем путь
            if (currentFilePath == "")
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

                if (dlg.ShowDialog() == DialogResult.OK)
                    currentFilePath = dlg.FileName;
                else
                    return;
            }

            StreamWriter writer = new StreamWriter(currentFilePath);
            writer.Write(textBox1.Text);
            writer.Close();

            UpdateTitle();
        }

        //Выход
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }
    }
}
