using System.Collections.Generic;
using Storage.Interfaces.Entities.ServiceState;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Repository;

namespace Storage.Tests
{
    public class FakeRepository : IRepository
    {
        public void Save(ServiceState state)
        {
            throw new System.NotImplementedException();
        }

        public ServiceState Load()
        {
            return new ServiceState {CurrentId = 0, Users = TestInfo.Users};
        }
    }
}