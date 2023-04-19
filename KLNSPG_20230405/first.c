#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <sys/time.h>
#include <sys/types.h>
#include <pthread.h>

#define N 5000
#define THREADNUMBER 10

typedef struct arg_struct {
    int arg1;
    int* arg2;
} Args;


void sumofarray(void* param1)
{
	int i,sum;
	int *array;
	Args *args = param1;
	
	i=args->arg1;
	array=args->arg2;
	sum=0;
	for(;i<N;i+=THREADNUMBER){
		sum+=array[i];
	}
	sleep(1);
	pthread_exit(sum);
}


int main(int argc, char* argv[])
{
	int array[N],i,sum_all;
	void *sum[THREADNUMBER];
	pthread_t threads[THREADNUMBER];
	Args args[THREADNUMBER];
	
	
	srand(time(NULL));

	sum_all = 0;

	printf("Generated array\n");

	for(i = 0; i < N; i++)
		array[i] = rand()%100;
	
    printf("Created threads\n");
	
    clock_t begin = clock();
	
    for(i = 0; i < THREADNUMBER; i++){
		args[i].arg1 = i;
		args[i].arg2 = array;
		pthread_create(&threads[i], NULL, sumofarray, (void *)&args[i]);
	}
	printf("Waiting for threads\n");
	for(i = 0; i < THREADNUMBER; i++)
		pthread_join(threads[i], &sum[i]);

	printf("Sum of array\n");
	for(i = 0; i < THREADNUMBER; i++)
		sum_all+=sum[i];

	clock_t end = clock();
	
    double time_spent = (double)(end - begin) / CLOCKS_PER_SEC;
	
    printf("Sum: in %d\ntime: %.12lf",sum_all,time_spent);
	
    return 0;
}