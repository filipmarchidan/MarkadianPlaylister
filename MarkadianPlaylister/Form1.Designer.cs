namespace MarkadianPlaylister
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BitRate = new ColumnHeader();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            downloadLocationToolStripMenuItem = new ToolStripMenuItem();
            preferencesToolStripMenuItem = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            bottomNavigator = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            statusQueue = new Label();
            queueStatus = new Label();
            progressSongStatus = new ProgressBar();
            pathDisplay = new Label();
            statusText = new Label();
            downloadButton = new Button();
            linkText = new TextBox();
            label1 = new Label();
            listViewSongs = new ListView();
            titleSongList = new ColumnHeader();
            durationSong = new ColumnHeader();
            youtubeSearchResults = new FlowLayoutPanel();
            youtubeSearchTextBox = new TextBox();
            youtubeSearchButton = new Button();
            panel1 = new Panel();
            saveButton = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            keyText = new TextBox();
            keyDisplay = new Label();
            yearText = new TextBox();
            yearDisplay = new Label();
            genreText = new TextBox();
            label3 = new Label();
            discText = new TextBox();
            label2 = new Label();
            bpmText = new TextBox();
            bpmDisplay = new Label();
            albumText = new TextBox();
            artistText = new TextBox();
            titleText = new TextBox();
            titleSongDisplay = new Label();
            artistDisplay = new Label();
            AlbumSongDisplay = new Label();
            contributingArtistsDisplayt = new Label();
            contributingArtistText = new TextBox();
            uploadImage = new Button();
            pictureBox1 = new PictureBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            openFileDialog1 = new OpenFileDialog();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)bottomNavigator).BeginInit();
            bottomNavigator.Panel1.SuspendLayout();
            bottomNavigator.Panel2.SuspendLayout();
            bottomNavigator.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // BitRate
            // 
            BitRate.Text = "BitRate";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(8, 2, 0, 2);
            menuStrip1.Size = new Size(1836, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(141, 34);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { downloadLocationToolStripMenuItem, preferencesToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(58, 29);
            editToolStripMenuItem.Text = "Edit";
            // 
            // downloadLocationToolStripMenuItem
            // 
            downloadLocationToolStripMenuItem.Name = "downloadLocationToolStripMenuItem";
            downloadLocationToolStripMenuItem.Size = new Size(268, 34);
            downloadLocationToolStripMenuItem.Text = "Download Location";
            downloadLocationToolStripMenuItem.Click += downloadLocationToolStripMenuItem_Click;
            // 
            // preferencesToolStripMenuItem
            // 
            preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            preferencesToolStripMenuItem.Size = new Size(268, 34);
            preferencesToolStripMenuItem.Text = "Preferences";
            preferencesToolStripMenuItem.Click += preferencesToolStripMenuItem_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 33);
            splitContainer1.Margin = new Padding(4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel1);
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel2);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            splitContainer1.Size = new Size(1836, 777);
            splitContainer1.SplitterDistance = 1351;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(bottomNavigator);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(youtubeSearchResults);
            splitContainer2.Panel2.Controls.Add(youtubeSearchTextBox);
            splitContainer2.Panel2.Controls.Add(youtubeSearchButton);
            splitContainer2.Size = new Size(1351, 777);
            splitContainer2.SplitterDistance = 422;
            splitContainer2.TabIndex = 6;
            // 
            // bottomNavigator
            // 
            bottomNavigator.Dock = DockStyle.Fill;
            bottomNavigator.Location = new Point(0, 0);
            bottomNavigator.Margin = new Padding(4);
            bottomNavigator.Name = "bottomNavigator";
            bottomNavigator.Orientation = Orientation.Horizontal;
            // 
            // bottomNavigator.Panel1
            // 
            bottomNavigator.Panel1.Controls.Add(tableLayoutPanel1);
            bottomNavigator.Panel1.Controls.Add(pathDisplay);
            bottomNavigator.Panel1.Controls.Add(statusText);
            bottomNavigator.Panel1.Controls.Add(downloadButton);
            bottomNavigator.Panel1.Controls.Add(linkText);
            bottomNavigator.Panel1.Controls.Add(label1);
            // 
            // bottomNavigator.Panel2
            // 
            bottomNavigator.Panel2.Controls.Add(listViewSongs);
            bottomNavigator.Size = new Size(1351, 422);
            bottomNavigator.SplitterDistance = 217;
            bottomNavigator.SplitterWidth = 5;
            bottomNavigator.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30.9455585F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35.5300865F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(statusQueue, 1, 0);
            tableLayoutPanel1.Controls.Add(queueStatus, 0, 0);
            tableLayoutPanel1.Controls.Add(progressSongStatus, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Bottom;
            tableLayoutPanel1.Location = new Point(0, 166);
            tableLayoutPanel1.Margin = new Padding(4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(1351, 51);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // statusQueue
            // 
            statusQueue.AutoSize = true;
            statusQueue.Dock = DockStyle.Fill;
            statusQueue.Location = new Point(422, 0);
            statusQueue.Margin = new Padding(4, 0, 4, 0);
            statusQueue.Name = "statusQueue";
            statusQueue.Size = new Size(472, 31);
            statusQueue.TabIndex = 1;
            statusQueue.Text = "0 / 0 Songs Downloaded";
            statusQueue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // queueStatus
            // 
            queueStatus.AutoSize = true;
            queueStatus.Dock = DockStyle.Fill;
            queueStatus.Location = new Point(4, 0);
            queueStatus.Margin = new Padding(4, 0, 4, 0);
            queueStatus.Name = "queueStatus";
            queueStatus.Size = new Size(410, 31);
            queueStatus.TabIndex = 0;
            queueStatus.Text = "Queue is Enabled";
            queueStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // progressSongStatus
            // 
            progressSongStatus.Dock = DockStyle.Fill;
            progressSongStatus.Location = new Point(902, 4);
            progressSongStatus.Margin = new Padding(4);
            progressSongStatus.Name = "progressSongStatus";
            progressSongStatus.Size = new Size(445, 23);
            progressSongStatus.Style = ProgressBarStyle.Continuous;
            progressSongStatus.TabIndex = 2;
            // 
            // pathDisplay
            // 
            pathDisplay.AutoSize = true;
            pathDisplay.Dock = DockStyle.Top;
            pathDisplay.Location = new Point(0, 117);
            pathDisplay.Margin = new Padding(4, 0, 4, 0);
            pathDisplay.Name = "pathDisplay";
            pathDisplay.Size = new Size(109, 25);
            pathDisplay.TabIndex = 4;
            pathDisplay.Text = "Current Path";
            pathDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // statusText
            // 
            statusText.AutoSize = true;
            statusText.Dock = DockStyle.Top;
            statusText.Location = new Point(0, 92);
            statusText.Margin = new Padding(4, 0, 4, 0);
            statusText.Name = "statusText";
            statusText.Size = new Size(106, 25);
            statusText.TabIndex = 3;
            statusText.Text = "Status - Idle";
            statusText.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // downloadButton
            // 
            downloadButton.Dock = DockStyle.Top;
            downloadButton.Location = new Point(0, 56);
            downloadButton.Margin = new Padding(4);
            downloadButton.Name = "downloadButton";
            downloadButton.Size = new Size(1351, 36);
            downloadButton.TabIndex = 2;
            downloadButton.Text = "Download Song";
            downloadButton.UseVisualStyleBackColor = true;
            downloadButton.Click += downloadButton_Click;
            // 
            // linkText
            // 
            linkText.Dock = DockStyle.Top;
            linkText.Location = new Point(0, 25);
            linkText.Margin = new Padding(4);
            linkText.Name = "linkText";
            linkText.Size = new Size(1351, 31);
            linkText.TabIndex = 1;
            linkText.DoubleClick += linkText_DoubleClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(0, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(135, 25);
            label1.TabIndex = 0;
            label1.Text = "Enter Song Link";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // listViewSongs
            // 
            listViewSongs.Columns.AddRange(new ColumnHeader[] { titleSongList, BitRate, durationSong });
            listViewSongs.Dock = DockStyle.Fill;
            listViewSongs.Location = new Point(0, 0);
            listViewSongs.Margin = new Padding(4);
            listViewSongs.MultiSelect = false;
            listViewSongs.Name = "listViewSongs";
            listViewSongs.Size = new Size(1351, 200);
            listViewSongs.TabIndex = 0;
            listViewSongs.UseCompatibleStateImageBehavior = false;
            listViewSongs.View = View.Details;
            listViewSongs.ItemSelectionChanged += listViewSongs_ItemSelectionChanged;
            // 
            // titleSongList
            // 
            titleSongList.Text = "Title";
            titleSongList.Width = 400;
            // 
            // durationSong
            // 
            durationSong.Text = "Duration";
            // 
            // youtubeSearchResults
            // 
            youtubeSearchResults.AutoSize = true;
            youtubeSearchResults.Dock = DockStyle.Fill;
            youtubeSearchResults.Location = new Point(0, 0);
            youtubeSearchResults.Name = "youtubeSearchResults";
            youtubeSearchResults.Size = new Size(1351, 286);
            youtubeSearchResults.TabIndex = 2;
            // 
            // youtubeSearchTextBox
            // 
            youtubeSearchTextBox.BackColor = Color.White;
            youtubeSearchTextBox.Dock = DockStyle.Bottom;
            youtubeSearchTextBox.Location = new Point(0, 286);
            youtubeSearchTextBox.Name = "youtubeSearchTextBox";
            youtubeSearchTextBox.Size = new Size(1351, 31);
            youtubeSearchTextBox.TabIndex = 0;
            // 
            // youtubeSearchButton
            // 
            youtubeSearchButton.Dock = DockStyle.Bottom;
            youtubeSearchButton.Location = new Point(0, 317);
            youtubeSearchButton.Name = "youtubeSearchButton";
            youtubeSearchButton.Size = new Size(1351, 34);
            youtubeSearchButton.TabIndex = 1;
            youtubeSearchButton.Text = "Search on youtube";
            youtubeSearchButton.UseVisualStyleBackColor = true;
            youtubeSearchButton.Click += youtubeSearchButton_Click;
            // 
            // panel1
            // 
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.Controls.Add(saveButton);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 689);
            panel1.Name = "panel1";
            panel1.Size = new Size(480, 88);
            panel1.TabIndex = 1;
            // 
            // saveButton
            // 
            saveButton.Dock = DockStyle.Fill;
            saveButton.Location = new Point(0, 0);
            saveButton.Margin = new Padding(4);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(480, 88);
            saveButton.TabIndex = 15;
            saveButton.Text = "Save";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += button1_Click_1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel2.BackColor = Color.White;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(keyText, 1, 8);
            tableLayoutPanel2.Controls.Add(keyDisplay, 0, 8);
            tableLayoutPanel2.Controls.Add(yearText, 1, 5);
            tableLayoutPanel2.Controls.Add(yearDisplay, 0, 5);
            tableLayoutPanel2.Controls.Add(genreText, 1, 4);
            tableLayoutPanel2.Controls.Add(label3, 0, 4);
            tableLayoutPanel2.Controls.Add(discText, 1, 7);
            tableLayoutPanel2.Controls.Add(label2, 0, 7);
            tableLayoutPanel2.Controls.Add(bpmText, 1, 6);
            tableLayoutPanel2.Controls.Add(bpmDisplay, 0, 6);
            tableLayoutPanel2.Controls.Add(albumText, 1, 3);
            tableLayoutPanel2.Controls.Add(artistText, 1, 1);
            tableLayoutPanel2.Controls.Add(titleText, 1, 0);
            tableLayoutPanel2.Controls.Add(titleSongDisplay, 0, 0);
            tableLayoutPanel2.Controls.Add(artistDisplay, 0, 1);
            tableLayoutPanel2.Controls.Add(AlbumSongDisplay, 0, 3);
            tableLayoutPanel2.Controls.Add(contributingArtistsDisplayt, 0, 2);
            tableLayoutPanel2.Controls.Add(contributingArtistText, 1, 2);
            tableLayoutPanel2.Controls.Add(uploadImage, 1, 9);
            tableLayoutPanel2.Controls.Add(pictureBox1, 0, 9);
            tableLayoutPanel2.Dock = DockStyle.Top;
            tableLayoutPanel2.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 10;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.36025143F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.36025333F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.36025333F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.36025333F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.3563714F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.360014F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.358044F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.358044F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 7.35593843F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 33.7705765F));
            tableLayoutPanel2.Size = new Size(480, 680);
            tableLayoutPanel2.TabIndex = 0;
            tableLayoutPanel2.Paint += tableLayoutPanel2_Paint;
            // 
            // keyText
            // 
            keyText.Dock = DockStyle.Fill;
            keyText.Location = new Point(244, 404);
            keyText.Margin = new Padding(4);
            keyText.Multiline = true;
            keyText.Name = "keyText";
            keyText.Size = new Size(232, 42);
            keyText.TabIndex = 13;
            // 
            // keyDisplay
            // 
            keyDisplay.AutoSize = true;
            keyDisplay.Dock = DockStyle.Fill;
            keyDisplay.Location = new Point(4, 400);
            keyDisplay.Margin = new Padding(4, 0, 4, 0);
            keyDisplay.Name = "keyDisplay";
            keyDisplay.Size = new Size(232, 50);
            keyDisplay.TabIndex = 21;
            keyDisplay.Text = "Key";
            keyDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // yearText
            // 
            yearText.Dock = DockStyle.Fill;
            yearText.Location = new Point(244, 254);
            yearText.Margin = new Padding(4);
            yearText.Multiline = true;
            yearText.Name = "yearText";
            yearText.Size = new Size(232, 42);
            yearText.TabIndex = 10;
            // 
            // yearDisplay
            // 
            yearDisplay.AutoSize = true;
            yearDisplay.Dock = DockStyle.Fill;
            yearDisplay.Location = new Point(4, 250);
            yearDisplay.Margin = new Padding(4, 0, 4, 0);
            yearDisplay.Name = "yearDisplay";
            yearDisplay.Size = new Size(232, 50);
            yearDisplay.TabIndex = 19;
            yearDisplay.Text = "Year";
            yearDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // genreText
            // 
            genreText.Dock = DockStyle.Fill;
            genreText.Location = new Point(244, 204);
            genreText.Margin = new Padding(4);
            genreText.Multiline = true;
            genreText.Name = "genreText";
            genreText.Size = new Size(232, 42);
            genreText.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(4, 200);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(232, 50);
            label3.TabIndex = 17;
            label3.Text = "Genre";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // discText
            // 
            discText.Dock = DockStyle.Fill;
            discText.Location = new Point(244, 354);
            discText.Margin = new Padding(4);
            discText.Multiline = true;
            discText.Name = "discText";
            discText.Size = new Size(232, 42);
            discText.TabIndex = 12;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(4, 350);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(232, 50);
            label2.TabIndex = 15;
            label2.Text = "Disc Number";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // bpmText
            // 
            bpmText.Dock = DockStyle.Fill;
            bpmText.Location = new Point(244, 304);
            bpmText.Margin = new Padding(4);
            bpmText.Multiline = true;
            bpmText.Name = "bpmText";
            bpmText.Size = new Size(232, 42);
            bpmText.TabIndex = 11;
            // 
            // bpmDisplay
            // 
            bpmDisplay.AutoSize = true;
            bpmDisplay.Dock = DockStyle.Fill;
            bpmDisplay.Location = new Point(4, 300);
            bpmDisplay.Margin = new Padding(4, 0, 4, 0);
            bpmDisplay.Name = "bpmDisplay";
            bpmDisplay.Size = new Size(232, 50);
            bpmDisplay.TabIndex = 13;
            bpmDisplay.Text = "BPM";
            bpmDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // albumText
            // 
            albumText.Dock = DockStyle.Fill;
            albumText.Location = new Point(244, 154);
            albumText.Margin = new Padding(4);
            albumText.Multiline = true;
            albumText.Name = "albumText";
            albumText.Size = new Size(232, 42);
            albumText.TabIndex = 8;
            // 
            // artistText
            // 
            artistText.Dock = DockStyle.Fill;
            artistText.Location = new Point(244, 54);
            artistText.Margin = new Padding(4);
            artistText.Multiline = true;
            artistText.Name = "artistText";
            artistText.Size = new Size(232, 42);
            artistText.TabIndex = 6;
            // 
            // titleText
            // 
            titleText.Dock = DockStyle.Fill;
            titleText.Location = new Point(244, 4);
            titleText.Margin = new Padding(4);
            titleText.Multiline = true;
            titleText.Name = "titleText";
            titleText.Size = new Size(232, 42);
            titleText.TabIndex = 5;
            // 
            // titleSongDisplay
            // 
            titleSongDisplay.AutoSize = true;
            titleSongDisplay.Dock = DockStyle.Fill;
            titleSongDisplay.Location = new Point(4, 0);
            titleSongDisplay.Margin = new Padding(4, 0, 4, 0);
            titleSongDisplay.Name = "titleSongDisplay";
            titleSongDisplay.Size = new Size(232, 50);
            titleSongDisplay.TabIndex = 0;
            titleSongDisplay.Text = "Title";
            titleSongDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // artistDisplay
            // 
            artistDisplay.AutoSize = true;
            artistDisplay.Dock = DockStyle.Fill;
            artistDisplay.Location = new Point(4, 50);
            artistDisplay.Margin = new Padding(4, 0, 4, 0);
            artistDisplay.Name = "artistDisplay";
            artistDisplay.Size = new Size(232, 50);
            artistDisplay.TabIndex = 1;
            artistDisplay.Text = "Artist";
            artistDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AlbumSongDisplay
            // 
            AlbumSongDisplay.AutoSize = true;
            AlbumSongDisplay.Dock = DockStyle.Fill;
            AlbumSongDisplay.Location = new Point(4, 150);
            AlbumSongDisplay.Margin = new Padding(4, 0, 4, 0);
            AlbumSongDisplay.Name = "AlbumSongDisplay";
            AlbumSongDisplay.Size = new Size(232, 50);
            AlbumSongDisplay.TabIndex = 3;
            AlbumSongDisplay.Text = "Album";
            AlbumSongDisplay.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // contributingArtistsDisplayt
            // 
            contributingArtistsDisplayt.AutoSize = true;
            contributingArtistsDisplayt.Dock = DockStyle.Fill;
            contributingArtistsDisplayt.Location = new Point(4, 100);
            contributingArtistsDisplayt.Margin = new Padding(4, 0, 4, 0);
            contributingArtistsDisplayt.Name = "contributingArtistsDisplayt";
            contributingArtistsDisplayt.Size = new Size(232, 50);
            contributingArtistsDisplayt.TabIndex = 2;
            contributingArtistsDisplayt.Text = "Contributing Artist";
            contributingArtistsDisplayt.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // contributingArtistText
            // 
            contributingArtistText.Dock = DockStyle.Fill;
            contributingArtistText.Location = new Point(244, 104);
            contributingArtistText.Margin = new Padding(4);
            contributingArtistText.Multiline = true;
            contributingArtistText.Name = "contributingArtistText";
            contributingArtistText.Size = new Size(232, 42);
            contributingArtistText.TabIndex = 7;
            // 
            // uploadImage
            // 
            uploadImage.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            uploadImage.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            uploadImage.Location = new Point(243, 548);
            uploadImage.Name = "uploadImage";
            uploadImage.Size = new Size(234, 34);
            uploadImage.TabIndex = 14;
            uploadImage.Text = "Upload Image";
            uploadImage.UseVisualStyleBackColor = true;
            uploadImage.Click += uploadImage_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.WhiteSmoke;
            pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            pictureBox1.Location = new Point(3, 453);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(224, 224);
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1836, 810);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            bottomNavigator.Panel1.ResumeLayout(false);
            bottomNavigator.Panel1.PerformLayout();
            bottomNavigator.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)bottomNavigator).EndInit();
            bottomNavigator.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private SplitContainer splitContainer1;
        private SplitContainer bottomNavigator;
        private Label label1;
        private TextBox linkText;
        private Button downloadButton;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem downloadLocationToolStripMenuItem;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label statusText;
        private Label pathDisplay;
        private ToolStripMenuItem preferencesToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private Label statusQueue;
        private Label queueStatus;
        private ProgressBar progressSongStatus;
        private ListView listViewSongs;
        private ColumnHeader titleSongList;
        private ColumnHeader durationSong;
        private ColumnHeader BitRate;
        private TableLayoutPanel tableLayoutPanel2;
        private Label titleSongDisplay;
        private Label artistDisplay;
        private Label AlbumSongDisplay;
        private Label contributingArtistsDisplayt;
        private TextBox artistText;
        private TextBox titleText;
        private TextBox contributingArtistText;
        private TextBox albumText;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Panel panel1;
        private Button saveButton;
        private OpenFileDialog openFileDialog1;
        private Button uploadImage;
        private PictureBox pictureBox1;
        private TextBox yearText;
        private Label yearDisplay;
        private TextBox genreText;
        private Label label3;
        private TextBox discText;
        private Label label2;
        private TextBox bpmText;
        private Label bpmDisplay;
        private TextBox keyText;
        private Label keyDisplay;
        private SplitContainer splitContainer2;
        private TextBox youtubeSearchTextBox;
        private Button youtubeSearchButton;
        private FlowLayoutPanel youtubeSearchResults;
    }
}
