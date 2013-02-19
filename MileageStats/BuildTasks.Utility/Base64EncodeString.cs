using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTasks.Utility 
{
	/// <summary>
	/// MSBuild task takes a plain string and Base64 encodes it so that it is safe to pass via HTTP to API's and other character sensitive endpoints
	/// </summary>
    public class Base64EncodeString : Base.StringManipulationBase
    {
		/// <summary>
		/// Override to Task.Execute that encodes the string and assigns the value to the OutoutString propery
		/// </summary>
		/// <returns></returns>
        public override bool Execute()
        {
            bool retVal = false;

            this.OutputString = Convert.ToBase64String(Encoding.UTF8.GetBytes(this.InputString));
            if (!string.IsNullOrWhiteSpace(this.OutputString))
            {
                retVal = true;
            }

            return retVal;
        }
    }
}
