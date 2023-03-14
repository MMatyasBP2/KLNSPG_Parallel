using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BluetoothConnector
{
    public partial class MainWindow : Form
    {
        Communication com = new Communication();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            DevicesBox.Items.Add(await com.DiscoverDevices());
        }
    }
}
