using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using PonderingProgrammer.QuickSheet.Annotations;
using PonderingProgrammer.QuickSheet.Model;

namespace PonderingProgrammer.QuickSheet.CheatSheetPanel
{
    public class CheatSheetViewModel : INotifyPropertyChanged
    {
        private int GetLineCount()
        {
            return 1 + Sections.Sum(s => s.GetLineCount());
        }

        private static List<SectionContent> CreateViewSections(Model.CheatSheet cheatSheet)
        {
            var viewSections = new List<SectionContent>();
            if (cheatSheet.Cheats.Count > 0)
            {
                viewSections.Add(new SectionContent(cheatSheet.Cheats));
            }

            viewSections.AddRange(cheatSheet.Sections.Select(section => new SectionContent(section.Name, section.Cheats)));
            
            return viewSections;
        }

        private CheatSheet _cheatSheet;

        public CheatSheet CheatSheet
        {
            get => _cheatSheet;
            set
            {
                if (value != _cheatSheet)
                {
                    _cheatSheet = value;
                    Sections = CreateViewSections(_cheatSheet);
                    OnPropertyChanged(nameof(CheatSheet));
                    OnPropertyChanged(nameof(Sections));
                }
            }
        }

        public List<SectionContent> Sections { get; set; }
        public int TitleFontSize => (int) (BaseFontSize * 1.5);
        public int SectionFontSize => (int) (BaseFontSize * 1.2);
        public int CaptionFontSize => (int) (BaseFontSize * 1.1);
        public int EntryFontSize => BaseFontSize;

        private int BaseFontSize
        {
            get
            {
                if (Application.Current.MainWindow == null) return 12;
                var h = (int) ((Panel) Application.Current.MainWindow.Content).ActualHeight;
                return 6 + (h / (GetRequiredLines() * 5));
            }
        }
        
        public void NotifyFontSizeChanged()
        {
            OnPropertyChanged(nameof(TitleFontSize));
            OnPropertyChanged(nameof(SectionFontSize));
            OnPropertyChanged(nameof(CaptionFontSize));
            OnPropertyChanged(nameof(EntryFontSize));
        }

        private int GetRequiredLines()
        {
            return Math.Max(GetHighestSectionLineCount(), GetLineCount() / GetMaxColumns());
        }

        private int GetMaxColumns()
        {
            return 10;
        }

        private int GetHighestSectionLineCount()
        {
            return Sections.Max(s => s.GetLineCount());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}