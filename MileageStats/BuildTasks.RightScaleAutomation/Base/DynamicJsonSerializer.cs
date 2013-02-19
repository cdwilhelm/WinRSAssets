using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BuildTasks.RightScaleAutomation.Base
{
    /// <summary>
    /// Json serializer helper class for managing data going to and coming from the RightScale API
    /// </summary>
    public static class DynamicJsonSerializer
    {
        /// <summary>
        /// Method uses the Newtonsoft.Json library to deserialize a Json string into an object of a specific type.  Dynamic types are supported 
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the given Json string to</typeparam>
        /// <param name="content">string representation of the Json object to be deserialized</param>
        /// <returns>an object of <typeparamref name="T"/> from the input of <paramref name="content"/></returns>
        public static T Deserialize<T>(string content) where T : new()
        {
            return JsonConvert.DeserializeObject<dynamic>(content);
        }
    }
}
