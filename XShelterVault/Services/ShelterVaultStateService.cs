using ShelterVault.Models;
using ShelterVault.Shared.Extensions;

namespace ShelterVault.Services
{
    public interface IShelterVaultStateService
    {
        (byte[], byte[]) GetLocalEncryptionValues();
        void SetVault(ShelterVaultModel shelterVaultModel);
        void SetVault(ShelterVaultModel shelterVaultModel, string masterKey);
        void ResetState();
        ShelterVaultModel ShelterVault { get; }
        void SetDialogStatus(bool isDialogOpen);
        bool GetDialogStatus();
    }

    public class ShelterVaultStateService : IShelterVaultStateService
    {
        private readonly IEncryptionService _encryptionService;
        private byte[] _inMemoryDerivedKeyProtected;
        private byte[] _inMemorySaltProtected;
        private bool _isDialogOpen;

        public ShelterVaultStateService(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }


        public ShelterVaultModel ShelterVault { get; private set; }

        private byte[] GetDerivedKeyUnprotected()
        {
            return _inMemoryDerivedKeyProtected;
        }

        private byte[] GetSaltUnprotected()
        {
            return _inMemorySaltProtected;
        }

        public (byte[], byte[]) GetLocalEncryptionValues()
        {
            return (GetDerivedKeyUnprotected(), GetSaltUnprotected());
        }

        public void SetVault(ShelterVaultModel shelterVaultModel, string masterKey)
        {
            SetVault(shelterVaultModel);
            byte[] salt = shelterVaultModel.Salt.FromBase64ToBytes();
            byte[] derivedKey = _encryptionService.DeriveKeyFromPassword(masterKey, salt);
            ProtectEncryptionValues(derivedKey, salt);
        }

        public void SetVault(ShelterVaultModel shelterVaultModel)
        {
            ShelterVault = shelterVaultModel;
        }

        public void ResetState()
        {
            _inMemoryDerivedKeyProtected = Array.Empty<byte>();
            _inMemorySaltProtected = Array.Empty<byte>();
            ShelterVault = new();
        }

        private void ProtectEncryptionValues(byte[] derivedKey, byte[] salt)
        {
            _inMemoryDerivedKeyProtected = derivedKey;
            _inMemorySaltProtected = salt;
        }

        public void SetDialogStatus(bool isDialogOpen)
        {
            _isDialogOpen = isDialogOpen;
        }

        public bool GetDialogStatus()
        {
            return _isDialogOpen;
        }
    }
}
