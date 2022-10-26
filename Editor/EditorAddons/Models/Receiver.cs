using System;

namespace BuildNotification.EditorAddons.Models
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