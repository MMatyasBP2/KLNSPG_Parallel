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
        private CancellationTokenSource _plotCancellationTokenSource;
        private readonly IDictionary<string, string> Results = new Dictionary<string, string>();
        private static readonly object FileLocking = new object();
        private int SampleCount;
        private Chart chart = new Chart();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlotButton_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
            {
                MessageBox.Show("Please start sampling before plotting the normal distribution!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_plotCancellationTokenSource != null && !_plotCancellationTokenSource.IsCancellationRequested)
            {
                _plotCancellationTokenSource.Cancel();
            }

            _plotCancellationTokenSource = new CancellationTokenSource();

            chart.ChartAreas.Add(new ChartArea());
            chart.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series());
            chart.Series[0].ChartType = SeriesChartType.Column;

            var form = new Form();
            form.Width = 600;
            form.Height = 400;
            form.Controls.Add(chart);

            Task.Run(() =>
            {
                while (!_plotCancellationTokenSource.Token.IsCancellationRequested)
                {
                    var normal = Normal.WithMeanStdDev(0, 1);

                    var samples = Enumerable.Range(0, SampleCount)
                        .Select(_ => normal.Sample())
                        .ToList();

                    var histogram = new Histogram(samples, 10);

                    try
                    {
                        chart.Invoke(new Action(() =>
                        {
                            chart.Series[0].Points.Clear();

                            for (int i = 0; i < histogram.BucketCount; i++)
                            {
                                chart.Series[0].Points.AddXY(histogram[i].LowerBound, histogram[i].Count);
                            }

                            chart.Invalidate();
                        }));
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Please restart sampling!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    Thread.Sleep(50);
                }
            }, _plotCancellationTokenSource.Token);

            form.FormClosing += (s, ev) => _plotCancellationTokenSource.Cancel();

            form.ShowDialog();
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


                var chart = new Chart();
                var chartArea = new ChartArea();
                chartArea.AxisX.Title = "Sample Value";
                chartArea.AxisY.Title = "Frequency";
                chart.ChartAreas.Add(chartArea);

                var series = new System.Windows.Forms.DataVisualization.Charting.Series();
                series.ChartType = SeriesChartType.Line;
                series.BorderWidth = 3;
                chart.Series.Add(series);

                for (int i = 0; i < SampleCount; i++)
                {
                    chart.Series[0].Points.Clear();

                    var samples = Enumerable.Range(0, SampleCount)
                        .Select(_ => normal.Sample())
                        .ToList();

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
                    lock (FileLocking)
                    {
                        using (var writer = new StreamWriter(filePath, false))
                        {
                            writer.WriteLine($"Results from {i + 1} measurement from {SampleCount} samples:\n");
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