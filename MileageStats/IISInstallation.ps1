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

	Add-WindowsFeature -Name Web-Common-Http,Web-Asp-Net,Web-Net-Ext,Web-ISAPI-Ext,Web-ISAPI-Filter,Web-Http-Logging,Web-Request-Monitor,Web-Basic-Auth,Web-Windows-Auth,Web-Filtering,Web-Performance,Web-Mgmt-Console,Web-Mgmt-Compat,RSAT-Web-Server,WAS -IncludeAllSubFeature
}
catch
{
    ResolveError
    exit 1
}