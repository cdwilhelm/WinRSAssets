using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BuildTasks.RightScaleAutomation.Base
{
    /// <summary>
    /// Collection of utilities for use by MSBuild process in preparing inputs and variables for transmission to the RightScale API when launching or modifying servers.
    /// </summary>
    public static class MSDeployUtility
    {
        /// <summary>
        /// Static helper method url encodes the parameters xml string based on a dictionary passed in.
        /// </summary>
        /// <param name="parameterSet">IDictionary object containing key/value pairs of parameters to be updated in a web.config for an MSDeploy package</param>
        /// <returns>URL Encoded string representing the parameters file for a given instance of a parameter set for MSDeploy</returns>
        public static string GetURLEncodedParametersXMLString(IDictionary<string, string> parameterSet)
        {
            string retVal = string.Empty;

            if (parameterSet.Count > 0)
            {
                retVal += @"<?xml version=""1.0"" encoding=""utf-8""?><parameters>";

                foreach(string parameterKey in parameterSet.Keys)
                {
                    retVal += @"<setParameter name=""" + parameterKey + @""" value=""" + parameterSet[parameterKey] + @""" />";
                }

                retVal += @"</parameters>";
            }
            return HttpUtility.UrlEncode(retVal);
        }
    }
}
