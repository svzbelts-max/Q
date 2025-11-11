using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleYellowButtonApp
{
    partial class MainForm
    {
        private IContainer? components = null;
        private Button? yellowButton;
        private PictureBox? pictureBoxControls;
        private FlowLayoutPanel? controlsPanel;
        private Button? playPauseButton;
        private Button? stopButton;
        private TrackBar? progressBar;
        private Label? lblProgress;
        private System.Windows.Forms.Timer? timer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.yellowButton = new Button();
            this.pictureBoxControls = new PictureBox();
            this.controlsPanel = new FlowLayoutPanel();
            this.playPauseButton = new Button();
            this.stopButton = new Button();
            this.progressBar = new TrackBar();
            this.lblProgress = new Label();
            this.timer = new System.Windows.Forms.Timer(this.components);

            ((ISupportInitialize)(this.pictureBoxControls)).BeginInit();
            ((ISupportInitialize)(this.progressBar)).BeginInit();
            this.SuspendLayout();
            // 
            // yellowButton
            // 
            this.yellowButton.BackColor = Color.Yellow;
            this.yellowButton.ForeColor = Color.Black;
            this.yellowButton.Location = new Point(100, 220);
            this.yellowButton.Name = "yellowButton";
            this.yellowButton.Size = new Size(120, 50);
            this.yellowButton.TabIndex = 0;
            this.yellowButton.Text = "Нажми меня";
            this.yellowButton.UseVisualStyleBackColor = false;
            this.yellowButton.Click += new System.EventHandler(this.yellowButton_Click);
            // 
            // pictureBoxControls
            // 
            this.pictureBoxControls.Location = new Point(20, 20);
            this.pictureBoxControls.Name = "pictureBoxControls";
            this.pictureBoxControls.Size = new Size(280, 120);
            this.pictureBoxControls.SizeMode = PictureBoxSizeMode.StretchImage;
            // Относительный путь к изображению в проекте (assets\\controls.png)
            this.pictureBoxControls.ImageLocation = "assets\\controls.png";
            this.pictureBoxControls.TabIndex = 1;
            this.pictureBoxControls.TabStop = false;
            // 
            // controlsPanel
            // 
            this.controlsPanel.Location = new Point(20, 150);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Size = new Size(280, 50);
            this.controlsPanel.TabIndex = 2;
            this.controlsPanel.FlowDirection = FlowDirection.LeftToRight;
            this.controlsPanel.WrapContents = false;
            // Немного прозрачный фон (в WinForms альфа для BackColor работает не всегда для всех системных тем)
            this.controlsPanel.BackColor = Color.FromArgb(220, Color.Black);
            // 
            // playPauseButton
            // 
            this.playPauseButton.Name = "playPauseButton";
            this.playPauseButton.Size = new Size(70, 30);
            this.playPauseButton.Text = "Play";
            this.playPauseButton.ForeColor = Color.White;
            this.playPauseButton.BackColor = Color.FromArgb(80, Color.Green);
            this.playPauseButton.Click += new System.EventHandler(this.playPauseButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new Size(70, 30);
            this.stopButton.Text = "Stop";
            this.stopButton.ForeColor = Color.White;
            this.stopButton.BackColor = Color.FromArgb(80, Color.Red);
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // progressBar (TrackBar)
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(100, 30);
            this.progressBar.Minimum = 0;
            this.progressBar.Maximum = 100;
            this.progressBar.Value = 0;
            this.progressBar.TickFrequency = 10;
            this.progressBar.Scroll += new System.EventHandler(this.progressBar_Scroll);
            // 
            // lblProgress
            // 
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new Size(40, 30);
            this.lblProgress.Text = "0%";
            this.lblProgress.ForeColor = Color.White;
            this.lblProgress.TextAlign = ContentAlignment.MiddleCenter;
            this.lblProgress.Padding = new Padding(6, 6, 6, 6);
            // 
            // timer
            // 
            this.timer.Interval = 250;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // Сборка controlsPanel
            // 
            this.controlsPanel.Controls.Add(this.playPauseButton);
            this.controlsPanel.Controls.Add(this.stopButton);
            this.controlsPanel.Controls.Add(this.progressBar);
            this.controlsPanel.Controls.Add(this.lblProgress);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(320, 300);
            this.Controls.Add(this.controlsPanel);
            this.Controls.Add(this.pictureBoxControls);
            this.Controls.Add(this.yellowButton);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Простое приложение";
            ((ISupportInitialize)(this.pictureBoxControls)).EndInit();
            ((ISupportInitialize)(this.progressBar)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
