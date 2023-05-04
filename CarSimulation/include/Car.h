#ifndef CAR_H
#define CAR_H

#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#define NUM_CARS 10

typedef struct {
    int car_id;
    int time;
} car_data;

void *race(void *arg);
int compare(const void *a, const void *b);


#endif