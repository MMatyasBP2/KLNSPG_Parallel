using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using StatisticalApp.Managing;

namespace StatisticalApp
{
    public partial class MainWindow : Form
    {
        private readonly IDictionary<string, string> Results;
        private readonly StatisticsController Statistics;
        private readonly ChartController Charts;
        private Chart Chart;
        private bool isRunnning;

        public MainWindow()
        {
            InitializeComponent();
            Statistics = new StatisticsController();
            Charts = new ChartController();
            Results = new Dictionary<string, string>();
            Chart = new Chart();
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            StatLabel.Visible = false;
            StatNameBox.Visible = false;
            StatValueBox.Visible = false;

            Results.Clear();
            SampleNameBox.Clear();
            StatNameBox.Clear();

            try
            {
                Chart = Charts.SetupChartSettings();

                isRunnning = true;

                await Statistics.Sampling(Chart, SampleNameBox, SampleValueBox, Results);

                isRunnning = false;
            }
            catch (OperationCanceledException)
            {
            }

            StatLabel.Visible = true;
            StatNameBox.Visible = true;
            StatValueBox.Visible = true;

            StatNameBox.Text = string.Join(Environment.NewLine, Results.Select(kv => $"{kv.Key}:"));
            StatValueBox.Text = string.Join(Environment.NewLine, Results.Select(kv => $"{kv.Value}"));
        }

        private void Lighting()
        {

        }

        private void StopButton_Click(object sender, EventArgs e) => Statistics.CancelSampling();

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