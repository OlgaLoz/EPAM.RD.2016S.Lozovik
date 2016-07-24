using System;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.Entities.ConnectionInfo
{
    [Serializable]
    public class Message
    {
        public Operation Operation { get; set; }
        public User User { get; set; }
    }
}