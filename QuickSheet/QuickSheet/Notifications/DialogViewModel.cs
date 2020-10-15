using System.Windows;

namespace QuickSheet.Notifications
{
    public class DialogViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DelegateCommand<Window> ExitCommand { get; }

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