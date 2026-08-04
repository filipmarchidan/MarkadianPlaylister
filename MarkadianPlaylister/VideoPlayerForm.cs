using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MarkadianPlaylister
{
    public partial class VideoPlayerForm : Form
    {
        private readonly YoutubeResult youtubeResult;
        private readonly MarkadianSettings markadianSettings;
        private readonly StreamLogic streamLogic;
        private LibVLC libVLC;
        private VideoView videoView;
        private bool isUserDraggingSlider = false;
        private bool isPlaying = false;
        private Label hoverTimeLabel;

        public VideoPlayerForm(YoutubeResult result, string ytDlpPath, MarkadianSettings settings)
        {
            InitializeComponent();
            youtubeResult = result;
            streamLogic = new StreamLogic(ytDlpPath, settings);
            this.Text = result.Title ?? "Video Player";
            this.Icon = SystemIcons.Application;
            markadianSettings = settings;
        }

        private async void VideoPlayerForm_Load(object sender, EventArgs e)
        {
            try
            {
                ShowStatus("⏳ Ensuring video resources...", Color.Yellow);

                // Step 1: Ensure all resources are downloaded/extracted
                await ResourceUpdater.EnsureResourcesAsync();

                ShowStatus("⏳ Loading stream...", Color.Yellow);

                // Step 2: Get the VLC directory path
                string vlcPath = Path.Combine(markadianSettings.resourceDirectory, "VLC");

                // Step 3: Verify VLC files exist
                if (!Directory.Exists(vlcPath) || 
                    !File.Exists(Path.Combine(vlcPath, "libvlc.dll")) ||
                    !File.Exists(Path.Combine(vlcPath, "libvlccore.dll")))
                {
                    throw new Exception($"LibVLC files not found in {vlcPath}. Please check that video resources were downloaded correctly.");
                }

                // Step 4: Initialize Core with VLC path
                Core.Initialize(vlcPath);

                // Step 5: Create LibVLC instance (without path parameter)
                libVLC = new LibVLC();

                // Create VideoView for the videoPanel
                videoView = new VideoView
                {
                    Dock = DockStyle.Fill,
                    MediaPlayer = new MediaPlayer(libVLC)
                };

                videoPanel.Controls.Clear();
                videoPanel.Controls.Add(timeBar);
                videoPanel.Controls.Add(videoView);

                timeBar.Dock = DockStyle.Bottom;
                videoView.Dock = DockStyle.Fill;

                videoView.SendToBack();
                timeBar.BringToFront();

                CreateHoverTimeLabel();

                // Initialize button handlers
                pauseButton.Click += PauseButton_Click;
                playButton.Click += PlayButton_Click;
                timeBar.MouseDown += TimeBar_MouseDown;
                timeBar.MouseUp += TimeBar_MouseUp;
                timeBar.MouseMove += TimeBar_MouseMove;

                // Set up media player event handlers
                videoView.MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
                videoView.MediaPlayer.LengthChanged += MediaPlayer_LengthChanged;
                videoView.MediaPlayer.Stopped += MediaPlayer_Stopped;

                // Start loading the stream asynchronously
                _ = LoadStreamAsync();
            }
            catch (Exception ex)
            {
                ShowStatus($"✗ Error: {ex.Message}", Color.Red);
                MessageBox.Show($"Failed to initialize player: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine($"VideoPlayerForm_Load Error: {ex}");
                this.Close();
            }
        }

        private void CreateHoverTimeLabel()
        {
            // Create time label for hover on trackbar
            hoverTimeLabel = new Label
            {
                Name = "hoverTimeLabel",
                AutoSize = false,
                Width = 80,
                Height = 25,
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            videoPanel.Parent.Controls.Add(hoverTimeLabel);
        }


        private int PixelToValue(int x)
        {
            int thumbWidth = 16;        // Approximate thumb width
            int usableWidth = timeBar.Width - thumbWidth;

            x -= thumbWidth / 2;

            x = Math.Max(0, Math.Min(x, usableWidth));

            double percent = (double)x / usableWidth;

            return (int)Math.Round(
                percent * (timeBar.Maximum - timeBar.Minimum)
            );
        }

        private async Task LoadStreamAsync()
        {
            try
            {
                ShowStatus("⏳ Fetching stream...", Color.Yellow);
                Debug.WriteLine($"Getting stream URL for: {youtubeResult.Url}");

                // Get streaming URL
                string streamUrl = await streamLogic.GetStreamUrlAsync(youtubeResult.Url, preferVideo: true);
                Debug.WriteLine($"Stream URL obtained successfully");

                if (string.IsNullOrWhiteSpace(streamUrl))
                {
                    throw new Exception("Stream URL is empty");
                }

                ShowStatus("✓ Stream ready to play", Color.LimeGreen);

                // Create and play media
                var media = new Media(libVLC, streamUrl, FromType.FromLocation);
                media.AddOption(":network-caching=5000");
                media.AddOption(":file-caching=5000");

                if (videoView?.MediaPlayer != null)
                {
                    videoView.MediaPlayer.Play(media);
                    isPlaying = true;
                    pauseButton.Text = "⏸ Pause";
                    playButton.Text = "▶ Play";

                    Debug.WriteLine("Playback started successfully");
                }
            }
            catch (Exception ex)
            {
                ShowStatus($"✗ Error loading stream: {ex.Message}", Color.Red);
                MessageBox.Show($"Failed to load stream: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine($"LoadStreamAsync Error: {ex}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        private void ShowStatus(string message, Color color)
        {
            if (statusText.InvokeRequired)
            {
                statusText.Invoke(() =>
                {
                    statusText.Text = message;
                    statusText.ForeColor = color;
                    statusText.Visible = true;
                });
            }
            else
            {
                statusText.Text = message;
                statusText.ForeColor = color;
                statusText.Visible = true;
            }
        }

        private void MediaPlayer_LengthChanged(object sender, MediaPlayerLengthChangedEventArgs e)
        {
            if (timeBar.InvokeRequired)
            {
                timeBar.Invoke(() =>
                {
                    timeBar.Maximum = (int)(e.Length / 1000);
                });
            }
            else
            {
                timeBar.Maximum = (int)(e.Length / 1000);
            }
        }

        private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (!isUserDraggingSlider)
            {
                if (timeBar.InvokeRequired)
                {
                    timeBar.Invoke(() =>
                    {
                        timeBar.Value = Math.Min((int)(e.Time / 1000), timeBar.Maximum);

                        // Show current time and duration when playing
                        if (isPlaying && videoView?.MediaPlayer != null)
                        {
                            statusText.Text = $"{TimeSpan.FromMilliseconds(e.Time):hh\\:mm\\:ss} / {TimeSpan.FromMilliseconds(videoView.MediaPlayer.Length):hh\\:mm\\:ss}";
                            statusText.Visible = true;
                        }
                    });
                }
                else
                {
                    timeBar.Value = Math.Min((int)(e.Time / 1000), timeBar.Maximum);

                    // Show current time and duration when playing
                    if (isPlaying && videoView?.MediaPlayer != null)
                    {
                        statusText.Text = $"{TimeSpan.FromMilliseconds(e.Time):hh\\:mm\\:ss} / {TimeSpan.FromMilliseconds(videoView.MediaPlayer.Length):hh\\:mm\\:ss}";
                        statusText.Visible = true;
                    }
                }
            }
        }

        private void MediaPlayer_Stopped(object sender, EventArgs e)
        {
            if (pauseButton.InvokeRequired)
            {
                pauseButton.Invoke(() =>
                {
                    isPlaying = false;
                    pauseButton.Text = "⏸ Pause";
                });
            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (videoView?.MediaPlayer != null)
                {
                    if (isPlaying)
                    {
                        videoView.MediaPlayer.Pause();
                        pauseButton.Text = "▶ Resume";
                        isPlaying = false;
                        ShowStatus("⏸ Paused", Color.Yellow);
                    }
                    else
                    {
                        videoView.MediaPlayer.Play();
                        pauseButton.Text = "⏸ Pause";
                        isPlaying = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Pause error: {ex}");
                ShowStatus($"❌ Error: {ex.Message}", Color.Red);
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (videoView?.MediaPlayer != null)
                {
                    videoView.MediaPlayer.Play();
                    pauseButton.Text = "⏸ Pause";
                    isPlaying = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Play error: {ex}");
                ShowStatus($"❌ Error: {ex.Message}", Color.Red);
            }
        }

        private void TimeBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (videoView?.MediaPlayer == null)
                return;

            int value = PixelToValue(e.X);

            timeBar.Value = value;

            videoView.MediaPlayer.Time = value * 1000L;
        }

        private void TimeBar_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (videoView?.MediaPlayer != null && timeBar.Maximum > 0)
                {
                    // Calculate position based on click - TrackBar uses Value directly
                    videoView.MediaPlayer.SeekTo(TimeSpan.FromSeconds(timeBar.Value));
                    ShowStatus("Seeking...", Color.Yellow);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Seek error: {ex}");
                ShowStatus($"❌ Seek error", Color.Red);
            }
            finally
            {
                isUserDraggingSlider = false;
                hoverTimeLabel.Visible = false;
            }
        }

        private void TimeBar_Scroll(object sender, EventArgs e)
        {
            if (videoView?.MediaPlayer == null)
                return;

            videoView.MediaPlayer.Time = timeBar.Value * 1000L;
        }



        private void TimeBar_MouseMove(object sender, MouseEventArgs e)
        {
            int value = PixelToValue(e.X);

            TimeSpan ts = TimeSpan.FromSeconds(value);

            hoverTimeLabel.Text = ts.ToString(@"hh\:mm\:ss");

            Point p = timeBar.PointToScreen(e.Location);
            p = this.PointToClient(p);

            hoverTimeLabel.Left =
                p.X - hoverTimeLabel.Width / 2;

            hoverTimeLabel.Top =
                timeBar.Top - hoverTimeLabel.Height - 5;

            hoverTimeLabel.Visible = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                if (videoView != null)
                {
                    videoView.MediaPlayer?.Stop();
                    videoView.MediaPlayer?.Dispose();
                    videoView.Dispose();
                }

                libVLC?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}