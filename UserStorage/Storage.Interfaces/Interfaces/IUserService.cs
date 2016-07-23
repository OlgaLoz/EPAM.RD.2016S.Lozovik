using System;
using System.Collections.Generic;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.Interfaces
{
    public interface IUserService
    {
        List<User> Users { get; }
        int Add(User user);
        IEnumerable<int> Search(Predicate<User>[] criteria);
        void Delete(int id);
    }
}