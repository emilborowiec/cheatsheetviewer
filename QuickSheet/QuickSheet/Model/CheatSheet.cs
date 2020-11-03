#region

using System.Collections.Generic;

#endregion

namespace QuickSheet.Model
{
    public class CheatSheet
    {
        public string Title { get; }
        public List<Section> Sections { get; } = new List<Section>();
        public List<Cheat> Cheats { get; } = new List<Cheat>();

        public CheatSheet(string title)
        {
            Title = title;
        }
    }
}