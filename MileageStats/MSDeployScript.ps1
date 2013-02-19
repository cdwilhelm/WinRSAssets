# Powershell 2.0
# Copyright (c) 2008-2013 RightScale, Inc, All Rights Reserved Worldwide.

# Stop and fail script when a command fails.
$errorActionPreference = "Stop"

function ConvertFrom-Base64($string) {
   $bytes  = [System.Convert]::FromBase64String($string);
   $decoded = [System.Text.Encoding]::UTF8.GetString($bytes); 
   return $decoded;
}

# load library functions
$rsLibDstDirPath = "$env:rs_sandbox_home\RightScript\lib"
. "$rsLibDstDirPath\tools\PsOutput.ps1"
. "$rsLibDstDirPath\ros\Ros.ps1"
. "$rsLibDstDirPath\tools\Archive.ps1"
. "$rsLibDstDirPath\tools\Checks.ps1"
. "$rsLibDstDirPath\tools\ResolveError.ps1"
. "$rsLibDstDirPath\tools\ExtractReturn.ps1"

try
{
    $s7z_cmd = Join-Path $env:programfiles "7-Zip\7z.exe"
    if (!(Test-Path $s7z_cmd))
    {
    $s7z_cmd = Join-Path ${env:programfiles(x86)} "7-Zip\7z.exe"
    }
    
    # Check inputs
    $zip_url = $env:ZIP_URL
    $StorageConteinerName = $env:REMOTE_STORAGE_CONTAINER_APP
    $StorageType = $env:REMOTE_STORAGE_ACCOUNT_PROVIDER_APP
    $zipName  = $env:ZIP_FILE_NAME
    
    if(!$zip_url -and !$StorageConteinerName) 
    { 
        throw "ERROR: ZIP_URL and REMOTE_STORAGE_CONTAINER_APP not specified." 
    }
        
    # Generate destination path and make sure it doesn't exist
	$datestamp = $(get-date -uformat "%Y%m%d%H%M%S")
	
    if ($env:RS_IIS_APP_VOLUME)
    {
        $working_path = Join-Path "${env:RS_IIS_APP_VOLUME}\MSDeploy" $datestamp
        $dest_path = Join-Path "${env:RS_IIS_APP_VOLUME}\wwwroot\release" $datestamp
    }
    else
    {
        $working_path = Join-Path "C:\Inetpub\MSDeploy\" $datestamp
        $dest_path = Join-Path "C:\Inetpub\wwwroot\release" $datestamp
    }
        
    # Create dir
    if (Test-Path $dest_path) 
    {
        Write-Host "Creating directory $dest_path"
      throw "Directory $dest_path already exists."
    }
    else
    {
        mkdir "$dest_path"
    }
    if(!(Test-Path $working_path))
    {
        mkdir "$working_path"
    }

    $sleep_delay_sec = 1
    $max_retries = 3
    $retires = 0
    
    # Proceed to download code from zip archive if specified
    if ($zip_url) 
    {
      # Download zip
      $web_zip = Join-Path $working_path "web.zip"
      Write-Host "Downloading $zip_url to $web_zip"
      $web = new-object System.Net.WebClient 
    
      while ($TRUE)
      {
        $web.DownloadFile($zip_url, $web_zip)
        Write-Host "Exit " $?
        if ($?) 
        {
          # success
          break
        }
        else
        {
          # failed try again
          if ($retries -le $max_retries)
          {      
            # sleep a bit before trying again
            Write-Host "Sleeping for $sleep_delay_sec"
            Start-Sleep -s $sleep_delay_sec
            $sleep_delay_sec = $sleep_delay_sec * 2
            $retries = $retries + 1 
          }
          else
          {
            throw "download zip file failed."
          }
        }
      }
  
      cd "$working_path"
      &"$s7z_cmd" x web.zip -y 
    
      Remove-Item "$web_zip"
    }
    else
    {        
        # Init ROS context (and declare inputs used by InitRosContextFromInputs)
        # $env:REMOTE_STORAGE_ACCOUNT_PROVIDER_APP
        # $env:REMOTE_STORAGE_ACCOUNT_ID_APP
        # $env:REMOTE_STORAGE_ACCOUNT_SECRET_APP
        # $env:REMOTE_STORAGE_CONTAINER_APP
        # $env:REMOTE_STORAGE_BLOCK_SIZE_APP
        # $env:REMOTE_STORAGE_THREAD_COUNT_APP
        # $env:OBJECT_STORAGE_SERVICENET_APP
        $rosCtx = InitRosContextFromInputsApp | ExtractReturn

        $fullPath = Join-Path $working_path $zipName
        RosGetObject $rosCtx $zipName $fullPath

        Write-Host "Unzipping ${fullPath}..."
        cd "$working_path"
        &"$s7z_cmd" x "$zipName" -y 

       # Write-Host "Removing ${fullPath}..."
       Remove-Item $fullPath
    }
    

    Write-Host "Application package downloaded succssfully to '$working_path'"

    #start msdeploy workflow

    
	# Add MSDeploy to path
	$env:path="$env:path;c:\Program Files\IIS\Microsoft Web Deploy V2"

	$parameterFileContent = $env:MSDEPLOY_PARAMETERFILECONTENT
	$parameterFileContentIsBase64Encoded = $env:MSDEPLOY_PARAMETERFILECONTENTISBASE64

	cd "$working_path"
	$gciSearchPath = "*.SetParameters.xml"
	$parameterFileName = (gci $gciSearchPath).Name
	$applicationName = $parameterFileName.Replace(".SetParameters.xml","").Replace($working_path, "")
	$deployCmd = $working_path + "/" + $applicationName + ".deploy.cmd /T"
	
	if($parameterFileContent)
	{
		if($parameterFleContentIsBase64Encoded -eq $TRUE)
		{
			$parameterXML = ConvertFrom-Base64($parameterFileContent)
		}
		else
		{
			$parameterXML = $parameterFileContent
		}
		
		echo $parameterXML | out-file -encoding 'ASCII' "$parameterFileName"
	}
	
	
	$fullPropXMLPath = Join-Path $working_path $parameterFileName
	Write-Host "FullPropXMLPath is $fullPropXMLPath"
	gc $fullPropXMLPath
	$xmlDoc = [XML](gc "$fullPropXMLPath")
	$xmlNode = $xmlDoc.parameters.setParameter | where {$_.name -eq "IIS Web Application Name"}
	
	if(!$env:MSDEPLOY_SITE_NAME)
	{
		$xmlNode.value = $env:MSDEPLOY_SITE_NAME
	}
	else
	{
		$xmlNode.value = "Default Web Site"
	}
	
	$xmlDoc.Save($fullPropXMLPath)
	
	#update web app name to put it in the default site:
	Write-Host "Deploy Command is: $deployCmd"
	Invoke-Expression """$deployCmd"""
}
catch
{
    ResolveError
    exit 1
}