﻿using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Entities.UserEventArgs;
using Storage.Entities.UserInfo;
using Storage.Interfaces;

namespace Storage.Service
{
    public class Slave : IUserService
    {
        public List<User> Users { get; set; }

        public Slave(IMaster master)
        {
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
            var userToRemove = Users.SingleOrDefault(user => user.PersonalId == e.User.PersonalId);
            if (userToRemove != null)
            {
                Users.Remove(userToRemove);
            }
        }

    }
}