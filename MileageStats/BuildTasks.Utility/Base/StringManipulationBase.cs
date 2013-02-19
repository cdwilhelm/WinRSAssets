using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace BuildTasks.Utility.Base
{
    /// <summary>
    /// Base class holds common inputs and outputs for utility string manipulation tasks
    /// </summary>
    public abstract class StringManipulationBase : Task
    {
        /// <summary>
        /// Input string to be manipulated for output
        /// </summary>
        [Required]
        public string InputString { get; set; }

        /// <summary>
        /// Output parameter containing the manipulated input string
        /// </summary>
        [Output]
        public string OutputString { get; set; }

    }
}
