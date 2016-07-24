using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Storage.Interfaces.Entities.ConnectionInfo;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Interfaces;

namespace Storage.Service
{
    [Serializable]
    public class Slave : MarshalByRefObject, ISlave
    {
        private readonly TcpListener tcpListener ;
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

        async public void ListenForUpdate()
        {
            try
            {
                while (true)
                {
                    using (var tcpClient = await tcpListener.AcceptTcpClientAsync())
                    {
                        var formatter = new BinaryFormatter();
                        Console.WriteLine($"{AppDomain.CurrentDomain.FriendlyName} connected!");

                        byte[] data = new byte[1024];
                        using (var stream = tcpClient.GetStream())
                        using (var mStream = new MemoryStream())
                        {
                            while (stream.DataAvailable)
                            {
                                int count = await stream.ReadAsync(data, 0, data.Length);
                                await mStream.WriteAsync(data, 0, count);
                            }
                            mStream.Position = 0;
                            var message = (Message)formatter.Deserialize(mStream);
                            Update(message);
                        }
                    }
                }
            }
            finally
            {
                tcpListener.Stop();
            }
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