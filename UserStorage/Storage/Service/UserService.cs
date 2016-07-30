using System;
using System.Collections.Generic;
using System.Diagnostics;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Service
{
    public class UserService/* : IUserService*/
    {
   /*     private readonly IUserService userService;
        private readonly BooleanSwitch traceSwitch;
        private readonly TraceSource traceSource;

        public UserService(IUserService userService)
        {
            if (userService == null)
            {
                if (traceSwitch.Enabled)
                    traceSource.TraceEvent(TraceEventType.Error, 0, "User service is null!");
                throw new ArgumentNullException(nameof(userService));
            }
            traceSwitch = new BooleanSwitch("traceSwitch",String.Empty);
            traceSource = new TraceSource("traceSource");
            this.userService = userService;
        }
        
        public int Add(User user)
        {
            if (traceSwitch.Enabled)
            {
                traceSource.TraceEvent(TraceEventType.Information, 0, "Add method work!");
            }

            return userService.Add(user);
        }

        public IEnumerable<int> Search(Predicate<User>[] criteria)
        {
            if (traceSwitch.Enabled) 
                traceSource.TraceEvent(TraceEventType.Information, 0, "Search method work!");
            return userService.Search(criteria);
        }

        public void Delete(int id)
        {
            if (traceSwitch.Enabled)
                traceSource.TraceEvent(TraceEventType.Information, 0, "Delete method work!");
            userService.Delete(id);
        }*/
    }
}