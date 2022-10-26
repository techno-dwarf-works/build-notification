using System.Collections.Generic;

namespace BuildNotification.EditorAddons.DatabaseModule.RequestWrappers
{
    public class ListWrapper<T> : Wrapper
    {
        public List<T> Data { get; }
        public ListWrapper(List<T> data)
        {
            Data = data;
        }
    }
}