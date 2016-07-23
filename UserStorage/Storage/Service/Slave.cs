using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Interfaces.Entities.UserEventArgs;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Interfaces;

namespace Storage.Service
{
    [Serializable]
    public class Slave : IUserService
    {
        public List<User> Users { get; set; }

        public Slave(IMaster master)
        {
            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            Users = master.Users;
            master.AddUser += UpdateAfterAdd;
            master.DeleteUser += UpdateAfterDelete;
        }

        public void Delete(int id)
        {
            throw new AccessViolationException();
        }

        public int Add(User user)
        {
            throw new AccessViolationException();
        }

        public virtual IEnumerable<int> Search(Predicate<User>[] criteria)
        {
            var result = new List<int>();
            for (int i = 0; i < criteria.Length; i++)
            {
                result.AddRange(Users.ToList().FindAll(criteria[i]).Select(user => user.PersonalId));
            }
            return result;
        }

        private void UpdateAfterDelete(object sender, UserEventArgs e)
        {
            Users.Add(e.User);
        }

        private void UpdateAfterAdd(object sender, UserEventArgs e)
        {
            var userToRemove = Users.FirstOrDefault(user => user.PersonalId == e.User.PersonalId);
            if (userToRemove != null)
            {
                Users.Remove(userToRemove);
            }
        }

    }
}