using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.AttributeIds;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BluetoothConnector
{
    public class Communication
    {
        public List<int> DiscoveringProgressBar = new List<int>();
        public List<string> DiscoveringList = new List<string>();
        private readonly BluetoothClient _bluetoothClient = new BluetoothClient();

        public void DiscoverDevices()
        {
            BluetoothDeviceInfo[] devices = null;

            try
            {
                devices = _bluetoothClient.DiscoverDevices();

                foreach (var item in devices)
                {
                    DiscoveringList.Add(item.DeviceName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void Initialize()
        {
            try
            {
                BluetoothDeviceInfo[] devices = await Task.Run(() => _bluetoothClient.DiscoverDevices());
                BluetoothDeviceInfo device = devices[0];
                await Task.Run(() => _bluetoothClient.Connect(device.DeviceAddress, BluetoothService.SerialPort));

                Stream stream = _bluetoothClient.GetStream();
                await SendMessageAsync(stream, "Hello, Bluetooth device!");
                string response = await ReceiveMessageAsync(stream);

                MessageBox.Show(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _bluetoothClient.Close();
            }
        }

        private async Task SendMessageAsync(Stream stream, string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }

        private async Task<string> ReceiveMessageAsync(Stream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
    }
}
