using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace systemEval
{
    static public class Logger   // Logs log data
    {
        public static Log Log  { get; private set; }

        public static void Output(LogData logData)
        {
            if (Log == null)
                Log = new Log();
                Log.Add(logData);
        }
    }
}
