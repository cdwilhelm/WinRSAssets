using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using RestSharp;
using System.Net;

namespace BuildTasks.RightScaleAutomation.Base
{
    /// <summary>
    /// Base class for creating specific MSBuild tasks to call the RightScale API
    /// </summary>
    public abstract class RightScaleAPIBase : Task
    {
        /// <summary>
        /// OAuth2 Token for RightScale API Access.  Information on how to generate this key can be found at http://support.rightscale.com/12-Guides/03-RightScale_API/OAuth
        /// </summary>
        [Required]
        public string OAuth2Token { get; set; }

        #region optional input parameters

        /// <summary>
        /// Base URI for API Calls - can be overridden but is set to https://my.rightscale.com/api by default
        /// </summary>
        public string BaseUri { get; set; }

        /// <summary>
        /// ID of the cloud being queried where applicable
        /// </summary>
        public string CloudID { get; set; }

        /// <summary>
        /// ID of the server to queried or acted upon where applicable
        /// </summary>
        public string ServerID { get; set; }


        /// <summary>
        /// HTTP Header value for identifying which version of the API is being accessed
        /// </summary>
        public string ApiHeaderValue { get; set; }
        
        /// <summary>
        /// HREF for methods for a given and speciifc call to the api.  This is prepended with the BaseUri value when contructing full URLs for request paths
        /// </summary>
        protected string MethodHref { get; set; }

        /// <summary>
        /// Timeout (in seconds) for the Api client to use when attempting to access the RightScale Api REST service
        /// </summary>
        public int ApiClientTimeout { get; set; }

        #endregion

        #region Internal objects and static values

        /// <summary>
        /// Common instance of RestSharp.RestClient object for executing requests to RightScale API
        /// </summary>
        protected RestClient restClient;

        protected WebClient httpClient;

        /// <summary>
        /// HTTP Header name for identifying which version of the API is being accessed
        /// </summary>
        private const string ApiHeaderName = "X_API_VERSION";

    #endregion

        /// <summary>
        /// Base class constructor initializes all optional parameters. 
        /// </summary>
        public RightScaleAPIBase()
        {            
            this.BaseUri = @"https://my.rightscale.com/api"; //default endpoint for RS API 1.5
            this.ApiClientTimeout = 120; //setting default timeout to 2 mins/120 seconds
            this.ApiHeaderValue = "1.5"; //default is API 1.5
            
            restClient = new RestClient(); //initialize restclient so we can persist values if necessary 
            httpClient = new WebClient(); //initialize web client so we can use it along side restClient for POST 
        }

        /// <summary>
        /// PrepareCall method sets up proper http headers for the restClient object
        /// </summary>
        internal void prepareRestCall()
        {
            restClient.BaseUrl = this.BaseUri; //initialize baseurl from inputs--is also set in constructor so that a default is present in the event that a value is not passed in
            restClient.Timeout = ApiClientTimeout * 1000; //timeout is in milliseconds
            restClient.AddDefaultHeader("Authorization", "Bearer " + this.OAuth2Token);
            restClient.AddDefaultHeader(ApiHeaderName, ApiHeaderValue);
        }

        internal void prepareHttpCall()
        {
            if (httpClient.Headers["Authorization"] == null)
            {
                httpClient.Headers.Add("Authorization", "Bearer " + this.OAuth2Token);
            }
            if (httpClient.Headers[ApiHeaderName] == null)
            {
                httpClient.Headers.Add(ApiHeaderName, ApiHeaderValue);
            }
        }

        /// <summary>
        /// Required input vaidation process for classes that inherit this base class--validation must be done per call as inputs vary
        /// </summary>
        protected abstract void ValidateInputs();


        /// <summary>
        /// Private method builds out put data for call to API including inputs
        /// </summary>
        /// <returns>string for data to be pushed to the RestClient object</returns>
        protected string buildPutData(Dictionary<string, object> parameterCollection)
        {
            string putData = GetDataString(parameterCollection);
            Log.LogMessage("  Put Data is: " + putData);
            return putData;
        }

        /// <summary>
        /// Private method builds out post data for call to API including inputs
        /// </summary>
        /// <returns>byte array for data to be pushed to the RestClient object</returns>
        protected byte[] buildPostData(Dictionary<string, object> parameterCollection)
        {
            string postData = GetDataString(parameterCollection);
            Log.LogMessage("  Post Data is: " + postData);
            return Encoding.UTF8.GetBytes(postData);
        }

        /// <summary>
        /// Private static method to take Dictionary of inputs and convert to string to be pushed to API/URL
        /// </summary>
        /// <param name="parameterCollection"></param>
        /// <returns>string in URL formatted setup</returns>
        private static string GetDataString(Dictionary<string, object> parameterCollection)
        {
            string postData = string.Empty;

            foreach (string outerCollectionKey in parameterCollection.Keys)
            {
                if (parameterCollection[outerCollectionKey].GetType() == typeof(Dictionary<string, string>))
                {
                    foreach (string innerCollectionKey in ((Dictionary<string, string>)parameterCollection[outerCollectionKey]).Keys)
                    {
                        postData += outerCollectionKey + @"[name]=" + innerCollectionKey + @"&";
                        postData += outerCollectionKey + @"[value]=" + ((Dictionary<string, string>)parameterCollection[outerCollectionKey])[innerCollectionKey] + @"&";
                    }
                }
                else 
                {
                    postData += outerCollectionKey + @"=" + parameterCollection[outerCollectionKey].ToString();
                }
            }

            postData = postData.TrimEnd('&');
            return postData;
        }
    }
}
