using System.Net.Sockets;
using System.Text;

namespace Better.BuildNotification.UnityPlatform.Runtime.ClientServer
{
    public class Client : ConnectionBase
    {
        public Client(IPPort port)
        {
            _client = new TcpClient(port.IP, port.Port);
        }
        
        public override void Start()
        {
            var stream = _client.GetStream();

            StartRead(stream);
            StartWrite(stream);
        }
    }
}