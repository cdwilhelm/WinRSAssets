# Powershell 2.0
# Copyright (c) 2008-2013 RightScale, Inc, All Rights Reserved Worldwide.

# Stop and fail script when a command fails.
$errorActionPreference = "Stop"

# load library functions
$rsLibDstDirPath = "$env:rs_sandbox_home\RightScript\lib"
. "$rsLibDstDirPath\tools\PsOutput.ps1"
. "$rsLibDstDirPath\tools\ResolveError.ps1"
. "$rsLibDstDirPath\tools\ExtractReturn.ps1"

try
{
	Import-Module ServerManager 
	
	Add-WindowsFeature -Name Application-Server,AS-NET-Framework,AS-Web-Support,AS-WAS-Support,AS-HTTP-Activation,Web-Server,Web-WebServer,Web-Common-Http,Web-Static-Content,Web-Default-Doc,Web-Dir-Browsing,Web-Http-Errors,Web-Http-Redirect,Web-App-Dev,Web-Asp-Net,Web-Net-Ext,Web-ISAPI-Ext,Web-ISAPI-Filter,Web-Health,Web-Http-Logging,Web-Log-Libraries,Web-Request-Monitor,Web-Http-Tracing,Web-Security,Web-Basic-Auth,Web-Windows-Auth,Web-Digest-Auth,Web-Client-Auth,Web-Cert-Auth,Web-Url-Auth,Web-Filtering,Web-IP-Security,Web-Performance,Web-Stat-Compression,Web-Dyn-Compression,Web-Mgmt-Tools,Web-Mgmt-Console,Web-Scripting-Tools,Web-Mgmt-Service,WAS,WAS-Process-Model,WAS-NET-Environment,WAS-Config-APIs,WSRM  -IncludeAllSubFeature
	
	$iisRegScript = "C:\windows\Microsoft.NET\framework\v4.0.30319\aspnet_regiis.exe -i"
	
	cmd.exe /c """$iisRegScript"""
}
catch
{
    ResolveError
    exit 1
}