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
            asu.AccountKey = "yM4Id0i1822gDY1V80612c/GQSfuaG5B2NOVRWg5mj5Oj8qZLocEdllghu1OVsCnS9Wi5hx4zAsPqWR5lIRrCw==";
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
