using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Interfaces.Entities.ServiceState;
using Storage.Interfaces.Entities.UserEventArgs;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Interfaces;

namespace Storage.Service
{

    public class Master : MarshalByRefObject, IMaster
    {
        private readonly IValidator validator;
        private readonly IRepository repository;
        private readonly IGenerator idGenerator;

        public List<User> Users { get; set; }
        
        public Master(IValidator validator, IRepository repository, IGenerator idGenerator)
        {
            Users = new List<User>();
            this.validator = validator;
            this.repository = repository;
            this.idGenerator = idGenerator;

            if (this.validator == null)
            {
                throw new ArgumentNullException(nameof(this.validator));
            }

            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (idGenerator == null)
            {
                throw new ArgumentNullException(nameof(idGenerator));
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

        public int Add(User user )
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (idGenerator == null)
            {
                throw new ArgumentNullException(nameof(idGenerator));
            }

            if (!validator.IsValid(user))
            {
                throw new ArgumentException("User is not valid!");
            }

            int userId = idGenerator.GetNextId();
            user.PersonalId = userId;
            Users.Add(user);

            OnAddUser(new UserEventArgs {User = user});
            return userId;
        }

        public void Delete(int id)
        {
            var userToRemove = Users.SingleOrDefault(user => user.PersonalId == id);
            if (userToRemove != null)
            {
                Users.Remove(userToRemove);
                OnDeleteUser(new UserEventArgs { User = userToRemove });
            }
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

        public void Save()
        {
            repository.Save(new ServiceState { Users = Users.ToList(), CurrentId = idGenerator.CurrentId });
        }

        public void Load()
        {
            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            var state = repository.Load();
            Users = state.Users;
        }
    }
}