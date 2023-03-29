#include <stdio.h>

int main() {
    int n, evens = 0, odds = 0;
    int arr[n];

    printf("Give me the length of the series: ");
    scanf("%d", &n);
    
    printf("Give me the items of the series: ");
    for (int i = 0; i < n; i++)
    {
        scanf("%d", &arr[i]);
        if (arr[i] % 2 == 0)
            evens++;
        else
            odds++;
    }
    printf("In the series there are %d even and %d odd numbers.\n", evens, odds);
    
    return 0;
}