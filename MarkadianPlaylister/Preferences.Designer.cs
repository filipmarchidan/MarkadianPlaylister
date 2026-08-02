namespace MarkadianPlaylister
{
    partial class Preferences
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Preferences));
            tableLayoutPanel1 = new TableLayoutPanel();
            pathDisplay = new Label();
            label2 = new Label();
            button1 = new Button();
            apply = new Button();
            button3 = new Button();
            enableQueue = new CheckBox();
            label1 = new Label();
            countNumber = new NumericUpDown();
            button2 = new Button();
            resourceDirectoryPath = new Label();
            label3 = new Label();
            fileTypeBox = new ComboBox();
            label4 = new Label();
            videoQualityBox = new ComboBox();
            bitRateSelector = new ComboBox();
            enableVideo = new CheckBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)countNumber).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(pathDisplay, 0, 1);
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(button1, 0, 7);
            tableLayoutPanel1.Controls.Add(apply, 1, 7);
            tableLayoutPanel1.Controls.Add(button3, 1, 1);
            tableLayoutPanel1.Controls.Add(enableQueue, 0, 2);
            tableLayoutPanel1.Controls.Add(label1, 0, 3);
            tableLayoutPanel1.Controls.Add(countNumber, 1, 3);
            tableLayoutPanel1.Controls.Add(button2, 1, 5);
            tableLayoutPanel1.Controls.Add(resourceDirectoryPath, 0, 5);
            tableLayoutPanel1.Controls.Add(label3, 0, 4);
            tableLayoutPanel1.Controls.Add(fileTypeBox, 1, 4);
            tableLayoutPanel1.Controls.Add(label4, 0, 6);
            tableLayoutPanel1.Controls.Add(videoQualityBox, 1, 6);
            tableLayoutPanel1.Controls.Add(bitRateSelector, 1, 0);
            tableLayoutPanel1.Controls.Add(enableVideo, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 8;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5220976F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.2992792F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5666542F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5220909F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.518589F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5245991F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5245962F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5220976F));
            tableLayoutPanel1.Size = new Size(1000, 562);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pathDisplay
            // 
            pathDisplay.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pathDisplay.AutoSize = true;
            pathDisplay.Location = new Point(4, 92);
            pathDisplay.Margin = new Padding(4, 0, 4, 0);
            pathDisplay.Name = "pathDisplay";
            pathDisplay.Size = new Size(492, 25);
            pathDisplay.TabIndex = 5;
            pathDisplay.Text = "Current Path";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.ImageAlign = ContentAlignment.MiddleLeft;
            label2.Location = new Point(4, 22);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(492, 25);
            label2.TabIndex = 3;
            label2.Text = "Bit Rate Download (kbps)";
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(4, 493);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(492, 65);
            button1.TabIndex = 0;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // apply
            // 
            apply.Dock = DockStyle.Fill;
            apply.Location = new Point(504, 493);
            apply.Margin = new Padding(4);
            apply.Name = "apply";
            apply.Size = new Size(492, 65);
            apply.TabIndex = 1;
            apply.Text = "Apply Changes";
            apply.UseVisualStyleBackColor = true;
            apply.Click += button2_Click;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Fill;
            button3.Location = new Point(504, 74);
            button3.Margin = new Padding(4);
            button3.Name = "button3";
            button3.Size = new Size(492, 61);
            button3.TabIndex = 6;
            button3.Text = "Change Path";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // enableQueue
            // 
            enableQueue.AutoSize = true;
            enableQueue.Dock = DockStyle.Fill;
            enableQueue.Location = new Point(4, 143);
            enableQueue.Margin = new Padding(4);
            enableQueue.Name = "enableQueue";
            enableQueue.Size = new Size(492, 62);
            enableQueue.TabIndex = 7;
            enableQueue.Text = "Enable Queue";
            enableQueue.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(3, 219);
            label1.Name = "label1";
            label1.Size = new Size(494, 50);
            label1.TabIndex = 8;
            label1.Text = "Search Count\r\n(Note: larger counts can impact performance)\r\n";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // countNumber
            // 
            countNumber.Dock = DockStyle.Fill;
            countNumber.Location = new Point(503, 212);
            countNumber.Name = "countNumber";
            countNumber.Size = new Size(494, 31);
            countNumber.TabIndex = 9;
            countNumber.TextAlign = HorizontalAlignment.Right;
            countNumber.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.Location = new Point(503, 352);
            button2.Name = "button2";
            button2.Size = new Size(494, 64);
            button2.TabIndex = 11;
            button2.Text = "Change Path";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click_1;
            // 
            // resourceDirectoryPath
            // 
            resourceDirectoryPath.AutoSize = true;
            resourceDirectoryPath.Dock = DockStyle.Fill;
            resourceDirectoryPath.Location = new Point(3, 349);
            resourceDirectoryPath.Name = "resourceDirectoryPath";
            resourceDirectoryPath.Size = new Size(494, 70);
            resourceDirectoryPath.TabIndex = 10;
            resourceDirectoryPath.Text = "Current Resource Directory";
            resourceDirectoryPath.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(3, 279);
            label3.Name = "label3";
            label3.Size = new Size(494, 70);
            label3.TabIndex = 12;
            label3.Text = "File type (.mp3 or .mp4)";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // fileTypeBox
            // 
            fileTypeBox.Dock = DockStyle.Fill;
            fileTypeBox.DropDownStyle = ComboBoxStyle.DropDownList;
            fileTypeBox.FormattingEnabled = true;
            fileTypeBox.Items.AddRange(new object[] { ".mp3", ".mp4" });
            fileTypeBox.Location = new Point(504, 283);
            fileTypeBox.Margin = new Padding(4);
            fileTypeBox.Name = "fileTypeBox";
            fileTypeBox.Size = new Size(492, 33);
            fileTypeBox.Sorted = true;
            fileTypeBox.TabIndex = 13;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(3, 419);
            label4.Name = "label4";
            label4.Size = new Size(494, 70);
            label4.TabIndex = 15;
            label4.Text = "Video Quality Download";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // videoQualityBox
            // 
            videoQualityBox.Dock = DockStyle.Fill;
            videoQualityBox.DropDownStyle = ComboBoxStyle.DropDownList;
            videoQualityBox.FormattingEnabled = true;
            videoQualityBox.Items.AddRange(new object[] { "1080p", "360p", "480p", "720p", "best" });
            videoQualityBox.Location = new Point(504, 423);
            videoQualityBox.Margin = new Padding(4);
            videoQualityBox.Name = "videoQualityBox";
            videoQualityBox.Size = new Size(492, 33);
            videoQualityBox.Sorted = true;
            videoQualityBox.TabIndex = 16;
            // 
            // bitRateSelector
            // 
            bitRateSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            bitRateSelector.FormattingEnabled = true;
            bitRateSelector.Items.AddRange(new object[] { "128", "192", "256", "320" });
            bitRateSelector.Location = new Point(504, 4);
            bitRateSelector.Margin = new Padding(4);
            bitRateSelector.Name = "bitRateSelector";
            bitRateSelector.Size = new Size(492, 33);
            bitRateSelector.Sorted = true;
            bitRateSelector.TabIndex = 2;
            // 
            // enableVideo
            // 
            enableVideo.AutoSize = true;
            enableVideo.Dock = DockStyle.Fill;
            enableVideo.Location = new Point(503, 142);
            enableVideo.Name = "enableVideo";
            enableVideo.Size = new Size(494, 64);
            enableVideo.TabIndex = 17;
            enableVideo.Text = "Enable Video Playback Streaming";
            enableVideo.UseVisualStyleBackColor = true;
            enableVideo.CheckedChanged += enableVideo_CheckedChanged;
            // 
            // Preferences
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 562);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            Name = "Preferences";
            Text = "Preferences";
            Load += Preferences_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)countNumber).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button button1;
        private Button apply;
        private ComboBox bitRateSelector;
        private Label label2;
        private Label pathDisplay;
        private Button button3;
        private CheckBox enableQueue;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label1;
        private NumericUpDown countNumber;
        private Label resourceDirectoryPath;
        private Button button2;
        private Label label3;
        private ComboBox fileTypeBox;
        private Label label4;
        private ComboBox videoQualityBox;
        private CheckBox enableVideo;
    }
}