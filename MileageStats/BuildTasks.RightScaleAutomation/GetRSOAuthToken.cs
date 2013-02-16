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
        /// <summary>
        /// Refresh token for the account that is being programmatically accessed.  This information is found on the RightScale dashboard and information on the process can be found on the RightScale Support site: http://support.rightscale.com/12-Guides/03-RightScale_API/OAuth
        /// </summary>
        [Required]
        public string OAuthRefreshToken { get; set; }

        /// <summary>
        /// Output parameter for MSBuild to access the OAuth token created for subsequent calls to the RightScale API
        /// </summary>
        [Output]
        public string OAuthAccessToken { get; set; }

        private const string OAuthURL = @"https://my.rightscale.com/api/oauth2";
        private const string APIVerHeaderName = "X_API_VERSION";
        private const string APIVerHeaderValue = "1.5";

        /// <summary>
        /// Override to Task.Execute method containing the custom process for gathering the OAuth key from the RightScale system
        /// </summary>
        /// <returns>true if an OAuth Access Token was retrieved, false if not</returns>
        public override bool Execute()
        {
            bool retVal = false;

            WebRequest request = WebRequest.Create(OAuthURL);

            request.Method = "POST";
            request.Headers.Add(APIVerHeaderName, APIVerHeaderValue);

            string requestString = string.Format("grant_type=refresh_token;refresh_token={0};", this.OAuthRefreshToken);
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(requestString);

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postData, 0, postData.Length);
            }

            WebResponse response = request.GetResponse();
            string streamContents = string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream))
                {
                    streamContents = streamReader.ReadToEnd();
                }
            }

            var OAuthResponseObject = Base.DynamicJsonDeserializer.Deserialize<dynamic>(streamContents);

            this.OAuthAccessToken = OAuthResponseObject.access_token.ToString();

            if (!string.IsNullOrWhiteSpace(this.OAuthAccessToken))
            {
                retVal = true;
            }

            return retVal;
        }
    }
}
