using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildTasks.RightScaleAutomation;

namespace testHarnessApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, object> contentObject = new Dictionary<string, object>();
            Dictionary<string, object> inputObject = new Dictionary<string, object>();
            inputObject.Add("input1", "value1");
            inputObject.Add("input2", "value2");
            contentObject.Add("inputs", contentObject);

        
        }
    }
}
