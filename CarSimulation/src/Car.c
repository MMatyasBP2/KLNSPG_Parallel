#include "Car.h"

void *race(void *arg)
{
    car_data *data = (car_data *)arg;
    int distance = 0;

    printf("Car %d started racing.\n", data->car_id);

    while (distance < RACE_DISTANCE) {
        distance += rand() % 50 + 1;
        data->time += rand() % 100 + 1;
    }

    printf("Car %d finished the race in %d time units.\n", data->car_id, data->time);
    return NULL;
}

int compare(const void *a, const void *b)
{
    car_data *car_a = (car_data *)a;
    car_data *car_b = (car_data *)b;

    return car_a->time - car_b->time;
}

int main()
{
    srand(time(NULL));
    pthread_t cars[NUM_CARS];
    car_data car_results[NUM_CARS];

    clock_t start_multi = clock();
    for (int i = 0; i < NUM_CARS; i++)
    {
        car_results[i].car_id = i + 1;
        car_results[i].time = 0;
        pthread_create(&cars[i], NULL, race, (void *)&car_results[i]);
    }

    for (int i = 0; i < NUM_CARS; i++)
        pthread_join(cars[i], NULL);

    clock_t end_multi = clock();
    qsort(car_results, NUM_CARS, sizeof(car_data), compare);

    printf("\nRanking:\n");
    for (int i = 0; i < NUM_CARS; i++)
        printf("%d. place: Car %d (%d time units)\n", i + 1, car_results[i].car_id, car_results[i].time);

    clock_t start_single = clock();
    for (int i = 0; i < NUM_CARS; i++)
        race((void *)&car_results[i]);
    clock_t end_single = clock();

    double time_multi = (double)(end_multi - start_multi) / CLOCKS_PER_SEC;
    double time_single = (double)(end_single - start_single) / CLOCKS_PER_SEC;

    printf("\nMulti-threaded execution time: %f seconds\n", time_multi);
    printf("Single-threaded execution time: %f seconds\n", time_single);

    return 0;
}