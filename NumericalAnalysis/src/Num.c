#include "Num.h"

double newton()
{
    double x = 1.0;
    for (int i = 0; i < 1000000000; i++) {
        x = x - (x * x - 2) / (2 * x);
    }
    return x;
}

double taylor_first()
{
    double x = 1.0;
    double sum = 0.0;
    for (int i = 0; i < 1000000000; i++) {
        sum += pow(-1, i) * pow(x, 2 * i + 1) / (2 * i + 1);
    }
    return sum;
}

double taylor_second()
{
    double x = 1.0;
    double sum = 0.0;
    for (int i = 0; i < 1000000000; i++) {
        sum += pow(-1, i) * pow(x, 4 * i + 1) / (4 * i + 1);
    }
    return sum;
}

double trapezoid_midpoint()
{
    double a = 0.0, b = 1.0;
    int n = 1000000000;
    double h = (b - a) / n;
    double sum = 0.0;
    for (int i = 1; i < n; i++) {
        double x = a + i * h;
        sum += 4 * x / (1 + x * x);
    }
    return h * sum;
}

double trapezoid_left()
{
    double a = 0.0, b = 1.0;
    int n = 1000000000;
    double h = (b - a) / n;
    double sum = 0.0;
    for (int i = 0; i < n; i++) {
        double x = a + i * h;
        sum += 4 * x / (1 + x * x);
    }
    return h * sum;
}

double trapezoid_right()
{
    double a = 0.0, b = 1.0;
    int n = 1000000000;
    double h = (b - a) / n;
    double sum = 0.0;
    for (int i = 1; i <= n; i++) {
        double x = a + i * h;
        sum += 4 * x / (1 + x * x);
    }
    return h * sum;
}

double simpson_one_third()
{
    double a = 0.0, b = 1.0;
    int n = 1000000000;
    double h = (b - a) / n;
    double sum = 0.0;
    for (int i = 1; i <= n / 2; i++) {
        double x = a + (2 * i - 1) * h;
        sum += 4 * x / (1 + x * x);
    }
    for (int i = 1; i <= n / 2 - 1; i++) {
        double x = a + 2 * i * h;
        sum += 2 * x / (1 + x * x);
    }
    return h / 3 * (1 + sum + 1 / (1 + b * b));
}

double simpson_three_eighth()
{
    double a = 0.0, b = 1.0;
    int n = 1000000000;
    double h = (b - a) / n;
    double sum = 0.0;
    for (int i = 0; i < n; i += 3) {
        double x1 = a + i * h;
        double x2 = x1 + h;
        double x3 = x1 + 2 * h;
        double x4 = x1 + 3 * h;
        sum += 3 * h / 8 * (4 * x1 / (1 + x1 * x1) + 3 * (4 * x2 / (1 + x2 * x2) + 4 * x3 / (1 + x3 * x3)) + 4 * x4 / (1 + x4 * x4));
    }
    return sum;
}

int main() {
    double (*methods[])() = {newton, taylor_first, taylor_second, trapezoid_midpoint, trapezoid_left, trapezoid_right, simpson_one_third, simpson_three_eighth};
    int num_methods = sizeof(methods) / sizeof(methods[0]);

    FILE *sequential_file = fopen("sequential_results.txt", "w");
    FILE *parallel_file = fopen("parallel_results.txt", "w");

    for (int i = 0; i < num_methods; i++) {
        double start_time = omp_get_wtime();
        double result = methods[i]();
        double end_time = omp_get_wtime();
        double sequential_time = end_time - start_time;
        fprintf(sequential_file, "%f,", sequential_time);

        start_time = omp_get_wtime();
        #pragma omp parallel
        {
            result = methods[i]();
        }
        end_time = omp_get_wtime();
        double parallel_time = end_time - start_time;
        fprintf(parallel_file, "%f,", parallel_time);
    }

    fclose(sequential_file);
    fclose(parallel_file);

    return 0;
}