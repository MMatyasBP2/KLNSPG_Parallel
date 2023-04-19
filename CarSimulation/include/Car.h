#ifndef CAR_H
#define CAR_H

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

#endif 
