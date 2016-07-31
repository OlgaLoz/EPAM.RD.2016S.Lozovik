using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Storage.Interfaces.ServiceInfo;
using Storage.Interfaces.Services;

namespace WcfLibrary
{
    public class WcfHelper : MarshalByRefObject, IWcfHelper
    {
        private readonly IUserService userService;
        private ServiceHost serviceHost;

        public WcfHelper(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            this.userService = userService;
        }

        public void Open(string host)
        {
            (userService as IListener)?.ListenForUpdate();
            (userService as ILoader)?.Load();

            Uri baseAddress = new Uri(host);
            WcfService service = new WcfService(userService);
            serviceHost = new ServiceHost(service, baseAddress);

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                MetadataExporter = { PolicyVersion = PolicyVersion.Policy15 }
            };

            serviceHost.Description.Behaviors.Add(smb);

            serviceHost.Open();
        }

        public void Close()
        {
            (userService as ILoader)?.Save();
            serviceHost.Close();
            ((IDisposable)serviceHost).Dispose();
        }
    }
}