using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web.Script.Serialization;
using Storage.Interfaces.Network;

namespace Storage.Network
{
    public class Sender : ISender
    {
        private readonly IEnumerable<IPEndPoint> receivers;

        public Sender(IEnumerable<IPEndPoint> receivers)
        {
            if (receivers == null)
            {
                throw new ArgumentNullException(nameof(receivers));
            }

            this.receivers = receivers;
        }
        
        public async void Send<T>(T message)
        {
            var formatter = new JavaScriptSerializer();
            var data = Encoding.UTF8.GetBytes(formatter.Serialize(message));

            foreach (var ipEndPoint in receivers)
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    await tcpClient.ConnectAsync(ipEndPoint.Address, ipEndPoint.Port);
                    using (var stream = tcpClient.GetStream())
                    {
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
            }
        }
    }
}