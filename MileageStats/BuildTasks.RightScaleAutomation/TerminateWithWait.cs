using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;
using System.Threading;
using RightScale.netClient;

namespace BuildTasks.RightScaleAutomation
{
    /// <summary>
    /// TerminateWithWait MSBuild task terminates a server and waits for it to complete the termination process based on parameters specified.
    /// </summary>
    public class TerminateWithWait : Base.RightScaleBuildTaskBase
    {
        /// <summary>
        /// ID of the Server to be terminated
        /// </summary>
        [Required]
        public string ServerID { get; set; }

        /// <summary>
        /// Time in seconds to wait between polls of the RS API for terminate server checks
        /// </summary>
        public int WaitCycleTime { get; set; }

        /// <summary>
        /// Maximum number of cycles to wait for the server to tetminate.  -1 is default and indicates no maximum.
        /// </summary>
        public int MaxWaitCycles { get; set; }

        /// <summary>
        /// public ctor sets defaults for non-required inputs
        /// </summary>
        public TerminateWithWait()
        {
            this.WaitCycleTime = 10;
            this.MaxWaitCycles = -1;
        }

        /// <summary>
        /// Execute method manages the process of executing and waiting for the server to terminate
        /// </summary>
        /// <returns>True when successful, exception otherwise</returns>
        public override bool Execute()
        {
            Log.LogMessage("TerminateWithWait starting for ServerID " + this.ServerID.ToString());
            if (RightScale.netClient.Core.APIClient.Instance.Authenticate(this.OAuthRefreshToken).Result)
            {
                string currentState = Server.show(this.ServerID).state;
                if (currentState == "inactive")
                {
                    Log.LogMessage("  Cannot terminate because machine is already not running");
                    return true;
                }
                else if (currentState == "terminating")
                {
                    Log.LogMessage("  Machine is already terminating, waiting to confirm");
                }
                else
                {
                    Log.LogMessage("  Calling terminate on server " + this.ServerID);
                    Server.terminate(this.ServerID);
                    Log.LogMessage("  Terminate call complete for server " + this.ServerID);
                }

                int cycleID = 0;
                Log.LogMessage("  Cycle " + cycleID.ToString() + " completed.  Current state of the server is " + currentState);

                if (currentState != "inactive" && (this.MaxWaitCycles > 0 && (cycleID > this.MaxWaitCycles)))
                {
                    Thread.Sleep(WaitCycleTime * 1000);
                    currentState = Server.show(this.ServerID).state;
                    cycleID++;
                    Log.LogMessage("  Cycle " + cycleID.ToString() + " completed.  Current state of the server is " + currentState);
                }
            }
            else
            {
                Log.LogMessage("  Cannot authenticate to RSAPI");
            }

            Log.LogMessage("TerminateWithWait complete");
            return true;
        }
    }
}
