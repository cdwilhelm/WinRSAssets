## Build Script set overview
The containing folder includes the proper MSBuild script process to build and publish a [Web Deploy 3.0](http://www.iis.net/downloads/microsoft/web-deploy) package to [Windows Azure Storage](http://www.windowsazure.com/en-us/manage/services/storage/) and then launch a server that consumes this package and installs it properly within a Windows-based ServerTemplate.  
Executing this script via MSDeploy will require the following dependencies:

 * Web Deploy 3.0
 * Windows Azure Storage
 * MSBuild (.net 4.5)
 * RightScale API 1.5 (for Windows Azure)
 
