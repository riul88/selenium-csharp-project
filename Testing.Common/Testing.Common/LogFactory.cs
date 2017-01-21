using System;

//Copyright (C) 2017 Raul Robledo <raul.robledo at acm.org>
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
