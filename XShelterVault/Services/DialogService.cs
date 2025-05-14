using XShelterVault;

namespace Desktiny.UI.Services
{
    public interface IDialogService
    {
        Task ShowConfirmationDialogAsync(string message);
        Task<bool> ShowContinueConfirmationDialogAsync(string message);
    }
    public class DialogService : IDialogService
    {

        public DialogService()
        {

        }

        public async Task ShowConfirmationDialogAsync(string message)
        {
            ContentDialog dialog = BuildDialog(message);
            await dialog.ShowAsync();
        }

        public async Task<bool> ShowContinueConfirmationDialogAsync(string message)
        {
            ContentDialog dialog = BuildDialog(message);
            //dialog.SecondaryButtonText = _languageService.GetLangValue(secondaryButtonResourceKey);
            ContentDialogResult result = await dialog.ShowAsync();
            return true;
        }

        private ContentDialog BuildDialog(string message)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = App.MainWindow.Content.XamlRoot;
            dialog.RequestedTheme = (App.MainWindow.Content as FrameworkElement).RequestedTheme;
            dialog.Title = "Attention";
            dialog.PrimaryButtonText = "Ok";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new TextBlock() { Text = message };

            return dialog;
        }
    }
}
