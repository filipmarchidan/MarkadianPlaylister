using System.CodeDom;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms.VisualStyles;
using TagLib.Id3v2;
using Xabe.FFmpeg;

namespace MarkadianPlaylister
{
    public partial class Form1 : Form
    {
        /*
         * Main class this contains the metadata logic and UI logic
         *
         */
        public MarkadianSettings markadianSettings;
        public SearchLogic searchLogic = new SearchLogic();
        public static string filePath;
        public bool locked;
        public static int songsDownloaded { get; set; }
        public static int songsEnqueued { get; set; }
        string ffmpeg { get; set; }
        string ffprobe { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffprobe.exe");
        Queue<String> videoLinks = new Queue<String>();
        string ffplay { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffplay.exe");
        public static string exePath { get; set; }

        public String currentImagePath = null;
        public Form1()
        {
            InitializeComponent();
            //ResourceManager.EnsureAllExtracted();


            //This handles the dynamic behavior for progress bars and number of songs downloaded
            searchLogic.downloadLogic.ProgressChanged += (value) =>
            {
                if (progressSongStatus.InvokeRequired)
                    progressSongStatus.Invoke(() => progressSongStatus.Value = value);
                else
                    progressSongStatus.Value = value;
            };

            searchLogic.downloadLogic.StatusChanged += (text) =>
            {
                if (statusText.InvokeRequired)
                    statusText.Invoke(() => statusText.Text = text);
                else
                    statusText.Text = text;
            };

            searchLogic.downloadLogic.QueueStatusChanged += (text) =>
            {
                if (statusQueue.InvokeRequired)
                    statusQueue.Invoke(() => statusQueue.Text = text);
                else
                    statusQueue.Text = text;
            };



        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        //function that checks if all dependencies are updated. For details see ResourceUpdater
        public static async void checkUpdates()
        {
            await ResourceUpdater.EnsureResourcesAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //load settings and initial load.
            listViewSongs.Items.Clear();
            markadianSettings = SettingsManager.LoadSettings();
            ThemeManager.SetTheme(markadianSettings.theme == "Dark" ? AppTheme.Dark : AppTheme.Light);
            ThemeManager.ApplyTheme(this);
            filePath = markadianSettings.filePath;
            pathDisplay.Text = filePath.ToString();
            songsDownloaded = 0;
            songsEnqueued = 0;

            splitContainer1.Panel2.AllowDrop = true;
            if (!markadianSettings.enableQueue)
            {
                queueStatus.Text = "Queue Disabled";
            }
            else { queueStatus.Text = "Queue Enabled"; }

            if (markadianSettings.enableUpdates)
            {

                enableAutomaticUpdatesToolStripMenuItem.Checked = true;
            }
            else
            {
                enableAutomaticUpdatesToolStripMenuItem.Checked = false;
            }

            if (markadianSettings.enableDragDrop)
            {
                enabToolStripMenuItem.Checked = true;
            }
            else enabToolStripMenuItem.Checked = false;
            checkUpdates();
            indexAudio(filePath);

            if (markadianSettings.enableDragDrop)
            {
                listViewSongs.AllowDrop = true;
                splitContainer1.Panel2.AllowDrop = true;
            }
            else
            {
                listViewSongs.AllowDrop = false;
                splitContainer1.Panel2.AllowDrop = false;
            }

            if (markadianSettings.theme == "Dark")
            {
                darkToolStripMenuItem.Checked = true;
                lightToolStripMenuItem.Checked = false;
            }
            else
            {
                darkToolStripMenuItem.Checked = false;
                lightToolStripMenuItem.Checked = true;
            }

            ffmpeg = Path.Combine(markadianSettings.resourceDirectory, "ffmpeg.exe");
            exePath = Path.Combine(markadianSettings.resourceDirectory, "yt-dlp.exe");

            //for debug only. Check dependencies
            Console.WriteLine("yt-dlp path: " + exePath);
            Console.WriteLine("ffmpeg dir: " + Path.GetDirectoryName(ffmpeg));
            Console.WriteLine("ffmpeg exists: " + File.Exists(ffmpeg));
            Console.WriteLine("ffprobe exists: " + File.Exists(Path.Combine(Path.GetDirectoryName(ffmpeg), "ffprobe.exe")));



            // Find the existing Form1_Load method and add the following near the end (after designer-setup code like ffmpeg/exePath logging)

            if (songOptionMenu != null)
            {
                // Open in Explorer
                if (!songOptionMenu.Items.OfType<ToolStripItem>().Any(i => i.Text == "Open in File Explorer"))
                    songOptionMenu.Items.Add(new ToolStripMenuItem("Open in File Explorer", null, (s, ev) => OpenSelectedInExplorer()));

                // Open with default player
                if (!songOptionMenu.Items.OfType<ToolStripItem>().Any(i => i.Text == "Open With Default Music Player"))
                    songOptionMenu.Items.Add(new ToolStripMenuItem("Open With Default Music Player", null, (s, ev) => OpenWithDefaultPlayer()));

                // Separator + Delete
                if (!songOptionMenu.Items.OfType<ToolStripItem>().Any(i => i.Text == "Delete"))
                {
                    songOptionMenu.Items.Add(new ToolStripSeparator());
                    songOptionMenu.Items.Add(new ToolStripMenuItem("Delete", null, (s, ev) => DeleteSelectedSong()));
                }

                // Assign the menu to the list view
                listViewSongs.ContextMenuStrip = songOptionMenu;
            }

            // Ensure right-click selects the item under the cursor
            listViewSongs.MouseDown -= listViewSongs_MouseDown;
            listViewSongs.MouseDown += listViewSongs_MouseDown;
        }

        //function that scans the folder for .mp3 and .wav files
        public async void indexAudio(String filePath)
        {
            listViewSongs.Items.Clear();
            var files = Directory.GetFiles(filePath, "*.*")
                       .Where(f => f.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase)
                                || f.EndsWith(".wav", StringComparison.OrdinalIgnoreCase));

            foreach (var f in files)
            {
                int bitRate = 0;
                string title = Path.GetFileNameWithoutExtension(f);
                var item = new ListViewItem(title);
                try
                {
                    using (var tfile = TagLib.File.Create(f))
                    {
                        bitRate = tfile.Properties.AudioBitrate;
                        TimeSpan duration = tfile.Properties.Duration;


                        item.SubItems.Add(bitRate.ToString());
                        item.SubItems.Add(duration.ToString(@"mm\:ss"));
                        item.Tag = f;

                        listViewSongs.Items.Add(item);

                    }
                }
                catch (Exception ex) { MessageBox.Show("error in reading audio files"); }


            }
        }

        //public void indexSongMetadata()
        //this calls the download logic
        private async void downloadButton_Click(object sender, EventArgs e)
        {

            statusText.Text = "Downloading";
            string videoUrl = linkText.Text.Trim();
            if (string.IsNullOrWhiteSpace(videoUrl))
            {
                MessageBox.Show("Please enter a YouTube URL.");
                return;
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                MessageBox.Show("Please select a download folder.");
                return;
            }

            await searchLogic.downloadLogic.handleDownloadLogic(videoUrl);

        }



        //function to change your download location
        private void downloadLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Select a new location for your music";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = folderBrowserDialog1.SelectedPath;
                pathDisplay.Text = filePath.ToString();
                markadianSettings.filePath = filePath;
                indexAudio(filePath);
            }
            else { MessageBox.Show("Not a valid path"); }
        }




