using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemEval
{
    class SubsystemClass                                              // Subsystem class contains the subsystem name, asset name, and relative state variables.
    {
        public string SubName { get; private set; }                   // Subsystem name
        public string AssetName { get; private set; }                 // Asset name where subsystem resides
        public List<string> StateVars { get; private set; }           // State variables that lie within the subsystem

        public SubsystemClass(string subName, string assetName, List<string> stateVars)
        {
            SubName = subName;
            AssetName = assetName;
            StateVars = stateVars;
        }
    }
}
