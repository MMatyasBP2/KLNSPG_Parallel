using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace StatisticalApp.Managing
{
    public class ChartController
    {
        private CancellationTokenSource cts;

        public Chart SetupChartSettings()
        {
            Chart Chart = new Chart();
            var chartArea = new ChartArea();
            chartArea.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
            chartArea.AxisX.Title = "Sample Value";
            chartArea.AxisY.Title = "Relative Frequency";
            Chart.ChartAreas.Add(chartArea);

            var series = new System.Windows.Forms.DataVisualization.Charting.Series();
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 3;
            Chart.Series.Add(series);

            return Chart;
        }

        public void Charting(int SampleCount, Chart chart)
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();

            cts = new CancellationTokenSource();

            chart.Series[0].ChartType = SeriesChartType.Column;

            var form = new Form();
            form.Width = 320;
            form.Height = 350;
            form.ShowIcon = false;
            form.Controls.Add(chart);

            Task.Run(() =>
            {
                while (!cts.Token.IsCancellationRequested)
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
            }, cts.Token);

            form.FormClosing += (s, ev) => cts.Cancel();

            form.ShowDialog();
        }
    }
}
