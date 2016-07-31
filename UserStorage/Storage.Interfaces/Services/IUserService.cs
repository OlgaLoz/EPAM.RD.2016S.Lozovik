using System;
using System.Collections.Generic;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Search;

namespace Storage.Interfaces.Services
{
    public interface IUserService 
    {
        int Add(User user);

        IEnumerable<int> Search(SearchCriteria<User> criteria);

        void Delete(int id);
    }
}