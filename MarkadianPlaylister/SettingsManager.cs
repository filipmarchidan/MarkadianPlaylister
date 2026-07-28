using System.IO;
using System.Text.Json;

namespace MarkadianPlaylister
{
    //This class retrieves and saves settings to file
    public static class SettingsManager
    {
        private static readonly string BaseDir =
            AppDomain.CurrentDomain.BaseDirectory;

        private static readonly string DefaultResourceDir =
            Path.Combine(BaseDir, "Resources");

        // Move settings.json into {APP_FOLDER}\Properties\settings.json
        private static readonly string settingsFilePath =
            Path.Combine(BaseDir, "Preferences", "settings.json");

        //if the file is not found then default settings will be loaded.
        public static MarkadianSettings LoadSettings()
        {
            // Ensure settings folder exists
            var settingsDir = Path.GetDirectoryName(settingsFilePath);
            if (!string.IsNullOrEmpty(settingsDir) && !Directory.Exists(settingsDir))
                Directory.CreateDirectory(settingsDir);

            // Ensure default resource folder exists
            if (!Directory.Exists(DefaultResourceDir))
                Directory.CreateDirectory(DefaultResourceDir);

            var defaultSettings = new MarkadianSettings
            {
                bitRateSelector = "192",
                filePath = BaseDir,
                enableQueue = true,
                theme = "Light",
                searchCount = "5",
                enableUpdates = true,
                resourceDirectory = DefaultResourceDir,
                enableDragDrop = true,
                fileType = ".mp3",
                videoQuality = "480p"
            };

            if (!File.Exists(settingsFilePath))
            {
                SaveSettings(defaultSettings);
                return defaultSettings;
            }

            string json = File.ReadAllText(settingsFilePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                SaveSettings(defaultSettings);
                return defaultSettings;
            }

            try
            {
                var loaded = JsonSerializer.Deserialize<MarkadianSettings>(json);

                if (loaded == null)
                {
                    SaveSettings(defaultSettings);
                    return defaultSettings;
                }

                var props = typeof(MarkadianSettings).GetProperties();
                bool changed = false;

                foreach (var prop in props)
                {
                    if (!prop.CanRead || !prop.CanWrite)
                        continue;

                    var currentValue = prop.GetValue(loaded);
                    var defaultValue = prop.GetValue(defaultSettings);

                    if (currentValue == null ||
                        (prop.PropertyType == typeof(string) &&
                         string.IsNullOrWhiteSpace((string)currentValue)))
                    {
                        prop.SetValue(loaded, defaultValue);
                        changed = true;
                    }
                }

                // Ensure resource directory exists and correct if invalid
                if (string.IsNullOrWhiteSpace(loaded.resourceDirectory) ||
                    !Directory.Exists(loaded.resourceDirectory))
                {
                    loaded.resourceDirectory = DefaultResourceDir;
                    changed = true;
                }

                if (changed)
                    SaveSettings(loaded);

                return loaded;
            }
            catch
            {
                SaveSettings(defaultSettings);
                return defaultSettings;
            }
        }

        //write the settings to file.
        public static void SaveSettings(MarkadianSettings settings)
        {
            // Ensure settings folder exists before saving
            var settingsDir = Path.GetDirectoryName(settingsFilePath);
            if (!string.IsNullOrEmpty(settingsDir) && !Directory.Exists(settingsDir))
                Directory.CreateDirectory(settingsDir);

            // Ensure resource folder exists before saving
            if (!Directory.Exists(settings.resourceDirectory))
                Directory.CreateDirectory(settings.resourceDirectory);

            string json = JsonSerializer.Serialize(
                settings,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(settingsFilePath, json);
        }
    }
}