#include <stdio.h>
#include <pthread.h>
#include <unistd.h>

void* thread_function(void* arg)
{
    printf("Second thread started\n");
    
    sleep(8);
    
    printf("Second thread finished\n");
    
    return NULL;
}

int main()
{
    pthread_t thread;
    
    printf("Main thread started\n");
    
    sleep(4);
    
    pthread_create(&thread, NULL, thread_function, NULL);
    pthread_join(thread, NULL);
    
    printf("Main thread finished\n");
    
    return 0;
}