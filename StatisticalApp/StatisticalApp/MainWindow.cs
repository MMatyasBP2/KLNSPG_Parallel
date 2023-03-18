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
using StatisticalApp.Managing;

namespace StatisticalApp
{
    public partial class MainWindow : Form
    {
        private readonly IDictionary<string, string> Results;
        private readonly Chart Chart;
        private readonly object FileLocking;
        private readonly StatisticsController Statistics;
        private readonly ChartController Charts;

        public MainWindow()
        {
            InitializeComponent();
            Statistics = new StatisticsController();
            Charts = new ChartController();
            Results = new Dictionary<string, string>();
            Chart = new Chart();
            FileLocking = new object();
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
                var chart = Charts.SetupChartSettings();
                await Statistics.Sampling(chart, FileLocking, SampleBox, Results);
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
            Statistics.CancelSampling();
        }

        private void PlotButton_Click(object sender, EventArgs e)
        {
            if (Statistics.cts == null || Statistics.cts.IsCancellationRequested)
            {
                MessageBox.Show("Please start sampling before plotting the normal distribution!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Charts.Charting(Statistics.SampleCount, Chart);
        }
    }
}