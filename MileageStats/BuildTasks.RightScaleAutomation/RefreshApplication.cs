using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Net;

namespace BuildTasks.RightScaleAutomation
{
    /// <summary>
    /// Build Task to refresh the application on a running server.
    /// </summary>
    public class RefreshApplication : BuildTasks.RightScaleAutomation.Base.RightScaleBuildTaskBase
    {
        #region Input Parameters

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

        /// <summary>
        /// ID of the server being updated
        /// </summary>
        [Required]
        public string ServerID { get; set; }

        /// <summary>
        /// Name of the recipe to run
        /// </summary>
        public string RecipeName { get; set; }

        /// <summary>
        /// ID of the RightScript to run
        /// </summary>
        public string RightScriptID { get; set; }

        #endregion

        /// <summary>
        /// Protected method returns a formatted set of inputs in a collection to be put into the 
        /// </summary>
        /// <returns></returns>
        protected List<KeyValuePair<string, string>> buildInputs()
        {
            List<KeyValuePair<string, string>> parameterSet = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrWhiteSpace(this.ROSAccountID))
            {
                parameterSet.Add(new KeyValuePair<string, string>("REMOTE_STORAGE_ACCOUNT_ID_APP", "text:" + this.ROSAccountID));
            }

            if (!string.IsNullOrWhiteSpace(this.ROSAccountKey))
            {
                parameterSet.Add(new KeyValuePair<string, string>("REMOTE_STORAGE_ACCOUNT_SECRET_APP", "text:" + this.ROSAccountKey));
            }

            if (!string.IsNullOrWhiteSpace(this.ROSPackageContainer))
            {
                parameterSet.Add(new KeyValuePair<string, string>("REMOTE_STORAGE_CONTAINER_APP", "text:" + this.ROSPackageContainer));
            }

            if (!string.IsNullOrWhiteSpace(this.ROSPackageName))
            {
                parameterSet.Add(new KeyValuePair<string, string>("ZIP_FILE_NAME", "text:" + this.ROSPackageName));
            }

            if (!string.IsNullOrWhiteSpace(this.ROSProviderName))
            {
                parameterSet.Add(new KeyValuePair<string, string>("REMOTE_STORAGE_ACCOUNT_PROVIDER_APP", "text:" + this.ROSProviderName));
            }

            return parameterSet;
        }

        /// <summary>
        /// method validates inputs and should be run within the execute() method for this task 
        /// </summary>
        protected override void ValidateInputs()
        {
            string errorString = string.Empty;

            if (string.IsNullOrWhiteSpace(this.RecipeName) && string.IsNullOrWhiteSpace(this.RightScriptID))
            {
                errorString += "RecipeName and RightScriptID cannot both be null or empty strings";
            }
            if (!string.IsNullOrWhiteSpace(this.RecipeName) && !string.IsNullOrWhiteSpace(this.RightScriptID))
            {
                errorString += "You cannot specify both a RecipeName and a RightScriptID.  Please only specify one of these inputs when executing the RefreshApplication Build Task.";
            }
            if (!string.IsNullOrWhiteSpace(errorString))
            {
                throw new ArgumentException(errorString);
            }
        }

        /// <summary>
        /// Override to Execute method refreshes the application as defined by inputs
        /// </summary>
        /// <returns>True if successful, false if not</returns>
        public override bool Execute()
        {
            bool retVal = false;
            ValidateInputs();

            RightScale.netClient.Server deployServer = RightScale.netClient.Server.show(this.ServerID);

            List<KeyValuePair<string, string>> inputs = buildInputs();

            List<RightScale.netClient.Task> tasks = deployServer.current_instance.multi_run_executable(deployServer.current_instance.cloud.ID, true, inputs, this.RecipeName, this.RightScriptID);

            if (tasks.Count > 0)
            {
                retVal = true;
            }

            return retVal;
        }
    }
}
