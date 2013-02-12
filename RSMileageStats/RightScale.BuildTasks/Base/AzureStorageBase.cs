using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace RightScale.BuildScripts.Base
{
    public class AzureStorageBase : Task 
    {
        /// <summary>
        /// Flag identifies if a development storage account is to be used when building a Windows Azure Storage connection string
        /// </summary>
        public bool UseDevelopmentStorage { get; set; }

        /// <summary>
        /// Flag indicates the default endpoint protocol for connectng to the Windows Azure storage service
        /// </summary>
        public string DefaultEndpointProtocol { get; set; }

        /// <summary>
        /// Name of the prefix to be used when uploading new files to Windows Azure Storage
        /// </summary>
        public string BuildNamePrefix { get; set; }
        
        /// <summary>
        /// Name of the Windows Azure account to use for storage
        /// </summary>
        [Required]
        public string AccountName { get; set; }

        /// <summary>
        /// Account key for accessing Windows Azure Storage account
        /// </summary>
        [Required]
        public string AccountKey { get; set; }

        /// <summary>
        /// Name of the storage account 
        /// </summary>
        [Required]
        public string StorageContainer { get; set; }

        /// <summary>
        /// Base constructor sets defaults for non-required inputs
        /// </summary>
        public AzureStorageBase()
        {
            this.DefaultEndpointProtocol = "https";
            this.BuildNamePrefix = "Build";
        }

        /// <summary>
        /// protected method manages building a connection string for connecting to the Windows Azure Storage account specified by <see cref="AccountName"/> and <see cref="AccountKey"/>
        /// </summary>
        /// <returns>Returns properly formatted connection string for Windows Azure storage account</returns>
        protected string GetAzureStorageConnectionString()
        {
            string retVal = string.Empty;

            if (this.UseDevelopmentStorage)
            {
                retVal += "UseDevelopmentStorage=true;";
            }

            retVal += "DefaultEndpointsProtocol=" + this.DefaultEndpointProtocol +";";
            retVal += "AccountName=" + this.AccountName + ";";
            retVal += "AccountKey=" + this.AccountKey + ";";

            return retVal;
        }

        /// <summary>
        /// Protected method gets the valid file name for uploading a zip package to Windows Azure Storage serialized by date with buildprefix_yyyymmddhhmm
        /// </summary>
        /// <returns>file name only from a full file path</returns>
        protected string GetFileName()
        {
            string retVal = string.Empty;

            retVal += this.BuildNamePrefix + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".zip";

            return retVal;
        }

        public override bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}
