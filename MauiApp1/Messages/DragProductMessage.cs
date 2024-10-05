using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MauiApp1.Messages;
public class DragProductMessage : ValueChangedMessage<bool>
{
    public DragProductMessage(bool value) : base(value)
    {
    }
}

