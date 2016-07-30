using System;
using Storage.Interfaces.Entities.ConnectionInfo;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.Entities.UserEventArgs
{
    public class UserEventArgs : EventArgs
    {
        public User User { get; set; }

        public Operation Operation { get; set; }
    }
}