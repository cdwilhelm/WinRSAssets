using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTasks.RightScaleAutomation
{
    /// <summary>
    /// MSBuild task to gather general information about a running instance within the RightScale system on a given cloud
    /// </summary>
    public class GetInstanceInfo : Base.RightScaleBuildTaskBase
    {
        #region Input Variables

        /// <summary>
        /// ID of the server to be queried
        /// </summary>
        [Required]
        public string ServerID { get; set; }

        #endregion

        #region Output Variables

        /// <summary>
        /// Collection of Private IP addresses assigned to the given instance
        /// </summary>
        [Output]
        public List<string> PrivateIPAddresses { get; set; }

        /// <summary>
        /// Collection of Public IP addresses assigned to the given instance
        /// </summary>
        [Output]
        public List<string> PublicIPAddresses { get; set; }

        /// <summary>
        /// Timestamp indicating when the instance was created
        /// </summary>
        [Output]
        public string CreatedAt { get; set; }
        
        /// <summary>
        /// Timestamp indicating the last update's date and time
        /// </summary>
        [Output]
        public string UpdatedAt { get; set; }

        /// <summary>
        /// Friendly name of server within the RightScale platform
        /// </summary>
        [Output]
        public string Name { get; set; }

        /// <summary>
        /// Resource UID for the instance within the RightScale platform
        /// </summary>
        [Output]
        public string ResourceUid { get; set; }

        /// <summary>
        /// Current state of the given server
        /// </summary>
        [Output]
        public string State { get; set; }

        /// <summary>
        /// OS Platform of the current Instance of this server
        /// </summary>
        [Output]
        public string OSPlatform { get; set; }

        /// <summary>
        /// Private DNS names of the current instance of this server
        /// </summary>
        [Output]
        public List<string> PrivateDNSNames { get; set; }

        /// <summary>
        /// InstanceID of the currently instance of this server
        /// </summary>
        [Output]
        public string InstanceID { get; set; }

        #endregion
        
        /// <summary>
        /// Override to Task.Execute to run custom process for gathering data on a server instance running within RightScale
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            bool retVal = false;
            Log.LogMessage("  GetInstanceInfo.Execute - beginning at " + DateTime.Now.ToString());
            ValidateInputs();

            RightScale.netClient.Server server = RightScale.netClient.Server.show(this.ServerID, "instance_detail");

            if (server.current_instance != null)
            {
                RightScale.netClient.Instance currentInstance = server.current_instance;
                this.PrivateIPAddresses = currentInstance.private_ip_addresses;
                this.PublicIPAddresses = currentInstance.public_ip_addresses;
                this.CreatedAt = currentInstance.created_at;
                this.UpdatedAt = currentInstance.updated_at;
                this.Name = currentInstance.name;
                this.ResourceUid = currentInstance.resource_uid;
                this.State = currentInstance.state;
                this.OSPlatform = currentInstance.os_platform;
                this.PrivateDNSNames = currentInstance.private_dns_names;
                this.InstanceID = currentInstance.ID;
            }

            Log.LogMessage("  GetInstanceInfo.Execute - complete at " + DateTime.Now.ToString());
            return retVal;
        }

        /// <summary>
        /// Implementing Input validation process for this MSBuild task to ensure CloudID and ServerID are populated
        /// </summary>
        protected void ValidateInputs()
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(this.ServerID))
            {
                errorMessage += "ServerID is not defined and is required. ";
            }
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException("Errors were found when attempting to validate inputs for the GetInstanceInfo MSBuild Task: " + errorMessage);
            }
            else
            {
                Log.LogMessage(@"  GetInstanceInfo.ValidateInputs - inputs validated - ServerID (" + this.ServerID.ToString() + @")"); 
            }
        }
    }
}
