using System;

namespace Better.BuildNotification.Platform.Tooling
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