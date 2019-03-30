using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Text.RegularExpressions;

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace AttandanceChecker
{
    public partial class Form1 : Form
    {
        List<BluetoothDeviceInfo> AvailableDevicesList = new List<BluetoothDeviceInfo>();
        public delegate void UpdateDiscoverBox(List<BluetoothDeviceInfo> list);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AvailableDevicesList.Clear();
            this.Cursor = Cursors.WaitCursor;
            button1.Enabled = false;
            FindBluetoothDevices(null);
        }

        private void FindBluetoothDevices(List<BluetoothDeviceInfo> list)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            BluetoothClient bluc = new BluetoothClient();
            BluetoothDeviceInfo[] DevList = bluc.DiscoverDevices(4);

            string maclist = "";

            StreamReader reader = new StreamReader("mac_list.txt");
            maclist += reader.ReadToEnd();
            reader.Close();

            MatchCollection manufacture = Regex.Matches(maclist, @"([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F]),*[^\d]\n", RegexOptions.IgnoreCase);
            string[] mac = new string[manufacture.Count];
            for (int i = 0; i < manufacture.Count; i++)
            {
                MatchCollection temp = Regex.Matches(manufacture[i].ToString(), @"([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F]),*[^\d]\n", RegexOptions.IgnoreCase);
                mac[i] = temp[0].ToString();
            }

            foreach (BluetoothDeviceInfo device in DevList)
            {
                listBox1.Items.Add(device.DeviceName);
            }

            this.Cursor = Cursors.Default;
            button1.Enabled = true;
        }
    }
}
