using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace systemEval
{
    class Constraint                                          // Constraint class contains the constraint ID, the corresponding subsystems, and the constraint value
    {
        public string ConName { get; private set; }           // Constraint Name
        public double Value { get; private set; }             // Constraint values
        public List<string> StateVars { get; private set; }   // List of state variable requirements constrained

        public Constraint(string conName, double value, List<string> stateVars)
        {
            ConName = conName;
            Value = value;
            StateVars = stateVars;
        }
    }
}
