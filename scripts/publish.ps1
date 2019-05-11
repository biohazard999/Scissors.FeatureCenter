
Param(
    [string]$Source,
    [string]$Target,
    [string]$Ftp,
    [string]$Username,
    [string]$Password,
    [switch]$WhatIf = $false
)

#Connect to ftp webclient
$webclient = New-Object System.Net.WebClient 
$webclient.Credentials = New-Object System.Net.NetworkCredential($user,$pass)  

function DownloadFromFtp($destination, $ftp_uri, $user, $pass){
    $dirs = GetDirecoryTree $ftp_uri $user $pass

    foreach($dir in $dirs){
       $path = [io.path]::Combine($destination,$dir)

       if ((Test-Path $path) -eq $false) {
          "Creating $path ..."
          New-Item -Path $path -ItemType Directory | Out-Null
       }else{
          "Exists $path ..."
       }
    }

    $files = GetFilesTree $ftp_uri $user $pass

    foreach($file in $files){
        $source = [io.path]::Combine($ftp_uri,$file)
        $dest = [io.path]::Combine($destination,$file)

        "Downloading $source ..."
        Get-FTPFile $source $dest $user $pass
    }
}

function UploadToFtp($artifacts, $ftp_uri, $user, $pass){
    Write-Host ----UploadToFtp----
    Write-Host artifacts: $artifacts
    Write-Host ftp_uri: $ftp_uri
    Write-Host user $user
    Write-Host To $ftpPath
    Write-Host ----UploadToFtp----
        
    $webclient = New-Object System.Net.WebClient 
    $webclient.Credentials = New-Object System.Net.NetworkCredential($user,$pass)  

    foreach($item in Get-ChildItem -recurse $artifacts){ 

        $fullPath = [system.io.path]::GetFullPath($item.FullName)
        $artifactsFullPath = [system.io.path]::GetFullPath($artifacts)
        $relpath = $fullPath.SubString($artifactsFullPath.Length)
        #$relpath = [system.string]::Replace("\", "/")
        $ftpPath = $ftp_uri+$relpath;
        
        Write-Host  
        Write-Host ----UploadFile----
        $uri = New-Object System.Uri($ftpPath)   
        Write-Host Item: $item.FullName
        Write-Host Is-Directory: ($item.Attributes -eq "Directory")
        Write-Host FullPath: $fullPath
        Write-Host ArtifactsFullPath: $artifactsFullPath 
        Write-Host RelPath: $relpath
        Write-Host Uploading $item
        Write-Host To $uri.ToString()
        
        if ($WhatIf -eq $true) {
            continue;
        }
        
        if ($item.Attributes -eq "Directory"){
            try{
                Write-Host Creating $item.Name

                $makeDirectory = [System.Net.WebRequest]::Create($ftpPath);
                $makeDirectory.Credentials = New-Object System.Net.NetworkCredential($user,$pass) 
                $makeDirectory.Method = [System.Net.WebRequestMethods+FTP]::MakeDirectory;
                $makeDirectory.GetResponse();

            }catch [Net.WebException] {
                Write-Host $item.Name probably exists ...
            }

            continue;
        }
        
        $s = [System.IO.File]::ReadAllBytes($item.FullName)
        $webclient.UploadData($uri, $s)
    }
}

 function Get-FTPFile ($Source,$Target,$UserName,$Password) 
 { 
     $ftprequest = [System.Net.FtpWebRequest]::create($Source) 
     $ftprequest.Credentials = New-Object System.Net.NetworkCredential($username,$password) 
     $ftprequest.Method = [System.Net.WebRequestMethods+Ftp]::DownloadFile 
     $ftprequest.UseBinary = $true 
     $ftprequest.KeepAlive = $false 

     $ftpresponse = $ftprequest.GetResponse() 
     $responsestream = $ftpresponse.GetResponseStream() 

     $targetfile = New-Object IO.FileStream ($Target,[IO.FileMode]::Create) 
     [byte[]]$readbuffer = New-Object byte[] 1024 

     do{ 
         $readlength = $responsestream.Read($readbuffer,0,1024) 
         $targetfile.Write($readbuffer,0,$readlength) 
     } 
     while ($readlength -ne 0) 

     $targetfile.close() 
 } 

#task ListFiles {
#   
#    $files = GetFilesTree 'ftp://127.0.0.1/' "web" "web"
#    $files | ForEach-Object {Write-Host $_ -foregroundcolor cyan}
#}

function GetDirecoryTree($ftp, $user, $pass){
    $creds = New-Object System.Net.NetworkCredential($user,$pass)

    $files = New-Object "system.collections.generic.list[string]"
    $folders = New-Object "system.collections.generic.queue[string]"
    $folders.Enqueue($ftp)

    while($folders.Count -gt 0){
        $fld = $folders.Dequeue()

        $newFiles = GetAllFiles $creds $fld
        $dirs = GetDirectories $creds $fld

        foreach ($line in $dirs){
            $dir = @($newFiles | Where { $line.EndsWith($_) })[0]
            [void]$newFiles.Remove($dir)
            $folders.Enqueue($fld + $dir + "/")

            [void]$files.Add($fld.Replace($ftp, "") + $dir + "/")
        }
    }

    return ,$files
}

function GetFilesTree($ftp, $user, $pass){
    $creds = New-Object System.Net.NetworkCredential($user,$pass)

    $files = New-Object "system.collections.generic.list[string]"
    $folders = New-Object "system.collections.generic.queue[string]"
    $folders.Enqueue($ftp)

    while($folders.Count -gt 0){
        $fld = $folders.Dequeue()

        $newFiles = GetAllFiles $creds $fld
        $dirs = GetDirectories $creds $fld

        foreach ($line in $dirs){
            $dir = @($newFiles | Where { $line.EndsWith($_) })[0]
            [void]$newFiles.Remove($dir)
            $folders.Enqueue($fld + $dir + "/")
        }

        $newFiles | ForEach-Object { 
            $files.Add($fld.Replace($ftp, "") + $_) 
        }
    }

    return ,$files
}

function GetDirectories($creds, $fld){
    $dirs = New-Object "system.collections.generic.list[string]"

    $operation = [System.Net.WebRequestMethods+Ftp]::ListDirectoryDetails
    $reader = GetStream $creds $fld $operation
    while (($line = $reader.ReadLine()) -ne $null) {

       if ($line.Trim().ToLower().StartsWith("d") -or $line.Contains(" <DIR> ")) {
            [void]$dirs.Add($line)
        }
    }
    $reader.Dispose();

    return ,$dirs
}

function GetAllFiles($creds, $fld){
    $newFiles = New-Object "system.collections.generic.list[string]"
    $operation = [System.Net.WebRequestMethods+Ftp]::ListDirectory

    $reader = GetStream $creds $fld $operation

    while (($line = $reader.ReadLine()) -ne $null) {
       [void]$newFiles.Add($line.Trim()) 
    }
    $reader.Dispose();

    return ,$newFiles
}

function GetStream($creds, $url, $meth){

    $ftp = [System.Net.WebRequest]::Create($url)
    $ftp.Credentials = $creds
    $ftp.Method = $meth
    $response = $ftp.GetResponse()

    return New-Object IO.StreamReader $response.GetResponseStream()
}

$localPath = "." + $Source;
$targetPath = $Ftp + $Target;
"localPath $localPath"
"targetPath $targetPath"


UploadToFtp $localPath $targetPath $Username $Password