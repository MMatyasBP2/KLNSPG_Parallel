#include "Num.h"

double f(double x)
{
    return sin(x);
}

double df(double x)
{
    return cos(x);
}

double ddf(double x)
{
    return -sin(x);
}

double trapezoidal_integration_mid(double a, double b, int n)
{
    double h = (b - a) / n;
    double sum = 0.0;
    #pragma omp parallel for reduction(+:sum)
        for (int i = 0; i < n; i++)
        {
            double x = a + (i + 0.5) * h;
            sum += f(x);
        }
        return h * sum;
}

double trapezoidal_integration_left(double a, double b, int n)
{
    double h = (b - a) / n;
    double sum = 0.0;
    #pragma omp parallel for reduction(+:sum)
        for (int i = 0; i < n; i++)
        {
            double x = a + i * h;
            sum += f(x);
        }
    sum -= f(b) / 2.0;
    return h * sum;
}

double trapezoidal_integration_right(double a, double b, int n)
{
    double h = (b - a) / n;
    double sum = 0.0;
    #pragma omp parallel for reduction(+:sum)
        for (int i = 1; i <= n; i++)
        {
            double x = a + i * h;
            sum += f(x);
        }
    sum -= f(a) / 2.0;
    return h * sum;
}

double simpson_integration_1_3(double a, double b, int n)
{
    double h = (b - a) / n;
    double sum = f(a) + f(b);
    #pragma omp parallel for reduction(+:sum)
        for (int i = 1; i < n; i++)
        {
            double x = a + i * h;
            sum += 4.0 * f(x + 0.5 * h);
            sum += 2.0 * f(x);
        }
    return h / 3.0 * sum;
}

double simpson_integration_3_8(double a, double b, int n)
{
    double h = (b - a) / n;
    double sum = f(a) + f(b);
    #pragma omp parallel for reduction(+:sum)
        for (int i = 1; i < n; i++) {
            double x = a + i * h;
            sum += 3.0 * f(x + h / 3.0);
            sum += 3.0 * f(x + 2.0 * h / 3.0);
        }
    return 3.0 * h / 8.0 * sum;
}

double newton_deriv(double a, double h)
{
    return (f(a + h) - f(a)) / h;
}

double taylor_1_deriv(double a, double h)
{
    return (f(a + h) - f(a - h)) / (2 * h);
}

double taylor_2_deriv(double a, double h)
{
    return (f(a + h) - 2 * f(a) + f(a - h)) / (h * h);
}

void print_results(double a, double b, int n)
{
    printf("Numerical derivative (Newton method): %f\n", newton_deriv(a, 0.01));
    printf("1st order derivative (Taylor polynom): %f\n", taylor_1_deriv(a, 0.01));
    printf("2nd order derivative (Taylor polynom): %f\n", taylor_2_deriv(a, 0.01));
    printf("Trapezoidal integration (mid-point rule): %f\n", trapezoidal_integration_mid(a, b, n));
    printf("Trapezoidal integration (left-end rule): %f\n", trapezoidal_integration_left(a, b, n));
    printf("Trapezoidal integration (right-end rule): %f\n", trapezoidal_integration_right(a, b, n));
    printf("Simpson integration (1/3 rule): %f\n", simpson_integration_1_3(a, b, n));
    printf("Simpson integration (3/8 rule): %f\n", simpson_integration_3_8(a, b, n));
}

void write_results(double a, double b, int n)
{
    FILE* f = fopen("results.txt", "w");
    fprintf(f, "Results with 'a' = %.2lf, 'b' = %.2lf, 'n' = %d:\n", a, b, n);
    fprintf(f, "Numerical derivative (Newton method): %f\n", newton_deriv(a, 0.01));
    fprintf(f, "1st order derivative (Taylor polynom): %f\n", taylor_1_deriv(a, 0.01));
    fprintf(f, "2nd order derivative (Taylor polynom): %f\n", taylor_2_deriv(a, 0.01));
    fprintf(f, "Trapezoidal integration (mid-point rule): %f\n", trapezoidal_integration_mid(a, b, n));
    fprintf(f, "Trapezoidal integration (left-end rule): %f\n", trapezoidal_integration_left(a, b, n));
    fprintf(f, "Trapezoidal integration (right-end rule): %f\n", trapezoidal_integration_right(a, b, n));
    fprintf(f, "Simpson integration (1/3 rule): %f\n", simpson_integration_1_3(a, b, n));
    fprintf(f, "Simpson integration (3/8 rule): %f\n", simpson_integration_3_8(a, b, n));
    fclose(f);
}

void check_input(char* msg, double* a, double* b, int* n)
{
    bool ok;

    do
    {
        ok = true;
        puts(msg);
        if (scanf("%lf, %lf, %d", a, b, n) != 3)
        {
            printf("Wrong input! Please retry!\n");
            while ((getchar()) != '\n');
            ok = false;
        }
        else if (*a <= 0 || *b <= 0 || *a == *b)
        {
            printf("Wrong input! Numbers must be positive!\n");
            while ((getchar()) != '\n');
            ok = false;
        }
        else if (*n <= 0 || *n > 1000000)
        {
            printf("Wrong input! Length must be between 0 and 1.000.000!\n");
            while ((getchar()) != '\n');
            ok = false;
        } 
    } while (!ok);
}

int main()
{
    double a, b;
    int n;
    double start_time, end_time;

    check_input("Please give me the values 'a', 'b' and 'n' with comma separated: ", &a, &b, &n);

    start_time = omp_get_wtime();

    #pragma omp parallel sections
        {
    #pragma omp section
            {
                print_results(a, b, n);
            }
    #pragma omp section
            {
                write_results(a, b, n);
            }
        }
    
    end_time = omp_get_wtime();

    double elapsed_time = end_time - start_time;

    printf("\nConclusion:\n");
    printf("Elapsed time with multithreads: %f seconds.\n", elapsed_time);
    printf("Elapsed time with one thread: %f seconds.", elapsed_time * omp_get_num_procs());

    return 0;
}