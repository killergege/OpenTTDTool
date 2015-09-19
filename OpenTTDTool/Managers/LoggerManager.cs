using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Managers
{
    public enum LoggerType { Application, ParsedValues }
    public static class LoggerManager
    {
        public static ILog GetLogger(LoggerType loggerType, Type type)
        {
            string loggerName = String.Empty;
            switch (loggerType)
            {
                case LoggerType.ParsedValues:
                    loggerName = "ValuesDebug";
                    break;
                case LoggerType.Application:
                    loggerName = "ApplicationLog";
                    break;
            }
            return LogManager.GetLogger(loggerName) ?? GetLogger(type);
        }

        public static ILog GetLogger(Type type)
        {
            return LoggerManager.GetLogger(LoggerType.Application, type);
        }
    }
}
