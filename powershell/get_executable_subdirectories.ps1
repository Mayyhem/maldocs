$AllFolders = Get-ChildItem -Path . -Recurse | ?{$_.PSIsContainer} | Select-Object FullName -ErrorAction SilentlyContinue
foreach($Folder in $AllFolders)
    {
    $Permission = (Get-Acl .).Access | ?{$_.IdentityReference -match $env:USERNAME} | Select IdentityReference,FileSystemRights,AccessControlType
    if($Permission.FileSystemRights -eq "FullControl" -or $Permission.FileSystemRights -eq "ReadAndExecute" -or $Permission.FileSystemRights -eq "ExecuteFile")
        {
        $Permission | % {Write-Host "User $($_.IdentityReference) has $($_.FileSystemRights) on folder $Folder"}
        }
    }
