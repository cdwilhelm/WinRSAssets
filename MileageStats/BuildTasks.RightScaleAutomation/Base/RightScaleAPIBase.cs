using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using RestSharp;

namespace BuildTasks.RightScaleAutomation.Base
{
    public abstract class RightScaleAPIBase : Task
    {
        /// <summary>
        /// OAuth2 Token for RightScale API Access.  Information on how to generate this key can be found at http://support.rightscale.com/12-Guides/03-RightScale_API/OAuth
        /// </summary>
        [Required]
        public string OAuth2Token { get; set; }

        public string BaseUri { get; set; }

        private const string ApiHeaderName = "X_API_VERSION";
        private const string ApiHeaderValue = "1.5";

        protected RestClient restClient;
        protected string MethodHref { get; set; }

        /// <summary>
        /// Base class constructor initializes all optional parameters. 
        /// </summary>
        public RightScaleAPIBase()
        {
            if (string.IsNullOrWhiteSpace(this.BaseUri))
            {
                this.BaseUri = "https://my.rightscale.com/api";
            }
            restClient = new RestClient();
            restClient.BaseUrl = this.BaseUri;
            restClient.Timeout = 120000;
        }

        internal void prepareCall()
        {
            restClient.AddDefaultHeader("Authorization", "Bearer " + this.OAuth2Token);
            restClient.AddDefaultHeader(ApiHeaderName, ApiHeaderValue);
        }

    }
}
