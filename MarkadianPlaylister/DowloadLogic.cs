using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MarkadianPlaylister
{
    public class DownloadLogic
    {
       

        public MarkadianSettings markadianSettings;
        
        public static string filePath;
        public bool locked;
        public static int songsDownloaded { get; set; }
        public static int songsEnqueued { get; set; }
        public static string exePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");

        string ffmpeg { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");

        Queue<string> videoLinks = new Queue<string>();

        // ✅ NEW: UI events
        public event Action<int>? ProgressChanged;
        public event Action<string>? StatusChanged;
        public event Action<string>? QueueStatusChanged;
        public event Action<string>? DownloadCompleted;


        public DownloadLogic()
        {
            markadianSettings = SettingsManager.LoadSettings();
            filePath = markadianSettings.filePath;
            songsDownloaded = 0;
            songsEnqueued = 0;
        }

        public async Task handleDownloadLogic(string videoUrl)
        {
            videoUrl = SanitizeYoutubeUrl(videoUrl);
            if (markadianSettings.enableQueue)
            {
                videoLinks.Enqueue(videoUrl);
                songsEnqueued++;

                QueueStatusChanged?.Invoke($"{songsDownloaded} / {songsEnqueued} Songs Downloaded");

                if (videoLinks.Count == 1)
                {
                    locked = false;
                    await startDownloadingWithQueue(videoLinks, filePath);
                }
                return;
            }
            else
                await DownloadWithYtDlp(videoUrl, filePath);
        }

        private async Task startDownloadingWithQueue(Queue<string> videoLinks, string filePath)
        {
            while (videoLinks.Count > 0)
            {
                if (!locked)
                {
                    string currentVideo = videoLinks.Dequeue();
                    await DownloadWithYtDlp(currentVideo, filePath);
                    locked = true;
                }
            }
        }

        private async Task DownloadWithYtDlp(string videoUrl, string folderPath)
        {
            ProgressChanged?.Invoke(0);
            StatusChanged?.Invoke("Preparing download...");

            if (!File.Exists(exePath))
                throw new FileNotFoundException("yt-dlp executable not found", exePath);

            // Validate URL
            bool IsValidYoutubeUrl(string url) =>
                Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
                (uri.Host.Contains("youtube.com") || uri.Host.Contains("youtu.be"));

            if (!IsValidYoutubeUrl(videoUrl))
            {
                StatusChanged?.Invoke("Invalid YouTube URL");
                return;
            }

            // --- Step 1: Get video title ---
            var titlePsi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"--get-title \"{videoUrl}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var titleProc = Process.Start(titlePsi);
            string videoTitle = (await titleProc.StandardOutput.ReadToEndAsync()).Trim();
            await titleProc.WaitForExitAsync();
            if (string.IsNullOrWhiteSpace(videoTitle))
                videoTitle = "UnknownTitle";

            string safeTitle = MakeSafeFileName(videoTitle);
            string outputTemplate = Path.Combine(folderPath, safeTitle + ".%(ext)s");
            string downloadedFile = Path.Combine(folderPath, safeTitle + ".mp3");
            string tempFile = Path.Combine(folderPath, safeTitle + "_temp.mp3");

            // --- Step 2: Download ---
            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"-f bestaudio --no-cache-dir --extract-audio --audio-format mp3 " +
                            $"--user-agent \"Mozilla/5.0\" " +
                            $"--ffmpeg-location \"{Path.GetDirectoryName(ffmpeg)}\" " +
                            $"-o \"{outputTemplate}\" \"{videoUrl}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            StatusChanged?.Invoke("Downloading...");
            using var proc = Process.Start(psi);
            string stderr = await proc.StandardError.ReadToEndAsync();
            await proc.WaitForExitAsync();

            if (proc.ExitCode != 0 || !File.Exists(downloadedFile))
            {
                StatusChanged?.Invoke("Download failed.");
                return;
            }

            // --- Step 3: Re-encode ---
            string bitRate = markadianSettings.bitRateSelector ?? "192";
            var conversion = Xabe.FFmpeg.FFmpeg.Conversions.New()
                .AddParameter($"-i \"{downloadedFile}\" -vn -ar 44100 -b:a {bitRate}k \"{tempFile}\"");

            conversion.OnProgress += (sender, args) =>
            {
                ProgressChanged?.Invoke((int)Math.Clamp(args.Percent, 0, 100));
            };

            StatusChanged?.Invoke("Converting...");
            await conversion.Start();

            if (File.Exists(downloadedFile))
                File.Delete(downloadedFile);
            File.Move(tempFile, downloadedFile);

            songsDownloaded++;
            QueueStatusChanged?.Invoke($"{songsDownloaded} / {songsEnqueued} Songs Downloaded");
            StatusChanged?.Invoke("Downloaded");
            DownloadCompleted?.Invoke(downloadedFile);
        }

        private string MakeSafeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }

        private string SanitizeYoutubeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return url;
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return url;

            if (uri.Host.Contains("youtu.be"))
                return url.Split('?')[0];
            if (uri.Host.Contains("youtube.com"))
                return url.Split('&')[0];

            return url;
        }


        private async Task<string> RunFFprobe(string filePath)
        {
            string ffprobePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffprobe.exe");

            var psi = new ProcessStartInfo
            {
                FileName = ffprobePath,
                Arguments = $"-v error -select_streams a:0 -show_entries stream=bit_rate -of default=noprint_wrappers=1:nokey=1 \"{filePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            string output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            return output.Trim(); // returns bitrate in bits per second
        }

    }
}
