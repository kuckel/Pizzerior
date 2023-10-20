using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzerior.Services
{
    public interface ILoggerService
    {
        void LogError(string message, Exception ex);
        void LogInfo(string message);
    }
}
