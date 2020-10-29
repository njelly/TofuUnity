using System;

namespace Tofunaut.TofuUnity
{
    public enum LogLevel
    {
        All = int.MinValue, 
        Debug = 0,
        Info = 1,
        Warn = 2,
        Error = 3,
        None = int.MaxValue, 
    }

    public interface ILogService
    {
        void Register(ILogger logger);
        void UnRegister(ILogger logger);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}