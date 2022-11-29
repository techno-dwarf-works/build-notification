namespace Better.BuildNotification.Runtime.MessageDataModes
{
    public interface IMessageData
    {
        public BufferSummary Body { get; }
        public string Guid { get; }
    }
}