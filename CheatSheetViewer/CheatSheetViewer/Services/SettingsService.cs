#region

using System;
using System.IO;
using System.Text;
using System.Text.Json;

#endregion

namespace CheatSheetViewerApp.Services
{
    public static class SettingsService
    {
        private static readonly string CheatSheetSettingsFolderName = "CheatSheetViewer";
        private static readonly string CheatSheetSettingsFileName = "settings.json";

        public static Settings LoadSettings()
        {
            if (!File.Exists(GetSettingsFilePath()))
            {
                return new Settings();
            }

            using var sr = File.OpenText(GetSettingsFilePath());
            var settingsJson = sr.ReadToEnd();
            return JsonSerializer.Deserialize<Settings>(settingsJson);
        }

        private static string GetSettingsFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                   Path.DirectorySeparatorChar + CheatSheetSettingsFolderName;
        }

        private static string GetSettingsFilePath()
        {
            return GetSettingsFolderPath() + Path.DirectorySeparatorChar + CheatSheetSettingsFileName;
        }

        public static void SaveSettings(Settings settings)
        {
            if (!Directory.Exists(GetSettingsFolderPath()))
            {
                Directory.CreateDirectory(GetSettingsFolderPath());
            }

            using var fileStream = File.Create(GetSettingsFilePath());
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);

            var settingsJson = JsonSerializer.Serialize(settings);

            streamWriter.Write(settingsJson);
            streamWriter.Flush();
        }
    }
}