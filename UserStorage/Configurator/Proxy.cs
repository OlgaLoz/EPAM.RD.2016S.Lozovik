using System;
using System.Collections.Generic;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Interfaces;

namespace Configurator
{
    public class Proxy : IUserService
    {
        private readonly IUserService master;
        private readonly List<IUserService> slaves;
        private int currentService;

        public Proxy(IUserService master, List<IUserService> slaves)
        {
            if (master == null)
            {
                throw new ArgumentNullException(nameof(master));
            }
            if (slaves == null)
            {
                throw new ArgumentNullException(nameof(slaves));
            }
            this.master = master;
            this.slaves = slaves;
        }

        public int Add(User user)
        {
            return master.Add(user);
        }

        public IEnumerable<int> Search(Predicate<User>[] criteria)
        {
            if (currentService == 0)
            {
                currentService++;
                return master.Search(criteria);
            }
            if (currentService == slaves.Count + 1)
            {
                currentService = 1;
                return master.Search(criteria);
            }
            return slaves[currentService - 1].Search(criteria);
        }

        public void Delete(int id)
        {
            master.Delete(id);
        }
    }
}