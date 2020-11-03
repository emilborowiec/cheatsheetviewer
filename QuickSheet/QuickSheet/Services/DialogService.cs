#region

using QuickSheet.Dialog;

#endregion

namespace QuickSheet.Services
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