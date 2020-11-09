#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private bool _darkMode;

        public event PropertyChangedEventHandler PropertyChanged;

        public DelegateCommand<string> SwitchSheetCommand { get; }
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand ReloadSheetsCommand { get; }
        public DelegateCommand<string> AdjustFontSizeCommand { get; }
        public DelegateCommand ToggleDarkModeCommand { get; }
        public DelegateCommand ToggleLockCommand { get; }
        public DelegateCommand ToggleSheetsInfoCommand { get; }
        public DelegateCommand OnlineHelpCommand { get; }
        public DelegateCommand ToggleShortcutsInfoCommand { get; }
        public DelegateCommand<string> ShowSheetAtPositionCommand { get; }
        public CheatSheet CurrentCheatSheet => CurrentIndex == -1 ? null : _cheatSheets[CurrentIndex];

        public List<Tuple<string, int>> CheatSheetPositions =>
            _cheatSheets.Select((sheet, index) => new Tuple<string, int>(sheet.Title, index + 1)).ToList();

        public Dictionary<string, string> KeymapDictionary { get; set; }

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

        public bool DarkMode
        {
            get => _darkMode;
            set
            {
                if (value == _darkMode) return;
                _darkMode = value;
                OnPropertyChanged(nameof(DarkMode));
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
            OnlineHelpCommand = new DelegateCommand(ShowOnlineHelp);
            ToggleSheetsInfoCommand = new DelegateCommand(ToggleSheetsInfoPanel);
            ToggleShortcutsInfoCommand = new DelegateCommand(ToggleShortcutsInfoPanel);
            ShowSheetAtPositionCommand = new DelegateCommand<string>(ShowSheetAtPosition);
            LoadSettings();
            ReloadCheatSheets();

            KeymapDictionary = new Dictionary<string, string>();
            KeymapDictionary["Online help"] = "F1";
            KeymapDictionary["Toggle keymap panel"] = "F2";
            KeymapDictionary["Toggle sheets index panel"] = "F3";
            KeymapDictionary["Next loaded sheet"] = "Right";
            KeymapDictionary["Previous loaded sheet"] = "Left";
            KeymapDictionary["View loaded sheet"] = "1-9";
            KeymapDictionary["Reload sheets"] = "R";
            KeymapDictionary["Increase font size and lock"] = "Up";
            KeymapDictionary["Decrease font size and lock"] = "Down";
            KeymapDictionary["Toggle font size lock"] = "L";
            KeymapDictionary["Toggle dark mode"] = "M";
            KeymapDictionary["Close dialog / program"] = "Esc";
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
            DarkMode = !DarkMode;
            CheatSheetViewModel.DarkMode = DarkMode;
        }

        private void ToggleLock()
        {
            if (CurrentCheatSheet == null) return;
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
                        await Task.Delay(500);
                        OpenDialog(
                            "Some quicksheet files failed to load!",
                            string.Join('\n', Errors.Select(e => e.Source + " - " + e.Message)));
                    }));
            } else if (_cheatSheets.Count == 0)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(
                    (Action) (async () =>
                    {
                        await Task.Delay(500);
                        OpenDialog(
                            "No quicksheet files found!",
                            "Quicksheet files are loaded from user's 'Documents\\My QuickSheets' folder.\nCheck online help (F1) for samples and file format documentation.");
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

        private void ShowOnlineHelp()
        {
            var myProcess = new Process();

            try
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "http://www.emilborowiec.com/quicksheet";
                myProcess.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void AdjustFontSize(string param)
        {
            if (CheatSheetViewModel.CheatSheet == null) return;
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