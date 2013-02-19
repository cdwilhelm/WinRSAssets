using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace BuildTasks.Utility
{
    /// <summary>
    /// Buil task decodes a base64 string back into its original value
    /// </summary>
    public class Base64DecodeString : Base.StringManipulationBase
    {
        /// <summary>
        /// Built task decodes the InputString and assigns a standard encoded value to the OutputString output parameter
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            bool retVal = false;

            byte[] stringByteArray = Convert.FromBase64String(this.InputString);
            this.OutputString = Encoding.UTF8.GetString(stringByteArray);
            if (!string.IsNullOrWhiteSpace(this.OutputString))
            {
                retVal = true;
            }
            //TODO: set base64 encoded string

            return retVal;
        }
    }
}
