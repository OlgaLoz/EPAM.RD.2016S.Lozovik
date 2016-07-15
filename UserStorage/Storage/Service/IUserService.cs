using System;
using System.Collections.Generic;
using Storage.UserInfo;

namespace Storage.Service
{
    public interface IUserService
    {
        int Add(User user);
        IEnumerable<int> Search(Predicate<User>[] criteria);
        void Delete(int id);
    }
}