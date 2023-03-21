#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <unistd.h>
#include <time.h>

#define NUM_OF_CARS 10
#define TRACK_LENGTH 100

typedef struct {
    int id;
    int speed;
    int position;
} Car;

Car cars[NUM_OF_CARS];
pthread_mutex_t mutex;
pthread_barrier_t barrier;

void wait(double seconds) {
    time_t sec = (time_t) seconds;
    long nsec = (long) ((seconds - sec) * 1e9);
    struct timespec req = {sec, nsec};
    struct timespec rem = {0, 0};
    while (nanosleep(&req, &rem) == -1)
    {
        req = rem;
    }
}

void* move_cars(void* args)
{
    Car* car = (Car*) args;
    int lap = 0;
    clock_t start_time = clock();

    while (lap < 10)
    {
        pthread_mutex_lock(&mutex);
        int new_pos = car->position + car->speed;
        if (new_pos >= TRACK_LENGTH)
        {
            new_pos -= TRACK_LENGTH;
            lap++;
        }

        printf("Car %d moved from position %d to %d\n", car->id, car->position, new_pos);

        car->position = new_pos;

        pthread_mutex_unlock(&mutex);
        pthread_barrier_wait(&barrier);

        wait(0.2);
        start_time = clock();
    }
    pthread_exit(NULL);
}

int main()
{
    srand(time(NULL));

    pthread_t threads[NUM_OF_CARS];

    for (size_t i = 0; i < NUM_OF_CARS; i++)
    {
        cars[i].id = i;
        cars[i].speed = rand() % 10 + 1;
        cars[i].position = 0;
    }

    pthread_mutex_init(&mutex, NULL);
    pthread_barrier_init(&barrier, NULL, NUM_OF_CARS);

    for (size_t i = 0; i < NUM_OF_CARS; i++)
    {
        pthread_create(&threads[i], NULL, move_cars, (void*) &cars[i]);
    }

    for (size_t i = 0; i < NUM_OF_CARS; i++)
    {
        pthread_join(threads[i], NULL);
    }

    pthread_mutex_destroy(&mutex);
    pthread_barrier_destroy(&barrier);

    return 0;
}