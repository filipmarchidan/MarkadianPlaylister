using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;

namespace MarkadianPlaylister
{

    //object that handles the searches on youtube.
    
    public class SearchLogic {
        public readonly MarkadianSettings markadianSettings;
        public DownloadLogic downloadLogic;
        public SearchLogic(MarkadianSettings settings) {

            markadianSettings = settings;
            downloadLogic = new DownloadLogic(settings);
        }
       

        
        /// <summary>
        /// Create a new card to display in the UI
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public Control CreateYoutubeResultCard(YoutubeResult result)
        {
            var card = new Panel
            {
                Width = 320,
                Height = 160,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                BackColor = Color.FromArgb(45, 45, 48),
                Cursor = Cursors.Hand
            };

            var thumbnail = new PictureBox
            {
                Width = 150,
                Height = 110,
                Location = new Point(8, 8),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.DarkGray
            };

            var titleLabel = new Label
            {
                Text = result.Title ?? "(No Title)",
                AutoSize = false,
                Width = 150,
                Height = 80,
                Location = new Point(170, 10),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.Transparent,
                MaximumSize = new Size(140, 80),
                AutoEllipsis = true
            };

            var durationLabel = new Label
            {
                Text = result.Duration ?? "Unknown",
                AutoSize = false,
                Width = 140,
                Height = 20,
                Location = new Point(170, 110),
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 8)
            };

            // ✅ Context menu for right-click
            var contextMenu = new ContextMenuStrip();
            var previewMenuItem = new ToolStripMenuItem("👁 Preview", null, (s, e) => ShowPreview(result));
            var downloadMenuItem = new ToolStripMenuItem("⬇ Download", null, async (s, e) => await DownloadAsync(result));
            contextMenu.Items.Add(previewMenuItem);
            contextMenu.Items.Add(downloadMenuItem);

            card.ContextMenuStrip = contextMenu;
            thumbnail.ContextMenuStrip = contextMenu;
            titleLabel.ContextMenuStrip = contextMenu;
            durationLabel.ContextMenuStrip = contextMenu;

            // Tooltip
            var tooltip = new ToolTip
            {
                AutoPopDelay = 3000,
                InitialDelay = 500,
                ReshowDelay = 200,
                BackColor = Color.FromArgb(55, 55, 60),
                ForeColor = Color.White
            };

            tooltip.SetToolTip(card, "Left-click to download | Right-click for options");
            tooltip.SetToolTip(thumbnail, "Left-click to download | Right-click for options");
            tooltip.SetToolTip(titleLabel, "Left-click to download | Right-click for options");
            tooltip.SetToolTip(durationLabel, "Left-click to download | Right-click for options");

            // Hover feedback
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(60, 60, 65);
            card.MouseLeave += (s, e) => card.BackColor = Color.FromArgb(45, 45, 48);

            // Add controls
            card.Controls.Add(thumbnail);
            card.Controls.Add(titleLabel);
            card.Controls.Add(durationLabel);

            // --- ✅ Fixed Async Image Loading ---
            _ = Task.Run(async () =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(result.Thumbnail))
                        return;

                    using var handler = new HttpClientHandler
                    {
                        AllowAutoRedirect = true,
                        AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
                    };

                    using var http = new HttpClient(handler);
                    http.Timeout = TimeSpan.FromSeconds(10);

                    var imgBytes = await http.GetByteArrayAsync(result.Thumbnail);

                    using var ms = new MemoryStream(imgBytes);
                    var img = Image.FromStream(ms);

                    if (thumbnail.InvokeRequired)
                        thumbnail.Invoke(() => thumbnail.Image = img);
                    else
                        thumbnail.Image = img;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Thumbnail Error] {ex.Message}");
                }
            });

            // Left-click for download
            async Task HandleDownloadAsync()
            {
                try
                {
                    await downloadLogic.handleDownloadLogic(result.Url);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Download error: {ex.Message}");
                }
            }

            card.Click += async (s, e) => await HandleDownloadAsync();
            thumbnail.Click += async (s, e) => await HandleDownloadAsync();
            titleLabel.Click += async (s, e) => await HandleDownloadAsync();
            durationLabel.Click += async (s, e) => await HandleDownloadAsync();

            return card;
        }

        // ✅ NEW: Show preview in player form
        private void ShowPreview(YoutubeResult result)
        {
            if (markadianSettings.enableVideoPlayback == false) { 
                MessageBox.Show("Video playback is disabled in settings. Go to settings and enable it.", "Info");
                return;
            }
            try
            {
                var ytDlpPath = Path.Combine(markadianSettings.resourceDirectory, "yt-dlp.exe");
                if (!File.Exists(ytDlpPath))
                {
                    MessageBox.Show("yt-dlp not found. Cannot preview.", "Error");
                    return;
                }

                var playerForm = new VideoPlayerForm(result, ytDlpPath, markadianSettings);
                playerForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open preview: {ex.Message}", "Error");
            }
        }

        // ✅ NEW: Download helper method
        private async Task DownloadAsync(YoutubeResult result)
        {
            try
            {
                await downloadLogic.handleDownloadLogic(result.Url);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Download failed: {ex.Message}", "Error");
            }
        }

        //actual search on youtube
        private static readonly YoutubeClient youtubeClient = new YoutubeClient();

        public async Task<List<YoutubeResult>> SearchYoutubeVideosAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<YoutubeResult>();

            try
            {
                var results = await youtubeClient.Search.GetVideosAsync(query).CollectAsync(int.Parse(markadianSettings.searchCount));

                return results.Select(v => new YoutubeResult
                {
                    Title = v.Title,
                    Duration = v.Duration?.ToString(@"mm\:ss") ?? "Unknown",
                    Thumbnail = v.Thumbnails?.GetWithHighestResolution()?.Url ?? "",
                    Url = v.Url
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search failed: {ex.Message}");
                return new List<YoutubeResult>();
            }
        }

        //deprecated method. Will be fully removed in future releases
        private async Task<List<YoutubeResult>> SearchYouTubeAsyncYTDLP(string query)
        {
            var results = new List<YoutubeResult>();
            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");
            if (!File.Exists(exePath))
                throw new FileNotFoundException("yt-dlp executable not found", exePath);

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"ytsearch5:\"{query}\" --dump-json --no-warnings --no-playlist --skip-download",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var proc = Process.Start(psi))
            {
                using (var reader = proc.StandardOutput)
                {
                    while (!reader.EndOfStream && results.Count < 5)
                    {
                        var line = await reader.ReadLineAsync();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        try
                        {
                            var json = System.Text.Json.JsonDocument.Parse(line).RootElement;
                            results.Add(new YoutubeResult
                            {
                                Title = json.GetProperty("title").GetString() ?? "",
                                Duration = json.TryGetProperty("duration_string", out var d) ? d.GetString() ?? "" : "",
                                Thumbnail = json.TryGetProperty("thumbnail", out var t) ? t.GetString() ?? "" : "",
                                Url = json.GetProperty("webpage_url").GetString() ?? ""
                            });
                        }
                        catch
                        {
                            // skip bad line
                        }
                    }
                }
                await proc.WaitForExitAsync();
            }

            return results;
        }
    }
}
