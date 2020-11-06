#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using QuickSheet.Model;

#endregion

namespace QuickSheet.CheatSheetPanel
{
    public class SectionContent
    {
        public bool IsRootSection { get; }
        public bool IsNotRootSection => !IsRootSection;
        public string Title { get; }
        public List<Cheat> Cheats { get; }
        public Color BackgroundColor { get; set; }

        public SectionContent(List<Cheat> cheats)
        {
            Cheats = cheats;
            IsRootSection = true;
        }

        public SectionContent(string title, List<Cheat> cheats)
        {
            Title = title;
            Cheats = cheats;
        }

        private static int GetLineCount(Cheat cheat)
        {
            return 1 + cheat.Entries.Count;
        }

        private static int GetWidth(Cheat cheat)
        {
            return Math.Max(cheat.Caption.Length, cheat.Entries.Max(e => e.Length));
        }

        public int GetLineCount()
        {
            return 1 + Cheats.Sum(GetLineCount);
        }

        public int GetWidthInCharacters()
        {
            return Math.Max(Title?.Length ?? 0, Cheats.Max(GetWidth));
        }
    }
}