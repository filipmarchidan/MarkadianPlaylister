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
            tableLayoutPanel1 = new TableLayoutPanel();
            pathDisplay = new Label();
            label2 = new Label();
            button1 = new Button();
            apply = new Button();
            bitRateSelector = new ComboBox();
            button3 = new Button();
            enableQueue = new CheckBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(pathDisplay, 0, 1);
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(button1, 0, 3);
            tableLayoutPanel1.Controls.Add(apply, 1, 3);
            tableLayoutPanel1.Controls.Add(bitRateSelector, 1, 0);
            tableLayoutPanel1.Controls.Add(button3, 1, 1);
            tableLayoutPanel1.Controls.Add(enableQueue, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Size = new Size(1000, 562);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pathDisplay
            // 
            pathDisplay.AutoSize = true;
            pathDisplay.Dock = DockStyle.Top;
            pathDisplay.Location = new Point(4, 140);
            pathDisplay.Margin = new Padding(4, 0, 4, 0);
            pathDisplay.Name = "pathDisplay";
            pathDisplay.Size = new Size(492, 25);
            pathDisplay.TabIndex = 5;
            pathDisplay.Text = "Current Path";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(4, 0);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(492, 140);
            label2.TabIndex = 3;
            label2.Text = "Bit Rate Download (kbps)";
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(4, 424);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(492, 134);
            button1.TabIndex = 0;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // apply
            // 
            apply.Dock = DockStyle.Fill;
            apply.Location = new Point(504, 424);
            apply.Margin = new Padding(4);
            apply.Name = "apply";
            apply.Size = new Size(492, 134);
            apply.TabIndex = 1;
            apply.Text = "Apply Changes";
            apply.UseVisualStyleBackColor = true;
            apply.Click += button2_Click;
            // 
            // bitRateSelector
            // 
            bitRateSelector.Dock = DockStyle.Fill;
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
            // button3
            // 
            button3.Dock = DockStyle.Fill;
            button3.Location = new Point(504, 144);
            button3.Margin = new Padding(4);
            button3.Name = "button3";
            button3.Size = new Size(492, 132);
            button3.TabIndex = 6;
            button3.Text = "Change Path";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // enableQueue
            // 
            enableQueue.AutoSize = true;
            enableQueue.Dock = DockStyle.Fill;
            enableQueue.Location = new Point(4, 284);
            enableQueue.Margin = new Padding(4);
            enableQueue.Name = "enableQueue";
            enableQueue.Size = new Size(492, 132);
            enableQueue.TabIndex = 7;
            enableQueue.Text = "Enable Queue";
            enableQueue.UseVisualStyleBackColor = true;
            // 
            // Preferences
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 562);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(4);
            Name = "Preferences";
            Text = "Preferences";
            Load += Preferences_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
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
    }
}