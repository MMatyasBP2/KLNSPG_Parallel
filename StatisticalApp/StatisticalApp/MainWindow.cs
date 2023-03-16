using MathNet.Numerics.Statistics;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.Distributions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows.Forms.DataVisualization.Charting;

namespace StatisticalApp
{
    public partial class MainWindow : Form
    {
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IDictionary<string, string> Results = new Dictionary<string, string>();
        private static readonly object FileLocking = new object();
        private int SampleCount;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            StatLabel.Visible = false;
            StatBox.Visible = false;
            Results.Clear();
            SampleBox.Clear();
            StatBox.Clear();

            try
            {
                string json = File.ReadAllText("appconfig.json");
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                SampleCount = jsonObj.SampleCount;

                var normal = Normal.WithMeanStdDev(0, 1);

                _cancellationTokenSource = new CancellationTokenSource();

                for (int i = 0; i < SampleCount; i++)
                {
                    var samples = Enumerable.Range(0, SampleCount)
                        .Select(_ => normal.Sample())
                        .ToList();

                    var skewness = samples.Skewness();
                    var kurtosis = samples.Kurtosis();
                    var variance = samples.Variance();
                    var stdDev = samples.StandardDeviation();
                    var mean = samples.Mean();
                    var median = samples.Median();
                    var min = samples.Min();
                    var max = samples.Max();
                    var rms = samples.RootMeanSquare();

                    Results["Skewness"] = $"{skewness:F4}";
                    Results["Kurtosis"] = $"{kurtosis:F4}";
                    Results["Variance"] = $"{variance:F4}";
                    Results["Standard deviation"] = $"{stdDev:F4}";
                    Results["Mean"] = $"{mean:F4}";
                    Results["Median"] = $"{median:F4}";
                    Results["Minimum"] = $"{min:F4}";
                    Results["Maximum"] = $"{max:F4}";
                    Results["RMS"] = $"{rms:F4}";

                    SampleBox.Text = string.Join(Environment.NewLine, Results.Select(kv => $"{kv.Key}: {kv.Value}"));

                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "results.txt");
                    lock (FileLocking)
                    {
                        using (var writer = new StreamWriter(filePath, false))
                        {
                            writer.WriteLine($"Results from {i} measurement from {SampleCount} samples:\n");
                            writer.WriteLine(string.Join("\n", SampleBox.Text));
                        }
                    }

                    await Task.Delay(50);

                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }

            StatLabel.Visible = true;
            StatBox.Visible = true;
            StatBox.Text = string.Join(Environment.NewLine, Results.Select(kv => $"{kv.Key}: {kv.Value}"));
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
