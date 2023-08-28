using System;

namespace Feature.Abstractions
{
    public interface ILog
    {
        event Action<string> LogChanged;
        void Information(string info);
        void Error(string error);
    }
}
