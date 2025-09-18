using System;

namespace Game.Launcher
{
    internal interface ILoggerService
    {
        void Log(string tag, string message);
        void LogError(string tag, string message);
        void LogException(Exception exception);
    }
}