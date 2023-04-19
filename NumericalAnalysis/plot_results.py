import os
import subprocess
import numpy as np
import matplotlib.pyplot as plt

def run_c_code(a, b, n):
    os.environ['a'] = str(a)
    os.environ['b'] = str(b)
    os.environ['n'] = str(n)

    output = subprocess.check_output("./Num").decode("utf-8")
    results = [float(line.split(':')[-1]) for line in output.split('\n') if ':' in line]

    return results

def main():
    a_values = np.linspace(1, 10, 10)
    b_values = np.linspace(1, 10, 10)
    n_values = np.linspace(1000, 10000, 10, dtype=int)

    for a, b, n in zip(a_values, b_values, n_values):
        results = run_c_code(a, b, n)
        plt.plot(results, label=f'a={a}, b={b}, n={n}')

    plt.xlabel('Method')
    plt.ylabel('Result')
    plt.legend()
    plt.title('Numerical Methods Results')
    plt.xticks(range(8), ['Newton', 'Taylor 1st', 'Taylor 2nd', 'Trap. Mid', 'Trap. Left', 'Trap. Right', 'Simp. 1/3', 'Simp. 3/8'])
    plt.show()

if __name__ == "__main__":
    main()