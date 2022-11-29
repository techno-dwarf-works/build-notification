namespace Better.BuildNotification.Runtime.MessageData
{
    public interface IMessageData
    {
        public BufferSummary Body { get; }
        public string Guid { get; }
    }
}