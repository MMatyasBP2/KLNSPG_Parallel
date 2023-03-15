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
        private readonly Communication Com = new Communication();
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
            Com.DiscoveringList.Clear();
            DevicesBox.DataSource = null;
            DiscoveringProgressbar.Value = 0;

            Com.DiscoverDevices();
            DevicesBox.DataSource = Com.DiscoveringList;
            
            
            if (Com.DiscoveringList.Count != 0)
            {
                DiscoveringProgressbar.Visible = true;
                DiscoveringProgressbar.Value = 100;
            }
        }
    }
}
