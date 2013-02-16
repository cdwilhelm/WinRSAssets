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
        /// ID of the server to be launched
        /// </summary>
        [Required]
        public string ServerID { get; set; }

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
            prepareCall();

            RestRequest request = new RestRequest(Method.POST);
            request.Resource = string.Format(this.MethodHref, ServerID.Trim());

            RestResponse response = (RestResponse)restClient.Post(request);

           throw new NotImplementedException();
        }
    }
}
