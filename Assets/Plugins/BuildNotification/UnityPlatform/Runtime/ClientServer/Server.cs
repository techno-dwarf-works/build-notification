using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Better.BuildNotification.UnityPlatform.Runtime.ClientServer
{
    public class Server : ConnectionBase
    {
        private readonly TcpListener _listener;

        public IPPort Current => _current;

        public Server() 
        {
            var addresses = Dns.GetHostAddresses("127.0.0.1");
            var address = addresses[0];

            var port = TcpExtensions.GetOpenPort();
            _listener = new TcpListener(address, port);
            _current = new IPPort(address.ToString(), port);
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public override void Start()
        {
            _cancellationToken?.Cancel(false);
            _cancellationToken = new CancellationTokenSource();

            _listener.Start();

            var workThread = new Thread(WorkingThread);
            workThread.IsBackground = true;
            workThread.Start();
        }

        private async void WorkingThread()
        {
            _client = await _listener.AcceptTcpClientAsync();

            var networkStream = _client.GetStream();

            StartRead(networkStream);
            StartWrite(networkStream);
        }

        public override void Stop()
        {
            base.Stop();
            _listener.Stop();
        }
    }
}