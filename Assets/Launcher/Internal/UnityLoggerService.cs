using System;
using UnityEngine;

namespace Game.Launcher
{
    internal class UnityLoggerService : ILoggerService
    {
        public void Log(string tag, string message)
        {
            Debug.Log($"[{tag}] {message}");
        }

        public void LogError(string tag, string message)
        {
            Debug.LogError($"[{tag}] {message}");
        }

        public void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}