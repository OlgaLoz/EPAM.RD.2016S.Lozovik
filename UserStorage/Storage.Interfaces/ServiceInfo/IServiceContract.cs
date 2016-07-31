using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.ServiceModel;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Search;

namespace Storage.Interfaces.ServiceInfo
{
    [ServiceContract]
    public interface IServiceContract
    {
        [OperationContract]
        int Add(User user);

        [OperationContract]
        List<int> Search(SearchCriteria<User> criteria);

        [OperationContract]
        void Delete(int id);
    }
}