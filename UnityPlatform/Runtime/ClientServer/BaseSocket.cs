using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Better.BuildNotification.UnityPlatform.Runtime.ClientServer
{
    public abstract class BaseSocket
    {
        private class SendData
        {
            public SendData(byte[] bytes, Action onDone)
            {
                _bytes = bytes;
                _onDone = onDone;
            }
            
            private Action _onDone;
            private byte[] _bytes;

            public byte[] Bytes => _bytes;

            public void Done()
            {
                _onDone?.Invoke();
            }
        }
        
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly CancellationTokenSource _cancellationToken;
        private readonly Queue<SendData> _sendQueue;

        private protected Socket _handler;
        private Thread _thread;
        private Thread _readThread;
        private Thread _writeThread;

        public event Action<byte[]> ReceivedBytes;

        public BaseSocket()
        {
            _sendQueue = new Queue<SendData>();
            _cancellationToken = new CancellationTokenSource();
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public async void ScheduleWrite(byte[] bytes, Action onDone = null)
        {
            await _semaphoreSlim.WaitAsync();
            _sendQueue.Enqueue(new SendData(bytes, onDone));
            _semaphoreSlim.Release();
        }

        public virtual void Start()
        {
            _thread = new Thread(InternalStart);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private protected abstract void InternalStart();

        private protected void StartReadThread()
        {
            _readThread = new Thread(ReadLoop);
            _readThread.IsBackground = true;
            _readThread.Start();
        }

        private protected void StartWriteThread()
        {
            _writeThread = new Thread(WriteLoop);
            _writeThread.IsBackground = true;
            _writeThread.Start();
        }

        public virtual void Stop()
        {
            if (_thread.IsAlive) _thread?.Abort();

            if (_readThread != null && _readThread.IsAlive) _readThread.Abort();
            if (_writeThread != null && _writeThread.IsAlive) _writeThread.Abort();

            _handler?.Shutdown(SocketShutdown.Both);
            _handler?.Close();
            _cancellationToken?.Cancel(false);
        }

        private async void ReadLoop()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                await _semaphoreSlim.WaitAsync(_cancellationToken.Token);
                if (!_handler.Connected || _handler.Available <= 0)
                {
                    _semaphoreSlim.Release();
                    Thread.Sleep(1000);
                    continue;
                }

                var bytes = new byte[_handler.SendBufferSize];
                var bytesRec = _handler.Receive(bytes);
                if (bytesRec <= 0)
                {
                    _semaphoreSlim.Release();
                    Thread.Sleep(1000);
                    continue;
                }

                var read = new byte[bytesRec];
                Array.Copy(bytes, 0, read, 0, bytesRec);
                ReceivedBytes?.Invoke(read);
                
                _semaphoreSlim.Release();
            }
        }

        private async void WriteLoop()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                await _semaphoreSlim.WaitAsync(_cancellationToken.Token);
                if (_sendQueue.Count <= 0 || !_handler.Connected)
                {
                    _semaphoreSlim.Release();
                    Thread.Sleep(1000);
                    continue;
                }

                var send = _sendQueue.Dequeue();
                var sendBytes = send.Bytes;
                _handler.SendBufferSize = sendBytes.Length;
                _handler.Send(sendBytes);
                send.Done();
                _semaphoreSlim.Release();
            }
        }
    }
}