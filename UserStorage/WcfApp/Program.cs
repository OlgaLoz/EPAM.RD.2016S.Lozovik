using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace WcfApp
{
        [ServiceContract]
        public interface IHelloWorldService
        {
            [OperationContract]
            string SayHello(string name);
        }

        public class HelloWorldService : IHelloWorldService
        {
            public string SayHello(string name)
            {
                return string.Format("Hello, {0}", name);
            }
        }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8080/hello");

            using (ServiceHost host = new ServiceHost(typeof (HelloWorldService), baseAddress))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    MetadataExporter = {PolicyVersion = PolicyVersion.Policy15}
                };
                host.Description.Behaviors.Add(smb);

                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                host.Close();
            }
        }
    }
}
