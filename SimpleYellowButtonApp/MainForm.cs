using System;
using System.Windows.Forms;

namespace SimpleYellowButtonApp
{
    public partial class MainForm : Form
    {
        private bool isPlaying = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void yellowButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Кнопка нажата!", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void playPauseButton_Click(object? sender, EventArgs e)
        {
            isPlaying = !isPlaying;
            playPauseButton.Text = isPlaying ? "Pause" : "Play";

            if (isPlaying)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

        private void stopButton_Click(object? sender, EventArgs e)
        {
            isPlaying = false;
            playPauseButton.Text = "Play";
            timer.Stop();
            progressBar.Value = 0;
            UpdateProgressLabel();
        }

        private void timer_Tick(object? sender, EventArgs e)
        {
            if (progressBar.Value < progressBar.Maximum)
            {
                progressBar.Value = Math.Min(progressBar.Maximum, progressBar.Value + 1);
                UpdateProgressLabel();
            }
            else
            {
                // Остановить при достижении конца
                timer.Stop();
                isPlaying = false;
                playPauseButton.Text = "Play";
            }
        }

        private void progressBar_Scroll(object? sender, EventArgs e)
        {
            UpdateProgressLabel();
        }

        private void UpdateProgressLabel()
        {
            lblProgress.Text = $"{progressBar.Value}%";
        }
    }
}
