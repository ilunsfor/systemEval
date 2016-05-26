using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace systemEval
{
    public class LogData                                         // Stores data regarding failed Schedules
    {
        public string AssetName { get; private set; }     // Asset Name
        public string SubName { get; private set; }       // Subsystem Name
        public string TaskName { get; private set; }      // Task Name
        public string TarName { get; private set; }       // Target Name
        public string ConName { get; private set; }       // Constraint Name
        public string VioName { get; private set; }       // State variable requirement number that violated a constraint
        public double Value { get; private set; }         // State variable number
        public double TimeInfo { get; private set; }      // Time information

        public LogData(string assetName, string subName, string taskName, string tarName, string conName, string vioName, double value, double timeInfo)
        {
            AssetName = assetName;
            SubName = subName;
            TaskName = taskName;
            TarName = tarName;
            ConName = conName;
            VioName = vioName;
            Value = value;
            TimeInfo = timeInfo;
        }
    }
}
