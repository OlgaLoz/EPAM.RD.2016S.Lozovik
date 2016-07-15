using System;
using System.Collections.Generic;
using Storage.UserInfo;

namespace Storage.Strategy
{
    public interface IStrategy
    {
        IList<User> Users { get; set; } 
        int Add(User user,IEnumerator<int> idGenerator  );
        IEnumerable<int> Search(Predicate<User>[] criteria);
        void Delete(int id);
    }
}