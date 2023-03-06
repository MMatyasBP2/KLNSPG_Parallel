#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>

#define NUM_THREADS 2
#define ARRAY_SIZE 100000

int array[ARRAY_SIZE];
pthread_mutex_t mutex;

typedef struct {
    int thread_id;
    int start_index;
    int end_index;
} thread_data;

void *sum_array(void *arg) {
    thread_data *data = (thread_data *) arg;
    int sum = 0;

    for (int i = data->start_index; i < data->end_index; i++) {
        sum += array[i];
    }

    pthread_mutex_lock(&mutex);
    printf("Thread %d: sum = %d\n", data->thread_id, sum);
    pthread_mutex_unlock(&mutex);

    pthread_exit(NULL);
}

int main() {
    pthread_t threads[NUM_THREADS];
    thread_data thread_args[NUM_THREADS];
    pthread_mutex_init(&mutex, NULL);

    for (int i = 0; i < ARRAY_SIZE; i++) {
        array[i] = rand() % 100;
    }

    for (int i = 0; i < NUM_THREADS; i++) {
        thread_args[i].thread_id = i;
        thread_args[i].start_index = i * (ARRAY_SIZE / NUM_THREADS);
        thread_args[i].end_index = (i + 1) * (ARRAY_SIZE / NUM_THREADS);

        pthread_create(&threads[i], NULL, sum_array, &thread_args[i]);
    }

    for (int i = 0; i < NUM_THREADS; i++) {
        pthread_join(threads[i], NULL);
    }

    pthread_mutex_destroy(&mutex);

    return 0;
}