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
        public double Min { get; set; }
        public double Max { get; set; }
        public double Mean { get; set; }
        public double Median { get; set; }
        public double STD { get; set; }
        public double Variance { get; set; }
        public double RMS { get; set; }
        public double Skewness { get; set; }
        public double Kurtosis { get; set; }
        public double Covariance { get; set; }

        public Normal Normal = Normal.WithMeanStdDev(0, 1);
        public CancellationTokenSource cts;
        private object FileLocker = new object();
        public StatisticsController()
        {
            JObject jConfig = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("appconfig.json"));
            SampleCount = Convert.ToInt32(jConfig["MeasurementSettings"]["SampleCount"]);
        }

        #region Sampling
        public List<double> AddSamplesToList() => Enumerable.Range(0, SampleCount).Select(_ => Normal.Sample()).ToList();

        private void FillResultDictionary(int iterate, IDictionary<string, string> Results)
        {
            Results["Sample"] = $"{iterate}";
            Results["Minimum"] = $"{Min:F4}";
            Results["Maximum"] = $"{Max:F4}";
            Results["Mean"] = $"{Mean:F4}";
            Results["Median"] = $"{Median:F4}";
            Results["STD"] = $"{STD:F4}";
            Results["Variance"] = $"{Variance:F4}";
            Results["RMS"] = $"{RMS:F4}";
            Results["Skewness"] = $"{Skewness:F4}";
            Results["Kurtosis"] = $"{Kurtosis:F4}";
            Results["Covariance"] = $"{Covariance:F4}";
        }

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
                
                Min = CalcMin(samples);
                Max = CalcMax(samples);
                Mean = CalcMean(samples);
                Median = CalcMedian(samples);
                STD = CalcSTD(samples);
                Variance = CalcVariance(samples);
                RMS = CalcRMS(samples);
                Skewness = CalcSkewness(samples);
                Kurtosis = CalcKurtosis(samples);
                Covariance = CalcCovariance(samples);

                FillResultDictionary(i + 1, Results);

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
        #endregion

        #region Calculations
        private double CalcMin(List<double> samples) => samples.Min();

        private double CalcMax(List<double> samples) => samples.Min();

        private double CalcMean(List<double> samples) => samples.Mean();

        private double CalcMedian(List<double> samples)=> samples.Median();

        private double CalcSTD(List<double> samples) => samples.StandardDeviation();

        private double CalcVariance(List<double> samples) => samples.Variance();

        private double CalcRMS(List<double> samples) => samples.RootMeanSquare();

        private double CalcSkewness(List<double> samples) => samples.Skewness();

        private double CalcKurtosis(List<double> samples) => samples.Kurtosis();

        private double CalcCovariance(List<double> samples) => samples.Covariance(samples);
        #endregion 
    }
}
