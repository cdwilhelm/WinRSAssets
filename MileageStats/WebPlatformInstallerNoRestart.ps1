$computer = gc env:computername

try
{
	Write-Host "Running WebPI Installer Process"
	
    $file_URL = $env:WPI_FILE_DOWNLOADURL
	$guid = [Guid]::NewGuid()

	$workingDirectory = Join-Path "C:\" $guid.ToString()
	
	if(!(Test-Path $workingDirectory))
	{
		md "$workingDirectory"
	}

    $userName = $env:username 
    
    write-host "Executing as $userName"

    $installer_path = Join-Path $workingDirectory "wpiinstaller.msi" 
    
    write-host "Installer path is: $installer_path"
    
	$custom_feeds = $env:WPI_CUSTOM_FEEDS
				
	if(test-path $workingDirectory)
	{
	   write-host "Destination path ($workingDirectory) exists."
	}
	else
	{
	   write-host "Destination path ($workingDirectory) does not exist -- creating directory."
	   $drivename = $workingDirectory.substring(0,2)
	   $folderpath = $workingDirectory.substring(2)
	   
	   write-host "drivename: $drivename"
	   write-host "folderpath: $folderpath"
	   
	   set-location "$drivename\"
	   MD $folderpath
	}
    		    
    if(test-path $installer_path)
    {
        del $installer_path
    }
    
    $computer = gc env:computername
    
    if(!$file_URL)
    {
        $OS = (Get-WmiObject -computername $computer -class Win32_OperatingSystem)
        
        if((Get-WmiObject -Class Win32_OperatingSystem -ComputerName $computer -ea 0).OSArchitecture -eq '64-bit')
        {
            $file_URL = "http://download.microsoft.com/download/7/0/4/704CEB4C-9F42-4962-A2B0-5C84B0682C7A/WebPlatformInstaller_amd64_en-US.msi"
        }
        else
        {
            $file_URL = "http://download.microsoft.com/download/7/0/4/704CEB4C-9F42-4962-A2B0-5C84B0682C7A/WebPlatformInstaller_x86_en-US.msi"
        }
    }

    Write-Host "Downloading file from $file_URL to $installer_path"

    $client = new-object System.Net.WebClient
    $client.DownloadFile($file_URL, $installer_path)
    
    Write-Host "$full_path downloaded successfully"
    
    $productList = "NETFramework45,MVC3"
    			    
    $computer = gc env:computername
    
    $command = """$installer_path"""
    
    cmd  /c $installer_path /qn
    
    $programfiledir = $env:programfiles
    
    $wpiCmdPath = """" + $programfiledir + "\Microsoft\Web Platform Installer\webpicmd.exe"""

    $canRun = $false
    
    $wpiCmdArgs = " /install"
    			    
    if ($productList)
    {
        $wpiCmdArgs = $wpiCmdArgs + " /Products:$productlist"
        $canRun = $true
    }

    write-host $wpiCmdArgs
    
    if($canRun)
    {
        $wpiCmdToRun = $wpiCmdPath + $wpiCmdArgs + " /AcceptEULA /SuppressReboot"
        write-host $wpiCmdToRun
        cmd /C ""$wpiCmdToRun"" 
    }	
}
catch
{
	Write-Host $_.Exception.Message
	Write-Host $_.Exception.StackTrace
	exit 1
}
