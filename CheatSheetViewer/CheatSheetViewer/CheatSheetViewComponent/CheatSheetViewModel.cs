#region

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CheatSheetViewerApp.Model;
using CheatSheetViewerApp.Services;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

#endregion

namespace CheatSheetViewerApp.CheatSheetViewComponent
{
    public class CheatSheetViewModel : INotifyPropertyChanged
    {
        private static readonly int DefaultFontSize = 12;
        private static readonly int MinBaseFontSize = 6;
        private static readonly int MaxBaseFontSize = 48;

        private int _baseFontSize = DefaultFontSize;
        private bool _darkMode;
        private CheatSheet _cheatSheet;
        private SheetSettings _settings;
        public event PropertyChangedEventHandler PropertyChanged;

        public int TitleFontSize
        {
            get
            {
                if (Application.Current.MainWindow == null) return DefaultFontSize;
                var h = (int) ((Panel) Application.Current.MainWindow.Content).ActualHeight;
                var size = 20;
                if (h >= 720) size = 24;
                if (h >= 1080) size = 28;
                if (h >= 1440) size = 32;
                if (h >= 2160) size = MaxBaseFontSize;
                if (h >= 4320) size = MaxBaseFontSize + 4;
                return DefaultFontSize + size;
            }
        }

        public int SectionFontSize => (int) (BaseFontSize * 1.2);
        public int CaptionFontSize => (int) (BaseFontSize * 1.1);
        public int EntryFontSize => BaseFontSize;

        public int BaseFontSize
        {
            get => _baseFontSize;
            set
            {
                if (value != _baseFontSize && value >= MinBaseFontSize && value <= MaxBaseFontSize)
                {
                    _baseFontSize = value;
                    NotifyFontSizeChanged();
                }
            }
        }

        public CheatSheet CheatSheet
        {
            get => _cheatSheet;
            private set
            {
                if (value != _cheatSheet)
                {
                    _cheatSheet = value;
                    Sections = CreateViewSections(_cheatSheet);
                    UpdateSectionBackgrounds();
                    OnPropertyChanged(nameof(CheatSheet));
                    OnPropertyChanged(nameof(Sections));
                    UpdateBaseFontSize();
                    NotifyFontSizeChanged();
                }
            }
        }

        public bool DarkMode
        {
            get => _darkMode;
            set
            {
                if (value != _darkMode)
                {
                    _darkMode = value;
                    UpdateSectionBackgrounds();
                    OnPropertyChanged(nameof(DarkMode));
                }
            }
        }

        public SheetSettings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        public List<SectionContent> Sections { get; private set; }


        private static List<SectionContent> CreateViewSections(CheatSheet cheatSheet)
        {
            var viewSections = new List<SectionContent>();
            if (cheatSheet.Cheats.Count > 0)
            {
                var sectionContent = new SectionContent(cheatSheet.Cheats);
                viewSections.Add(sectionContent);
            }

            viewSections.AddRange(
                cheatSheet.Sections.Select((section, index) =>
                {
                    var sectionContent =
                        new SectionContent(section.Name, section.Cheats);
                    return sectionContent;
                }));

            return viewSections;
        }

        public void SetCheatSheet(CheatSheet sheet, SheetSettings settings)
        {
            Settings = settings;
            CheatSheet = sheet;
        }

        public void UpdateBaseFontSize()
        {
            BaseFontSize = _settings != null && _settings.FontSizeLock
                ? _settings.BaseFontSize
                : CalculateBaseFontSize();
        }

        public void NotifyFontSizeChanged()
        {
            OnPropertyChanged(nameof(TitleFontSize));
            OnPropertyChanged(nameof(SectionFontSize));
            OnPropertyChanged(nameof(CaptionFontSize));
            OnPropertyChanged(nameof(EntryFontSize));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int GetLineCount()
        {
            return 1 + Sections?.Sum(s => s.GetLineCount()) ?? 0;
        }

        private int CalculateBaseFontSize()
        {
            if (Application.Current.MainWindow?.Content == null) return DefaultFontSize;
            var h = (int) ((Panel) Application.Current.MainWindow.Content).ActualHeight;
            var w = (int) ((Panel) Application.Current.MainWindow.Content).ActualWidth;
            var referenceString = new string('o', GetMaxWidth());
            double size = 4;
            var increment = 2;

            while (size + increment < MaxBaseFontSize && WillItFit(
                referenceString, new Typeface("Arial"), size + increment, w, h))
            {
                size += increment;
            }

            return (int) size;
        }

        private bool WillItFit(
            string referenceString,
            Typeface typeface,
            double fontSize,
            int windowWidth,
            int windowHeight)
        {
            var formattedText = new FormattedText(
                referenceString,
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

        private int GetMaxWidth()
        {
            return Sections?.Max(s => s.GetWidthInCharacters()) ?? 0;
        }

        private void UpdateSectionBackgrounds()
        {
            for (var i = 0; i < Sections.Count; i++)
            {
                var section = Sections[i];
                section.BackgroundBrush = GetIndexedSectionBrush(i);
            }
        }
        
        private Brush GetIndexedSectionBrush(int index)
        {
            var baseColor = Colors.Aqua;
            var colors = SectionColors.All;
            if (index >= 0 && colors.Length > index)
            {
                baseColor = colors[index];
            }
            if (_darkMode)
            {
                baseColor = Darken(baseColor, 0.4);
            }
            var darker = Darken(baseColor, 0.8);
            return new LinearGradientBrush(baseColor, darker, new Point(0, 0), new Point(1, 1));
        }

        private static Color Darken(Color color, double d)
        {
            return Color.FromRgb((byte) (color.R * d), (byte) (color.G * d), (byte) (color.B * d));
        }

    }
}