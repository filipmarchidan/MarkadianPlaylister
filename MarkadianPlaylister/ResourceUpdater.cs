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


        private static readonly string TempDir =
            Path.Combine(Path.GetTempPath(), "MarkadianUpdater");

        private static readonly string RealAppDir =
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rbin");

        private static readonly string YtDlpPath =
            Path.Combine(RealAppDir, "yt-dlp.exe");

        private static readonly string FfmpegPath =
            Path.Combine(RealAppDir, "ffmpeg.exe");


        private const string APP_REPO = "filipmarchidan/MarkadianPlaylister";

        public static async Task CheckForUpdatesAsync()
        {
            try
            {
                Directory.CreateDirectory(TempDir);

                await CheckAppUpdateAsync();
                await CheckYtDlpAsync();
                await CheckFfmpegAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[Updater] Fatal error: " + ex);
            }

            

        }

        // ================= APP UPDATE =================

        private static async Task CheckAppUpdateAsync()
        {
            string latest = await GetLatestGitHubTagAsync(
                $"https://api.github.com/repos/{APP_REPO}/releases/latest");

            if (string.IsNullOrWhiteSpace(latest))
                return;

            string local = Application.ProductVersion;

            if (!VersionsEqual(local, latest))
            {
                var res = MessageBox.Show(
                    $"A new version is available.\n\nCurrent: {local}\nLatest: {latest}\n\nOpen download page?",
                    "Update Available - Markadian Playlister",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = $"https://github.com/{APP_REPO}/releases/latest",
                        UseShellExecute = true
                    });
                }
            }
        }

        // ================= YT-DLP UPDATE =================

        private static async Task CheckYtDlpAsync()
        {
            string latest = await GetLatestGitHubTagAsync(
                "https://api.github.com/repos/yt-dlp/yt-dlp/releases/latest");

            if (string.IsNullOrWhiteSpace(latest))
                return;

            string local = await GetLocalYtDlpVersionAsync();

            if (VersionsEqual(local, latest))
                return;

            var res = MessageBox.Show(
                $"yt-dlp update available.\n\nCurrent: {local}\nLatest: {latest}\n\nUpdate now?",
                "Update Available - yt-dlp",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (res != DialogResult.Yes)
                return;

            string tempFile = Path.Combine(TempDir, "yt-dlp.exe");

            await DownloadFileAsync(
                "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe",
                tempFile);

            ReplaceFile(tempFile, YtDlpPath);

            string newVersion = await GetLocalYtDlpVersionAsync();

            if (VersionsEqual(newVersion, latest))
            {
                MessageBox.Show("yt-dlp updated successfully.",
                    "Update Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("yt-dlp update failed.",
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            Debug.WriteLine("New version after update: " + newVersion);
        }

        // ================= FFMPEG UPDATE =================

        private static async Task CheckFfmpegAsync()
        {
            var res = MessageBox.Show(
                "Check for FFmpeg update?",
                "FFmpeg Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (res != DialogResult.Yes)
                return;

            string zipPath = Path.Combine(TempDir, "ffmpeg.zip");

            await DownloadFileAsync(
                "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip",
                zipPath);

            using var zip = ZipFile.OpenRead(zipPath);

            var entry = zip.Entries
                .FirstOrDefault(e =>
                    e.FullName.EndsWith("ffmpeg.exe", StringComparison.OrdinalIgnoreCase));

            if (entry == null)
            {
                MessageBox.Show("ffmpeg.exe not found in archive.",
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string tempExe = Path.Combine(TempDir, "ffmpeg.exe");
            entry.ExtractToFile(tempExe, true);

            ReplaceFile(tempExe, FfmpegPath);

            MessageBox.Show("FFmpeg updated successfully.",
                "Update Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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

                return NormalizeVersion(
                    doc.RootElement.GetProperty("tag_name").GetString());
            }
            catch
            {
                return "";
            }
        }

        private static async Task<string> GetLocalYtDlpVersionAsync()
        {
            try
            {
                if (!File.Exists(YtDlpPath))
                    return "none";

                var psi = new ProcessStartInfo
                {
                    FileName = YtDlpPath,
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var proc = Process.Start(psi);
                string output = await proc.StandardOutput.ReadToEndAsync();
                await proc.WaitForExitAsync();

                return NormalizeVersion(output.Trim());
            }
            catch
            {
                return "unknown";
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
            try
            {
                if (File.Exists(target))
                    File.Delete(target);

                File.Move(source, target);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to replace file:\n{ex.Message}",
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static string NormalizeVersion(string? version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return "";

            return version.Trim().TrimStart('v', 'V');
        }

        private static bool VersionsEqual(string v1, string v2)
        {
            return NormalizeVersion(v1) == NormalizeVersion(v2);
        }
    }
}
