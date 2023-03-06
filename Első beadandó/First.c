#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>

#define NUM_THREADS 3

int sum = 0; /* global variable to store the sum */

void *thread_function(void *arg)
{
    int thread_num = *((int*) arg);
    int random_num = rand() % 10 + 1; /* generate a random number between 1 and 10 */
    printf("Thread %d generated random number: %d\n", thread_num, random_num);
    sum += random_num; /* add the random number to the sum */
    pthread_exit(NULL);
}

int main()
{
    pthread_t threads[NUM_THREADS];
    int thread_args[NUM_THREADS];
    int i, result;

    /* create the threads */
    for (i = 0; i < NUM_THREADS; i++) {
        thread_args[i] = i;
        result = pthread_create(&threads[i], NULL, thread_function, (void*) &thread_args[i]);
        if (result != 0) {
            perror("Thread creation failed");
            exit(EXIT_FAILURE);
        }
    }

    /* wait for the threads to finish */
    for (i = 0; i < NUM_THREADS; i++) {
        result = pthread_join(threads[i], NULL);
        if (result != 0) {
            perror("Thread join failed");
            exit(EXIT_FAILURE);
        }
    }

    /* print the sum of the random numbers */
    printf("The sum of the random numbers is %d\n", sum);

    return 0;
}