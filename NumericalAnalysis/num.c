#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <omp.h>

double f(double x) {
    return sin(x);
}

double df(double x) {
    return cos(x);
}

double trapezoidal_integration(double a, double b, int n) {
    double h = (b - a) / n;
    double sum = 0.0;
#pragma omp parallel for reduction(+:sum)
    for (int i = 1; i < n; i++) {
        double x = a + i * h;
        sum += f(x);
    }
    sum += (f(a) + f(b)) / 2.0;
    return h * sum;
}

double simpson_integration(double a, double b, int n) {
    double h = (b - a) / n;
    double sum = f(a) + f(b);
#pragma omp parallel for reduction(+:sum)
    for (int i = 1; i < n; i++) {
        double x = a + i * h;
        sum += 2.0 * (i % 2 + 1) * f(x);
    }
    return h / 3.0 * sum;
}

double newton_deriv(double a, double h) {
    return (f(a + h) - f(a)) / h;
}

int main() {
    double a, b, h;
    printf("Enter a: ");
    scanf("%lf", &a);
    printf("Enter b: ");
    scanf("%lf", &b);
    printf("Enter the hop: ");
    scanf("%lf", &h);

    double deriv, trap_integral, simp_integral;

#pragma omp parallel sections
    {
#pragma omp section
        deriv = newton_deriv(a, h);
#pragma omp section
        trap_integral = trapezoidal_integration(a, b, 100000);
#pragma omp section
        simp_integral = simpson_integration(a, b, 100000);
    }

    printf("Numerical Derivative with Newton Method: %lf\n", deriv);
    printf("Numerical Integral with Trapezoidal Method: %lf\n", trap_integral);
    printf("Numerical Integral with Simpson's Method: %lf\n", simp_integral);

    FILE *fptr;
    fptr = fopen("results.txt", "w");
    if (fptr == NULL) {
        printf("Error opening file!\n");
        exit(1);
    }
    fprintf(fptr, "Numerical Derivative with Newton Method: %lf\n", deriv);
    fprintf(fptr, "Numerical Integral with Trapezoidal Method: %lf\n", trap_integral);
    fprintf(fptr, "Numerical Integral with Simpson's Method: %lf\n", simp_integral);
    fclose(fptr);
    return 0;
}