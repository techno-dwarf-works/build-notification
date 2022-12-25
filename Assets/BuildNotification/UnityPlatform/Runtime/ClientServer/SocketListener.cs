using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.Runtime.ClientServer
{
    public class SocketListener : BaseSocket
    {
        private readonly IPPort _port;
        public event Action Connected;

        public SocketListener()
        {
            _port = new IPPort(GetLocalIPAddress().ToString(), 11000);
        }

        public IPPort Port => _port;

        private IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private protected override async void InternalStart()
        {
            // Get Host IP Address that is used to establish a connection
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1
            // If a host has multiple addresses, you will get a list of addresses
            var ipAddress = IPAddress.Parse(Port.IP);
            var localEndPoint = new IPEndPoint(ipAddress, Port.Port);
            try
            {
                // Create a Socket that will use Tcp protocol
                var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.
                // We will listen 10 requests at a time
                listener.Listen(10);


                Debug.Log("Waiting for a connection...");
                _handler = await listener.AcceptAsync();
                
                Debug.Log("Connected...");
                Connected?.Invoke();
                StartReadThread();
                StartWriteThread();
                
            }
            catch (Exception e)
            {
                Debug.LogFormat(e.ToString());
                Stop();
            }
        }
    }
}