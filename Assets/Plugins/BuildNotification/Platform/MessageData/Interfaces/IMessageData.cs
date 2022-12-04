using Better.BuildNotification.Platform.MessageData.Models;

namespace Better.BuildNotification.Platform.MessageData.Interfaces
{
    public interface IMessageData
    {
        public BufferSummary Body { get; }
        public string Guid { get; }
    }
}