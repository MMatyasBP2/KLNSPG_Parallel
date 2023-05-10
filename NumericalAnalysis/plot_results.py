import matplotlib.pyplot as plt

def read_results(filename):
    with open(filename, "r") as f:
        lines = f.readlines()

    times = [float(time.strip()) for time in lines[0].split(',') if time.strip()]
    return times

def main():
    try:
        sequential_times = read_results("sequential_results.txt")
        parallel_times = read_results("parallel_results.txt")

        num_methods = len(sequential_times)
        x = list(range(1, num_methods + 1))

        speedup = [seq_time / par_time for seq_time, par_time in zip(sequential_times, parallel_times)]

        fig, ax1 = plt.subplots()

        ax1.plot(x, sequential_times, label="Sequential", marker="o")
        ax1.plot(x, parallel_times, label="Parallel", marker="o")
        ax1.set_xlabel("Method")
        ax1.set_ylabel("Execution Time (s)")
        ax1.legend(loc="upper left")
        ax1.set_title("Sequential vs Parallel Execution Times and Speedup")

        ax2 = ax1.twinx()
        ax2.plot(x, speedup, label="Speedup", marker="o", color="r")
        ax2.set_ylabel("Speedup")
        ax2.legend(loc="upper right")
        ax2.axhline(y=1, color='gray', linestyle='--')

        plt.show()
    except:
        print("Wrong file content! Please check your files!")

if __name__ == "__main__":
    main()