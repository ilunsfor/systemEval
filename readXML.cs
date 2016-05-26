using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace systemEval
{
    class readXML
    {

        public static List<Constraint> XMLCon(OpenFileDialog ofd)
        {
            XDocument xDoc = XDocument.Load(ofd.FileName);                       // Opens XML file within framework and loads chosen XML file

            //LINQ query to read the xml file details
            var modelDetails = from readInfo in xDoc.Descendants("MODEL")        // Find the Model and read-in its descendant elements
                               select new
                               {
                                   conInfo = readInfo.Descendants("CONSTRAINT").DescendantsAndSelf(),       // Find all constraints and read-in their descendant elements
                               };

            // Asset details retrieval
            Constraint constraint = new Constraint(null,0,null);                 // New constraint class
            List<Constraint> constraintsList = new List<Constraint>();           // New list of information for each constraint

            int i = 0;                                                           // Declare counter
            string ConName = null;
            List<string> StateVars = new List<string>();
            double Value = 0;

            // Using foreach loop to retrieve all details by iterating through each element with the MODEL element
            foreach (var modelElem in modelDetails)
            {
                // Using foreach loop to retrieve each constraint's respective details by iterating through each element with the CONSTRAINT element
                foreach (XElement ce in modelElem.conInfo)
                {
                    double conValue = Convert.ToDouble(ce.Attribute("value") != null ? ce.Attribute("value").Value : "0");                // Extract constraint value data
                    string stateID = ce.Attribute("key") != null ? ce.Attribute("key").Value : String.Empty;                              // Extract state variable data
                    string conName = ce.Attribute("constraintName") != null ? ce.Attribute("constraintName").Value : String.Empty;        // Extract constraint name data

                    if (conValue > 0)                                                            // If the element is the constraint value
                    {
                        if (i > 0)                                                               // If a new constraint
                        {
                            constraint = new Constraint(ConName, Value, StateVars);              // Populate Constraint class
                            StateVars = new List<string>();
                            constraintsList.Add(constraint);                                     // Add constraint to constraints list
                        }
                        Value = conValue;                                                        // Record constraint value in subsystem constraint class
                    }

                    if (conName != String.Empty)                                                 // If the element is a constraint name
                    {
                        ConName = conName;                                                       // Record constraint name in the constraint class
                    }

                    if (stateID != String.Empty)                                                 // If the element is a state variable
                    {
                        StateVars.Add(stateID);                                                  // Add state ID associated with constraint
                        i++;
                    }
                }
                constraint = new Constraint(ConName, Value, StateVars);                          // Populate Constraint class
                constraintsList.Add(constraint);                                                 // Add constraint to constraints list
            }
            return constraintsList;
        }

        public static List<SubsystemClass> XMLSub(OpenFileDialog ofd)            // Read in subsystem information within XML file
        {
            XDocument xDoc = XDocument.Load(ofd.FileName);                       // Opens XML file within framework and loads chosen XML file

            //LINQ query to read the xml file details
            var modelDetails = from readInfo in xDoc.Descendants("MODEL")        // Find the Model and read-in its descendant elements
                               select new
                               {
                                   subInfo = readInfo.Descendants("SUBSYSTEM").DescendantsAndSelf(),        // Find all subsystems and read-in their descendant elements
                                   assetInfo = readInfo.DescendantsAndSelf("ASSET").DescendantsAndSelf()    // Find all assets and read-in in their descendant elements
                               };

            // Asset details retrieval
            Asset asset = new Asset(null, null);                                    // New asset class
            SubsystemClass subSys = new SubsystemClass(null,null,null);        // New subsystem class

            List<Asset> assetsList = new List<Asset>();                             // New list of information for each asset
            List<SubsystemClass> subsysList = new List<SubsystemClass>();           // New list of information for each subsystem

            int i = 0;                              // Declare counter
            string SubName = null;
            string StateVar = null;
            string AssetName = null;
            List<string> StateVars = new List<string>();
            List<string> SubNames = new List<string>();

            // Using foreach loop to retrieve all details by iterating through each element with the MODEL element
            foreach (var modelElem in modelDetails)
            {
                // Using foreach loop to retrieve each asset's details by iterating through each element with the ASSET element                  
                foreach (XElement ae in modelElem.assetInfo)
                {
                    string assetName = ae.Attribute("AssetName") != null ? ae.Attribute("AssetName").Value : String.Empty;             // Extract asset name data
                    string subName = ae.Attribute("SubsystemName") != null ? ae.Attribute("SubsystemName").Value : String.Empty;       // Extract subsystem name data

                    if (assetName != String.Empty)                                                                                     // If the element is an asset name
                    {
                        if (i > 0)                                                                                                     // If there is a new asset
                        {
                            asset = new Asset(AssetName, SubNames);
                            assetsList.Add(asset);                                                                                     // Add asset to asset list
                            asset = new Asset(AssetName,SubNames);                                                                     // Populate asset class
                            SubNames = new List<string>();
                        }
                        AssetName = assetName;                                                                                         // Record asset name
                    }

                    if (subName != String.Empty)                                                                                       // If the element is a subsystem name
                    {
                        SubNames.Add(subName);                                                                                         // Record subsystem name in subsystem names list
                        i++;
                    }
                }
                asset = new Asset(AssetName, SubNames);                                                                                // Populate asset class
                assetsList.Add(asset);                                                                                                 // Add asset to asset list

                i = 0;

                // Using foreach loop to retrieve each subsystem and dependecies respective details by iterating through each element with the SUBSYSTEM element
                foreach (XElement se in modelElem.subInfo)
                {
                    string stateName = se.Attribute("key") != null ? se.Attribute("key").Value : String.Empty;                         // Extract state variable data
                    string subName = se.Attribute("SubsystemName") != null ? se.Attribute("SubsystemName").Value : String.Empty;       // Extract subsystem name data

                    if (subName != String.Empty)                                                            // If the element is a subsystem name
                    {
                        if (i > 0)                                                                          // If there is a new subsystem
                        {
                            foreach (Asset item in assetsList)                                              // Check each asset
                            {
                                if (item.SubNames.Contains(SubName))                                        // Find subsystem name within asset
                                {
                                    AssetName = item.AssetName;                                             // Record asset name for subsystem class
                                }
                            }
                            subSys = new SubsystemClass(SubName, AssetName, StateVars);                     // Populate subsystem class
                            subsysList.Add(subSys);                                                         // Add subsystem class to subsystem list
                            StateVars = new List<string>();
                        }
                        SubName = subName;                                                                  // Record subsystem name for subsystem class
                        i++;
                    }


                    if (stateName != String.Empty)                                                          // If the element is a state variable
                    {
                        StateVar = stateName;                                                    
                        StateVars.Add(stateName);                                                           // Record state variable requirement for subsystem class
                    }
                }
                subSys = new SubsystemClass(SubName, AssetName, StateVars);                                 // Populate subsystem class
                subsysList.Add(subSys);                                                                     // Add subsystem class to subsystem list
            }
            return subsysList;
        }

        public static List<StateVar> XMLState(List<Constraint> constraintsList, List<SubsystemClass> subsysList)   // Using previously read-in XML data, create a state variable list
        {
            StateVar stateVar = new StateVar(null, null, null);
            List<StateVar> stateVarList = new List<StateVar>();                                                    // New list of information for each state variable

            string StateName = null;
            string SubName = null;
            string ConName = null;
            List<string> ConNames = new List<string>();

            List<Constraint> conNames = new List<Constraint>();

            foreach (SubsystemClass item in subsysList)
            {
                foreach (string item2 in item.StateVars)
                {
                    StateName = item2;

                    foreach (Constraint item6 in constraintsList)
                    {
                        foreach (string item7 in item6.StateVars)
                        {
                            if (item2 == item7)
                            {
                                ConName = item6.ConName;
                                ConNames.Add(ConName);
                            }
                        }
                    }

                    SubName = item.SubName;
                    stateVar = new StateVar(StateName, SubName, ConNames);
                    stateVarList.Add(stateVar);
                    ConNames = new List<string>();
                }            
            }            
            return stateVarList;
        }
    }
}
