using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.Runtime.ClientServer
{
// Client app is the one sending messages to a Server/listener.
// Both listener and client can send messages back and forth once a
// communication is established.
    public class SocketClient : BaseSocket
    {
        private IPPort _port;

        public SocketClient(IPPort ipPort)
        {
            _port = ipPort;
        }

        private Thread _thread;

        private protected override async void InternalStart()
        {
            try
            {
                // Connect to a Remote server
                // Get Host IP Address that is used to establish a connection
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1
                // If a host has multiple addresses, you will get a list of addresses
                var ipAddress = IPAddress.Parse(_port.IP);
                IPEndPoint remoteEp = new IPEndPoint(ipAddress, _port.Port);

                // Create a TCP/IP  socket.
                _handler = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    // Connect to Remote EndPoint
                    await _handler.ConnectAsync(remoteEp);
                    StartReadThread();
                    StartWriteThread();
                }
                catch (ArgumentNullException ane)
                {
                    Debug.LogFormat("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Debug.LogFormat("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Debug.LogFormat("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.LogFormat(e.ToString());
            }
        }
    }
}