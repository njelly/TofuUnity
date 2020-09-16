namespace Tofunaut.TofuUnity
{
    public interface ITofuLogger
    {
        void Info(string s);
        void Warn(string s);
        void Error(string s);
    }
}