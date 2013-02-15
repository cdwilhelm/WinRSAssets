using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTasks.RightScaleAutomation.Base
{
    public abstract class RightScaleAPIBase
    {
        [Required]
        public string APIUserName { get; set; }

        [Required]
        public string APIPassword { get; set; }

        public string BaseUri { get; set; }

        /// <summary>
        /// Base class constructor initializes all optional parameters. 
        /// </summary>
        public RightScaleAPIBase()
        {
            this.BaseUri = "";
        }
    }
}
