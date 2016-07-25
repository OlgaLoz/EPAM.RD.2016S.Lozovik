using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Storage.Interfaces.Entities.ConnectionInfo;
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
        private readonly IEnumerable<IPEndPoint> slaves; 

        public List<User> Users { get; set; }
        
        public Master(IValidator validator, IRepository repository, IGenerator idGenerator, IEnumerable<IPEndPoint> slaves)
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
            Users = new List<User>();
            this.validator = validator;
            this.repository = repository;
            this.idGenerator = idGenerator;
            this.slaves = slaves;
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

            NotifySlaves(new Message {Operation = Operation.Add, User = user});
            return userId;
        }

        public void Delete(int id)
        {
            var userToRemove = Users.SingleOrDefault(user => user.PersonalId == id);
            if (userToRemove != null)
            {
                Users.Remove(userToRemove);
                NotifySlaves(new Message { Operation = Operation.Delete, User = userToRemove });
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

        async private void NotifySlaves<T>(T message)
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
                    using (var stream = tcpClient.GetStream())
                    {
                         stream.Write(data, 0, data.Length);
                    }
                }
            }           
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
            NotifySlaves(Users);
        }
    }
}