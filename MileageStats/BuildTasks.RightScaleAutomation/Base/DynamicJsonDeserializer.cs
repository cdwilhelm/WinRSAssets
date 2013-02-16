using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BuildTasks.RightScaleAutomation.Base
{
    public static class DynamicJsonDeserializer
    {
        public static T Deserialize<T>(string content) where T : new()
        {
            return JsonConvert.DeserializeObject<dynamic>(content);
        }
    }
}