        //this opens the settings form
        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var prefForm = new Preferences(this.markadianSettings);
            if (prefForm.ShowDialog() == DialogResult.OK)
            {
                prefForm.Show();
            }
            pathDisplay.Text = "Current Path: " + markadianSettings.filePath.ToString();
            if (markadianSettings.enableQueue) queueStatus.Text = "Queue Enabled"; else queueStatus.Text = "Queue Disabled";
        }

        private void linkText_DoubleClick(object sender, EventArgs e)
        {
            linkText.SelectAll();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        //this handles the metadata when you click a certain file in the list
        private void listViewSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSongs.SelectedItems.Count == 0)
                return; // no selection

            var selectedItem = listViewSongs.SelectedItems[0];


            // Retrieve full file path stored in Tag
            string filePath = selectedItem.Tag as string;
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                using (var tfile = TagLib.File.Create(filePath))
                {
                    string title = tfile.Tag.Title ?? Path.GetFileNameWithoutExtension(filePath);
                    string artist = tfile.Tag.FirstPerformer ?? "Unknown";
                    string album = tfile.Tag.Album ?? "Unknown";
                    int bitrate = tfile.Properties.AudioBitrate;
                    TimeSpan duration = tfile.Properties.Duration;

                    // Update UI labels or panel
                    titleText.Text = title;
                    artistText.Text = artist;
                    albumText.Text = album;
                    genreText.Text = tfile.Tag.Genres.ToString();
                    yearDisplay.Text = tfile.Tag.Year.ToString();
                    bpmText.Text = tfile.Tag.BeatsPerMinute.ToString();
                    var tagTemp = tfile.GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag;
                    discText.Text = tfile.Tag.Disc.ToString();
                    contributingArtistText.Text = tfile.Tag.AlbumArtists.ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file metadata: {ex.Message}");
            }
        }

        //this saves the metadata of the file you edited in the metadata editor.
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (listViewSongs.SelectedItems.Count == 0 || listViewSongs.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please select a song to edit.");
                return;
            }

            var selectedItem = listViewSongs.SelectedItems[0];
            string filePath = selectedItem.Tag as string;
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show($"Error saving metadata: ");
                return;
            }

            try
            {
                using (var tfile = TagLib.File.Create(filePath))
                {
                    // Example: override metadata from textboxes
                    tfile.Tag.Title = titleText.Text.Trim();
                    tfile.Tag.Performers = new[] { contributingArtistText.Text.Trim() };
                    tfile.Tag.Album = albumText.Text.Trim();
                    tfile.Tag.Genres = new[] { genreText.Text.Trim() };

                    if (uint.TryParse(yearText.Text.Trim(), out var year))
                        tfile.Tag.Year = year;

                    if (uint.TryParse(discText.Text.Trim(), out var disc))
                        tfile.Tag.Disc = disc;

                    if (uint.TryParse(bpmText.Text.Trim(), out var bpm))
                        tfile.Tag.BeatsPerMinute = bpm;

                    // --- Explicitly write musical key (TKEY) to ID3v2 ---
                    var id3v2 = tfile.GetTag(TagLib.TagTypes.Id3v2, true) as TagLib.Id3v2.Tag;
                    if (id3v2 != null)
                    {
                        var keyFrame = TagLib.Id3v2.TextInformationFrame.Get(id3v2, "TKEY", true);
                        keyFrame.Text = new[] { keyText.Text.Trim() };
                    }

                    if (!string.IsNullOrEmpty(currentImagePath) && File.Exists(currentImagePath))
                    {
                        var pictureData = File.ReadAllBytes(currentImagePath);
                        var picture = new TagLib.Picture
                        {
                            Type = TagLib.PictureType.FrontCover,
                            Description = "Cover",
                            MimeType = GetMimeType(currentImagePath),
                            Data = new TagLib.ByteVector(pictureData)
                        };
                        tfile.Tag.Pictures = new TagLib.IPicture[] { picture };
                    }


                    // Save changes back to file
                    tfile.Save();
                }

                // Update list view immediately
                selectedItem.Text = titleText.Text;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving metadata: {ex.Message}");
            }
        }

        //handles image formats
        private string GetMimeType(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp",
                _ => "image/unknown"
            };
        }

        //this handles the metadata when you click a certain file in the list
        private void listViewSongs_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listViewSongs.SelectedItems.Count == 0)
                return; // no selection

            var selectedItem = listViewSongs.SelectedItems[0];

            // Retrieve full file path stored in Tag
            string filePath = selectedItem.Tag as string;
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                using (var tfile = TagLib.File.Create(filePath))
                {
                    string title = tfile.Tag.Title ?? Path.GetFileNameWithoutExtension(filePath);
                    string artist = tfile.Tag.FirstPerformer ?? "Unknown";
                    string album = tfile.Tag.Album ?? "Unknown";
                    int bitrate = tfile.Properties.AudioBitrate;
                    TimeSpan duration = tfile.Properties.Duration;

                    // Update UI labels or panel
                    titleText.Text = title;
                    artistText.Text = artist;
                    albumText.Text = album;

                    // --- Properly handle arrays ---
                    contributingArtistText.Text = string.Join(", ", tfile.Tag.Performers ?? Array.Empty<string>());
                    genreText.Text = string.Join(", ", tfile.Tag.Genres ?? Array.Empty<string>());

                    // numeric properties
                    yearText.Text = tfile.Tag.Year > 0 ? tfile.Tag.Year.ToString() : string.Empty;
                    discText.Text = tfile.Tag.Disc > 0 ? tfile.Tag.Disc.ToString() : string.Empty;
                    bpmText.Text = tfile.Tag.BeatsPerMinute > 0 ? tfile.Tag.BeatsPerMinute.ToString() : string.Empty;

                    // --- Musical key (TKEY) ---
                    var id3v2 = tfile.GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag;
                    string key = string.Empty;

                    if (id3v2 != null)
                    {
                        var keyFrame = id3v2.GetFrames("TKEY")
                                            .OfType<TagLib.Id3v2.TextInformationFrame>()
                                            .FirstOrDefault();
                        if (keyFrame != null && keyFrame.Text.Length > 0)
                            key = keyFrame.Text[0];
                    }

                    keyText.Text = key;

                    if (tfile.Tag.Pictures != null && tfile.Tag.Pictures.Length > 0)
                    {
                        var bin = tfile.Tag.Pictures[0].Data.Data;
                        using (var ms = new MemoryStream(bin))
                        {
                            pictureBox1.Image = Image.FromStream(ms);
                            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    }
                    else
                    {
                        pictureBox1.Image = null; // no embedded art
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file metadata: {ex.Message}");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //handles images to update the metadata for a file
        private void uploadImage_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Select Album Art";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    currentImagePath = ofd.FileName;
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //handles searches in youtube
        private async void youtubeSearchButton_Click(object sender, EventArgs e)
        {
            string query = youtubeSearchTextBox.Text;
            if (string.IsNullOrEmpty(query))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            youtubeSearchResults.Controls.Clear();
            youtubeSearchResults.SuspendLayout();
            //results will depend on the search count
            // the higher the search count the slower the performance
            // see SearchLogic.cs for further details
            try
            {
                var results = await searchLogic.SearchYoutubeVideosAsync(query);

                foreach (var r in results.Take(int.Parse(markadianSettings.searchCount)))
                {
                    var card = searchLogic.CreateYoutubeResultCard(r);
                    youtubeSearchResults.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search failed: {ex.Message}");
            }
            finally
            {
                youtubeSearchResults.ResumeLayout();
            }
        }

        private void youtubeSearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        //handles enter as a search
        private void youtubeSearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    youtubeSearchButton_Click(sender, e);
                    break;

            }
        }

        private void linkText_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    downloadButton_Click(sender, e);
                    break;

            }
        }
        //handles panel viewing
        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (downloadToolStripMenuItem.Checked)
            {
                downloadToolStripMenuItem.Checked = false;
                bottomNavigator.Panel1Collapsed = true;
            }
            else
            {
                downloadToolStripMenuItem.Checked = true;
                bottomNavigator.Panel1Collapsed = false;
            }
        }
        //handles panel viewing
        private void youtubeSearchPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (youtubeSearchPanelToolStripMenuItem.Checked)
            {
                youtubeSearchPanelToolStripMenuItem.Checked = false;
                splitContainer2.Panel2Collapsed = true;
            }
            else
            {
                youtubeSearchPanelToolStripMenuItem.Checked = true;
                splitContainer2.Panel2Collapsed = false;

            }
        }
        //handles panel viewing
        private void metadataPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (metadataPanelToolStripMenuItem.Checked)
            {
                metadataPanelToolStripMenuItem.Checked = false;
                splitContainer1.Panel2Collapsed = true;
            }
            else
            {

                metadataPanelToolStripMenuItem.Checked = true;
                splitContainer1.Panel2Collapsed = false;
            }
        }
        //handles panel viewing
        private void listPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listPanelToolStripMenuItem.Checked)
            {
                listPanelToolStripMenuItem.Checked = false;
                bottomNavigator.Panel2Collapsed = true;
            }
            else
            {
                listPanelToolStripMenuItem.Checked = true;
                bottomNavigator.Panel2Collapsed = false;
            }
        }
        //handles theme option
        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lightToolStripMenuItem.Checked)
            {
                return;
            }
            else
            {
                //see theme manager for further details
                darkToolStripMenuItem.Checked = false;
                ThemeManager.SetTheme(AppTheme.Light);
                markadianSettings.theme = "Light";
                lightToolStripMenuItem.Checked = true;
                ThemeManager.ApplyTheme(this);
                SettingsManager.SaveSettings(markadianSettings);
            }


        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (darkToolStripMenuItem.Checked) { return; }

            else
            {
                lightToolStripMenuItem.Checked = false;
                darkToolStripMenuItem.Checked = true;
                ThemeManager.SetTheme(AppTheme.Dark);
                markadianSettings.theme = "Dark";
                ThemeManager.ApplyTheme(this);
                SettingsManager.SaveSettings(markadianSettings);
            }
        }

        private void listViewSongs_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }


        //handles drag and drop functionality for metadata
        private void listViewSongs_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
            bool valid = files.Any(f =>
            {
                if (string.IsNullOrEmpty(f) || !File.Exists(f)) return false;
                string ext = Path.GetExtension(f).ToLowerInvariant();
                return ext == ".mp3" || ext == ".wav";
            });

            e.Effect = valid ? DragDropEffects.Copy : DragDropEffects.None;

            // Use theme-aware drag-over color instead of hard-coded grey
            listViewSongs.BackColor = ThemeManager.GetDragOverColor();
        }

        //handles drag and drop functionality for metadata
        private void listViewSongs_DragLeave(object sender, EventArgs e)
        {
            // Restore themed background instead of DefaultBackColor (which breaks dark theme)
            listViewSongs.BackColor = ThemeManager.GetDefaultBackColor(listViewSongs);
        }

        //handles drag and drop functionality for metadata
        private void listViewSongs_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
            foreach (var file in files)
            {
                try
                {
                    if (string.IsNullOrEmpty(file) || !File.Exists(file))
                        continue;

                    string ext = Path.GetExtension(file).ToLowerInvariant();
                    if (ext != ".mp3" && ext != ".wav")
                        continue;

                    // Reference only — do not copy the file
                    string pathToAdd = file;

                    int bitRate = 0;
                    string title = Path.GetFileNameWithoutExtension(pathToAdd);
                    var item = new ListViewItem(title);

                    using (var tfile = TagLib.File.Create(pathToAdd))
                    {
                        bitRate = tfile.Properties.AudioBitrate;
                        TimeSpan duration = tfile.Properties.Duration;

                        item.SubItems.Add(bitRate.ToString());
                        item.SubItems.Add(duration.ToString(@"mm\:ss"));
                        item.Tag = pathToAdd;

                        listViewSongs.Items.Add(item);
                        listViewSongs.Items[listViewSongs.Items.Count - 1].Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to load file:\n{Path.GetFileName(file)}\n\n{ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            // Restore themed background
            listViewSongs.BackColor = ThemeManager.GetDefaultBackColor(listViewSongs);
        }

        //handles metadata drag and drop in the metadata panel

        private void splitContainer1_Panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;

            bool valid = files.Any(f =>
            {
                if (string.IsNullOrEmpty(f) || !File.Exists(f)) return false;
                string ext = Path.GetExtension(f).ToLowerInvariant();
                return ext == ".mp3" || ext == ".wav";
            });

            e.Effect = valid ? DragDropEffects.Copy : DragDropEffects.None;

            // Use theme-aware drag-over color
            tableLayoutPanel2.BackColor = ThemeManager.GetDragOverColor();
        }

        //handles metadata drag and drop in the metadata panel

        private void splitContainer1_Panel2_DragLeave(object sender, EventArgs e)
        {
            // Restore themed background for panel
            tableLayoutPanel2.BackColor = ThemeManager.GetDefaultBackColor(tableLayoutPanel2);
        }

        //handles metadata drag and drop in the metadata panel

        private void splitContainer1_Panel2_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
            foreach (var file in files)
            {
                try
                {
                    if (string.IsNullOrEmpty(file) || !File.Exists(file))
                        continue;

                    string ext = Path.GetExtension(file).ToLowerInvariant();
                    if (ext != ".mp3" && ext != ".wav")
                        continue;

                    // Reference only — do not copy the file
                    string pathToAdd = file;

                    int bitRate = 0;
                    string title = Path.GetFileNameWithoutExtension(pathToAdd);
                    var item = new ListViewItem(title);

                    using (var tfile = TagLib.File.Create(pathToAdd))
                    {
                        bitRate = tfile.Properties.AudioBitrate;
                        TimeSpan duration = tfile.Properties.Duration;

                        item.SubItems.Add(bitRate.ToString());
                        item.SubItems.Add(duration.ToString(@"mm\:ss"));
                        item.Tag = pathToAdd;

                        listViewSongs.Items.Add(item);
                        listViewSongs.Items[listViewSongs.Items.Count - 1].Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to load file:\n{Path.GetFileName(file)}\n\n{ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            tableLayoutPanel2.BackColor = ThemeManager.GetDefaultBackColor(tableLayoutPanel2);
        }

        //handles auto updates
        private void enableAutomaticUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enableAutomaticUpdatesToolStripMenuItem.Checked)
            {
                enableAutomaticUpdatesToolStripMenuItem.Checked = false;
                markadianSettings.enableUpdates = false;
                listViewSongs.AllowDrop = false;
                splitContainer1.Panel2.AllowDrop = false;
            }
            else
            {
                enableAutomaticUpdatesToolStripMenuItem.Checked = true;
                markadianSettings.enableUpdates = true;
                listViewSongs.AllowDrop = true;
                splitContainer1.Panel2.AllowDrop = true;
            }
        }

        private void enabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enabToolStripMenuItem.Checked)
            {
                enabToolStripMenuItem.Checked = false;
                markadianSettings.enableDragDrop = false;
            }
            else
            {
                enabToolStripMenuItem.Checked = true;
                markadianSettings.enableDragDrop = true;
            }
        }

        //save settings when the application closes
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingsManager.SaveSettings(markadianSettings);
        }

        //reindex the files
        private void rescanAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {

            indexAudio(filePath);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This app has been created by Markadian. This app is open source. Any donation is highly appreciated.");
        }

        private void discordServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var uri = "https://discord.gg/GeGanQaZ";
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = uri
            };
            System.Diagnostics.Process.Start(psi);
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Markadian Playlister v1.0.3");
        }

        private void listViewSongs_DoubleClick(object sender, EventArgs e)
        {
            OpenSelectedInExplorer();
        }


        private void OpenSelectedInExplorer()
        {
            if (listViewSongs.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewSongs.SelectedItems[0];
            string fullPath = selectedItem.Tag as string;

            if (string.IsNullOrEmpty(fullPath))
            {
                MessageBox.Show("File path not available for the selected item.");
                return;
            }

            if (!File.Exists(fullPath))
            {
                MessageBox.Show("File not found on disk.");
                return;
            }

            try
            {
                var psi = new ProcessStartInfo("explorer.exe", $"/select,\"{fullPath}\"")
                {
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open File Explorer: {ex.Message}");
            }
        }

        private void OpenWithDefaultPlayer()
        {
            if (listViewSongs.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewSongs.SelectedItems[0];
            string fullPath = selectedItem.Tag as string;

            if (string.IsNullOrEmpty(fullPath))
            {
                MessageBox.Show("File path not available for the selected item.");
                return;
            }

            if (!File.Exists(fullPath))
            {
                MessageBox.Show("File not found on disk.");
                return;
            }

            try
            {
                var psi = new ProcessStartInfo(fullPath)
                {
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open file with default player: {ex.Message}");
            }
        }

        private void DeleteSelectedSong()
        {
            if (listViewSongs.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewSongs.SelectedItems[0];
            string fullPath = selectedItem.Tag as string;

            if (string.IsNullOrEmpty(fullPath))
            {
                MessageBox.Show("File path not available for the selected item.");
                return;
            }

            if (!File.Exists(fullPath))
            {
                // If the file is already missing, remove from list and notify.
                listViewSongs.Items.Remove(selectedItem);
                MessageBox.Show("File not found on disk. Removed from list.");
                ClearMetadataFields();
                return;
            }

            var confirm = MessageBox.Show($"Are you sure you want to permanently delete '{Path.GetFileName(fullPath)}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                File.Delete(fullPath);
                listViewSongs.Items.Remove(selectedItem);
                ClearMetadataFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete file: {ex.Message}");
            }
        }

        private void ClearMetadataFields()
        {
            titleText.Text = string.Empty;
            artistText.Text = string.Empty;
            albumText.Text = string.Empty;
            contributingArtistText.Text = string.Empty;
            genreText.Text = string.Empty;
            yearText.Text = string.Empty;
            discText.Text = string.Empty;
            bpmText.Text = string.Empty;
            keyText.Text = string.Empty;
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;
        }

        private void listViewSongs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var hit = listViewSongs.HitTest(e.Location);
            if (hit.Item != null)
                hit.Item.Selected = true;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void listViewSongs_KeyDown(object sender, KeyEventArgs e)
        {
            if(listViewSongs.SelectedItems.Count == 0)
                return;
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    OpenSelectedInExplorer();
                    break;
            }
        }
    }











}
