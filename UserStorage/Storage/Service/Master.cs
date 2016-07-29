using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Storage.Interfaces.Entities.ConnectionInfo;
using Storage.Interfaces.Entities.ServiceState;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Interfaces;

namespace Storage.Service
{
    public class Master : MarshalByRefObject, IMaster
    {
        private readonly IValidator validator;
        private readonly IRepository repository;
        private readonly IGenerator idGenerator;
        private readonly IEnumerable<IPEndPoint> slaves;
        private readonly ILogger logger;
        private readonly ReaderWriterLockSlim locker;

        private List<User> users;

        public Master(IFactory factory, IEnumerable<IPEndPoint> slaves)
        {
            validator = factory.GetInstance<IValidator>();
            repository = factory.GetInstance<IRepository>(); 
            idGenerator = factory.GetInstance<IGenerator>();
            logger = factory.GetInstance<ILogger>();

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

            if (slaves == null)
            {
                throw new ArgumentNullException(nameof(slaves));
            }

            this.slaves = slaves;

            locker = new ReaderWriterLockSlim();
            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} create!");
        }
        
        public Master(IValidator validator, IRepository repository, IGenerator idGenerator, IEnumerable<IPEndPoint> slaves, ILogger logger)
        {
            if (validator == null)
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

            if (slaves == null)
            {
                throw new ArgumentNullException(nameof(slaves));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.validator = validator;
            this.repository = repository;
            this.idGenerator = idGenerator;
            this.slaves = slaves;
            this.logger = logger;
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

            NotifySlaves(new Message { Operation = Operation.Add, User = user });
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
                    NotifySlaves(new Message { Operation = Operation.Delete, User = userToRemove });
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

        private async void NotifySlaves<T>(T message)
        {
            var formatter = new BinaryFormatter();
            byte[] data;
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, message);
                data = stream.ToArray();
            }

            foreach (var ipEndPoint in slaves)
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    await tcpClient.ConnectAsync(ipEndPoint.Address, ipEndPoint.Port);
                    logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} notify {ipEndPoint.Address}-{ipEndPoint.Port}!");

                    using (var stream = tcpClient.GetStream())
                    {
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
            }
        }
    }
}