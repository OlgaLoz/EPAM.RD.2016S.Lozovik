using System;
using System.Collections.Generic;
using System.Linq;
using Storage.UserInfo;

namespace Storage.Strategy
{
    public class Slave : Strategy
    {
        public Slave(Master master)
        {
            master.AddUser += UpdateAfterAdd;
            master.DeleteUser += UpdateAfterDelete;
        }

        public override int Add(User user, IEnumerator<int> idGenerator)
        {
            throw new AccessViolationException();
        }

        public override void Delete(int id)
        {
            throw new AccessViolationException();
        }

        private void UpdateAfterDelete(object sender, UserEventArgs e)
        {
            Users.Add(e.User);
        }

        private void UpdateAfterAdd(object sender, UserEventArgs e)
        {
            var userToRemove = Users.SingleOrDefault(user => user.PersonalId == e.User.PersonalId);
            if (userToRemove != null)
            {
                Users.Remove(userToRemove);
            }
        }

    }
}