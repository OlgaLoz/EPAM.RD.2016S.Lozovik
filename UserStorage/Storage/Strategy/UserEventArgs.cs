using System;
using Storage.UserInfo;

namespace Storage.Strategy
{
    public sealed class UserEventArgs : EventArgs
    {
        public User User { get; set; }
    }
}