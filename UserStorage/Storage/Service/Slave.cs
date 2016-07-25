using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Storage.Interfaces.Entities.ConnectionInfo;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Interfaces;

namespace Storage.Service
{
    [Serializable]
    public class Slave : MarshalByRefObject, ISlave
    {
        private readonly TcpListener tcpListener ;

        private delegate void Callback<in T>(T parametr);
        public List<User> Users { get; set; }

        

        public Slave(/*IMaster master*/IPEndPoint connectionInfo)
        {
            if (connectionInfo == null)
            {
                throw new ArgumentNullException(nameof(connectionInfo));
            }

            tcpListener = new TcpListener(connectionInfo);
            tcpListener.Start();

            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
          /*  Users = master.Users;*/
          Users = new List<User>();
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

        async public void InitializeCollection()
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

       async public void ListenForUpdate()
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

        async private Task ProcessTcp<T>(Callback<T> callback)
        {
            int dataSize = 1024;
            var formatter = new BinaryFormatter();
            byte[] data = new byte[dataSize];
            using (var tcpClient = await tcpListener.AcceptTcpClientAsync())
            {
                Console.WriteLine($"{AppDomain.CurrentDomain.FriendlyName} connected to {callback.Method.Name}!");

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

        private void SetCollection(List<User> users )
        {
            Users = users;
        }

        private void Update(Message message)
        {
            switch (message.Operation)
            {
                case Operation.Add:
                     Users.Add(message.User);
                break;
                case Operation.Delete:
                    var userToRemove = Users.FirstOrDefault(user => user.PersonalId == message.User.PersonalId);
                    if (userToRemove != null)
                    {
                        Users.Remove(userToRemove);
                    }
                break;
            }        
        }
    }
}