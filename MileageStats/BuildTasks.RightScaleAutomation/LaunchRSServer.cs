using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTasks.RightScaleAutomation
{
    public class LaunchRSServer : Base.RightScaleAPIBase
    {
        [Required]
        public string ServerID { get; set; }

        public LaunchRSServer()
        {
            this.MethodHref = "/api/servers/{0}/launch";
        }

        public override bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}
