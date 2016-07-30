using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Storage.Interfaces.Entities.ConnectionInfo;
using Storage.Interfaces.Entities.ServiceState;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Factory;
using Storage.Interfaces.Generator;
using Storage.Interfaces.Logger;
using Storage.Interfaces.Network;
using Storage.Interfaces.Repository;
using Storage.Interfaces.Services;
using Storage.Interfaces.Validator;

namespace Storage.Service
{
    public class Master : MarshalByRefObject, IUserService, ILoader
    {
        private readonly IValidator validator;
        private readonly IRepository repository;
        private readonly IGenerator idGenerator;
        private readonly ILogger logger;
        private readonly ISender sender;
        private readonly ReaderWriterLockSlim locker;

        private List<User> users;

        public Master(IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            users = new List<User>();
            validator = factory.GetInstance<IValidator>();
            repository = factory.GetInstance<IRepository>(); 
            idGenerator = factory.GetInstance<IGenerator>();
            logger = factory.GetInstance<ILogger>();
            sender = factory.GetInstance<ISender>();
            
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (idGenerator == null)
            {
                throw new ArgumentNullException(nameof(idGenerator));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            locker = new ReaderWriterLockSlim();
            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} create!");
        }
        
        public int Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!validator.IsValid(user))
            {
                throw new ArgumentException("User is not valid!");
            }

            locker.EnterWriteLock();
            try
            {
                idGenerator.GetNextId();
                user.PersonalId = idGenerator.CurrentId;
                users.Add(user);
            }
            finally
            {
                locker.ExitWriteLock();
            }

            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} add!");

            sender.Send(new Message { Operation = Operation.Add, User = user });

            return user.PersonalId;
        }

        public void Delete(int id)
        {
            locker.EnterWriteLock();
            try
            {
                var userToRemove = users.SingleOrDefault(user => user.PersonalId == id);
                if (userToRemove != null)
                {
                    users.Remove(userToRemove);
                    sender.Send(new Message { Operation = Operation.Delete, User = userToRemove });
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }

            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} delete!");
        }

        public virtual IEnumerable<int> Search(Predicate<User>[] criteria)
        {
            var result = new List<int>();
            locker.EnterReadLock();
            try
            {
                for (int i = 0; i < criteria.Length; i++)
                {
                    result.AddRange(users.ToList().FindAll(criteria[i]).Select(user => user.PersonalId));
                }
            }
            finally 
            {
                locker.ExitReadLock();
            }

            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} search!");

            return result;
        }

        public void Save()
        {
            locker.EnterWriteLock();
            try
            {
                repository.Save(new ServiceState { Users = users.ToList(), CurrentId = idGenerator.CurrentId });
            }
            finally 
            {
                locker.ExitWriteLock();
            }

            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} save!");
        }

        public void Load()
        {
            locker.EnterWriteLock();
            try
            {
                var state = repository.Load();
                users = state.Users ?? new List<User>();
            }
            finally
            {
                locker.ExitWriteLock();
            }

            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} load!");
        }

        public List<User> GetAll()
        {
            return users;
        } 
    }
}