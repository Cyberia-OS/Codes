using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncPractice
{
    public partial class Task2Form : Form
    {
        private ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);
        private CancellationTokenSource cts;
        private long totalBytesCopied = 0;

        public Task2Form()
        {
            InitializeComponent();
        }

        private void btnBrowseSrc_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                    txtSource.Text = ofd.FileName;
            }
        }

        private void btnBrowseDest_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                    txtDest.Text = sfd.FileName;
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            string src = txtSource.Text;
            string dest = txtDest.Text;
            int threadCount = (int)numThreads.Value;

            if (!File.Exists(src) || string.IsNullOrWhiteSpace(dest))
            {
                MessageBox.Show("Укажите корректные пути к файлам!");
                return;
            }

            // Блокируем кнопки выбора во время работы
            btnBrowseSrc.Enabled = btnBrowseDest.Enabled = btnStart.Enabled = false;

            cts = new CancellationTokenSource();
            pauseEvent.Set();
            btnPause.Text = "Пауза";
            totalBytesCopied = 0;
            progressBar1.Value = 0;
            progressBar1.Maximum = 100;

            long fileSize = new FileInfo(src).Length;

            // Создаем пустой файл нужного размера
            using (FileStream fs = new FileStream(dest, FileMode.Create, FileAccess.Write))
            {
                fs.SetLength(fileSize);
            }

            long chunkSize = fileSize / threadCount;
            Task[] tasks = new Task[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                long start = i * chunkSize;
                long end = (i == threadCount - 1) ? fileSize : start + chunkSize;
                tasks[i] = Task.Run(() => CopyChunk(src, dest, start, end, fileSize, cts.Token));
            }

            try
            {
                await Task.WhenAll(tasks);
                MessageBox.Show("Копирование успешно завершено!");
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Копирование полностью остановлено. Вы можете выбрать другой файл.");
                if (File.Exists(dest)) File.Delete(dest);
                progressBar1.Value = 0;
            }
            finally
            {
                // Разблокируем выбор нового файла (Задание 3)
                btnBrowseSrc.Enabled = btnBrowseDest.Enabled = btnStart.Enabled = true;
            }
        }

        private void CopyChunk(string src, string dest, long start, long end, long totalSize, CancellationToken token)
        {
            byte[] buffer = new byte[81920]; // Буфер 80 КБ
            using (FileStream fsRead = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (FileStream fsWrite = new FileStream(dest, FileMode.Open, FileAccess.Write, FileShare.Write))
            {
                fsRead.Position = start;
                fsWrite.Position = start;
                long remaining = end - start;

                while (remaining > 0)
                {
                    token.ThrowIfCancellationRequested(); // Проверка кнопки Стоп
                    pauseEvent.Wait(token); // Проверка кнопки Пауза

                    int toRead = (int)Math.Min(buffer.Length, remaining);
                    int read = fsRead.Read(buffer, 0, toRead);
                    if (read == 0) break;

                    fsWrite.Write(buffer, 0, read);
                    remaining -= read;

                    long currentTotal = Interlocked.Add(ref totalBytesCopied, read);
                    int percent = (int)((currentTotal * 100) / totalSize);
                    Invoke(new Action(() => progressBar1.Value = Math.Min(100, percent)));
                }
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (pauseEvent.IsSet)
            {
                pauseEvent.Reset();
                btnPause.Text = "Продолжить";
            }
            else
            {
                pauseEvent.Set();
                btnPause.Text = "Пауза";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }
    }
}
