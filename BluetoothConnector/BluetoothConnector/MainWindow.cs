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

        private void SearchButton_Click(object sender, EventArgs e)
        {
            com.DiscoveringList.Clear();
            DiscoveringProgressbar.Visible = true;
            com.DiscoverDevices();
            DevicesBox.DataSource = com.DiscoveringList;
        }
    }
}
