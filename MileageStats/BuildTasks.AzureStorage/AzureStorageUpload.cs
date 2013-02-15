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
        /// <returns></returns>
        public override bool Execute()
        {
            bool retVal = false;
            string connectionString = GetStorageConnectionString();
            //Log.LogMessage("Connection string is: " + connectionString);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(this.ContainerName);
            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(this.BlobName);
            using (var filestream = System.IO.File.OpenRead(this.FilePath))
            {
                blockBlob.UploadFromStream(filestream);
                retVal = true;
            }

            return retVal;
        }
    }
}
