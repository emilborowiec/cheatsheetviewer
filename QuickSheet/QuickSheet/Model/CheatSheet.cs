using System.Collections.Generic;

namespace QuickSheet.Model
{
    public class CheatSheet
    {
        public CheatSheet(string title)
        {
            Title = title;
        }

        public string Title { get; }
        public List<Section> Sections { get; } = new List<Section>();
        public List<Cheat> Cheats { get; } = new List<Cheat>();
    }
}