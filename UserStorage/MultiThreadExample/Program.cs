using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Configurator.Factory;
using Configurator.Logging;
using FibbonacciGenerator;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Generator;
using Storage.Interfaces.Logger;
using Storage.Interfaces.Network;
using Storage.Interfaces.Repository;
using Storage.Interfaces.Validator;
using Storage.Network;
using Storage.Repository;
using Storage.Service;
using Storage.Validator;

namespace MultiThreadExample
{
    public class Program
    {
        private static Dictionary<Type, InstanceInfo> typesSingle = new Dictionary<Type, InstanceInfo>
        {
            { typeof(IGenerator), new InstanceInfo { Type = typeof(FibonacciGenerator).AssemblyQualifiedName } },
            { typeof(IRepository), new InstanceInfo { Type = typeof(Repository).AssemblyQualifiedName } },
            { typeof(IValidator), new InstanceInfo { Type = typeof(UserValidator).AssemblyQualifiedName } },
            { typeof(ILogger), new InstanceInfo { Type = typeof(DefaultLogger).AssemblyQualifiedName } },
            { typeof(ISender), new InstanceInfo { Type = typeof(Sender).AssemblyQualifiedName, Params = new object[] { new List<IPEndPoint> { } } } },
            { typeof(IReceiver), new InstanceInfo { Type = typeof(Receiver).AssemblyQualifiedName, Params = new object[] { new IPEndPoint(IPAddress.None, 10001) } } }
        };

        public static DependencyFactory Factory => new DependencyFactory(typesSingle);

        private static void Main(string[] args)
        {
            List<Thread> threads = new List<Thread>();
            var masterService = new Master(Factory);
            masterService.Load();

            var predicate = new Predicate<User>[] { u => u.FirstName != null };

            threads.Add(new Thread(() =>
            {
                masterService.Add(new User { FirstName = "Test", LastName = "qwerty" });
            }));

            threads.Add(new Thread(() =>
            {
                masterService.Add(new User { FirstName = "Test2", LastName = "qwerty2" });
            }));

            threads.Add(new Thread(() =>
            {
                int firstId = masterService.Search(predicate).FirstOrDefault();
                masterService.Delete(firstId);
            }));

            threads.Add(new Thread(() =>
            {
                int lastId = masterService.Search(predicate).LastOrDefault();
                masterService.Delete(lastId);
            }));

            threads.Add(new Thread(() =>
            {
                masterService.Search(predicate);
            }));

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Ready!");
            Console.ReadLine();
        }
    }
}
