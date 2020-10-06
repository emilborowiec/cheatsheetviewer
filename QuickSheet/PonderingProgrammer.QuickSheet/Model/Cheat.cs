using System.Collections.Generic;

namespace PonderingProgrammer.QuickSheet.Model
{
    public class Cheat
    {
        public Cheat(string caption)
        {
            Caption = caption;
        }

        public string Caption { get; }
        public List<string> Entries { get; } = new List<string>();
    }
}