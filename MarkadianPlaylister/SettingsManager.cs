using System;
using System.IO;
using System.Text.Json;

namespace MarkadianPlaylister
{
    public static class SettingsManager
    {
        private static readonly string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public static MarkadianSettings LoadSettings()
        {
            // Create default settings object
            var defaultSettings = new MarkadianSettings
            {
                bitRateSelector = "192",
                filePath = AppDomain.CurrentDomain.BaseDirectory,
                enableQueue = true,
                theme = "Light",
                searchCount = "5"
            };

            if (!File.Exists(settingsFilePath))
            {
                // Create default settings file
                SaveSettings(defaultSettings);
                return defaultSettings;
            }

            string json = File.ReadAllText(settingsFilePath);

            // Handle empty file (or whitespace)
            if (string.IsNullOrWhiteSpace(json))
            {
                // overwrite with defaults and return them
                SaveSettings(defaultSettings);
                return defaultSettings;
            }

            try
            {
                var loaded = JsonSerializer.Deserialize<MarkadianSettings>(json);

                if (loaded == null)
                {
                    // Deserialize returned null (invalid/empty), reset to defaults
                    SaveSettings(defaultSettings);
                    return defaultSettings;
                }

                // Fill missing/null string properties with defaults (minimal in-place merge)
                var props = typeof(MarkadianSettings).GetProperties();
                bool changed = false;
                foreach (var prop in props)
                {
                    if (!prop.CanRead || !prop.CanWrite) continue;

                    var currentValue = prop.GetValue(loaded);
                    var defaultValue = prop.GetValue(defaultSettings);

                    if (currentValue == null ||
                        (prop.PropertyType == typeof(string) && string.IsNullOrWhiteSpace((string)currentValue)))
                    {
                        prop.SetValue(loaded, defaultValue);
                        changed = true;
                    }
                }

                if (changed)
                    SaveSettings(loaded);

                return loaded;
            }
            catch
            {
                // If file is corrupted or deserialize throws -> restore defaults
                SaveSettings(defaultSettings);
                return defaultSettings;
            }
        }

        public static void SaveSettings(MarkadianSettings settings)
        {
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsFilePath, json);
        }
    }
}
