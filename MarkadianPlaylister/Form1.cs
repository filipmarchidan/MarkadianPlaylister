using System.CodeDom;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms.VisualStyles;
using TagLib.Id3v2;
using Xabe.FFmpeg;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace MarkadianPlaylister
{
    public partial class Form1 : Form
    {

        public MarkadianSettings markadianSettings;
        public SearchLogic searchLogic = new SearchLogic();
        public static string filePath;
        public bool locked;
        public static int songsDownloaded { get; set; }
        public static int songsEnqueued { get; set; }
        string ffmpeg { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
        string ffprobe { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffprobe.exe");
        Queue<String> videoLinks = new Queue<String>();
        string ffplay { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffplay.exe");
        public static string exePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");

        public String currentImagePath = null;
        public Form1()
        {
            InitializeComponent();



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

        private void Form1_Load(object sender, EventArgs e)
        {
            listViewSongs.Items.Clear();
            markadianSettings = SettingsManager.LoadSettings();
            ThemeManager.SetTheme(markadianSettings.theme == "Dark" ? AppTheme.Dark : AppTheme.Light);
            ThemeManager.ApplyTheme(this);
            filePath = markadianSettings.filePath;
            pathDisplay.Text = filePath.ToString();
            songsDownloaded = 0;
            songsEnqueued = 0;
            if (!markadianSettings.enableQueue)
            {
                queueStatus.Text = "Queue Disabled";

            }
            else { queueStatus.Text = "Queue Enabled"; }

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




        private void downloadLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Select a new location for your music";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = folderBrowserDialog1.SelectedPath;
                pathDisplay.Text = filePath.ToString();
                markadianSettings.filePath = filePath;
            }
            else { MessageBox.Show("Not a valid path"); }
        }





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

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lightToolStripMenuItem.Checked)
            {
                return;
            }
            else
            {
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






        //public async Task handleDownloadLogic(String videoUrl)
        //{
        //    videoUrl = SanitizeYoutubeUrl(videoUrl);
        //    if (markadianSettings.enableQueue)
        //    {

        //        videoLinks.Enqueue(videoUrl);
        //        songsEnqueued++;
        //        statusQueue.Text = songsDownloaded.ToString() + " / " + songsEnqueued.ToString() + " Songs Downloaded";

        //        if (videoLinks.Count == 1)
        //        {
        //            locked = false;
        //            await startDownloadingWithQueue(videoLinks, filePath);
        //        }
        //        return;
        //    }
        //    else await DownloadWithYtDlp(videoUrl, filePath);
        //}

        //private async Task startDownloadingWithQueue(Queue<string> videoLinks, string filePath)
        //{


        //    while (videoLinks.Count > 0)
        //    {
        //        if (!locked)
        //        {
        //            string currentVideo = videoLinks.Dequeue();
        //            await DownloadWithYtDlp(currentVideo, filePath);
        //            locked = true;
        //        }

        //    }
        //}

        //private async Task DownloadWithYtDlp(string videoUrl, string folderPath)
        //{
        //    progressSongStatus.Value = 0;

        //    // Path to yt-dlp
        //    string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");
        //    if (!File.Exists(exePath))
        //        throw new FileNotFoundException("yt-dlp executable not found", exePath);

        //    // Validate URL
        //    bool IsValidYoutubeUrl(string url) =>
        //        Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
        //        (uri.Host.Contains("youtube.com") || uri.Host.Contains("youtu.be"));

        //    if (!IsValidYoutubeUrl(videoUrl))
        //    {
        //        MessageBox.Show("Not a valid YouTube URL");
        //        return;
        //    }

        //    // --- Step 1: Get video title ---
        //    var titlePsi = new ProcessStartInfo
        //    {
        //        FileName = exePath,
        //        Arguments = $"--get-title \"{videoUrl}\"",
        //        RedirectStandardOutput = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = true
        //    };

        //    using var titleProc = Process.Start(titlePsi);
        //    string videoTitle = (await titleProc.StandardOutput.ReadToEndAsync()).Trim();
        //    await titleProc.WaitForExitAsync();

        //    if (string.IsNullOrWhiteSpace(videoTitle))
        //        videoTitle = "UnknownTitle";

        //    // --- Step 2: Sanitize filename ---
        //    string MakeSafeFileName(string name)
        //    {
        //        foreach (char c in Path.GetInvalidFileNameChars())
        //            name = name.Replace(c, '_');
        //        return name;
        //    }

        //    string safeTitle = MakeSafeFileName(videoTitle);

        //    // --- Step 3: Set output template and target file ---
        //    string outputTemplate = Path.Combine(folderPath, safeTitle + ".%(ext)s");
        //    string downloadedFile = Path.Combine(folderPath, safeTitle + ".mp3");
        //    string tempFile = Path.Combine(folderPath, safeTitle + "_temp.mp3");

        //    // --- Step 4: Download with yt-dlp ---
        //    var psi = new ProcessStartInfo
        //    {
        //        FileName = exePath,
        //        Arguments = $"-f bestaudio --no-cache-dir --extract-audio --audio-format mp3 " +
        //                    $"--user-agent \"Mozilla/5.0\" " +
        //                    $"--ffmpeg-location \"{Path.GetDirectoryName(ffmpeg)}\" " +
        //                    $"-o \"{outputTemplate}\" \"{videoUrl}\"",
        //        RedirectStandardOutput = true,
        //        RedirectStandardError = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = true
        //    };

        //    using var proc = Process.Start(psi);
        //    string stdout = await proc.StandardOutput.ReadToEndAsync();
        //    string stderr = await proc.StandardError.ReadToEndAsync();
        //    await proc.WaitForExitAsync();

        //    if (proc.ExitCode != 0 || !File.Exists(downloadedFile))
        //    {
        //        MessageBox.Show($"Error downloading video:\n{stderr}");
        //        return;
        //    }

        //    // --- Step 5: FFmpeg conversion to target bitrate ---
        //    string bitRate = markadianSettings.bitRateSelector; // default 192 kbps

        //    var conversion = Xabe.FFmpeg.FFmpeg.Conversions.New()
        //        .AddParameter($"-i \"{downloadedFile}\" -vn -ar 44100 -b:a {bitRate}k \"{tempFile}\"");

        //    conversion.OnProgress += (sender, args) =>
        //    {
        //        progressSongStatus.Invoke((Action)(() =>
        //        {
        //            progressSongStatus.Value = Math.Clamp((int)args.Percent, 0, 100);
        //        }));
        //    };

        //    await conversion.Start();

        //    // --- Step 6: Replace original file safely ---
        //    if (File.Exists(downloadedFile))
        //        File.Delete(downloadedFile);

        //    File.Move(tempFile, downloadedFile);

        //    // --- Step 7: Update UI ---
        //    MessageBox.Show($"Download complete!\nSaved as: {downloadedFile}");
        //    statusText.Text = "Downloaded";

        //    if (markadianSettings.enableQueue)
        //    {
        //        songsDownloaded++;
        //        statusQueue.Text = $"{songsDownloaded} / {songsEnqueued} Songs Downloaded";
        //    }
        //}

        //private string MakeSafeFileName(string name)
        //{
        //    foreach (char c in Path.GetInvalidFileNameChars())
        //        name = name.Replace(c, '_');
        //    return name;
        //}

        //private string SanitizeYoutubeUrl(string url)
        //{
        //    if (string.IsNullOrWhiteSpace(url))
        //        return url;

        //    Uri uri;
        //    if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
        //        return url;

        //    if (uri.Host.Contains("youtu.be"))
        //    {
        //        // Short link: keep only up to "?"
        //        int qIndex = url.IndexOf('?');
        //        return qIndex != -1 ? url.Substring(0, qIndex) : url;
        //    }
        //    else if (uri.Host.Contains("youtube.com"))
        //    {
        //        // Full link: keep only up to "&"
        //        int ampIndex = url.IndexOf('&');
        //        return ampIndex != -1 ? url.Substring(0, ampIndex) : url;
        //    }
        //    MessageBox.Show("now it's" + url);
        //    return url;
        //}


        //private Control CreateYoutubeResultCard(YoutubeResult result)
        //{
        //    var card = new Panel
        //    {
        //        Width = 320,
        //        Height = 160,
        //        BorderStyle = BorderStyle.FixedSingle,
        //        Margin = new Padding(10),
        //        BackColor = Color.FromArgb(45, 45, 48),
        //        Cursor = Cursors.Hand
        //    };

        //    var thumbnail = new PictureBox
        //    {
        //        Width = 150,
        //        Height = 110,
        //        Location = new Point(8, 8),
        //        SizeMode = PictureBoxSizeMode.StretchImage,
        //        BorderStyle = BorderStyle.FixedSingle
        //    };

        //    try
        //    {
        //        using var wc = new WebClient();
        //        byte[] imgBytes = wc.DownloadData(result.Thumbnail);
        //        using var ms = new MemoryStream(imgBytes);
        //        thumbnail.Image = Image.FromStream(ms);
        //    }
        //    catch
        //    {
        //        thumbnail.BackColor = Color.DarkGray;
        //    }

        //    var titleLabel = new Label
        //    {
        //        Text = result.Title ?? "(No Title)",
        //        AutoSize = false,
        //        Width = 150,
        //        Height = 80,
        //        Location = new Point(170, 10),
        //        ForeColor = Color.White,
        //        Font = new Font("Segoe UI", 9, FontStyle.Bold),
        //        BackColor = Color.Transparent,
        //        MaximumSize = new Size(140, 80),
        //        AutoEllipsis = true
        //    };

        //    var durationLabel = new Label
        //    {
        //        Text = result.Duration ?? "Unknown",
        //        AutoSize = false,
        //        Width = 140,
        //        Height = 20,
        //        Location = new Point(170, 110),
        //        ForeColor = Color.LightGray,
        //        Font = new Font("Segoe UI", 8)
        //    };

        //    // Add a reusable tooltip
        //    var tooltip = new ToolTip
        //    {
        //        AutoPopDelay = 3000,
        //        InitialDelay = 500,
        //        ReshowDelay = 200,
        //        BackColor = Color.FromArgb(55, 55, 60),
        //        ForeColor = Color.White
        //    };

        //    tooltip.SetToolTip(card, "Click to download");
        //    tooltip.SetToolTip(thumbnail, "Click to download");
        //    tooltip.SetToolTip(titleLabel, "Click to download");
        //    tooltip.SetToolTip(durationLabel, "Click to download");

        //    // Hover color feedback
        //    card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(60, 60, 65);
        //    card.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(45, 45, 48);

        //    card.Controls.Add(thumbnail);
        //    card.Controls.Add(titleLabel);
        //    card.Controls.Add(durationLabel);

        //    // Entire card clickable
        //    card.Click += async (s, e) => await handleDownloadLogic(result.Url);
        //    thumbnail.Click += async (s, e) => await handleDownloadLogic(result.Url);
        //    titleLabel.Click += async (s, e) => await handleDownloadLogic(result.Url);
        //    durationLabel.Click += async (s, e) => await handleDownloadLogic(result.Url);

        //    return card;
        //}



        //private static readonly YoutubeClient youtubeClient = new YoutubeClient();

        //private async Task<List<YoutubeResult>> SearchYoutubeVideosAsync(string query)
        //{
        //    if (string.IsNullOrWhiteSpace(query))
        //        return new List<YoutubeResult>();

        //    try
        //    {
        //        var results = await youtubeClient.Search.GetVideosAsync(query).CollectAsync(5);

        //        return results.Select(v => new YoutubeResult
        //        {
        //            Title = v.Title,
        //            Duration = v.Duration?.ToString(@"mm\:ss") ?? "Unknown",
        //            Thumbnail = v.Thumbnails?.GetWithHighestResolution()?.Url ?? "",
        //            Url = v.Url
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Search failed: {ex.Message}");
        //        return new List<YoutubeResult>();
        //    }
        //}


        //private async Task<List<YoutubeResult>> SearchYouTubeAsyncYTDLP(string query)
        //{
        //    var results = new List<YoutubeResult>();
        //    string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");
        //    if (!File.Exists(exePath))
        //        throw new FileNotFoundException("yt-dlp executable not found", exePath);

        //    var psi = new ProcessStartInfo
        //    {
        //        FileName = exePath,
        //        Arguments = $"ytsearch5:\"{query}\" --dump-json --no-warnings --no-playlist --skip-download",
        //        RedirectStandardOutput = true,
        //        RedirectStandardError = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = true
        //    };

        //    using (var proc = Process.Start(psi))
        //    {
        //        using (var reader = proc.StandardOutput)
        //        {
        //            while (!reader.EndOfStream && results.Count < 5)
        //            {
        //                var line = await reader.ReadLineAsync();
        //                if (string.IsNullOrWhiteSpace(line))
        //                    continue;

        //                try
        //                {
        //                    var json = System.Text.Json.JsonDocument.Parse(line).RootElement;
        //                    results.Add(new YoutubeResult
        //                    {
        //                        Title = json.GetProperty("title").GetString() ?? "",
        //                        Duration = json.TryGetProperty("duration_string", out var d) ? d.GetString() ?? "" : "",
        //                        Thumbnail = json.TryGetProperty("thumbnail", out var t) ? t.GetString() ?? "" : "",
        //                        Url = json.GetProperty("webpage_url").GetString() ?? ""
        //                    });
        //                }
        //                catch
        //                {
        //                    // skip bad line
        //                }
        //            }
        //        }
        //        await proc.WaitForExitAsync();
        //    }

        //    return results;
        //}
    }











}
