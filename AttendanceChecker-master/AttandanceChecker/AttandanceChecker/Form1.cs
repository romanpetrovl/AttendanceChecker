using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using System.IO;
using System.Text.RegularExpressions;

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;


namespace AttandanceChecker
{
    public partial class Form1 : Form
    {
        List<Clients> RegisterClients = new List<Clients>();
        List<BluetoothDeviceInfo> ThreadList = new List<BluetoothDeviceInfo>();
        List<BluetoothDeviceInfo> AvailableDevicesList = new List<BluetoothDeviceInfo>();
        public delegate void UpdateDiscoverBox(List<BluetoothDeviceInfo> list);
        public delegate void AddListItem();
        public AddListItem myDelegate;

        public Form1()
        {
            InitializeComponent();
            myDelegate = new AddListItem(UpdateGUI);
            Thread finderThread = new Thread(FindBluetoothDevices);
            finderThread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Cursor = Cursors.WaitCursor;
            button1.Enabled = false;
        }

        public void UpdateGUI()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            foreach (BluetoothDeviceInfo device in AvailableDevicesList)
            {
                bool bFound = false;
                string name = "test";
                string surname = "test";
                foreach (Clients client in RegisterClients)
                {
                    if (device.DeviceName == client.deviceName)
                    { 
                        bFound = true;
                        name = client.clientName;
                        surname = client.clientSurname;
                    }
                }
                if (bFound)
                    listBox2.Items.Add(name + " " + surname);
                else
                    listBox1.Items.Add(device.DeviceName);
            }
        }

        public void UpdateRegisterList(Clients client)
        {
            RegisterClients.Add(client);
        }

        public void FindBluetoothDevices()
        {
            while(true)
            {

            bool find = false;
            bool find2 = false;
            BluetoothClient bluc = new BluetoothClient();
            BluetoothDeviceInfo[] DevList = bluc.DiscoverDevices(4);
            List<BluetoothDeviceInfo> copyDevicesList = ThreadList.GetRange(0, ThreadList.Count);


            ThreadList.Clear();
            //string maclist = "";

            /*StreamReader reader = new StreamReader("mac_list.txt");
            maclist += reader.ReadToEnd();
            reader.Close();

            MatchCollection manufacture = Regex.Matches(maclist, @"([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F]),*[^\d]\n", RegexOptions.IgnoreCase);
            string[] mac = new string[manufacture.Count];
            for (int i = 0; i < manufacture.Count; i++)
            {
                MatchCollection temp = Regex.Matches(manufacture[i].ToString(), @"([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F])([0-9A-F]),*[^\d]\n", RegexOptions.IgnoreCase);
                mac[i] = temp[0].ToString();
            }*/

            foreach (BluetoothDeviceInfo device in DevList)
            {
                ThreadList.Add(device);
                if (copyDevicesList.Count == 0)
                    find = true;
                foreach (BluetoothDeviceInfo oldDevice in copyDevicesList)
                {
                    if (device.DeviceName == oldDevice.DeviceName)
                        if (!find2)
                            find2 = true;
               }
            }
            find = !find2;

            if (find)
            {

                    AvailableDevicesList = ThreadList.GetRange(0, ThreadList.Count);

                    if (this.IsHandleCreated)
                        this.Invoke(myDelegate);
            }

            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string devName = listBox1.SelectedItem.ToString();
            RegistrationForm regForm = new RegistrationForm(devName, this);
            regForm.ShowDialog();
        }

        private void списокЗарегистрированныхПользователейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistredUsers regListForm;
            if (RegisterClients.Count == 0)
                MessageBox.Show("Нет зарегестрированных пользователей");
            else
            { 
                regListForm = new RegistredUsers(RegisterClients);
                regListForm.ShowDialog();
            }

        }
    }
}
