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
        #region Optional Input Parameters
        // These are mostly optional because they can be set at the server or deployment level within the RightScale platform--it may not be necessary to pass these in all the time, but it's certainly available if need be.
   
        /// <summary>
        /// Name of the Web package container to be used when deploying via MSDeploy on the server from Windows Azure Storage - used to point the RghtScale ServerTemplate to the proper place to download the packaged MSBuild artifact
        /// Values can either be in the form of "text:raw_value" or a pointer to an existing RightScale Credential "cred:existing_cred"
        /// </summary>
        public string ROSPackageContainer { get; set; }

        /// <summary>
        /// Name of the Web package blob to be used when deploying via MSDeploy on the server from Windows Azure Storage - used to point the RghtScale ServerTemplate to the proper place to download the packaged MSBuild artifact
        /// Values can either be in the form of "text:raw_value" or a pointer to an existing RightScale Credential "cred:existing_cred"
        /// </summary>
        public string ROSPackageName { get; set; }

        /// <summary>
        /// Account ID for accssing a specific Windows Azure Storage Account - used to point the RghtScale ServerTemplate to the proper place to download the packaged MSBuild artifact
        /// Values can either be in the form of "text:raw_value" or a pointer to an existing RightScale Credential "cred:existing_cred"
        /// </summary>
        public string ROSAccountID { get; set; }

        /// <summary>
        /// Account Key for accessing a specific Windows Azure Storage Account - used to point the RghtScale ServerTemplate to the proper place to download the packaged MSBuild artifact
        /// Values can either be in the form of "text:raw_value" or a pointer to an existing RightScale Credential "cred:existing_cred"
        /// </summary>
        public string ROSAccountKey { get; set; }

        /// <summary>
        /// Provider name for storage account to be passed to the RightScript downloading and deploying the MSDeploy package 
        /// </summary>
        public string ROSProviderName { get; set; }
 
        #endregion

        #region Internal objects and static values

        /// <summary>
        /// Collection holds the names of all valid ROS types per the implementation within the PowerShell scripts provided by RS Engineering
        /// </summary>
        string[] validROSTypes = new string[]{"Amazon_S3", "Rackspace_Cloud_Files", "Rackspace_Cloud_Files_UK", "Windows_Azure_Storage", "Softlayer_Object_Storage_Dallas", "Softlayer_Object_Storage_Singapore", "Softlayer_Object_Storage_Amsterdam"};

        #endregion

        /// <summary>
        /// Class constructor populates optional but required parameters within this class.
        /// </summary>
        public LaunchRSServer()
        {
            this.MethodHref = "/servers/{0}/launch";
            this.ROSProviderName = "Windows_Azure_Storage";  //TODO: validate that this is the right string for the RightScript accessing WAZ Storage
        }

        /// <summary>
        /// Override to the Task.Execute class which contains the custom implementation to launch a server via the RightScale API
        /// </summary>
        /// <returns>true if the process was successful, false if not</returns>
        public override bool Execute()
        {
            bool retVal = false;
            ValidateInputs();
            prepareCall();
            
            RestRequest request = new RestRequest(Method.POST);
            request.Resource = string.Format(this.MethodHref, ServerID.ToString());

            RestResponse response = (RestResponse)restClient.Post(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created) //looking for a 201 completed
            {
                retVal = true;
            }

            Log.LogMessage("  LaunchRSServer.Execute - complete at " + DateTime.Now.ToString());
            return retVal;
        }

        protected Dictionary<string, Dictionary<string, string>> buildInuputs()
        {
            Dictionary<string, Dictionary<string, string>> retVal = new Dictionary<string, Dictionary<string, string>>();



            return retVal;
        }

        /// <summary>
        /// Implementing Input validation process for this MSBuild task to ensure ServerID is populated
        /// </summary>
        protected override void ValidateInputs()
        {
            string errorMessage = string.Empty;
            if(!this.validROSTypes.Contains<string>(this.ROSProviderName))
            {
                errorMessage += @"The Remote Object Storage Type selected (" + this.ROSProviderName + @") does not match one of the supported ROS Providers";
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
                Log.LogMessage(@"  GetInstanceInfo.ValidateInputs - inputs validated - ServerID (" + this.ServerID.ToString() + @")");
            }
        }

        /// <summary>
        /// Private method builds out post data for call to API including inputs
        /// </summary>
        /// <returns>byte array for data to be pushed to the RestClient object</returns>
        private byte[] buildPostData(Dictionary<string, Dictionary<string, string>> parameterCollection)
        {
            byte[] retVal;
            string postData = string.Empty;

            foreach (string outerCollectionKey in parameterCollection.Keys)
            {
                foreach (string innerCollectionKey in parameterCollection[outerCollectionKey].Keys)
                {
                    postData += outerCollectionKey + "[" + innerCollectionKey + @"]=""" + parameterCollection[outerCollectionKey][innerCollectionKey] + @"""&";
                }
            }

            postData = postData.TrimEnd('&');

            return System.Text.Encoding.UTF8.GetBytes(postData);
        }
    }
}

