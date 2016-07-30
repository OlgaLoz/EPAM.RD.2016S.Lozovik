using System;
using System.Runtime.InteropServices.ComTypes;
using Storage.Interfaces.Entities.UserEventArgs;

namespace Storage.Interfaces.Network
{
    public interface IReceiver
    {
        EventHandler<UserEventArgs> Update { get; set; }

        void Receive();
    }
}