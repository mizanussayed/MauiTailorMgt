using System;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MYPM.Messages;
public class DragProductMessage : ValueChangedMessage<bool>
{
    public DragProductMessage(bool value) : base(value)
    {
    }
}

