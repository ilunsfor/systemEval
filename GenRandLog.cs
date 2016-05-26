using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemEval
{
    class GenRandLogData
    {

        public static LogData Output(List<Constraint> constraintsList, List<StateVar> stateVarList, List<SubsystemClass> subsysList, int index)
        {
            // Generating random log data

            List<string> stateVarSubList = new List<string>();

            Random rnd = new Random(index);

            string assetName = null;
            string subName = null;
            string taskName = "Task" + rnd.Next(1, 6);
            string tarName = "GroundStation" + rnd.Next(1, 6);
            string conName = null;
            string failName = null;
            double value = 0;
            double timeInfo = rnd.Next(1, 100);
            double w = 0;

            LogData logData;                 // New log data input class
            
            bool passedSub = rnd.Next(100) < 75 ? true : false;

            if (passedSub == false)
            {
                foreach (SubsystemClass item in subsysList)
                {
                    if (Convert.ToDouble(item.SubName.Replace("SubsystemNode", "")) > w)
                    {
                        w = Convert.ToDouble(item.SubName.Replace("SubsystemNode", ""));
                    }
                }
                subName = "SubsystemNode" + rnd.Next(1, Convert.ToInt32(w) + 1);
                assetName = subsysList.Find(y => y.SubName == subName).AssetName;
                logData = new LogData(assetName, subName, taskName, tarName, conName, failName, value, timeInfo);
            }

           else
	       {
                conName = constraintsList[rnd.Next(constraintsList.Count)].ConName;

                foreach (StateVar item in stateVarList)
                {
                    if (item.ConNames.Contains(conName))
                    {
                        failName = item.StateName;
                        stateVarSubList.Add(failName);
                    }
                }

                if (stateVarSubList.Count > 0)
                {
                    failName = stateVarSubList[rnd.Next(stateVarSubList.Count)];
                }

                foreach (SubsystemClass item in subsysList)
                {
                    if (item.StateVars.Contains(failName))
                    {
                        subName = item.SubName;
                        assetName = item.AssetName;
                    }
                }

                double div = 100;
                int val = Convert.ToInt32(constraintsList.Find(x => x.ConName == conName).Value * 100);
                value = rnd.Next(val, 100) / div;
                logData = new LogData(assetName, subName, taskName, tarName, conName, failName, value, timeInfo);
           }
           return logData;
           // Done generating random log data
        }
    }
}
