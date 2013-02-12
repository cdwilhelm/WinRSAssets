using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.StorageClient;

namespace RightScale.BuildScripts
{
    public class AzureStorageUpload : Base.AzureStorageBase
    {
        /// <summary>
        /// Full file path of the file to be uploaded
        /// </summary>
        [Required]
        public string UploadFile { get; set; }

        /// <summary>
        /// Override to MSBuild Task's default Execute to upload a file to Windows Azure storage
        /// </summary>
        /// <returns>true if successful, false if not</returns>
        public override bool Execute()
        {
            bool retVal = false;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(GetAzureStorageConnectionString());

            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(this.StorageContainer);

            var blockBlob = container.GetBlockBlobReference(this.UploadFile);

            string uploadFileName = GetFileName();

            using (var fileStream = System.IO.File.OpenRead(uploadFileName))
            {
                Log.LogMessage("Starting upload of " + uploadFileName + " to Windows Azure Storage");
                blockBlob.UploadFromStream(fileStream);
                Log.LogMessage("Upload of " + uploadFileName + " complete.");
                retVal = true;
            }

            return retVal;
        }
    }
}
