using System.Collections.Generic;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.Entities.ServiceState
{
    public class ServiceState
    {
        public List<User> Users { get; set; }
        public int CurrentId { get; set; }
    }
}