using System;

namespace BuildNotification.Runtime.Tooling.Models
{
    [Serializable]
    public class Topic
    {
        public Topic(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}