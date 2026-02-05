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
        private static readonly string BinDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rbin");

        public static string Extract(string fileName, byte[] resource)
        {
            Directory.CreateDirectory(BinDir);
            string targetPath = Path.Combine(BinDir, fileName);

            if (!File.Exists(targetPath))
                File.WriteAllBytes(targetPath, resource);

            return targetPath;
        }

        public static void EnsureAllExtracted()
        {
            Extract("yt-dlp.exe", ResourceDll.GetYtDlp());
            Extract("ffmpeg.exe", ResourceDll.GetFfmpeg());
            Extract("ffprobe.exe", ResourceDll.GetFfprobe());
        }
    }

    public static class ResourceDll
    {
        public static byte[] GetYtDlp() => Properties.Resources.yt_dlp;
        public static byte[] GetFfmpeg() => Properties.Resources.ffmpeg;
        public static byte[] GetFfprobe() => Properties.Resources.ffprobe;
    }

}
