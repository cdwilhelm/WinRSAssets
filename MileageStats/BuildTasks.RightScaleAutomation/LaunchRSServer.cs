using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using RestSharp;

namespace BuildTasks.RightScaleAutomation
{
    /// <summary>
    /// MSBuild task to launch a RightScale server based on a server ID from the RS Dashboard.
    /// </summary>
    public class LaunchRSServer : Base.RightScaleAPIBase
    {

        /// <summary>
        /// Name of the Web package container to be used when deploying via MSDeploy on the server from Windows Azure Storage
        /// </summary>
        public string WebPackageContainer { get; set; }

        /// <summary>
        /// Name of the Web package blob to be used when deploying via MSDeploy on the server from Windows Azure Storage
        /// </summary>
        public string WebPackageName { get; set; }

        /// <summary>
        /// Class constructor populates optional but required parameters within this class.
        /// </summary>
        public LaunchRSServer()
        {
            this.MethodHref = "/servers/{0}/launch";
        }

        /// <summary>
        /// Override to the Task.Execute class which contains the custom implementation to launch a server via the RightScale API
        /// </summary>
        /// <returns>true if the process was successful, false if not</returns>
        public override bool Execute()
        {
            Log.LogMessage("  GetRSOAuthToken.Execute - beginning at " + DateTime.Now.ToString());
            bool retVal = false;
            ValidateInputs();
            prepareCall();
            
            RestRequest request = new RestRequest(Method.POST);
            request.Resource = string.Format(this.MethodHref, ServerID.ToString());

            RestResponse response = (RestResponse)restClient.Post(request);


            Log.LogMessage("  LaunchRSServer.Execute - complete at " + DateTime.Now.ToString());
            return retVal;
        }

        /// <summary>
        /// Implementing Input validation process for this MSBuild task to ensure ServerID is populated
        /// </summary>
        protected override void ValidateInputs()
        {
            string errorMessage = string.Empty;
            if (this.ServerID == null)
            {
                errorMessage += "ServerID is not defined and is required. ";
            }
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException("Errors were found when attempting to validate inputs for the GetInstanceInfo MSBuild Task: " + errorMessage);
            }
            else
            {
                Log.LogMessage(@"  GetInstanceInfo.ValidateInputs - inputs validated - ServerID (" + this.ServerID.ToString() + @")");
            }
        }
    }
}
