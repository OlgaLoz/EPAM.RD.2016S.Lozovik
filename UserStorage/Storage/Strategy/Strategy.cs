using System;
using System.Collections.Generic;
using System.Linq;
using Storage.UserInfo;

namespace Storage.Strategy
{
    public abstract class Strategy : IStrategy
    {
        public IList<User> Users { get; set; }
        public abstract int Add(User user, IEnumerator<int> idGenerator );
        public abstract void Delete(int id);

        public virtual IEnumerable<int> Search(Predicate<User>[] criteria)
        {
            var result = new List<int>();
            for (int i = 0; i < criteria.Length; i++)
            {
                result.AddRange(Users.ToList().FindAll(criteria[i]).Select(user => user.PersonalId));
            }
            return result;
        }
    }
}