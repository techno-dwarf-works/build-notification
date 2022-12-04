using System;

namespace Better.BuildNotification.Platform.Tooling
{
    [Serializable]
    public class Condition
    {
        public Condition(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}