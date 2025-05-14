using ShelterVault.DataLayer;
using ShelterVault.Managers;

namespace XShelterVault.Presentation
{
    public partial class CreateMasterKeyViewModel : ObservableObject
    {
        private INavigator _navigator;
        private string _shelterVaultDefaultPath;

        [ObservableProperty]
        public partial PasswordConfirmationViewModel PasswordRequirementsVM { get; set; }
        [ObservableProperty]
        public partial string Name { get; set; }
        [ObservableProperty]
        public partial string Password { get; set; }
        [ObservableProperty]
        public partial string PasswordConfirmation { get; set; }
        [ObservableProperty]
        public partial string ShelterVaultPath { get; set; }
        [ObservableProperty]
        public partial bool ShowCancel { get; set; }
        [ObservableProperty]
        public partial string DefaultPath { get; set; }

        private readonly IVaultCreatorManager _vaultCreatorManager;
        //private readonly IProgressBarService _progressBarService;

        public CreateMasterKeyViewModel(IVaultCreatorManager shelterVaultCreatorManager, IShelterVault shelterVault, IShelterVaultLocalDb shelterVaultLocalDb, INavigator navigator, IDialogManager dialogManager)
        {
            _navigator = navigator;
            _vaultCreatorManager = shelterVaultCreatorManager;
            //_progressBarService = progressBarService;
            _shelterVaultDefaultPath = shelterVaultLocalDb.DefaultShelterVaultPath;
            PasswordRequirementsVM = new PasswordConfirmationViewModel(dialogManager, "Master key password must:");
            ShowCancel = shelterVault.GetAllActiveVaults().Any();
            DefaultPath = _shelterVaultDefaultPath;
        }

        partial void OnNameChanged(string value)
        {
            DefaultPath = Path.Combine(_shelterVaultDefaultPath, string.Concat(value, ".db"));
        }

        [RelayCommand]
        private async Task CreateMasterKey()
        {
            try
            {
                if (await PasswordRequirementsVM.ArePasswordsValid(Password, PasswordConfirmation))
                {
                    //await _progressBarService.Show();
                    string salt = Guid.NewGuid().ToString();
                    string uuid = Guid.NewGuid().ToString();
                    bool wasVaultCreated = _vaultCreatorManager.CreateVault(uuid, Name, Password, salt);
                    if (wasVaultCreated) await _navigator.NavigateBackAsync(this);
                }
            }
            finally
            {
                //await _progressBarService.Hide();
            }
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await _navigator.NavigateBackAsync(this);
        }
    }
}
