namespace Tofunaut.TofuUnity
{
    public interface ILogger
    {
        void Info(string s);
        void Warn(string s);
        void Error(string s);
    }
}