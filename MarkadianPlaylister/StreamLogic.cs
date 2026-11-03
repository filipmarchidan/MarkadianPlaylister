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


        public record StreamUrls(string VideoUrl, string? AudioUrl);

        public async Task<StreamUrls> GetVideoStreamsAsync(
            string youtubeUrl)
        {
            if (!File.Exists(exePath))
                throw new FileNotFoundException(
                    "yt-dlp executable not found", exePath);

            int? height = markadianSettings.videoQuality?
                .ToLowerInvariant() switch
            {
                "1080" or "1080p" => 1080,
                "720" or "720p" => 720,
                "480" or "480p" => 480,
                "360" or "360p" => 360,
                "240" or "240p" => 240,
                "144" or "144p" => 144,
                _ => null
            };

            string video = height.HasValue
                ? $"bv[ext=mp4][vcodec^=avc1][height<=?{height.Value}]"
                : "bv[ext=mp4][vcodec^=avc1]";

            // Prefer H.264 video + AAC audio.
            // Fall back to a combined stream if available.
            string format =
                $"{video}+ba[ext=m4a]/" +
                $"{video}+ba/" +
                "b[ext=mp4]/b";

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            psi.ArgumentList.Add("-f");
            psi.ArgumentList.Add(format);
            psi.ArgumentList.Add("--no-playlist");
            psi.ArgumentList.Add("--no-warnings");
            psi.ArgumentList.Add("-g");
            psi.ArgumentList.Add(youtubeUrl);

            using var process = Process.Start(psi)
                ?? throw new Exception("Could not start yt-dlp.");

            var outputTask =
                process.StandardOutput.ReadToEndAsync();
            var errorTask =
                process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            string output = await outputTask;
            string errors = await errorTask;

            if (process.ExitCode != 0)
                throw new Exception(
                    $"Failed to retrieve streams: {errors}");

            string[] urls = output.Split(
                new[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);

            if (urls.Length is < 1 or > 2)
                throw new Exception(
                    "yt-dlp returned an unexpected number of URLs.");

            foreach (string url in urls)
            {
                if (!Uri.TryCreate(
                        url, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttps &&
                     uri.Scheme != Uri.UriSchemeHttp))
                {
                    throw new Exception("Invalid media URL returned.");
                }
            }

            return new StreamUrls(
                urls[0],
                urls.Length == 2 ? urls[1] : null);
        }


    }
}