using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarkadianPlaylister
{
    public static class ResourceManager
    {
        private static string ExtractResource(string resourceName, string fileName)
        {
            string outPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(outPath))
            {
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                if (stream == null)
                    throw new Exception($"Resource '{resourceName}' not found!");

                using FileStream fs = new FileStream(outPath, FileMode.Create, FileAccess.Write);
                stream.CopyTo(fs);
            }
            return outPath;
        }

        public static string GetYtDlp() =>
            ExtractResource("MarkadianPlaylister.Resources.yt_dlp.exe", "yt-dlp.exe");

        public static string GetFfmpeg() =>
            ExtractResource("MarkadianPlaylister.Resources.ffmpeg.exe", "ffmpeg.exe");

        public static string GetFfprobe() =>
            ExtractResource("MarkadianPlaylister.Resources.ffprobe.exe", "ffprobe.exe");
    }

}
