using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTasks.RightScaleAutomation
{
    /// <summary>
    /// MSBuild task executes a POST request for the Rightscale OAuth2 API authentication URL to get an OAuth Access Token
    /// </summary>
    public class GetRSOAuthToken : Task
    {
        #region Required Input Parameters

        /// <summary>
        /// Refresh token for the account that is being programmatically accessed.  This information is found on the RightScale dashboard and information on the process can be found on the RightScale Support site: http://support.rightscale.com/12-Guides/03-RightScale_API/OAuth
        /// </summary>
        [Required]
        public string OAuthRefreshToken { get; set; }

        #endregion

        #region Output Variables

        /// <summary>
        /// Output parameter for MSBuild to access the OAuth token created for subsequent calls to the RightScale API
        /// </summary>
        [Output]
        public string OAuthAccessToken { get; set; }

        #endregion

        #region Optional Input Parameters

        /// <summary>
        /// OAuth URL for accessing the API's token generation process
        /// </summary>
        public string OAuthURL { get; set; }

        /// <summary>
        /// API Version to be used when identifying the RightScale API Version
        /// </summary>
        public string APIVersionHeaderValue { get; set; }

        #endregion

        /// <summary>
        /// Name for HTTP header which identifies the version of the RightScale API being accessed
        /// </summary>
        private const string APIVerHeaderName = "X_API_VERSION";

        /// <summary>
        /// Private constructor defines default values for non-required inputs
        /// </summary>
        public GetRSOAuthToken ()
	    {
            Log.LogMessage("GetRSOAuthToken MSBuild task is initiating");
            this.OAuthURL = @"https://my.rightscale.com/api/oauth2"; //oauth url for API 1.5
            this.APIVersionHeaderValue = "1.5"; //API ver 1.5 identified 
	    }

        /// <summary>
        /// Override to Task.Execute method containing the custom process for gathering the OAuth key from the RightScale system
        /// </summary>
        /// <returns>true if an OAuth Access Token was retrieved, false if not</returns>
        public override bool Execute()
        {
            Log.LogMessage("  GetRSOAuthToken.Execute - beginning at " + DateTime.Now.ToString());
            bool retVal = false;

            WebRequest request = WebRequest.Create(OAuthURL);

            request.Method = "POST";
            request.Headers.Add(APIVerHeaderName, this.APIVersionHeaderValue);

            string requestString = string.Format("grant_type=refresh_token;refresh_token={0};", this.OAuthRefreshToken);
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(requestString);

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postData, 0, postData.Length);
            }

            Log.LogMessage("    GetRSOAuthToken.Execute - Getting Response from API endpoint");

            WebResponse response = request.GetResponse();
            string streamContents = string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream))
                {
                    Log.LogMessage("    GetRSOAuthToken.Execute - reading data from response stream");
                    streamContents = streamReader.ReadToEnd();
                }
            }

            var OAuthResponseObject = Base.DynamicJsonSerializer.Deserialize<dynamic>(streamContents);

            this.OAuthAccessToken = OAuthResponseObject.access_token.ToString();
            Log.LogMessage("    GetRSOAuthToken.Execute - OAuth bearer token output to variable for MSBuild");
            if (!string.IsNullOrWhiteSpace(this.OAuthAccessToken))
            {
                Log.LogMessage("    GetRSOAuthToken.Execute - Success!");
                retVal = true;
            }
            Log.LogMessage("  GetRSOAuthToken.Execute - complete at " + DateTime.Now.ToString());
            return retVal;
        }
    }
}
