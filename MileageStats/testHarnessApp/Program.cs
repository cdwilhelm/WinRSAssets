using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuildTasks.AzureStorage;

namespace testHarnessApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AzureStorageUpload asu = new AzureStorageUpload();
            asu.AccountKey = "this is not the account key you're looking for...";
            asu.AccountName = "pmdevusweststorageacct";
            asu.BlobName = "uploadpackage.7z";
            asu.ContainerName = "media";
            asu.DefaultEndpointsProtocol = "https";
            asu.FilePath = @"D:\publish\publish.7z";
            asu.UseDevelopmentStorage = false;

            asu.Execute();
        }
    }
}
