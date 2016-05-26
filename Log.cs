using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemEval
{
    public class Log                                              // List of log data
    {
        public List<LogData> Data {get; private set;}      // Log data list

        public Log()
        {
            Data = new List<LogData>();
        }

        public void Add(LogData logData)
        {
            Data.Add(logData);
        }
    }
}
