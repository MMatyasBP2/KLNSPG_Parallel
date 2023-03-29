#include <stdio.h>
#include <stdlib.h>

int main() {
    int n, pos = 0, neg = 0;
    int arr[n];

    printf("Give me the length of the series: ");
    
    scanf("%d", &n);
    printf("Give me the values of the series: ");
    
    for (int i = 0; i < n; i++)
    {
        scanf("%d", &arr[i]);
        if (arr[i] > 0)
            pos++;
        else if (arr[i] < 0)
            neg++;
    }
    
    printf("In the series there are %d positive and %d negative numbers.\n", pos, neg);
    
    return 0;
}