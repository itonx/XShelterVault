using ShelterVault.DataLayer;
using ShelterVault.Managers;
using ShelterVault.Models;
using ShelterVault.Services;

namespace XShelterVault.Presentation;
public partial class ConfirmMasterKeyViewModel : ObservableObject
{
    private INavigator _navigator;
    private readonly IShelterVaultStateService _shelterVaultStateService;
    //private readonly IDialogManager _dialogManager;
    //private readonly IProgressBarService _progressBarService;
    private readonly IShelterVaultLocalDb _shelterVaultLocalDb;
    private readonly IVaultManager _vaultManager;

    [ObservableProperty]
    private string? name;
    [ObservableProperty]
    public partial List<ShelterVaultModel> Vaults { get; set; }
    [ObservableProperty]
    public partial ShelterVaultModel SelectedVault { get; set; }

    public ConfirmMasterKeyViewModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator,
        IShelterVault shelterVault,
        IVaultManager vaultManager,
        IShelterVaultLocalDb shelterVaultLocalDb,
        IShelterVaultStateService shelterVaultStateService/*,
        IDialogManager dialogManager,
        IProgressBarService progressBarService,
        */)
    {
        _navigator = navigator;
        Title = "ConfirmMasterKeyPage";
        Vaults = shelterVault.GetAllActiveVaults().ToList();
        if (Vaults.Any()) SelectedVault = Vaults.FirstOrDefault();
        _vaultManager = vaultManager;
        _shelterVaultStateService = shelterVaultStateService;
        //_dialogManager = dialogManager;
        //_progressBarService = progressBarService;
        _shelterVaultLocalDb = shelterVaultLocalDb;
    }
    public string? Title { get; }

    [RelayCommand]
    private async Task ConfirmMasterKey(object parameter)
    {
        try
        {
            //await _progressBarService.Show();
            if (_vaultManager.IsValid(parameter?.ToString(), SelectedVault))
            {
                _shelterVaultLocalDb.SetDbName(SelectedVault.Name);
                _shelterVaultStateService.SetVault(SelectedVault, parameter?.ToString());
                await _navigator.NavigateViewModelAsync<NavigationViewModel>(this);
            }
            //else await _dialogManager.ShowConfirmationDialogAsync(LangResourceKeys.DIALOG_WRONG_MASTER_KEY);
        }
        finally
        {
            //await _progressBarService.Hide();
        }
    }

    [RelayCommand]
    private async Task NewVault()
    {
        await _navigator.NavigateViewModelAsync<CreateMasterKeyViewModel>(this);
    }
}
