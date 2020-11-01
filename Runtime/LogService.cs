using System.Collections.Generic;
using Tofunaut.TofuUnity.Interfaces;

namespace Tofunaut.TofuUnity
{
    public class LogService
    {
        private HashSet<ILogger> _loggers;

        public LogService()
        {
            _loggers = new HashSet<ILogger>();
        }

        public void Register(ILogger logger)
        {
            _loggers.Add(logger);
        }

        public void UnRegister(ILogger logger)
        {
            _loggers.Remove(logger);
        }

        public void Info(string s)
        {
            foreach (var logger in _loggers)
                logger.Info(s);
        }

        public void Warn(string s)
        {
            foreach (var logger in _loggers)
                logger.Warn(s);
        }

        public void Error(string s)
        {
            foreach (var logger in _loggers)
                logger.Error(s);
        }
    }
}
