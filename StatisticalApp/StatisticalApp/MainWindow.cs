using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using StatisticalApp.Managing;

namespace StatisticalApp
{
    public partial class MainWindow : Form
    {
        private readonly IDictionary<string, string> Results;
        private readonly StatisticsController Statistics;
        private Chart Chart;
        private Thread lightingThread;
        private volatile bool isRunning;
        private readonly LedUpdate LedUpdate;

        public MainWindow()
        {
            InitializeComponent();
            Statistics = new StatisticsController();
            Results = new Dictionary<string, string>();
            Chart = new Chart();
            isRunning = false;
            LedUpdate = new LedUpdate();
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            StatLabel.Visible = false;
            StatNameBox.Visible = false;
            StatValueBox.Visible = false;
            ResultButton.Visible = false;
            ParallelResultBox.Visible = false;

            Results.Clear();
            SampleNameBox.Clear();
            StatNameBox.Clear();

            try
            {
                Chart = ChartController.SetupChartSettings();

                isRunning = true;

                lightingThread = new Thread(Lighting);
                lightingThread.IsBackground = true;
                lightingThread.Start();

                await Statistics.Sampling(Chart, SampleNameBox, SampleValueBox, Results);

                lightingThread.Abort();
                GreenLight.Visible = false;
            }
            catch (OperationCanceledException)
            {
            }

            StatLabel.Visible = true;
            StatNameBox.Visible = true;
            StatValueBox.Visible = true;
            ResultButton.Visible = true;

            StatNameBox.Text = string.Join(Environment.NewLine, Results.Select(kv => $"{kv.Key}:"));
            StatValueBox.Text = string.Join(Environment.NewLine, Results.Select(kv => $"{kv.Value}"));
        }

        private void Lighting() => LedUpdate.ModifyLedActivity(isRunning, GreenLight);

        private void StopButton_Click(object sender, EventArgs e) => Statistics.CancelSampling();

        private void PlotButton_Click(object sender, EventArgs e)
        {
            if (Statistics.cts == null || Statistics.cts.IsCancellationRequested)
            {
                MessageBox.Show("Please start sampling before plotting the normal distribution!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ChartController.Charting(Statistics.SampleCount, Chart);
        }

        private void ConfigButton_Click(object sender, EventArgs e) => Process.Start("appconfig.json");

        private void ResultButton_Click(object sender, EventArgs e) => Process.Start("results.txt");
    }
}