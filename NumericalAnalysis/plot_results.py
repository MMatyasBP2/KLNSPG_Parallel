import matplotlib.pyplot as plt


def read_results(filename):
    with open(filename, "r") as file:
        lines = file.readlines()

    times = [float(time.strip()) for time in lines[0].split(',') if time.strip()]
    return times


def main():
    try:
        sequential_times = read_results("sequential_results.txt")
        parallel_times = read_results("parallel_results.txt")

        x = list(range(1, len(sequential_times) + 1))
        plt.plot(x, sequential_times, label="Sequential", marker="o")
        plt.plot(x, parallel_times, label="Parallel", marker="x")

        plt.xlabel("Method")
        plt.ylabel("Time (s)")
        plt.title("Sequential vs Parallel Execution Time")
        plt.legend()

        plt.show()
    except:
        print("Wrong file content! Please check your files!")

if __name__ == "__main__":
    main()