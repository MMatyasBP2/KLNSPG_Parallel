using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace StatisticalApp.Managing
{
    public class WindowUpdate : DispatcherObject
    {
        public void ModifyLedActivity(bool isRunning, PictureBox GreenLight)
        {
            while (isRunning)
            {
                Thread.Sleep(250);

                Dispatcher.Invoke(() =>
                {
                    GreenLight.Visible = true;
                });

                Thread.Sleep(250);

                Dispatcher.Invoke(() =>
                {
                    GreenLight.Visible = false;
                });
            }
        }
    }
}
