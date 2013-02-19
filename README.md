=====
Dev/Test with TFS, Windows Azure and RightScale
=====

This repo contains demo material and code samples for building out a cloud-enabled test process within a fairly standard Agile/Rapid development SDLC.  The reference application is from Microsoft's Patterns and Practices Project Silk with a few modifications:

  * The data tier has been changed from localdb to utilize SQL Server so that it can be deployed within a scalable environment
    * The unity configurations have been updated accordingly as well to map entities to the SQL-enabled class
  * Database backups for SQL 2008r2 and SQL 2012 have been uploaded to the $gitroot/assets/ folder
  * I also disabled the test projects as getting Moq and xunit to resolve (when I was working on it) was a bit of a pain.  I might come back and revisit this in the future, but for now it's set to not build the test projects from the .sln
  
The rest of the work is contained within the BuildScripts and BuildTasks groupings of solutions.  The BuildScripts project is a collection of MSBuild Project files architected to be flexible and meaningfully useful across a variety of environments and setups.  The BuildTasks projects are individiual sets of BuildTasks developed to make use of specific workflows and processes and are leveraged within the BuildScripts project to manage the process of compiling, packaging, uploading and deploying a web application to a Windows Azure Virtual Machine.  

This process has been tested with Microsoft Team Foundation Server 2012 and requires that MSDeploy v3 be installed on the server running the build agent--this can be done via the Microsoft Web Platform Installer (V4.0).  I also used PowerGUI 3.2 (http://www.powergui.org/entry.jspa?externalID=3891&categoryID=299) and the PowerGUI Visual Studio Extensions (http://powerguivsx.codeplex.com/) to get all of my assets in one place (PS for RightScripts, MSBuild + custom tasks and the web app itself).

Licensing Microsoft's code related to Project Silk and the MileageStats application is located at $gitroot/WinRSAssets/MileageStats/MileageStats.Web/license.txt

All other software is available under the Creative Commons v3 license--please feel free to use it if you find it useful and let me know what you think--I'm always interested in expanding this kind of stuff as more people see a need for it.

Best of luck!

-Patrick McClory [pmcclory (at) gmail.com]