using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using RightScale.netClient;

namespace BuildTasks.RightScaleAutomation
{
    /// <summary>
    /// MSBuild task executes a POST request for the Rightscale OAuth2 API authentication URL to get an OAuth Access Token
    /// </summary>
    public class AuthenticateRSAPI : BuildTasks.RightScaleAutomation.Base.RightScaleBuildTaskBase
    {
        #region Required Input Parameters

        /// <summary>
        /// Refresh token for the account that is being programmatically accessed.  This information is found on the RightScale dashboard and information on the process can be found on the RightScale Support site: http://support.rightscale.com/12-Guides/03-RightScale_API/OAuth
        /// </summary>
        [Required]
        public string OAuthRefreshToken { get; set; }

        #endregion

        /// <summary>
        /// Override to Task.Execute method containing the custom process for gathering the OAuth key from the RightScale system
        /// </summary>
        /// <returns>true if an OAuth Access Token was retrieved, false if not</returns>
        public override bool Execute()
        {
            Log.LogMessage("  AuthenticateRSAPI.Execute - beginning at " + DateTime.Now.ToString());
            bool retVal = false;

            if (base.authenticateClient())
            {
                retVal = RightScale.netClient.Core.APIClient.Instance.Authenticate(this.OAuthRefreshToken).Result;
            }
            else
            {
                throw new RightScale.netClient.RightScaleAPIException("Failed to authenticate to RightScale API");
            }

            Log.LogMessage("  AuthenticateRSAPI.Execute - complete at " + DateTime.Now.ToString());
            return retVal;
        }		
    }
}
