using System.Collections.Generic;
using Storage.UserInfo;

namespace Storage.Service
{
    public class ServiceState
    {
        public List<User> Users { get; set; }

        public int CurrentId { get; set; }
    }
}