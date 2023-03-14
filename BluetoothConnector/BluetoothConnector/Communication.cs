﻿using System;
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

namespace BluetoothConnector
{
    public class Communication
    {
        private readonly BluetoothClient bluetoothClient = new BluetoothClient();
    
        public async Task<string> DiscoverDevices()
        {
            BluetoothDeviceInfo[] devices;
            string devicesString = "";
            try
            {
                devices = await Task.Run(() => bluetoothClient.DiscoverDevices());

                foreach (BluetoothDeviceInfo device in devices)
                {
                    devicesString += device.DeviceName + "\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return devicesString;
        }
        public async void Initialize()
        {
            try
            {
                BluetoothDeviceInfo[] devices = await Task.Run(() => bluetoothClient.DiscoverDevices());
                BluetoothDeviceInfo device = devices[0];
                await Task.Run(() => bluetoothClient.Connect(device.DeviceAddress, BluetoothService.SerialPort));

                Stream stream = bluetoothClient.GetStream();
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
                bluetoothClient.Close();
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
