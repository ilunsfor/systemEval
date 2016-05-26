using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace systemEval
{
    class Program
    {

        [STAThread]

        public static void Main(string[] args)
        {
            // Subsystem evaluation:

            // Multiple functions: Logger, RunTime mode, and
            // PostProcess mode.

            // Inputs are read from .xml files.
            // All model information reside within .xml files.

            // Outputs are comma-delimited .csv files

            // Read in .xml

            List<Constraint> constraintsList = new List<Constraint>();               // New lsit of constraints
            List<SubsystemClass> subsysList = new List<SubsystemClass>();            // New list of subsystems
            List<StateVar> stateVarList = new List<StateVar>();                      // New list of state variables

            OpenFileDialog ofd = new OpenFileDialog();                               // Dialog prompt to user to choose xml file location
            ofd.Filter = "XML| *.xml";                                               // Only allows xml files to be inputted
            if (ofd.ShowDialog() == DialogResult.OK)                                 // If the correct file was selected
            {
                constraintsList = readXML.XMLCon(ofd);                               // List of constraints
                subsysList = readXML.XMLSub(ofd);                                    // List of subsystems
                stateVarList = readXML.XMLState(constraintsList, subsysList);        // List of state variables
            }

            // Done reading in .xml           

            // Read-in Log

            // Implement checking for passed schedules and failed schedules

            LogData logData = new LogData(null, null, null, null, null, null, 0, 0);
            //List<LogData> logDataList = new List<LogData>();                        // New List of Log Data
            List<LogAnalyzer> LogAnalytics = new List<LogAnalyzer>();                 // New List of Log Analytics
            //var logger = new Logger();                                                // New Log class


            // Create specified subsystems and constraints for reordering
            List<SubsystemClass> subsystemList = new List<SubsystemClass>();
            SubsystemClass subsystem = new SubsystemClass("SubsystemNode2", null, null);
            subsystemList.Add(subsystem);
            subsystem = new SubsystemClass("SubsystemNode3", null, null);
            subsystemList.Add(subsystem);
            subsystem = new SubsystemClass("SubsystemNode4", null, null);
            subsystemList.Add(subsystem);

            List<Constraint> constraintList = new List<Constraint>();
            Constraint constraint = new Constraint("Constraint1", 0, null);
            constraintList.Add(constraint);
            constraint = new Constraint("Constraint2", 0, null);
            constraintList.Add(constraint);

            List<Constraint> conOrder = new List<Constraint>();
            List<SubsystemClass> subOrder = new List<SubsystemClass>();
            // Done creating specified subsystems and constraints for reordering

            // Specify the number of failed schedules
            // Create data for each failed schedule
            // Output runTime data for every 25 failed schedules
            // Output postProcess data for every 100 failed schedules

            Console.WriteLine("Input number of failed schedules for randomized log data");      // Prompt user to input number of iterations
            string input = Console.ReadLine();                                                  // Read number of iterations
            double iters = Convert.ToDouble(input);                                             // Specify number of failed schedules
            int i = 0;

            Console.WriteLine("Press 1 and then press enter for RunTime mode, otherwise enter a different value "
                               + "and press enter to begin PostProcess mode.");                                     // Prompt user for mode specification
            int mode;                                                                                               // Declare mode

            if (int.TryParse(Console.ReadLine(), out mode))
            {
                for (i = 1; i <= iters; i++)
                {
                    logData = GenRandLogData.Output(constraintsList, stateVarList, subsysList, i);
                    Logger.Output(logData);

                    // Calculate and record percentage of constraint failure, their respective subsystems, etc.

                    if (mode == 1 && i % 25 == 0)      // Check if in RunTime mode && if the iteration is the 25th
                    {
                        LogAnalytics = LogAnalyzer.Analyze(LogAnalytics, Logger.Log, constraintsList, subsysList, stateVarList);
                        conOrder = LogAnalyzer.reorderCons(LogAnalytics, constraintList);
                        subOrder = LogAnalyzer.reorderSubs(LogAnalytics, subsystemList);
                        Logger.Log.Data.Clear();
                    }

                    else if (i % 10 == 0)             // PostProcess mode
                    {
                        LogAnalytics = LogAnalyzer.Analyze(LogAnalytics, Logger.Log, constraintsList, subsysList, stateVarList);
                        LogAnalyzer.PostProcess(LogAnalytics, subsysList);
                        conOrder = LogAnalyzer.reorderCons(LogAnalytics, constraintList);
                        subOrder = LogAnalyzer.reorderSubs(LogAnalytics, subsystemList);
                        Logger.Log.Data.Clear();
                    }
                }
            }
            // Done logging and calculating subsystem failures and constraint violations with relative data
        }
    }
}