using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTasks.RightScaleAutomation
{
    /// <summary>
    /// Custom build task takes a collection or a delimited string and polls the RightScale API on the indicated interval until the instance has reached the desired state
    /// </summary>
    public class WaitForInstanceState : Base.RightScaleBuildTaskBase
    {

        #region optional parameters

        /// <summary>
        /// private list of instances used to centralize the instances that are going to be
        /// </summary>
        private List<string> listOfInstances;

        /// <summary>
        /// List of RightScale Instance ID's passed in as a list from MSBuild.  InstanceList or InstanceSet must be populated.
        /// </summary>
        public ITaskItem[] InstanceList { get; set; }

        /// <summary>
        /// Delimited list of RightScale Instance ID's to query the RightScale API for its current state.  InstanceList or InstanceSet must be populated.
        /// </summary>
        public string InstanceSet { get; set; }

        /// <summary>
        /// Target State defines the state to wait for with each listed instance
        /// </summary>
        public string TargetState { get; set; }

        /// <summary>
        /// Polling interval defines the wait time (in seconds) between queries to the RightScale API for instance state
        /// </summary>
        public int PollingInterval { get; set; }

        /// <summary>
        /// Delimiter Character is the character that is used to split the InstanceSet string into its composite ID's
        /// </summary>
        public string DelimiterChar { get; set; }

        #endregion

        /// <summary>
        /// Constructor sets default values for properties that are not required inputs
        /// </summary>
        public WaitForInstanceState()
        {
            this.TargetState = "operational";
            this.listOfInstances = new List<string>();
            this.DelimiterChar = ",";
            this.PollingInterval = 30;
            this.InstanceSet = string.Empty;
        }

        /// <summary>
        /// Protected method validates that inputs are ok for execution
        /// </summary>
        protected override void ValidateInputs()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Override of Task.Execute method to run wait process for waiting for instances to reach a desired state
        /// </summary>
        /// <returns>true if successful after wait, false on error or otherwise</returns>
        public override bool Execute()
        {
            bool retVal = false;

            if (this.DelimiterChar.Length > 1)
            {
                throw new ArgumentOutOfRangeException("Delimiter Character input is not valid - its length must be 1 and its length is " + this.DelimiterChar.Length.ToString());
            }

            char splitDelim = Convert.ToChar(this.DelimiterChar);

            if (this.InstanceList.Length < 1 && this.InstanceSet.Split(splitDelim).Count<string>() < 1)
            {
                string errorMessage = "Errors found when parsing instance list: " + Environment.NewLine;
                if (this.listOfInstances.Count < 1)
                {
                    errorMessage += "InstanceList input is not valid - the number of items in the collection is 0" + Environment.NewLine;
                }
                if (this.InstanceSet.Split(splitDelim).Count<string>() < 1)
                {
                    errorMessage += "InstanceSet input is not valid - the number of items in the collection is 0" + Environment.NewLine;
                }
            }

            if (this.InstanceList.Length > 0)
            {
                foreach (var item in this.InstanceList)
                {
                    if (!this.listOfInstances.Contains(item.ItemSpec))
                    {
                        this.listOfInstances.Add(item.ItemSpec);
                    }
                }
            }

            if (this.InstanceSet.Split(splitDelim).Count<string>() > 0)
            {
                foreach (string s in this.InstanceSet.Split(splitDelim))
                {
                    if (!this.listOfInstances.Contains(s))
                    {
                        this.listOfInstances.Add(s);
                    }
                }
            }

            return retVal;
        }

    }
}
