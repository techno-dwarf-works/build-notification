using System;

namespace BuildNotification.EditorAddons.Models
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