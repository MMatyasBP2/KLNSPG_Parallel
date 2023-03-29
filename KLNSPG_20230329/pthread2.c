#include <stdio.h>
#include <pthread.h>
#include <stdlib.h>

#define NUM_THREADS 10
#define INTERVAL_SIZE 100

int primes[NUM_THREADS];

void* thread_function(void* arg)
{
    int index = *(int*)arg;
    int start = index * INTERVAL_SIZE;
    int end = (index + 1) * INTERVAL_SIZE - 1;
    int count = 0;
    int i, j;

    char is_prime[INTERVAL_SIZE];
    for (i = 0; i < INTERVAL_SIZE; i++)
        is_prime[i] = 1;

    for (i = 2; i * i <= end; i++)
    {
        for (j = (start + i - 1) / i * i; j <= end; j += i)
        {
            if (j < start)
                continue;
            
            is_prime[j - start] = 0;
        }
    }

    for (i = 0; i < INTERVAL_SIZE; i++)
    {
        if (is_prime[i])
            count++;
    }

    primes[index] = count;
    return NULL;
}

int main()
{
    pthread_t threads[NUM_THREADS];
    int i;

    printf("Main thread started\n");
    for (i = 0; i < NUM_THREADS; i++)
    {
        int* index = malloc(sizeof(int));
        *index = i;
        pthread_create(&threads[i], NULL, thread_function, index);
    }

    for (i = 0; i < NUM_THREADS; i++)
        pthread_join(threads[i], NULL);

    int total = 0;
    for (i = 0; i < NUM_THREADS; i++)
    {
        printf("Interval %d-%d: %d primes\n", i * INTERVAL_SIZE, (i + 1) * INTERVAL_SIZE - 1, primes[i]);
        total += primes[i];
    }

    printf("Total primes: %d\n", total);

    printf("Main thread finished\n");
    return 0;
}