using ShelterVault.DataLayer;

namespace XShelterVault.Presentation;

public partial class MainViewModel : ObservableObject
{
    private INavigator _navigator;
    private IShelterVault _shelterVault;

    public MainViewModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator,
        IShelterVault shelterVault)
    {
        _navigator = navigator;
        _shelterVault = shelterVault;
        Init();
    }

    public async Task Init()
    {
        if (_shelterVault.AreThereVaults())
            await _navigator.NavigateViewModelAsync<ConfirmMasterKeyViewModel>(this);
        else
            await _navigator.NavigateViewModelAsync<CreateMasterKeyViewModel>(this);
    }
}
