using MathNet.Numerics.Distributions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics.Statistics;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace StatisticalApp.Managing
{
    public class StatisticsController
    {
        public int SampleCount { get; set; }

        public Normal normal = Normal.WithMeanStdDev(0, 1);
        public CancellationTokenSource cts;
        private object FileLocker = new object();
        public StatisticsController()
        {
            JObject jConfig = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("appconfig.json"));
            SampleCount = Convert.ToInt32(jConfig["MeasurementSettings"]["SampleCount"]);
        }

        public List<double> AddSamplesToList() => Enumerable.Range(0, SampleCount).Select(_ => normal.Sample()).ToList();

        public async Task Sampling(Chart chart, RichTextBox SampleNameBox, RichTextBox SampleValueBox, IDictionary<string, string> Results)
        {
            cts = new CancellationTokenSource();

            for (int i = 0; i < SampleCount; i++)
            {
                chart.Series[0].Points.Clear();

                List<double> samples = AddSamplesToList();

                var histogram = new Histogram(samples, 10);
                for (int j = 0; j < histogram.BucketCount; j++)
                {
                    chart.Series[0].Points.AddXY(histogram[j].LowerBound, histogram[j].Count);
                }
                chart.Invalidate();

                double skewness = samples.Skewness();
                double kurtosis = samples.Kurtosis();
                double variance = samples.Variance();
                double stdDev = samples.StandardDeviation();
                double mean = samples.Mean();
                double median = samples.Median();
                double min = samples.Min();
                double max = samples.Max();
                double rms = samples.RootMeanSquare();

                Results["Skewness"] = $"{skewness:F4}";
                Results["Kurtosis"] = $"{kurtosis:F4}";
                Results["Variance"] = $"{variance:F4}";
                Results["Standard deviation"] = $"{stdDev:F4}";
                Results["Mean"] = $"{mean:F4}";
                Results["Median"] = $"{median:F4}";
                Results["Minimum"] = $"{min:F4}";
                Results["Maximum"] = $"{max:F4}";
                Results["RMS"] = $"{rms:F4}";

                SampleNameBox.Text = string.Join(Environment.NewLine, Results.Select(kv => $"{kv.Key}:"));
                SampleValueBox.Text = string.Join(Environment.NewLine, Results.Select(kv => $"{kv.Value}"));

                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "results.txt");
                lock (FileLocker)
                {
                    using (var writer = new StreamWriter(filePath, false))
                    {
                        writer.WriteLine($"Results from {i + 1} measurement from {SampleCount} samples:\n");
                        writer.WriteLine(string.Join(Environment.NewLine, Results.Select(kv => $"{kv.Key}: {kv.Value}")));
                    }
                }

                await Task.Delay(50);

                if (cts.Token.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        public void CancelSampling() => cts.Cancel();
    }
}
