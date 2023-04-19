#ifndef NUM_H
#define NUM_H

#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <omp.h>
#include <stdbool.h>

double f(double x);
double df(double x);
double ddf(double x);
double trapezoidal_integration_mid(double a, double b, int n);
double trapezoidal_integration_left(double a, double b, int n);
double trapezoidal_integration_right(double a, double b, int n);
double simpson_integration_1_3(double a, double b, int n);
double simpson_integration_3_8(double a, double b, int n);
double newton_deriv(double a, double h);
double taylor_1_deriv(double a, double h);
double taylor_2_deriv(double a, double h);
void print_results(double a, double b, int n);
void write_results(double a, double b, int n);
void check_input(char* msg, double* a, double* b, int* n);

#endif 