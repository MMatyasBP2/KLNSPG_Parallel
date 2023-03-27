#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <omp.h>

#define NUM_THREADS 4

double f(double x) {
    return exp(-x) * cos(x);
}

double simpson(double a, double b, int n) {
    double h = (b - a) / n;
    double sum = f(a) + f(b);

    #pragma omp parallel num_threads(NUM_THREADS)
    {
        int id = omp_get_thread_num();
        double partial_sum = 0.0;

        #pragma omp for
        for (int i = 1 + id; i < n; i += NUM_THREADS) {
            double x = a + i * h;
            if (i % 2 == 0) {
                partial_sum += 2 * f(x);
            } else {
                partial_sum += 4 * f(x);
            }
        }

        #pragma omp critical
        {
            sum += partial_sum;
        }
    }

    return (h / 3) * sum;
}

int main() {
    double a = 0.0;
    double b = 1.0;
    int n = 10000;

    double result = simpson(a, b, n);
    printf("Value of integrals: %lf\n", result);

    return 0;
}