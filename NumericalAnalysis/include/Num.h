#ifndef NUM_H
#define NUM_H

#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <omp.h>
#include <math.h>

#define NUM_METHODS 8
#define NUM_ITERATIONS 1000000

double newton();
double taylor_first();
double taylor_second();
double trapezoid_midpoint();
double trapezoid_left();
double trapezoid_right();
double simpson_one_third();
double simpson_three_eighth();

#endif 