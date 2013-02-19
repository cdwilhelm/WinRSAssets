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
	$guid = [Guid]::NewGuid()
	
	$workingDirectory = Join-Path "C:\" $guid.ToString()
	
	if(!(Test-Path $workingDirectory))
	{
		md "$workingDirectory"
	}

	#determine if 32 or 64 bit...
    $systemArchitecture = gwmi win32_operatingsystem 
    write-host "System Architecture is: " + $systemArchitecture.osarchitecture
    if($systemArchitecture.OSArchitecture -eq "64-bit")
    {
        $architecture = "x64"
    }
    else
    {
        $architecture = "x86"
    }

	if ($architecture -eq "x86")
	{
		$downloadUrl="http://go.microsoft.com/fwlink/?LinkID=260812"
	}
	else
	{
		$downloadUrl="http://go.microsoft.com/fwlink/?LinkID=260811"
	}
	
	$storageFile = Join-Path $workingDirectory "WebDeploy.msi"
	
    Write-Host "Beginning download of web deploy msi"
	$webclient = New-Object System.Net.WebClient
	$webclient.DownloadFile($downloadUrl, $storageFile)
    Write-Host "Done downloading web deploy msi"
	
    Write-Host "Beginning installation"
	$msiExecCmd = "msiexec /i $storageFile ADDLOCAL=ALL /qn /norestart"
    cmd.exe /c """$msiExecCmd"""
    Write-Host "Installation complete"
}
catch
{
    ResolveError
    exit 1
}

if(Test-Path $workingDirectory)
{
    Write-host "Removing working folder $workingDirectory"
	Remove-Item -Recurse -Force $workingDirectory
}