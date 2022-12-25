using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Better.BuildNotification.UnityPlatform.Runtime.ClientServer
{
    public abstract class ConnectionBase
    {
        private Thread _readThread;
        private Thread _writeThread;
        private protected CancellationTokenSource _cancellationToken;
        private protected TcpClient _client;
        private protected IPPort _current;

        private protected SemaphoreSlim _semaphore;
        private readonly Queue<byte[]> _writeQueue;

        public event Action<byte[]> ReceivedBytes;

        public ConnectionBase()
        {
            _cancellationToken = new CancellationTokenSource();
            _writeQueue = new Queue<byte[]>();
        }

        public async void ScheduleWrite(byte[] toSend)
        {
            await _semaphore.WaitAsync();
            _writeQueue.Enqueue(toSend);
            _semaphore.Release();
        }

        private protected void StartRead(NetworkStream networkStream)
        {
            _readThread = new Thread(() => Read(networkStream, _cancellationToken.Token));
            _readThread.IsBackground = true;
            _readThread.Start();
        }
        
        private protected void StartWrite(NetworkStream networkStream)
        {
            _writeThread = new Thread(() => Write(networkStream, _cancellationToken.Token));
            _writeThread.IsBackground = true;
            _writeThread.Start();
        }
        
        private async void Read(NetworkStream networkStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await _semaphore.WaitAsync(token);
                var bytes = await networkStream.ReadToEndAsync();
                ReceivedBytes?.Invoke(bytes);
                _semaphore.Release();
            }
        }

        private async void Write(NetworkStream networkStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await _semaphore.WaitAsync(token);
                if (_writeQueue.Count <= 0)
                {
                    _semaphore.Release();
                    Thread.Sleep(500);
                    continue;
                }

                var toSend = _writeQueue.Dequeue();
                await networkStream.WriteAsync(toSend, 0, toSend.Length, token);

                _semaphore.Release();
            }
        }

        public abstract void Start();

        public virtual void Stop()
        {
            _client.Close();
            _cancellationToken?.Cancel(false);
        }
    }
}