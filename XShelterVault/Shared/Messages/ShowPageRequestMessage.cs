using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ShelterVault.Shared.Messages
{
    public class ShowPageRequestMessage : ValueChangedMessage<Type>
    {
        public ShowPageRequestMessage(Type value) : base(value)
        {
        }
    }
}
