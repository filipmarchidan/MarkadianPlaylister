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
            bitRateSelector = new ComboBox();
            button3 = new Button();
            enableQueue = new CheckBox();
            label1 = new Label();
            countNumber = new NumericUpDown();
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
            tableLayoutPanel1.Controls.Add(button1, 0, 5);
            tableLayoutPanel1.Controls.Add(apply, 1, 5);
            tableLayoutPanel1.Controls.Add(bitRateSelector, 1, 0);
            tableLayoutPanel1.Controls.Add(button3, 1, 1);
            tableLayoutPanel1.Controls.Add(enableQueue, 0, 2);
            tableLayoutPanel1.Controls.Add(label1, 0, 3);
            tableLayoutPanel1.Controls.Add(countNumber, 1, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel1.Size = new Size(1000, 562);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pathDisplay
            // 
            pathDisplay.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pathDisplay.AutoSize = true;
            pathDisplay.Location = new Point(4, 127);
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
            label2.Location = new Point(4, 34);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(492, 25);
            label2.TabIndex = 3;
            label2.Text = "Bit Rate Download (kbps)";
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(4, 469);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(492, 89);
            button1.TabIndex = 0;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // apply
            // 
            apply.Dock = DockStyle.Fill;
            apply.Location = new Point(504, 469);
            apply.Margin = new Padding(4);
            apply.Name = "apply";
            apply.Size = new Size(492, 89);
            apply.TabIndex = 1;
            apply.Text = "Apply Changes";
            apply.UseVisualStyleBackColor = true;
            apply.Click += button2_Click;
            // 
            // bitRateSelector
            // 
            bitRateSelector.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            bitRateSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            bitRateSelector.FormattingEnabled = true;
            bitRateSelector.Items.AddRange(new object[] { "128", "192", "256", "320" });
            bitRateSelector.Location = new Point(504, 30);
            bitRateSelector.Margin = new Padding(4);
            bitRateSelector.Name = "bitRateSelector";
            bitRateSelector.Size = new Size(492, 33);
            bitRateSelector.Sorted = true;
            bitRateSelector.TabIndex = 2;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Fill;
            button3.Location = new Point(504, 97);
            button3.Margin = new Padding(4);
            button3.Name = "button3";
            button3.Size = new Size(492, 85);
            button3.TabIndex = 6;
            button3.Text = "Change Path";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // enableQueue
            // 
            enableQueue.AutoSize = true;
            enableQueue.Dock = DockStyle.Fill;
            enableQueue.Location = new Point(4, 190);
            enableQueue.Margin = new Padding(4);
            enableQueue.Name = "enableQueue";
            enableQueue.Size = new Size(492, 85);
            enableQueue.TabIndex = 7;
            enableQueue.Text = "Enable Queue";
            enableQueue.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(3, 300);
            label1.Name = "label1";
            label1.Size = new Size(494, 50);
            label1.TabIndex = 8;
            label1.Text = "Search Count\r\n(Note: larger counts can impact performance)\r\n";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // countNumber
            // 
            countNumber.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            countNumber.Location = new Point(503, 310);
            countNumber.Name = "countNumber";
            countNumber.Size = new Size(494, 31);
            countNumber.TabIndex = 9;
            countNumber.TextAlign = HorizontalAlignment.Right;
            countNumber.Value = new decimal(new int[] { 5, 0, 0, 0 });
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
    }
}