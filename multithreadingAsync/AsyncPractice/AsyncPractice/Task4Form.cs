using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncPractice
{
    public partial class Task4Form : Form
    {
        private ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);
        private CancellationTokenSource cts;

        public Task4Form()
        {
            InitializeComponent();
        }

        private void btnBrowseSrc_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    txtSource.Text = fbd.SelectedPath;
            }
        }

        private void btnBrowseDest_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    txtDest.Text = fbd.SelectedPath;
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            string srcDir = txtSource.Text;
            string destDir = txtDest.Text;

            if (!Directory.Exists(srcDir) || string.IsNullOrWhiteSpace(destDir))
            {
                MessageBox.Show("Укажите корректные пути к папкам!");
                return;
            }

            btnBrowseSrc.Enabled = btnBrowseDest.Enabled = btnStart.Enabled = false;
            cts = new CancellationTokenSource();
            pauseEvent.Set();
            btnPause.Text = "Пауза";

            string[] files = Directory.GetFiles(srcDir, "*.*", SearchOption.AllDirectories);
            progressBar1.Maximum = files.Length;
            progressBar1.Value = 0;

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = (int)numThreads.Value,
                CancellationToken = cts.Token
            };

            try
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(files, options, file =>
                    {
                        options.CancellationToken.ThrowIfCancellationRequested();
                        pauseEvent.Wait(options.CancellationToken);

                        string relPath = file.Substring(srcDir.Length).TrimStart('\\', '/');
                        string targetPath = Path.Combine(destDir, relPath);

                        Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                        File.Copy(file, targetPath, true);

                        Invoke(new Action(() =>
                        {
                            if (progressBar1.Value < progressBar1.Maximum)
                                progressBar1.Value++;
                        }));
                    });
                });

                MessageBox.Show("Папка успешно скопирована!");
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Копирование прервано. Вы можете выбрать другую папку.");
                progressBar1.Value = 0;
            }
            finally
            {
                btnBrowseSrc.Enabled = btnBrowseDest.Enabled = btnStart.Enabled = true;
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
