using System;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class Receiver
    {
        public Receiver(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}