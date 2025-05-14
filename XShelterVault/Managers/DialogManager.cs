using Desktiny.UI.Services;
using ShelterVault.Services;

namespace ShelterVault.Managers
{
    public interface IDialogManager
    {
        Task ShowConfirmationDialogAsync(string message);
        Task<bool> ShowContinueConfirmationDialogAsync(string message);
    }

    public class DialogManager : IDialogManager
    {
        private readonly IShelterVaultStateService _shelterVaultStateService;
        private readonly IDialogService _dialogService;

        public DialogManager(IShelterVaultStateService shelterVaultStateService, IDialogService dialogService)
        {
            _shelterVaultStateService = shelterVaultStateService;
            _dialogService = dialogService;
        }

        public async Task ShowConfirmationDialogAsync(string message)
        {
            _shelterVaultStateService.SetDialogStatus(true);
            await _dialogService.ShowConfirmationDialogAsync(message);
            _shelterVaultStateService.SetDialogStatus(false);
        }

        public async Task<bool> ShowContinueConfirmationDialogAsync(string message)
        {
            _shelterVaultStateService.SetDialogStatus(true);
            bool result = await _dialogService.ShowContinueConfirmationDialogAsync(message);
            _shelterVaultStateService.SetDialogStatus(false);
            return result;
        }
    }
}
