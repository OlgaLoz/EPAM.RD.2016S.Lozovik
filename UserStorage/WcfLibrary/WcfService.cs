using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Search;
using Storage.Interfaces.ServiceInfo;
using Storage.Interfaces.Services;

namespace WcfLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WcfService : MarshalByRefObject, IServiceContract
    {
        private readonly IUserService userService;

        public WcfService(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            this.userService = userService;
        }

        public int Add(User user)
        {
            return userService.Add(user);
        }

        public List<int> Search(User criteria)
        {
            return userService.Search(criteria.ConvertToPredicate()).ToList();
        }

        public void Delete(int id)
        {
            userService.Delete(id);
        }
    }
}