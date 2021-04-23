$assemblyFile = "C:\cmdExec.dll"
$stringBuilder = New-Object -Type System.Text.StringBuilder
$fileStream = [IO.File]::OpenRead($assemblyFile)
while (($byte = $fileStream.ReadByte()) -gt -1) {
$stringBuilder.Append($byte.ToString("X2")) | Out-Null
}
$stringBuilder.ToString() -join "" | Out-File C:\cmdExec.txt