using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace BuildTasks.AzureStorage
{
    /// <summary>
    /// Build task uploads a file to a specific blob storage account and container.  A file will be created or replaced (if it exists) in the configured storage account)
    /// </summary>
    public class AzureStorageUpload  : Base.AzureStorageBase
    {
        /// <summary>
        /// Path of the file to be uploaded to Windows Azure Storage
        /// </summary>
        [Required]
        public string FilePath { get; set; } 

        /// <summary>
        /// Override to Build Task.Execute() method 
        /// </summary>
        /// <returns>true if success, false if not/returns>
        public override bool Execute()
        {
            Log.LogMessage("  AzureStorageUpload.Execute - beginning at " + DateTime.Now.ToString());
            bool retVal = false;
            string connectionString = GetStorageConnectionString();
            if (this.DebugMode)
            {
                Log.LogMessage("Connection string is: " + connectionString);
            }
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            Log.LogMessage("    AzureStorageUpload.Execute - Connection string is valid");
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(this.ContainerName);
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(this.BlobName);
            using (var filestream = System.IO.File.OpenRead(this.FilePath))
            {
                Log.LogMessage("    AzureStorageUpload.Execute - Upload beginning");
                DateTime starttime = DateTime.Now;
                blockBlob.UploadFromStream(filestream);
                retVal = true;
                DateTime endtime = DateTime.Now;
                Log.LogMessage("    AzureStorageUpload.Execute - Upload completed in " + endtime.Subtract(starttime).TotalSeconds.ToString() + " seconds");
            }

            Log.LogMessage("  AzureStorageUpload.Execute - complete at " + DateTime.Now.ToString());

            return retVal;
        }
    }
}
