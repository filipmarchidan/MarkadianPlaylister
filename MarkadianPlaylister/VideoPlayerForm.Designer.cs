namespace MarkadianPlaylister
{
    partial class VideoPlayerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoPlayerForm));
            pauseButton = new Button();
            videoPanel = new Panel();
            timeBar = new TrackBar();
            panel2 = new Panel();
            statusText = new Label();
            playButton = new Button();
            videoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)timeBar).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // pauseButton
            // 
            pauseButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pauseButton.BackColor = Color.FromArgb(45, 45, 48);
            pauseButton.Cursor = Cursors.Hand;
            pauseButton.Dock = DockStyle.Left;
            pauseButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pauseButton.ForeColor = Color.White;
            pauseButton.Location = new Point(0, 0);
            pauseButton.Name = "pauseButton";
            pauseButton.Size = new Size(120, 62);
            pauseButton.TabIndex = 0;
            pauseButton.Text = "⏸ Pause";
            pauseButton.UseVisualStyleBackColor = false;
            // 
            // videoPanel
            // 
            videoPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            videoPanel.BackColor = Color.Black;
            videoPanel.Controls.Add(timeBar);
            videoPanel.Dock = DockStyle.Fill;
            videoPanel.Location = new Point(0, 0);
            videoPanel.Name = "videoPanel";
            videoPanel.Size = new Size(1178, 782);
            videoPanel.TabIndex = 10;
            // 
            // timeBar
            // 
            timeBar.Dock = DockStyle.Bottom;
            timeBar.Location = new Point(0, 713);
            timeBar.Name = "timeBar";
            timeBar.Size = new Size(1178, 69);
            timeBar.TabIndex = 0;
            timeBar.TickStyle = TickStyle.Both;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(30, 30, 30);
            panel2.Controls.Add(statusText);
            panel2.Controls.Add(pauseButton);
            panel2.Controls.Add(playButton);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 782);
            panel2.Name = "panel2";
            panel2.Size = new Size(1178, 62);
            panel2.TabIndex = 2;
            // 
            // statusText
            // 
            statusText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            statusText.AutoSize = true;
            statusText.BackColor = Color.Transparent;
            statusText.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            statusText.ForeColor = Color.LimeGreen;
            statusText.Location = new Point(482, 20);
            statusText.Name = "statusText";
            statusText.Size = new Size(189, 25);
            statusText.TabIndex = 3;
            statusText.Text = "⏳ Loading stream...";
            statusText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // playButton
            // 
            playButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            playButton.BackColor = Color.FromArgb(45, 45, 48);
            playButton.Cursor = Cursors.Hand;
            playButton.Dock = DockStyle.Right;
            playButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            playButton.ForeColor = Color.White;
            playButton.Location = new Point(1058, 0);
            playButton.Name = "playButton";
            playButton.Size = new Size(120, 62);
            playButton.TabIndex = 1;
            playButton.Text = "▶ Play";
            playButton.UseVisualStyleBackColor = false;
            // 
            // VideoPlayerForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1178, 844);
            Controls.Add(videoPanel);
            Controls.Add(panel2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "VideoPlayerForm";
            Text = "Video Player";
            Load += VideoPlayerForm_Load;
            videoPanel.ResumeLayout(false);
            videoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)timeBar).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button pauseButton;
        private Panel videoPanel;
        private Panel panel2;
        private Button playButton;
        private Label statusText;
        private TrackBar timeBar;
    }
}