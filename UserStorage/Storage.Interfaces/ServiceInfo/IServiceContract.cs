using System;
using System.Collections.Generic;
using System.ServiceModel;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.ServiceInfo
{
    [ServiceContract]
    public interface IServiceContract
    {
        [OperationContract]
        int Add(User user);

        [OperationContract]
        IEnumerable<int> Search(Predicate<User>[] criteria);

        [OperationContract]
        void Delete(int id);
    }
}