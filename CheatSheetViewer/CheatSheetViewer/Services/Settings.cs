#region

using System.Collections.Generic;

#endregion

namespace CheatSheetViewerApp.Services
{
    public class Settings
    {
        public Dictionary<string, SheetSettings> SheetSettings { get; set; } = new Dictionary<string, SheetSettings>();

        public SheetSettings GetSettings(string sheetName)
        {
            if (!SheetSettings.ContainsKey(sheetName))
            {
                SheetSettings[sheetName] = new SheetSettings();
            }

            return SheetSettings[sheetName];
        }
    }
}