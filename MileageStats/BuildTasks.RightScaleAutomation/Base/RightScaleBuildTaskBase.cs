using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightScale.netClient;

namespace BuildTasks.RightScaleAutomation.Base
{
    public class RightScaleBuildTaskBase : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Refresh token for the account that is being programmatically accessed.  This information is found on the RightScale dashboard and information on the process can be found on the RightScale Support site: http://support.rightscale.com/12-Guides/03-RightScale_API/OAuth
        /// </summary>
        public string OAuthRefreshToken { get; set; }

        public string APIUserName { get; set; }

        public string APIPassword { get; set; }

        public string APIAccountNo { get; set; }

        /// <summary>
        /// Protected method handles authentication calls to RSAPI and validates parameters to ensure that we're able to make a valid call
        /// </summary>
        /// <returns>True if authenticated, false if not</returns>
        protected bool authenticateClient()
        {
            if (string.IsNullOrWhiteSpace(RightScale.netClient.Core.APIClient.Instance.oauthRefreshToken) && !string.IsNullOrWhiteSpace(this.OAuthRefreshToken))
            {
                RightScale.netClient.Core.APIClient.Instance.oauthRefreshToken = this.OAuthRefreshToken;
                return RightScale.netClient.Core.APIClient.Instance.Authenticate().Result; //new auth with oauth
            }
            else if (string.IsNullOrWhiteSpace(RightScale.netClient.Core.APIClient.Instance.userName) && string.IsNullOrWhiteSpace(RightScale.netClient.Core.APIClient.Instance.password) && string.IsNullOrWhiteSpace(RightScale.netClient.Core.APIClient.Instance.accountId) && !string.IsNullOrWhiteSpace(this.APIAccountNo) && !string.IsNullOrWhiteSpace(this.APIUserName) && !string.IsNullOrWhiteSpace(this.APIPassword))
            {
                RightScale.netClient.Core.APIClient.Instance.userName = this.APIUserName;
                RightScale.netClient.Core.APIClient.Instance.password = this.APIPassword;
                RightScale.netClient.Core.APIClient.Instance.accountId = this.APIAccountNo;
                return RightScale.netClient.Core.APIClient.Instance.Authenticate().Result;  //new auth with username and password
            }
            else if (!string.IsNullOrWhiteSpace(RightScale.netClient.Core.APIClient.Instance.oauthBearerToken))
            {
                return true; //already authenticated via oauth
            }
            else if (!string.IsNullOrWhiteSpace(RightScale.netClient.Core.APIClient.Instance.oauthRefreshToken) || (!string.IsNullOrWhiteSpace(RightScale.netClient.Core.APIClient.Instance.userName) && !string.IsNullOrWhiteSpace(RightScale.netClient.Core.APIClient.Instance.password) && !string.IsNullOrWhiteSpace(RightScale.netClient.Core.APIClient.Instance.accountId)))
            {
                return RightScale.netClient.Core.APIClient.Instance.Authenticate().Result; //props are already set, just need to auth
            }
            else
            {
                throw new RightScale.netClient.RightScaleAPIException("Failed to authenticate due to missing parameters.  OAuth refresh token or username/password/accountid are required"); //don't have enough info to actually perform an auth request
            }
        }
    }
}
