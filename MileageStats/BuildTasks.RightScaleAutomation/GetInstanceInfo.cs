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
    /// MSBuild task to gather general information about a running instance within the RightScale system on a given cloud
    /// </summary>
    public class GetInstanceInfo : Base.RightScaleAPIBase
    {
        #region Output Variables

        /// <summary>
        /// ID of the server to be queried
        /// </summary>
        [Required]
        public string ServerID { get; set; }

        /// <summary>
        /// Collection of Private IP addresses assigned to the given instance
        /// </summary>
        [Output]
        public List<string> PrivateIPAddresses { get; set; }

        /// <summary>
        /// Collection of Public IP addresses assigned to the given instance
        /// </summary>
        [Output]
        public List<string> PublicIPAddresses { get; set; }

        /// <summary>
        /// Timestamp indicating when the instance was created
        /// </summary>
        [Output]
        public string CreatedAt { get; set; }
        
        /// <summary>
        /// Timestamp indicating the last update's date and time
        /// </summary>
        [Output]
        public string UpdatedAt { get; set; }

        /// <summary>
        /// Friendly name of server within the RightScale platform
        /// </summary>
        [Output]
        public string Name { get; set; }

        /// <summary>
        /// Resource UID for the instance within the RightScale platform
        /// </summary>
        [Output]
        public string ResourceUid { get; set; }

        /// <summary>
        /// Current state of the given server
        /// </summary>
        [Output]
        public string State { get; set; }

        #endregion

        /// <summary>
        /// Constructor initializes defaults variables that are not required inputs
        /// </summary>
        public GetInstanceInfo()
        {
            this.MethodHref = @"/api/clouds/{0}/instances/{1}";
        }

        /// <summary>
        /// Override to Task.Execute to run custom process for gathering data on a server instance running within RightScale
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            bool retVal = false;
            Log.LogMessage("  GetInstanceInfo.Execute - beginning at " + DateTime.Now.ToString());
            ValidateInputs();
            prepareRestCall();
            RestRequest request = new RestRequest(Method.GET);
            request.Resource = string.Format(this.MethodHref, this.CloudID.ToString(), this.ServerID.ToString());
            Log.LogMessage("  GetInstanceInfo.Execute - making call to restClient");
            RestResponse response = (RestResponse)restClient.Post(request);

            dynamic responseObject = Base.DynamicJsonSerializer.Deserialize<dynamic>(response.Content);

            //TODO: finish implementing execute and parse data back out to output variables as necessary

            Log.LogMessage("  GetInstanceInfo.Execute - complete at " + DateTime.Now.ToString());
            return retVal;
        }

        /// <summary>
        /// Implementing Input validation process for this MSBuild task to ensure CloudID and ServerID are populated
        /// </summary>
        protected override void ValidateInputs()
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.CloudID))
            {
                errorMessage += "CloudID is not defined and is required. ";
            }
            if (string.IsNullOrWhiteSpace(this.ServerID))
            {
                errorMessage += "ServerID is not defined and is required. ";
            }
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException("Errors were found when attempting to validate inputs for the GetInstanceInfo MSBuild Task: " + errorMessage);
            }
            else
            {
                Log.LogMessage(@"  GetInstanceInfo.ValidateInputs - inputs validated - CloudID (" + this.CloudID.ToString() + @"), ServerID (" + this.ServerID.ToString() + @")"); 
            }
        }
    }
}
