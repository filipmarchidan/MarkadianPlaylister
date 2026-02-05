using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarkadianPlaylister
{
    public static class ResourceUpdater
    {
        private const string YTDLP_API =
            "https://api.github.com/repos/yt-dlp/yt-dlp/releases/latest";

        private const string YTDLP_DOWNLOAD =
            "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";

        private const string FFMPEG_DOWNLOAD =
            "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";

        private static readonly string TempDir =
            Path.Combine(Path.GetTempPath(), "MarkadianUpdater");


        private static bool enableUpdates;
        public static async Task EnsureResourcesAsync()
        {
            var settings = SettingsManager.LoadSettings();
            string resourceDir = settings.resourceDirectory;

            Directory.CreateDirectory(resourceDir);
            Directory.CreateDirectory(TempDir);

            string ytPath = Path.Combine(resourceDir, "yt-dlp.exe");
            string ffmpegPath = Path.Combine(resourceDir, "ffmpeg.exe");
            enableUpdates = settings.enableUpdates;
            await EnsureYtDlpAsync(ytPath);
            await EnsureFfmpegAsync(ffmpegPath);
        }

        // ================= YT-DLP =================

        private static async Task EnsureYtDlpAsync(string ytPath)
        {
            if (!File.Exists(ytPath))
            {
                await DownloadFileAsync(YTDLP_DOWNLOAD, ytPath);
                MessageBox.Show("Dependency yt-dlp not found. It will be downloaded now");
                return;
            }

            string local = await GetLocalYtDlpVersionAsync(ytPath);
            string latest = await GetLatestGitHubTagAsync(YTDLP_API);

            if (string.IsNullOrWhiteSpace(latest))
                return;

            if (Normalize(local) != Normalize(latest) && enableUpdates)
            {
                var result = MessageBox.Show(
                    $"yt-dlp update available.\n\nCurrent: {local}\nLatest: {latest}\n\nUpdate now?",
                    "yt-dlp Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string tempFile = Path.Combine(TempDir, "yt-dlp.exe");
                    await DownloadFileAsync(YTDLP_DOWNLOAD, tempFile);

                    ReplaceFile(tempFile, ytPath);
                }
            }
        }

        private static async Task<string> GetLocalYtDlpVersionAsync(string ytPath)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = ytPath,
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var proc = Process.Start(psi);
                string output = await proc.StandardOutput.ReadToEndAsync();
                await proc.WaitForExitAsync();
                return output.Trim();
            }
            catch
            {
                return "unknown";
            }
        }

        // ================= FFMPEG =================

        private static async Task EnsureFfmpegAsync(string ffmpegPath)
        {
            if (File.Exists(ffmpegPath))
                return;

            MessageBox.Show("Dependency ffmpeg not found. It will be downloaded now");
            string zipPath = Path.Combine(TempDir, "ffmpeg.zip");
            await DownloadFileAsync(FFMPEG_DOWNLOAD, zipPath);

            using var zip = ZipFile.OpenRead(zipPath);

            var entry = zip.Entries
                .FirstOrDefault(e =>
                    e.FullName.EndsWith("ffmpeg.exe",
                        StringComparison.OrdinalIgnoreCase));

            if (entry == null)
                throw new Exception("ffmpeg.exe not found in archive.");

            entry.ExtractToFile(ffmpegPath, true);
        }

        // ================= HELPERS =================

        private static async Task<string> GetLatestGitHubTagAsync(string apiUrl)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "MarkadianPlaylister");

                string json = await client.GetStringAsync(apiUrl);
                using var doc = JsonDocument.Parse(json);

                return doc.RootElement.GetProperty("tag_name").GetString() ?? "";
            }
            catch
            {
                return "";
            }
        }

        private static async Task DownloadFileAsync(string url, string destination)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            await using var fs = new FileStream(destination, FileMode.Create, FileAccess.Write);
            await response.Content.CopyToAsync(fs);
        }

        private static void ReplaceFile(string source, string target)
        {
            if (File.Exists(target))
                File.Delete(target);

            File.Move(source, target);
        }

        private static string Normalize(string version)
        {
            return version.Trim().TrimStart('v', 'V');
        }
    }
}
