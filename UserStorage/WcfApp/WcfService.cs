using System;
using System.Collections.Generic;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.ServiceInfo;

namespace WcfApp
{
    public class WcfService : IServiceContract
    {
        public int Add(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Search(Predicate<User>[] criteria)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}