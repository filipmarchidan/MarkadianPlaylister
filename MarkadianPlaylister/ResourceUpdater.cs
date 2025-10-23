using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

namespace MarkadianPlaylister
{
    public static class ResourceUpdater
    {
        private static readonly string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string TempDir = Path.Combine(Path.GetTempPath(), "MarkadianUpdater");

        private static readonly string YtDlpPath = Path.Combine(BaseDir, "yt-dlp.exe");
        private static readonly string FfmpegPath = Path.Combine(BaseDir, "ffmpeg.exe");

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
                Debug.WriteLine($"[Updater] Error: {ex}");
            }
        }

        // ---------- APP UPDATE ----------
        private static async Task CheckAppUpdateAsync()
        {
            try
            {
                string apiUrl = $"https://api.github.com/repos/{APP_REPO}/releases/latest";
                string latestVersion = await GetLatestGitHubTagAsync(apiUrl);
                if (string.IsNullOrEmpty(latestVersion)) return;

                string localVersion = Application.ProductVersion;

                if (localVersion != latestVersion)
                {
                    var res = MessageBox.Show(
                        $"A newer version of Markadian Playlister is available.\n\n" +
                        $"Current: {localVersion}\nLatest: {latestVersion}\n\nDo you want to open the download page?",
                        "Update Available - Markadian Playlister",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );

                    if (res == DialogResult.Yes)
                    {
                        string releasePage = $"https://github.com/{APP_REPO}/releases/latest";
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = releasePage,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Updater] App update check failed: {ex.Message}");
            }
        }

        // ---------- YT-DLP UPDATE ----------
        private static async Task CheckYtDlpAsync()
        {
            try
            {
                string apiUrl = "https://api.github.com/repos/yt-dlp/yt-dlp/releases/latest";
                string latestVersion = await GetLatestGitHubTagAsync(apiUrl);
                string localVersion = await GetLocalYtDlpVersionAsync();

                if (string.IsNullOrEmpty(latestVersion)) return;
                if (localVersion == latestVersion) return;

                var result = MessageBox.Show(
                    $"A newer version of yt-dlp is available.\n\n" +
                    $"Current: {localVersion}\nLatest: {latestVersion}\n\nDo you want to update now?",
                    "Update Available - yt-dlp",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    await DownloadAndReplaceAsync(
                        "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe",
                        YtDlpPath,
                        "yt-dlp"
                    );
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Updater] yt-dlp check failed: {ex.Message}");
            }
        }

        // ---------- FFMPEG UPDATE ----------
        private static async Task CheckFfmpegAsync()
        {
            try
            {
                string latestUrl = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";
                string tempZip = Path.Combine(TempDir, "ffmpeg.zip");

                var result = MessageBox.Show(
                    "Do you want to check for an FFmpeg update?",
                    "Update Available - FFmpeg",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes) return;

                await DownloadFileAsync(latestUrl, tempZip);

                using var zip = ZipFile.OpenRead(tempZip);
                var ffmpegEntry = zip.Entries.FirstOrDefault(e =>
                    e.FullName.EndsWith("ffmpeg.exe", StringComparison.OrdinalIgnoreCase));

                if (ffmpegEntry != null)
                {
                    ffmpegEntry.ExtractToFile(FfmpegPath, true);
                    MessageBox.Show("FFmpeg has been updated successfully.",
                        "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Could not find ffmpeg.exe in the downloaded archive.",
                        "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Updater] FFmpeg check failed: {ex.Message}");
            }
        }

        // ---------- SUPPORT METHODS ----------
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

        private static async Task<string> GetLocalYtDlpVersionAsync()
        {
            try
            {
                if (!File.Exists(YtDlpPath)) return "none";

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
                return output.Trim();
            }
            catch
            {
                return "unknown";
            }
        }

        private static async Task DownloadAndReplaceAsync(string url, string targetPath, string label)
        {
            try
            {
                string tempFile = Path.Combine(TempDir, Path.GetFileName(targetPath));
                await DownloadFileAsync(url, tempFile);
                File.Copy(tempFile, targetPath, true);

                MessageBox.Show($"{label} has been updated successfully.",
                    "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update {label}: {ex.Message}",
                    "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static async Task DownloadFileAsync(string url, string destination)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            await using var fs = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fs);
        }
    }
}
