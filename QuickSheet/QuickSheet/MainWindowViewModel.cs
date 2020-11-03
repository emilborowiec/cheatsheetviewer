#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using QuickSheet.Annotations;
using QuickSheet.CheatSheetPanel;
using QuickSheet.Dialog;
using QuickSheet.Model;
using QuickSheet.Services;

#endregion

namespace QuickSheet
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private List<CheatSheet> _cheatSheets;
        private Settings _settings;
        private int _currentIndex = -1;
        private CheatSheetViewModel _cheatSheetViewModel;
        private List<Result<CheatSheet>> _errors;
        private bool _sheetsInfoPanelVisible;
        private bool _shortcutsInfoPanelVisible;

        public event PropertyChangedEventHandler PropertyChanged;

        public DelegateCommand<string> SwitchSheetCommand { get; }
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand ReloadSheetsCommand { get; }
        public DelegateCommand<string> AdjustFontSizeCommand { get; }
        public DelegateCommand ToggleDarkModeCommand { get; }
        public DelegateCommand ToggleLockCommand { get; }
        public DelegateCommand ToggleSheetsInfoCommand { get; }
        public DelegateCommand ToggleShortcutsInfoCommand { get; }
        public DelegateCommand<string> ShowSheetAtPositionCommand { get; }
        public CheatSheet CurrentCheatSheet => CurrentIndex == -1 ? null : _cheatSheets[CurrentIndex];
        public Dictionary<string, string> KeyboardShortcutsDictionary { get; set; }
        public List<Tuple<string, int>> CheatSheetPositions => _cheatSheets.Select((CheatSheet sheet, int index) => new Tuple<string, int>(sheet.Title, index+1)).ToList();

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                if (value == _currentIndex) return;
                _currentIndex = value;
                _cheatSheetViewModel.SetCheatSheet(CurrentCheatSheet, _settings.GetSettings(CurrentCheatSheet.Title));
            }
        }

        public CheatSheetViewModel CheatSheetViewModel
        {
            get => _cheatSheetViewModel;
            set
            {
                _cheatSheetViewModel = value;
                OnPropertyChanged(nameof(CheatSheetViewModel));
            }
        }

        public List<Result<CheatSheet>> Errors
        {
            get => _errors;
            set
            {
                _errors = value;
                OnPropertyChanged(nameof(Errors));
            }
        }

        public bool SheetsInfoPanelVisible
        {
            get => _sheetsInfoPanelVisible;
            set
            {
                _sheetsInfoPanelVisible = value;
                OnPropertyChanged(nameof(SheetsInfoPanelVisible));
            }
        }

        public bool ShortcutsInfoPanelVisible
        {
            get => _shortcutsInfoPanelVisible;
            set
            {
                _shortcutsInfoPanelVisible = value;
                OnPropertyChanged(nameof(ShortcutsInfoPanelVisible));
            }
        }

        public MainWindowViewModel()
        {
            _cheatSheetViewModel = new CheatSheetViewModel();
            SwitchSheetCommand = new DelegateCommand<string>(SwitchSheet);
            AdjustFontSizeCommand = new DelegateCommand<string>(AdjustFontSize);
            ExitCommand = new DelegateCommand(Exit);
            ReloadSheetsCommand = new DelegateCommand(ReloadCheatSheets);
            ToggleDarkModeCommand = new DelegateCommand(ToggleDarkMode);
            ToggleLockCommand = new DelegateCommand(ToggleLock);
            ToggleSheetsInfoCommand = new DelegateCommand(ToggleSheetsInfoPanel);
            ToggleShortcutsInfoCommand = new DelegateCommand(ToggleShortcutsInfoPanel);
            ShowSheetAtPositionCommand = new DelegateCommand<string>(ShowSheetAtPosition);
            LoadSettings();
            ReloadCheatSheets();

            KeyboardShortcutsDictionary = new Dictionary<string, string>();
            KeyboardShortcutsDictionary["Online help"] = "F1";
            KeyboardShortcutsDictionary["Toggle loaded sheets info panel"] = "F2";
            KeyboardShortcutsDictionary["Toggle keyboard shortcuts panel"] = "F10";
            KeyboardShortcutsDictionary["Next loaded sheet"] = "Right";
            KeyboardShortcutsDictionary["Previous loaded sheet"] = "Left";
            KeyboardShortcutsDictionary["View loaded sheet"] = "1-9";
            KeyboardShortcutsDictionary["Reload sheets"] = "R";
            KeyboardShortcutsDictionary["Increase font size and lock"] = "Up";
            KeyboardShortcutsDictionary["Decrease font size and lock"] = "Down";
            KeyboardShortcutsDictionary["Toggle font size lock"] = "L";
            KeyboardShortcutsDictionary["Toggle dark mode"] = "M";
            KeyboardShortcutsDictionary["Close dialog / program"] = "Esc";
        }

        private void ToggleSheetsInfoPanel()
        {
            SheetsInfoPanelVisible = !SheetsInfoPanelVisible;
        }

        private void ToggleShortcutsInfoPanel()
        {
            ShortcutsInfoPanelVisible = !ShortcutsInfoPanelVisible;
        }

        private void ShowSheetAtPosition(string index)
        {
            var i = int.Parse(index);
            if (i <= _cheatSheets.Count)
            {
                CurrentIndex = i - 1;
            }
        }

        private void LoadSettings()
        {
            _settings = SettingsService.LoadSettings();
        }

        private void ToggleDarkMode()
        {
            CheatSheetViewModel.DarkMode = !CheatSheetViewModel.DarkMode;
        }

        private void ToggleLock()
        {
            var settings = _settings.GetSettings(CurrentCheatSheet.Title);
            if (settings != null)
            {
                settings.FontSizeLock = !settings.FontSizeLock;
                SettingsService.SaveSettings(_settings);
                _cheatSheetViewModel.Settings = _settings.GetSettings(CurrentCheatSheet.Title);
            }
        }

        private void ReloadCheatSheets()
        {
            _cheatSheets = new List<CheatSheet>();
            var results = CheatSheetLoader.LoadCheatSheets();
            var cheatSheets = results.Where(r => r.IsSuccess).Select(r => r.Data);
            foreach (var sheet in cheatSheets)
            {
                _cheatSheets.Add(sheet);
            }

            CurrentIndex = _cheatSheets.Count > 0 ? 0 : -1;

            Errors = results.Where(r => r.IsSuccess != true).ToList();
            if (Errors.Count > 0)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(
                    (Action) (async () =>
                    {
                        await Task.Delay(1000);
                        OpenDialog(
                            "Some quicksheet files failed to load!",
                            string.Join('\n', Errors.Select(e => e.Source + " - " + e.Message)));
                    }));
            }
        }

        private void SwitchSheet(string directionString)
        {
            if (CurrentIndex == -1) return;
            if ("left".Equals(directionString))
            {
                if (CurrentIndex > 0)
                {
                    CurrentIndex -= 1;
                }
                else
                {
                    CurrentIndex = _cheatSheets.Count - 1;
                }
            }
            else
            {
                if (CurrentIndex < _cheatSheets.Count - 1)
                {
                    CurrentIndex += 1;
                }
                else
                {
                    CurrentIndex = 0;
                }
            }
        }

        private void AdjustFontSize(string param)
        {
            var amount = "up".Equals(param) ? 2 : -2;
            CheatSheetViewModel.BaseFontSize += amount;
            var sheetSettings = _settings.GetSettings(CheatSheetViewModel.CheatSheet.Title);
            sheetSettings.FontSizeLock = true;
            sheetSettings.BaseFontSize = CheatSheetViewModel.BaseFontSize;
            SettingsService.SaveSettings(_settings);
            _cheatSheetViewModel.Settings = _settings.GetSettings(CurrentCheatSheet.Title);
        }

        private void OpenDialog(string title, string message)
        {
            DialogService.OpenDialog(new DialogViewModel {Title = title, Message = message});
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}