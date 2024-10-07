using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MYPM.Messages;

public class AddToOrderMessage : ValueChangedMessage<Item>
{
    public AddToOrderMessage(Item product) : base(product)
    {
    }
}
