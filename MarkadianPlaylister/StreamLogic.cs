using System.Diagnostics;

namespace MarkadianPlaylister
{
    /// <summary>
    /// Handles getting streaming URLs from YouTube without downloading
    /// </summary>
    public class StreamLogic
    {
        private readonly string exePath;
        private readonly MarkadianSettings markadianSettings;

        public StreamLogic(string ytDlpPath, MarkadianSettings settings)
        {
            markadianSettings = settings;
            exePath = ytDlpPath;
        }

        /// <summary>
        /// Gets a direct streaming URL for audio/video without downloading to disk
        /// </summary>
        public async Task<string> GetStreamUrlAsync(string youtubeUrl, bool preferVideo = true)
        {
            if (!File.Exists(exePath))
                throw new FileNotFoundException("yt-dlp executable not found", exePath);

            // For LibVLC streaming, we need to use yt-dlp's --print-json option
            // and let it handle the format selection internally
            string qualityFormat = markadianSettings.videoQuality?.ToLowerInvariant() switch
            {
                "best" => "18", // 720p MP4 with audio (best balance)
                "1080" or "1080p" => "22", // 1080p (if available)
                "720" or "720p" => "18", // 720p MP4
                "480" or "480p" => "135", // 480p video + 251 audio
                "360" or "360p" => "18", // 360p
                "240" or "240p" => "17", // 240p
                "144" or "144p" => "17", // 144p
                _ => "18" // Default to 720p with audio
            };

            // Use direct URL retrieval - simpler and more reliable for streaming
            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                // Use -f with best format that includes audio automatically
                Arguments = $"-f \"best\" --no-warnings -g \"{youtubeUrl}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8
            };

            using var process = Process.Start(psi);
            if (process == null)
                throw new Exception("Failed to start yt-dlp process");

            string output = await process.StandardOutput.ReadToEndAsync();
            string errorOutput = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                Debug.WriteLine($"yt-dlp error: {errorOutput}");
                throw new Exception($"Failed to get stream URL: {errorOutput}");
            }

            // yt-dlp returns the URL directly when using -g flag
            string streamUrl = output.Trim();
            
            if (string.IsNullOrWhiteSpace(streamUrl))
                throw new Exception("No stream URL returned from yt-dlp");

            Debug.WriteLine($"Stream URL obtained: {streamUrl.Substring(0, Math.Min(100, streamUrl.Length))}...");
            return streamUrl;
        }
    }
}