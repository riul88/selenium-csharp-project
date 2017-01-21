using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.Common
{
    public interface ILogger
    {
        void message(string info);
        void logException(Exception ex);
    }

    public class ConsoleLog : ILogger
    {
        public void message(string info)
        {
            Console.WriteLine(info);
        }
        public void logException(Exception ex)
        {
            var tmp = ex;
            while (tmp != null)
            {
                message(String.Format("{0} {1}", ex.Message, ex.StackTrace));
                tmp = tmp.InnerException;
                if (ex == tmp) break;
            }
        }
    }

    public class LogFactory
    {
        private static ILogger log;

        public static ILogger GetLogger()
        {
            if (log == null)
                log = new ConsoleLog();
            return log;
        }
    }
}
