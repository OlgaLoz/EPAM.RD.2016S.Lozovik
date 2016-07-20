using System;
using Storage.Entities.UserInfo;

namespace Storage.Entities.UserEventArgs
{
    public sealed class UserEventArgs : EventArgs
    {
        public User User { get; set; }
    }
}