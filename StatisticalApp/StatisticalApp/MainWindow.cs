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

namespace StatisticalApp
{
    public partial class MainWindow : Form
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly IDictionary<string, string> _results = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            // clear results dictionary and textboxes
            _results.Clear();
            SampleBox.Clear();
            StatBox.Clear();

            // read sample size from config file (here we use a constant instead)
            string json = File.ReadAllText("appconfig.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            int sampleSize = jsonObj.SampleCount;

            // create distribution
            var normal = Normal.WithMeanStdDev(0, 1);

            try
            {
                while (true)
                {
                    // check for cancellation
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    // simulate samples
                    var samples = Enumerable.Range(0, sampleSize)
                        .Select(_ => normal.Sample())
                        .ToList();

                    // calculate statistics
                    var medianAbsDev = samples.Median();
                    var skewness = samples.Skewness();
                    var kurtosis = samples.Kurtosis();
                    var variance = samples.Variance();
                    var stdDev = samples.StandardDeviation();
                    var mean = samples.Mean();
                    var median = samples.Median();
                    var min = samples.Min();
                    var max = samples.Max();

                    // update results dictionary
                    _results["Sample size"] = $"{sampleSize}";
                    _results["Median absolute deviation"] = $"{medianAbsDev:F4}";
                    _results["Skewness"] = $"{skewness:F4}";
                    _results["Kurtosis"] = $"{kurtosis:F4}";
                    _results["Variance"] = $"{variance:F4}";
                    _results["Standard deviation"] = $"{stdDev:F4}";
                    _results["Mean"] = $"{mean:F4}";
                    _results["Median"] = $"{median:F4}";
                    _results["Minimum"] = $"{min:F4}";
                    _results["Maximum"] = $"{max:F4}";

                    // update results textbox
                    SampleBox.Text = string.Join(Environment.NewLine, _results.Select(kv => $"{kv.Key}: {kv.Value}"));

                    // write results to file
                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "results.txt");
                    using (var writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine(string.Join(",", _results.Values));
                    }

                    // wait 2 seconds
                    await Task.Delay(50, _cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // do nothing, just exit the loop
            }

            // display final stats in stats textbox
            StatBox.Text = string.Join(Environment.NewLine, _results.Select(kv => $"{kv.Key}: {kv.Value}"));
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
