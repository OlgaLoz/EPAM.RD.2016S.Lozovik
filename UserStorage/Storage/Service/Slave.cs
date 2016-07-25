﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Storage.Interfaces.Entities.ConnectionInfo;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Interfaces;
using Storage.Logging;

namespace Storage.Service
{
    [Serializable]
    public class Slave : MarshalByRefObject, ISlave
    {
        private readonly TcpListener tcpListener;
        private readonly Logger logger;
        private readonly ReaderWriterLockSlim locker;

        private delegate void Callback<in T>(T parametr);

        private List<User> users;  

        public Slave(IPEndPoint connectionInfo, IRepository repository)
        {
            if (connectionInfo == null)
            {
                throw new ArgumentNullException(nameof(connectionInfo));
            }
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            tcpListener = new TcpListener(connectionInfo);
            tcpListener.Start();
            logger = Logger.Instance;
            locker = new ReaderWriterLockSlim();

            locker.EnterReadLock();
            try
            {
                users = repository.Load().Users ?? new List<User>();
            }
            finally
            {
                locker.ExitReadLock();
            }

            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} create!");
        }

        public void Delete(int id)
        {
            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} delete!");
            throw new AccessViolationException();
        }

        public int Add(User user)
        {
            logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} add!");
            throw new AccessViolationException();
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

        public async void InitializeCollection()
        {
            try
            {
                await ProcessTcp<List<User>>(SetCollection);
            }
            catch 
            {
                tcpListener.Stop();
            }
        }

       public async void ListenForUpdate()
        {
           try
           {
               while (true)
               {
                   await ProcessTcp<Message>(Update);
               }
           }
           finally 
           {
               tcpListener.Stop();
           }
        }

        private async Task ProcessTcp<T>(Callback<T> callback)
        {
            int dataSize = 1024;
            var formatter = new BinaryFormatter();
            byte[] data = new byte[dataSize];
            using (var tcpClient = await tcpListener.AcceptTcpClientAsync())
            {

                using (var stream = tcpClient.GetStream())
                using (var mStream = new MemoryStream())
                {
                    int count;
                    do
                    {
                        count = await stream.ReadAsync(data, 0, data.Length);
                        mStream.Write(data, 0, count);
                    } while (count >= dataSize);
                    mStream.Position = 0;
                    try
                    {
                        logger.Log(TraceEventType.Information, $"{AppDomain.CurrentDomain.FriendlyName} connected to {callback.Method.Name}!");

                        var message = (T)formatter.Deserialize(mStream);
                        callback(message);
                    }
                    catch 
                    {
                        throw new InvalidDataException("Unable to deserialize.");
                    }                  
                }
            }
        }

        private void SetCollection(List<User> passedUsers )
        {
            locker.EnterWriteLock();
            try
            {
                users = passedUsers ?? new List<User>();
            }
            finally 
            {
                locker.ExitWriteLock();
            }
        }

        private void Update(Message message)
        {
            locker.EnterWriteLock();
            try
            {
                switch (message.Operation)
                {
                    case Operation.Add:
                        users.Add(message.User);
                        break;
                    case Operation.Delete:
                        var userToRemove = users.FirstOrDefault(user => user.PersonalId == message.User.PersonalId);
                        if (userToRemove != null)
                        {
                            users.Remove(userToRemove);
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