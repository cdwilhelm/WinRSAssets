using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTasks.Utility
{
    /// <summary>
    /// Build Name Generator MSBuild task is meant to serve the purpose of creating a unique build name that's consistent and identifiable across all environments.  This process should be used when generating file package names for builds.
    /// </summary>
    public class BuildNameGenerator : Task
    {
        /// <summary>
        /// Optional input parameter to define the build name prefix for the build name kicked out of this process
        /// </summary>
        public string BuildNamePrefix { get; set; }

        /// <summary>
        /// Optional input parameter to define the separator used between the BuildNamePrefix and the date identifier.  This must be a charater that is valid within the context of a directory name.  
        /// </summary>
        public string BuildNameSeparator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BuildDateStringFormatPattern { get; set; }

        /// <summary>
        /// Output parameter passed back to MSBuild containing the formatted build name based on the optional inputs or defaults.  
        /// </summary>
        [Output]
        public string BuildNameOutput { get; set; }

        /// <summary>
        /// Constructor initialized optional inputs that are required with default values
        /// </summary>
        public BuildNameGenerator()
        {
            this.BuildNamePrefix = "Build";
            this.BuildNameSeparator = "_";
            this.BuildDateStringFormatPattern = "yyyyMMddhhmmss";
        }

        /// <summary>
        /// Override of Task.Execute which generates the custom build name itself
        /// </summary>
        /// <returns>true if name is built successfully, false if not</returns>
        public override bool Execute()
        {
            bool retVal = false;

            //TODO: set up date format
            string dateStamp = DateTime.Now.ToString(this.BuildDateStringFormatPattern);

            this.BuildNameOutput = this.BuildNamePrefix + this.BuildNameSeparator + dateStamp;

            if(!string.IsNullOrWhiteSpace(this.BuildNameOutput))
            {
                retVal = true;
            }

            return retVal;
        }
    }
}
