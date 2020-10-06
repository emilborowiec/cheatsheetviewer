using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PonderingProgrammer.QuickSheet.Model;

namespace PonderingProgrammer.QuickSheet.CheatSheetPanel
{
    public class SectionContent
    {
        private static int GetLineCount(Cheat cheat)
        {
            return 1 + cheat.Entries.Count;
        }

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

        public bool IsRootSection { get; }
        public bool IsNotRootSection => !IsRootSection;
        public Color BackgroundColor { get; set; }
        public string Title { get; }
        public List<Cheat> Cheats { get; }
        
        public int GetLineCount()
        {
            return 1 + Cheats.Sum(GetLineCount);
        }

    }
}