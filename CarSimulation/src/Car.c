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

int main(int argc, char *argv[])
{
    srand(time(NULL));

    int num_cars = NUM_CARS;
    if (argc > 1) {
        int input_threads = atoi(argv[1]);
        if (input_threads > 0) {
            num_cars = input_threads;
        } else {
            printf("Invalid number of threads. Using default value: %d\n", NUM_CARS);
        }
    }

    pthread_t cars[num_cars];
    car_data car_results[num_cars];

    clock_t start_multi = clock();
    for (int i = 0; i < num_cars; i++)
    {
        car_results[i].car_id = i + 1;
        car_results[i].time = 0;
        pthread_create(&cars[i], NULL, race, (void *)&car_results[i]);
    }

    for (int i = 0; i < num_cars; i++)
        pthread_join(cars[i], NULL);

    clock_t end_multi = clock();
    qsort(car_results, num_cars, sizeof(car_data), compare);

    printf("\nRanking:\n");
    for (int i = 0; i < num_cars; i++)
        printf("%d. place: Car %d (%d time units)\n", i + 1, car_results[i].car_id, car_results[i].time);

    clock_t start_single = clock();
    for (int i = 0; i < num_cars; i++)
        race((void *)&car_results[i]);
    clock_t end_single = clock();

    double time_multi = (double)(end_multi - start_multi) / CLOCKS_PER_SEC;
    double time_single = (double)(end_single - start_single) / CLOCKS_PER_SEC;

    printf("\nMulti-threaded execution time: %f seconds\n", time_multi);
    printf("Single-threaded execution time: %f seconds\n", time_single);

    return 0;
}