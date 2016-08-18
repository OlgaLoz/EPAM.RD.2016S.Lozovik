using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Storage.Interfaces.Entities.ConnectionInfo;
using Storage.Interfaces.Entities.UserEventArgs;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Factory;
using Storage.Interfaces.Logger;
using Storage.Interfaces.Network;
using Storage.Interfaces.Repository;
using Storage.Interfaces.Services;

namespace Storage.Service
{
    [Serializable]
    public class Slave : MarshalByRefObject, IUserService, IListener
    {
        private readonly ILogger logger;
        private readonly IReceiver receiver;
        private readonly ReaderWriterLockSlim locker;
        private readonly List<User> users;

        public Slave(IFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            locker = new ReaderWriterLockSlim();

            var repository = factory.GetInstance<IRepository>();
            if (repository == null)
            {
                throw new NullReferenceException(nameof(repository));
            }

            logger = factory.GetInstance<ILogger>();
            if (logger == null)
            {
                throw new NullReferenceException(nameof(logger));
            }

            receiver = factory.GetInstance<IReceiver>();
            if (receiver == null)
            {
                throw new NullReferenceException(nameof(receiver));
            }

            receiver.Update += Update;
           
            locker.EnterReadLock();
            try
            {
                users = repository.Load().Users ?? new List<User>();
                logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} load info!");
            }
            finally
            {
                locker.ExitReadLock();
            }

            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} create completed!");
        }

        public void Delete(int id)
        {
            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} try to delete!");
            throw new AccessViolationException();
        }

        public int Add(User user)
        {
            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} try to add!");
            throw new AccessViolationException();
        }

        public virtual IEnumerable<int> Search(Predicate<User>[] criteria)
        {
            List<int> result;
            locker.EnterReadLock();
            try
            {
                result = users.Select(u => u.PersonalId).ToList();
                for (int i = 0; i < criteria.Length; i++)
                {
                    result = result.Intersect(users.ToList().FindAll(criteria[i]).Select(user => user.PersonalId)).ToList();
                }
            }
            finally
            {
                locker.ExitReadLock();
            }

            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} search completed ; count of users: {result.Count}!");

            return result;
        }

        public void ListenForUpdate()
        {
            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} start listen for messages!");
            receiver.Receive();
        }

        private void Update(object sender, UserEventArgs message)
        {
            locker.EnterWriteLock();
            try
            {
                switch (message.Operation)
                {
                    case Operation.Add:
                        users.Add(message.User);
                        logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} connected to add; userId={message.User.PersonalId}!");
                        break;
                    case Operation.Delete:
                        var userToRemove = users.FirstOrDefault(user => user.PersonalId == message.User.PersonalId);
                        if (userToRemove != null)
                        {
                            users.Remove(userToRemove);
                            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} connected to remove; userId={message.User.PersonalId}!");
                        }

                        break;
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }
}