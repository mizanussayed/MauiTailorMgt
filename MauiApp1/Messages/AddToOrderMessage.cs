using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiApp1.Messages;

public class AddToOrderMessage : ValueChangedMessage<Item>
{
    public AddToOrderMessage(Item product) : base(product)
    {
    }
}
