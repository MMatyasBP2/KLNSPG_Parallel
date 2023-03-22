#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include<stdbool.h>

void fillArany(int* array, int lower, int upper, int size);
int sumArray(int* array, int size);
int recursiveArraySum(int* array, int size);
int minArray(int* array, int size);
int maxArray(int* array, int size);
bool recursiveArrayMin(int* array, int size);
bool recursiveArrayMax(int* array, int size);
bool minInterval(int a, int b);
bool maxInterval(int a, int b);

int main()
{
   int lower = 100;
   int upper = 300;
   int size = 50;
   int array[size];

   fillArany(array, lower, upper, size);
   int sum = sumArray(array, size);
   int recursiveSum = recursiveArraySum(array, size);
   int min = minArray(array, size);
   int max = maxArray(array, size);
   bool recursiveMin = recursiveArrayMin(array, size);
   bool recursiveMax = recursiveArrayMax(array, size);

   printf("Array sum: %d, \nArray sum with recursion: %d, \nArray min: %d, \nArray min with recursion: %d, \nArray max: %d\nArray max with recursion: %d \n", sum, recursiveSum, min, recursiveMin, max, recursiveMax);

   return 0;
}

void fillArany(int* array, int lower, int upper, int size)
{
   if (size < 1)
   {
      printf("Error. Array must have more elements than 0.");
      exit(1);
   }

   if (lower > upper)
   {
      printf("Error. Lower interval is higher than upper interval.");
      exit(1);
   }

   srand(time(0));

    int num;

   for (int i = 0; i < size; i++)
   {
      num = (rand() % (upper - lower + 1)) + lower;
      array[i] = num;
   }
}

int sumArray(int* array, int size)
{
    int sum = 0;

    for (int i = 0; i < size; i++)
        sum += array[i];
    
    return sum;
}

int array_min(int* array, int size)
{
    int min = array[0];

    for (int i = 1; i < size; i++)
    {
        if (array[i] < min)
            min = array[i];        
    }
    return min;
}

int array_max(int* array, int size)
{
    int max = array[0];

    for (int i = 1; i < size; i++)
    {
        if (array[i] > max)
            max = array[i];
    }

    return max;
}

int recursiveArraySum(int* array, int size)
{
    if (size <= 0)
        return 0;
    return (recursiveArraySum(array, size - 1) + array[size - 1]);
}

bool recursiveArrayMin(int* array, int size)
{
    if (size == 1)
        return array[0];
    return minInterval(array[size-1], recursiveArrayMin(array, size-1));
}

bool recursiveArrayMax(int* array, int size)
{
    if (size == 1)
        return array[0];
    return maxInterval(array[size-1], recursiveArrayMax(array, size-1));
}

bool maxInterval(int a, int b)
{
    return a > b ? a : b;
}

bool minInterval(int a, int b)
{
    return a < b ? a : b;
}