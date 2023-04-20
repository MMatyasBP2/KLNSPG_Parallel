import matplotlib.pyplot as plt

def read_results(filename):
    times = []
    with open(filename, "r") as f:
        for line in f.readlines():
            data = line.strip().split(',')
            for time_str in data[:-1]:
                time = float(time_str)
                times.append(time)
    return times

def main():
    x = list(range(1, 9))

    sequential_times = read_results("sequential_results.txt")
    if len(sequential_times) == len(x):
        plt.plot(x, sequential_times, label="Sequential", marker="o")

    parallel_times = read_results("parallel_results.txt")
    if len(parallel_times) == len(x):
        plt.plot(x, parallel_times, label="Parallel", marker="o")

    plt.xlabel("Method")
    plt.ylabel("Execution Time (s)")
    plt.title("Execution Time Comparison")
    plt.legend()
    plt.xticks(range(1, 9), ['Newton', 'Taylor 1st', 'Taylor 2nd', 'Trap. Mid', 'Trap. Left', 'Trap. Right', 'Simp. 1/3', 'Simp. 3/8'])
    plt.grid()
    plt.show()

if __name__ == "__main__":
    main()