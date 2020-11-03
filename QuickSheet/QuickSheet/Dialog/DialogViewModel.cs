#region

using System.Windows;

#endregion

namespace QuickSheet.Dialog
{
    public class DialogViewModel
    {
        public DelegateCommand<Window> ExitCommand { get; }
        public string Title { get; set; }
        public string Message { get; set; }

        public DialogViewModel()
        {
            ExitCommand = new DelegateCommand<Window>(Exit);
        }

        private void Exit(Window window)
        {
            window.Close();
        }
    }
}