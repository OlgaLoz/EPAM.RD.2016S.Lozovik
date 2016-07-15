using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Loader;
using Storage.Strategy;
using Storage.UserInfo;

//https://msdn.microsoft.com/en-us/library/system.diagnostics.tracelistener(v=vs.110).aspx
//https://msdn.microsoft.com/ru-ru/library/system.diagnostics.trace(v=vs.110).aspx

namespace Storage.Service
{
    public class UserService : IUserService
    {
        private readonly ILoader loader;
        private readonly IEnumerator<int> idGenerator;
        private readonly IStrategy strategy;

        public UserService(IStrategy strategy, ILoader loader, IEnumerator<int> idGenerator )
        {
            this.strategy = strategy;
            this.loader = loader;
            this.idGenerator = idGenerator;
            if (strategy ==null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }
        }

        public int Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return strategy.Add(user, idGenerator);
        }

        public IEnumerable<int> Search(Predicate<User>[] criteria)
        {
            return strategy.Search(criteria);
        }

        public void Delete(int id)
        {
            strategy.Delete(id);
        }

        public void Save()
        {
            loader.Save(new ServiceState {Users = strategy.Users.ToList(), CurrentId = idGenerator.Current});
        }

        public void Load()
        {
            var state =  loader.Load();
            strategy.Users = state.Users;
            while (idGenerator.Current < state.CurrentId)
            {
                idGenerator.MoveNext();
            }
        }

    }
}