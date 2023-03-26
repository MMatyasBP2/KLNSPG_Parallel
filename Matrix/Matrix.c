#include <stdio.h>
#include <stdlib.h>
#include <omp.h>

#define N 1000

int main() {
  int i, j, k;
  double start_time, end_time;

  double **matrixA = (double**)malloc(N * sizeof(double*));
  double **matrixB = (double**)malloc(N * sizeof(double*));
  double **resultMatrix = (double**)malloc(N * sizeof(double*));

  for(i = 0; i < N; i++) {
    matrixA[i] = (double*)malloc(N * sizeof(double));
    matrixB[i] = (double*)malloc(N * sizeof(double));
    resultMatrix[i] = (double*)malloc(N * sizeof(double));
  }

  for(i = 0; i < N; i++) {
    for(j = 0; j < N; j++) {
      matrixA[i][j] = (double)rand() / RAND_MAX;
      matrixB[i][j] = (double)rand() / RAND_MAX;
    }
  }

  start_time = omp_get_wtime();
  #pragma omp parallel for private(i, j, k) shared(matrixA, matrixB, resultMatrix)
  for(i = 0; i < N; i++) {
    for(j = 0; j < N; j++) {
      for(k = 0; k < N; k++) {
        resultMatrix[i][j] += matrixA[i][k] * matrixB[k][j];
      }
    }
  }
  end_time = omp_get_wtime();

  printf("Matrix multiply time: %f seconds\n", end_time - start_time);

  for(i = 0; i < N; i++) {
    free(matrixA[i]);
    free(matrixB[i]);
    free(resultMatrix[i]);
  }
  free(matrixA);
  free(matrixB);
  free(resultMatrix);

  return 0;
}