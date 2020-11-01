namespace Tofunaut.TofuUnity.Interfaces
{
    public enum LogLevel
    {
        All = int.MinValue,
        Info = 0,
        Warn = 1,
        Error = 2,
        Exception = 3,
        None = int.MinValue,
    }

    public interface ILogger
    {

        void Info(string s);
        void Warn(string s);
        void Error(string s);
    }
}