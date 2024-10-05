using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiApp1.Messages;

public class AddProductMessage : ValueChangedMessage<bool>
{
    public AddProductMessage(bool value) : base(value)
    {
    }
}

