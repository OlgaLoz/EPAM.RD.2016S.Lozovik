using System;
using System.Collections.Generic;
using Storage.Entities.UserInfo;

namespace Storage.Entities.ServiceState
{
    [Serializable]
    public class ServiceState
    {
        public List<User> Users { get; set; }
        public int CurrentId { get; set; }
    }
}