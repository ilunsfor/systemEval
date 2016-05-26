using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemEval
{
    class StateVar                                            // Stores information regarding state variables
    {
        public string StateName { get; private set; }         // State variable name
        public string SubName { get; private set; }           // Subsystem name where the state variable resides in
        public List<string> ConNames { get; private set; }    // Constraint names that applies the state variable

        public StateVar(string stateName, string subName, List<string> conNames)
        {
            StateName = stateName;
            SubName = subName;
            ConNames = conNames;
        }
    }
}
