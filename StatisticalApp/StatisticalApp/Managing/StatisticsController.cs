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
        public StatisticsController()
        {
            JObject jConfig = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("appconfig.json"));
            SampleCount = Convert.ToInt32(jConfig["MeasurementSettings"]["SampleCount"]);
        }

        public List<double> AddSamplesToList()
        {
            return Enumerable.Range(0, SampleCount).Select(_ => normal.Sample()).ToList();
        }

        public async Task Sampling(Chart chart, object fileLocking, RichTextBox SampleBox, IDictionary<string, string> Results)
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
                lock (fileLocking)
                {
                    using (var writer = new StreamWriter(filePath, false))
                    {
                        writer.WriteLine($"Results from {i + 1} measurement from {SampleCount} samples:\n");
                        writer.WriteLine(string.Join("\n", SampleBox.Text));
                    }
                }

                await Task.Delay(50);

                if (cts.Token.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        public void CancelSampling() { cts.Cancel(); }
    }
}
