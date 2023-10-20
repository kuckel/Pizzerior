#nullable disable 
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzerior.Services
{
    public partial class LoggerService : ILoggerService
    {
        private readonly ILog log;

        public LoggerService()
        {
            log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public void LogInfo(string message)
        {
            log.Info(message);
        }

        public void LogError(string message, Exception ex)
        {
            log.Error(message, ex);
        }
    }
}
