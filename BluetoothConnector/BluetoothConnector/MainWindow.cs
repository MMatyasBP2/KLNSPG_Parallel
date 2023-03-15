using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BluetoothConnector
{
    public partial class MainWindow : Form
    {
        private Communication com = new Communication();
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// TODO:
        /// - Reset DeviceBox
        /// - ProgressBar Async working while Bluetooth discovering
        /// - Async connection started
        /// - Async message send and receive function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SearchButton_Click(object sender, EventArgs e)
        {
            com.DiscoveringList.Clear();
            DevicesBox.DataSource = null;

            DiscoveringProgressbar.Visible = true;
            com.DiscoverDevices();
            DevicesBox.DataSource = com.DiscoveringList;
        }
    }
}
