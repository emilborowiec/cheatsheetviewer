#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using QuickSheet.Annotations;
using QuickSheet.Model;

#endregion

namespace QuickSheet.CheatSheetViewComponent
{
    public class SectionContent : INotifyPropertyChanged
    {
        private Brush _backgroundBrush;
        
        public bool IsRootSection { get; }
        public bool IsNotRootSection => !IsRootSection;
        public string Title { get; }
        public List<Cheat> Cheats { get; }
        public Brush BackgroundBrush
        {
            get => _backgroundBrush;
            set
            {
                _backgroundBrush = value;
                OnPropertyChanged(nameof(BackgroundBrush));
            } 
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}