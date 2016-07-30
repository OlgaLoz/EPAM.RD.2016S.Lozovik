using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Storage.Interfaces.Entities.ConnectionInfo;
using Storage.Interfaces.Entities.UserEventArgs;
using Storage.Interfaces.Network;
using Storage.Service;

namespace Storage.Network
{
    public class Receiver : IReceiver
    {
        private readonly TcpListener tcpListener;

        public Receiver(IPEndPoint connectionInfo)
        {
            if (connectionInfo == null)
            {
                throw new ArgumentNullException(nameof(connectionInfo));
            }

            tcpListener = new TcpListener(connectionInfo);
        }

        public EventHandler<UserEventArgs> Update { get; set; } = delegate { };
        
        public async void Receive()
        {         
            tcpListener.Start();
            int dataSize = 1024;
            var formatter = new JavaScriptSerializer();
            byte[] data = new byte[dataSize];
            try
            {
                while (true)
                {
                    using (var tcpClient = await tcpListener.AcceptTcpClientAsync())
                    {
                        string input = string.Empty;
                        using (var stream = tcpClient.GetStream())
                        {
                            int count;
                            do
                            {
                                count = await stream.ReadAsync(data, 0, data.Length);
                                input += Encoding.UTF8.GetString(data, 0, count);
                            }
                            while (count >= dataSize);

                            try
                            {
                                var message = formatter.Deserialize<Message>(input);
                                OnUpdate(message.ToUserEventArgs());
                            }
                            catch
                            {
                                throw new InvalidDataException("Unable to deserialize.");
                            }
                        }
                    }
                }
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        protected virtual void OnUpdate(UserEventArgs e)
        {
            EventHandler<UserEventArgs> temp = Update;
            temp?.Invoke(this, e);
        }
    }
}