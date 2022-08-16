using System;

namespace BuildNotification.EditorAddons.Models
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