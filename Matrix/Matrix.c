#include <stdio.h>
#include <stdlib.h>
#include <omp.h>

#define N 5

void print_matrix(double *matrix, int n) {
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            printf("%.2f |", matrix[i*n + j]);
        }
        printf("\n");
    }
    printf("\n");
}

void add_matrices(double *matrix1, double *matrix2, double *result, int n) {
    #pragma omp parallel for
    for (int i = 0; i < n*n; i++) {
        result[i] = matrix1[i] + matrix2[i];
    }
}

void multiply_matrices(double *matrix1, double *matrix2, double *result, int n) {
    #pragma omp parallel for
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            double sum = 0.0;
            for (int k = 0; k < n; k++) {
                sum += matrix1[i*n + k] * matrix2[k*n + j];
            }
            result[i*n + j] = sum;
        }
    }
}

double determinant(double* matrix, int n) {
    double det = 0;
    double* submatrix = (double*) malloc((n-1)*(n-1)*sizeof(double));
    int sign = 1;
    if (n == 1) {
        det = matrix[0];
    } else if (n == 2) {
        det = matrix[0]*matrix[3] - matrix[1]*matrix[2];
    } else {
        for (int k = 0; k < n; k++) {
            int i_sub = 0;
            for (int i = 1; i < n; i++) {
                int j_sub = 0;
                for (int j = 0; j < n; j++) {
                    if (j != k) {
                        submatrix[i_sub*(n-1) + j_sub] = matrix[i*n + j];
                        j_sub++;
                    }
                }
                i_sub++;
            }
            det += sign * matrix[k] * determinant(submatrix, n-1);
            sign = -sign;
        }
    }
    free(submatrix);
    return det;
}

void cofactor(double* matrix, double* result, int n) {
    double* submatrix = (double*) malloc((n-1)*(n-1)*sizeof(double));
    int sign = 1;
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            int i_sub = 0;
            for (int i_m = 0; i_m < n; i_m++) {
                if (i_m != i) {
                    int j_sub = 0;
                    for (int j_m = 0; j_m < n; j_m++) {
                        if (j_m != j) {
                            submatrix[i_sub*(n-1) + j_sub] = matrix[i_m*n + j_m];
                            j_sub++;
                        }
                    }
                    i_sub++;
                }
            }
            double det_submatrix = determinant(submatrix, n-1);
            result[i*n + j] = sign * det_submatrix;
            sign = -sign;
        }
    }
    free(submatrix);
}

void get_submatrix(double* matrix, double* result, int row, int col, int n) {
    int index = 0;
    for(int i=0; i<n; i++) {
        for(int j=0; j<n; j++) {
            if(i != row && j != col) {
                result[index++] = matrix[i*n+j];
            }
        }
    }
}

void adjoint(double* matrix, double* result, int n) {
    double* submatrix = (double*) malloc((n-1)*(n-1) * sizeof(double));
    double sign;
    for(int i=0; i<n; i++) {
        for(int j=0; j<n; j++) {
            sign = ((i+j) % 2 == 0) ? 1.0 : -1.0;
            get_submatrix(matrix, submatrix, i, j, n);
            result[j*n+i] = sign * determinant(submatrix, n-1);
        }
    }
    free(submatrix);
}

void invert_matrix(double* matrix, double* result, int n) {
    double det = determinant(matrix, n);
    if (det == 0) {
        printf("Cannot invert matrix, determinant is zero\n");
        return;
    }
    double* adj = (double*) malloc(n*n*sizeof(double));
    adjoint(matrix, adj, n);
    #pragma omp parallel for
    for (int i = 0; i < n*n; i++) {
        result[i] = adj[i] / det;
    }
    free(adj);
}

int main()
{
    int n = N;
    double *matrix1 = (double*) malloc(n*n*sizeof(double));
    double *matrix2 = (double*) malloc(n*n*sizeof(double));
    double *result = (double*) malloc(n*n*sizeof(double));

    #pragma omp parallel for
    for (int i = 0; i < n*n; i++) {
        matrix1[i] = rand() % 100 - 11 + 10;
        matrix2[i] = rand() % 100 - 11 + 10;
    }

    printf("Matrix1:\n");
    print_matrix(matrix1, n);
    printf("Matrix2:\n");
    print_matrix(matrix2, n);
    add_matrices(matrix1, matrix2, result, n);
    printf("Addition result:\n");
    print_matrix(result, n);

    printf("Matrix1:\n");
    print_matrix(matrix1, n);
    printf("Matrix2:\n");
    print_matrix(matrix2, n);
    multiply_matrices(matrix1, matrix2, result, n);
    printf("Multiplication result:\n");
    print_matrix(result, n);

    printf("Matrix1:\n");
    print_matrix(matrix1, n);
    invert_matrix(matrix1, result, n);
    printf("Inversion result:\n");
    print_matrix(result, n);

    free(matrix1);
    free(matrix2);
    free(result);

    return 0;
}