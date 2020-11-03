using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using QuickSheet.Model;

namespace QuickSheet.Services
{
    public static class CheatSheetLoader
    {
        private const string QuickSheetsFolderName = "My QuickSheets";

        public static List<Result<CheatSheet>> LoadCheatSheets()
        {
            var list = new List<Result<CheatSheet>>();

                var quickSheetsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                      Path.DirectorySeparatorChar + QuickSheetsFolderName;
                IEnumerable<string> qsheetFiles = new List<string>();
                if (Directory.Exists(quickSheetsPath))
                {
                    qsheetFiles = Directory.EnumerateFiles(quickSheetsPath, "*.qsheet", SearchOption.TopDirectoryOnly);
                }

                foreach (var file in qsheetFiles)
                {
                    try
                    {
                        var sheet = LoadSheet(file);
                        if (sheet != null)
                        {
                            list.Add(Result<CheatSheet>.Success(sheet));
                        }
                        else
                        {
                            list.Add(Result<CheatSheet>.Failure("Failed to parse QuickSheet file", file));
                        }
                    }
                    catch (FileFormatException e)
                    {
                        list.Add(Result<CheatSheet>.Failure(e.Message, Path.GetFileName(file)));
                    }
                }
                
            return list;
        }

        private static CheatSheet LoadSheet(string path)
        {
            var lines = File.ReadLines(path, Encoding.UTF8);

            return ParseCheatSheet(lines);
        }

        private static CheatSheet ParseCheatSheet(IEnumerable<string> lines)
        {
            using var enumerator = lines.GetEnumerator();
            var title = ParseTitle(enumerator);
            var sheet = new CheatSheet(title);
            Section currentSection = null;
            Cheat currentCheat = null;
            while (enumerator.MoveNext())
            {
                var line = enumerator.Current ?? throw new FileFormatException("Line null");
                if (string.IsNullOrWhiteSpace(line)) continue;
                     
                if (line.StartsWith("## "))
                {
                    // start section
                    var sectionName = line.Substring(3);
                    currentSection = new Section(sectionName);
                    sheet.Sections.Add(currentSection);
                } else if (line.StartsWith("### "))
                {
                    // start cheat
                    var cheatCaption = line.Substring(4);
                    currentCheat = new Cheat(cheatCaption);
                    if (currentSection != null)
                    {
                        currentSection.Cheats.Add(currentCheat);
                    }
                    else
                    {
                        sheet.Cheats.Add(currentCheat);
                    }
                }
                else
                {
                    // add entry to current cheat
                    if (currentCheat == null) throw new FileFormatException("No Cheat was found before entry item");
                    currentCheat.Entries.Add(line);
                }
            }

            return sheet;
        }

        private static string ParseTitle(IEnumerator<string> enumerator)
        {
            if (!enumerator.MoveNext()) throw new FileFormatException("File is empty");
            var line = enumerator.Current ?? throw new FileFormatException("Line null");
            if (!line.StartsWith("# ")) throw new FileFormatException("File does not start with properly formatted title");
            return line.Substring(2);
        }
    }
}