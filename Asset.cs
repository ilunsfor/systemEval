using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemEval
{
    class Asset                                                    // Asset class contains asset IDs and their respective list of subsystems
    {
        public string AssetName { get; private set; }              // Asset Name
        public List<string> SubNames { get; private set; }         // List of subsystems

        public Asset(string assetName, List<string> subNames)
        {
            AssetName = assetName;
            SubNames = subNames;
        }
    }
}
