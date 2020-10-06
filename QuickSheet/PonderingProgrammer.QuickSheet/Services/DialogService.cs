using PonderingProgrammer.QuickSheet.Notifications;

namespace PonderingProgrammer.QuickSheet.Services
{
    public static class DialogService
    {
        public static void OpenDialog(DialogViewModel dialogViewModel)
        {
            var win = new DialogWindow {DataContext = dialogViewModel};
            win.ShowDialog();  
        }
    }
}