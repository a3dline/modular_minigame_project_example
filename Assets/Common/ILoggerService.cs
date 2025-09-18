using System;

namespace Game.Common
{
    public interface ILoggerService
    {
        void Log(string tag, string message);
        void LogError(string tag, string message);
        void LogException(Exception exception);
    }
}