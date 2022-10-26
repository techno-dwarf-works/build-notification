using BuildNotification.Runtime.MessageDataModes.Models;

namespace BuildNotification.Runtime.MessageDataModes.Interfaces
{
    public interface IMessageData
    {
        public BufferSummary Body { get; }
        public string Guid { get; }
    }
}