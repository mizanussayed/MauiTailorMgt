using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MYPM.Messages;

public class AddProductMessage : ValueChangedMessage<bool>
{
    public AddProductMessage(bool value) : base(value)
    {
    }
}

