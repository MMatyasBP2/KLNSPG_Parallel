#include <stdio.h>

int CountPrimes(int lower, int upper);

int main() {

    int lower = 2;

    int upper;

    printf("Please give me the upper number: ");
    scanf("%d", &upper);

    printf("Number of primes on the interval [%d,%d] is: %d", lower, upper, CountPrimes(lower, upper));

    return 0;
}

int CountPrimes(int lower, int upper){
    
    int primes = 0;

    for (int i = lower; i < upper; i++)
    {
        int change = 0;
        for (int j = 2; j <= i / 2; ++j)
        {
            if(i % j == 0)
            {
                change = 1;
                break;
            }
        }

        if(change == 0)
            primes += 1;
    }

    return primes;
}