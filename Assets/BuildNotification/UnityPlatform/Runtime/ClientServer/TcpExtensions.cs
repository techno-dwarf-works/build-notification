using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Better.BuildNotification.UnityPlatform.Runtime.ClientServer
{
    public static class TcpExtensions
    {
        public static async Task<byte[]> ReadToEndAsync(this NetworkStream networkStream)
        {
            var bigbuffer = new List<byte>();

            var tempbuffer = new byte[254];

            while (await networkStream.ReadAsync(tempbuffer, 0, tempbuffer.Length) > 0)
            {
                bigbuffer.AddRange(tempbuffer);
            }

            var completedbuffer = new byte[bigbuffer.Count];

            bigbuffer.CopyTo(completedbuffer);

            return completedbuffer;
        }
        
        
        private const int PortStartIndex = 1000;
        private const ushort PortEndIndex = UInt16.MaxValue;
        public static int GetOpenPort()
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpEndPoints = properties.GetActiveTcpListeners();

            var usedPorts = tcpEndPoints.Select(p => p.Port).ToList();
            var unusedPort = 0;

            for (var port = PortStartIndex; port < PortEndIndex; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }

            return unusedPort;
        }
    }
}