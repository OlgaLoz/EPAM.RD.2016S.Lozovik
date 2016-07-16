using System;
using System.Collections.Generic;
using System.Linq;
using Storage.UserInfo;

namespace Storage.Strategy
{
    public class Master : Strategy
    {
        private readonly IEnumerable<Func<User, bool>> validators;

        public Master(IEnumerable<Func<User, bool>> validators)
        {
            this.Users = new List<User>();
            this.validators = validators;
            if (validators == null)
            {
                throw new ArgumentNullException(nameof(validators));
            }
        }

        public event EventHandler<UserEventArgs> AddUser = delegate {};
        
        public event EventHandler<UserEventArgs> DeleteUser = delegate { };

        protected virtual void OnAddUser(UserEventArgs e)
        {
            EventHandler<UserEventArgs> temp = AddUser;
            temp?.Invoke(this, e);
        }

        protected virtual void OnDeleteUser(UserEventArgs e)
        {
            EventHandler<UserEventArgs> temp = DeleteUser;
            temp?.Invoke(this, e);
        }

        public override int Add(User user, IEnumerator<int> idGenerator )
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (idGenerator == null)
            {
                throw new ArgumentNullException(nameof(idGenerator));
            }

            if (!validators.All(validator => validator(user)))
            {
                throw new ArgumentException("User is not valid!");
            }

            idGenerator.MoveNext();
            int userId = idGenerator.Current;
            user.PersonalId = userId;
            Users.Add(user);

            OnAddUser(new UserEventArgs {User = user});
            return userId;
        }

        public override void Delete(int id)
        {
            var userToRemove = Users.SingleOrDefault(user => user.PersonalId == id);
            if (userToRemove != null)
            {
                Users.Remove(userToRemove);
                OnDeleteUser(new UserEventArgs { User = userToRemove });
            }
        }
    }
}