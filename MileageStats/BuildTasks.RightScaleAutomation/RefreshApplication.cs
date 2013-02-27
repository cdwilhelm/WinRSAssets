using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace BuildTasks.RightScaleAutomation
{
    public class RefreshApplication : Base.RightScaleAPIBase
    {
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
        /// Protected method returns a formatted set of inputs in a collection to be put into the 
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, object> buildInputs()
        {
            Dictionary<string, object> parameterSet = new Dictionary<string, object>();

            parameterSet.Add("inputs[]", new Dictionary<string, string>());

            if (!string.IsNullOrWhiteSpace(this.ROSAccountID))
            {
                ((Dictionary<string, string>)parameterSet["inputs[]"]).Add("REMOTE_STORAGE_ACCOUNT_ID_APP", "text:" + this.ROSAccountID);
            }

            if (!string.IsNullOrWhiteSpace(this.ROSAccountKey))
            {
                ((Dictionary<string, string>)parameterSet["inputs[]"]).Add("REMOTE_STORAGE_ACCOUNT_SECRET_APP", "text:" + this.ROSAccountKey);
            }

            if (!string.IsNullOrWhiteSpace(this.ROSPackageContainer))
            {
                ((Dictionary<string, string>)parameterSet["inputs[]"]).Add("REMOTE_STORAGE_CONTAINER_APP", "text:" + this.ROSPackageContainer);
            }

            if (!string.IsNullOrWhiteSpace(this.ROSPackageName))
            {
                ((Dictionary<string, string>)parameterSet["inputs[]"]).Add("ZIP_FILE_NAME", "text:" + this.ROSPackageName);
            }

            if (!string.IsNullOrWhiteSpace(this.ROSProviderName))
            {
                ((Dictionary<string, string>)parameterSet["inputs[]"]).Add("REMOTE_STORAGE_ACCOUNT_PROVIDER_APP", "text:" + this.ROSProviderName);
            }

            parameterSet.Add("right_script_href", "/api/right_scripts/425343001");

            return parameterSet;
        }

        public RefreshApplication()
        {
            this.BaseUri = "https://my.rightscale.com/api/clouds/{0}/instances/{1}/run_executable";
        }

        protected override void ValidateInputs()
        {
            throw new NotImplementedException();
        }

        public override bool Execute()
        {
            bool retVal = false;

            byte[] postData = buildPostData(buildInputs());
            Log.LogMessage(string.Format(this.BaseUri, this.CloudID, this.ServerID));
            WebRequest request = WebRequest.Create(string.Format(this.BaseUri, this.CloudID, this.ServerID));
            request.Method = "POST";

            request.Headers.Add("X_API_VERSION", this.ApiHeaderValue);
            request.Headers.Add("Authorization", "Bearer " + this.OAuth2Token);

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postData, 0, postData.Length);
            }

            WebResponse response = request.GetResponse();
            string streamContents = string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader streamreader = new System.IO.StreamReader(responseStream))
                {
                    streamContents = streamreader.ReadToEnd();
                }
            }

            

            return retVal;
        }
    }
}
