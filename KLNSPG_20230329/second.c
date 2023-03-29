#include <stdio.h>

int main() {
    int n, zeros = 0;
    int arr[n];

    printf("Give me the length of the series: ");
    scanf("%d", &n);
    printf("Give me the items of the series: ");
    for (int i = 0; i < n; i++)
    {
        scanf("%d", &arr[i]);
        if (arr[i] == 0)
            zeros++;
    }
    
    printf("In the series there are %d zeros.\n", zeros);
    
    return 0;
}