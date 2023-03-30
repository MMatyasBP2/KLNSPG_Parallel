using MathNet.Numerics.Statistics;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace StatisticalApp.Managing
{
    public static class ChartController
    {
        private static CancellationTokenSource cts;
        private readonly static StatisticsController Stat = new StatisticsController();

        public static Chart SetupChartSettings()
        {
            var chart = new Chart();
            chart.ChartAreas.Add(new ChartArea("Default"));

            chart.ChartAreas[0].AxisX.Minimum = 0;
            chart.ChartAreas[0].AxisX.Maximum = 10;
            chart.ChartAreas[0].AxisY.Minimum = -5;
            chart.ChartAreas[0].AxisY.Maximum = 5;

            chart.Series.Add(new Series());
            chart.Series[0].ChartType = SeriesChartType.Column;

            return chart;
        }

        public static void Charting(int SampleCount, Chart chart)
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();

            cts = new CancellationTokenSource();

            chart.Series[0].ChartType = SeriesChartType.Column;
            var form = GenerateChartingForm(chart);

            Task.Run(() =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    var samples = Stat.AddSamplesToList();

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
                        return;
                    }

                    Thread.Sleep(50);
                }
            }, cts.Token);

            form.FormClosing += (s, ev) => cts.Cancel();

            form.ShowDialog();
        }

        private static Form GenerateChartingForm(Chart chart)
        {
            var form = new Form();
            form.Width = 320;
            form.Height = 350;
            form.ShowIcon = false;
            form.Controls.Add(chart);
            return form;
        }
    }
}
