﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PonderingProgrammer.QuickSheet.Annotations;
using PonderingProgrammer.QuickSheet.Model;

namespace PonderingProgrammer.QuickSheet.CheatSheetPanel
{
    public class CheatSheetViewModel : INotifyPropertyChanged
    {
        private static readonly int DefaultFontSize = 12;
        private static readonly int MaxFontSize = 48;
        
        private int GetLineCount()
        {
            return 1 + Sections.Sum(s => s.GetLineCount());
        }

        private static List<SectionContent> CreateViewSections(CheatSheet cheatSheet)
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
                    NotifyFontSizeChanged();
                }
            }
        }

        public List<SectionContent> Sections { get; set; }
        public int TitleFontSize => (int) (CalculateBaseFontSize() * 1.5);
        public int SectionFontSize => (int) (CalculateBaseFontSize() * 1.2);
        public int CaptionFontSize => (int) (CalculateBaseFontSize() * 1.1);
        public int EntryFontSize => CalculateBaseFontSize();

        private int CalculateBaseFontSize()
        {
            if (Application.Current.MainWindow == null) return DefaultFontSize;
            var h = (int) ((Panel) Application.Current.MainWindow.Content).ActualHeight;
            var w = (int) ((Panel) Application.Current.MainWindow.Content).ActualWidth;
            var referenceString = new string('o', GetMaxWidth());
            double size = 4;
            var increment = 2;
            
            while (size + increment < MaxFontSize && WillItFit(referenceString, new Typeface("Arial"), size + increment, w, h))
            {
                size += increment;
            }

            return (int) size;
        }

        public bool WillItFit(string referenceString, Typeface typeface, double fontSize, int windowWidth, int windowHeight)
        {
            var formattedText = new FormattedText(referenceString, 
                                                  CultureInfo.CurrentCulture, 
                                                  FlowDirection.LeftToRight, 
                                                  typeface, 
                                                  fontSize, 
                                                  Brushes.Black, 
                                                  VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip);
            if (windowWidth < formattedText.Width) return false;
            if (windowHeight < formattedText.Height) return false;

            var columns = windowWidth / (formattedText.Width + 20);
            var rows = windowHeight / (formattedText.Height * 1.8);

            return columns * rows >= GetLineCount();
        }
        
        public void NotifyFontSizeChanged()
        {
            OnPropertyChanged(nameof(TitleFontSize));
            OnPropertyChanged(nameof(SectionFontSize));
            OnPropertyChanged(nameof(CaptionFontSize));
            OnPropertyChanged(nameof(EntryFontSize));
        }

        private int GetMaxWidth()
        {
            return Sections.Max(s => s.GetWidth());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}