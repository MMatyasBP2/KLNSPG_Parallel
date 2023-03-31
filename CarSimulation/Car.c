#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <time.h>
#include <Windows.h>

#define NUM_CARS 5
#define MAX_DISTANCE 1000
#define MAX_SPEED 200

int distance[NUM_CARS];
int winner = -1;
pthread_t threads[NUM_CARS];

void *car_race(void *arg);
void write_distances_to_file();
void write_winner_to_file();
void print_race_results();

int main() {
    int ids[NUM_CARS];

    for (int i = 0; i < NUM_CARS; i++) {
        ids[i] = i;
        printf("Creating thread for car %d.\n", i);
        pthread_create(&threads[i], NULL, car_race, &ids[i]);
    }

    printf("All threads created.\n");

    for (int i = 0; i < NUM_CARS; i++) {
        pthread_join(threads[i], NULL);
        printf("Thread for car %d has joined.\n", i);
    }

    printf("All threads joined.\n");

    write_distances_to_file();
    write_winner_to_file();
    print_race_results();

    return 0;
}

void *car_race(void *arg) {
    int id = *(int*)arg;
    int traveled = 0;
    srand(time(NULL) + id);

    while (traveled < MAX_DISTANCE) {
        int speed = rand() % MAX_SPEED + 1;
        traveled += speed;
        distance[id] = traveled;

        printf("Car %d has traveled %d meters.\n", id, traveled);

        if (traveled >= MAX_DISTANCE) {
            winner = id;
            printf("Car %d has crossed the finish line.\n", id);
            break;
        }

        Sleep(100);
    }

    pthread_exit(NULL);
}

void write_distances_to_file() {
    FILE *fptr;
    fptr = fopen("distances.txt", "w");

    if (fptr == NULL) {
        printf("Error opening file.\n");
        exit(1);
    }

    for (int i = 0; i < NUM_CARS; i++) {
        fprintf(fptr, "Car %d: %d meters\n", i, distance[i]);
    }

    fclose(fptr);

    printf("Distances written to file.\n");
}

void write_winner_to_file() {
    FILE *fptr;
    fptr = fopen("winner.txt", "w");

    if (fptr == NULL) {
        printf("Error opening file.\n");
        exit(1);
    }

    if (winner != -1)
        fprintf(fptr, "Winner: Car %d (Thread %lu)\n", winner, threads[winner]);
    else 
        fprintf(fptr, "No winner found.\n");

    fclose(fptr);

    printf("Winner written to file.\n");
}

void print_race_results() {
    printf("Race results:\n");

    for (int i = 0; i < NUM_CARS; i++) 
        printf("Car %d (Thread %lu) finished at position %d.\n", i, threads[i], distance[i] < MAX_DISTANCE ? -1 : 1);

    if (winner != -1)
        printf("Winner: Car %d (Thread %lu)!\n", winner, threads[winner]);
    else
        printf("No winner found.\n");
}