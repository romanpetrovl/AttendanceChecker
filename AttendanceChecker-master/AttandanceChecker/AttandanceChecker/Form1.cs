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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;

namespace AttandanceChecker
{
    public partial class Form1 : Form
    {
        List<Clients> RegisterClients = new List<Clients>();
        List<ClientsStock> ClientsOnStock = new List<ClientsStock>();
        List<BluetoothDeviceInfo> ThreadList = new List<BluetoothDeviceInfo>();
        List<BluetoothDeviceInfo> AvailableDevicesList = new List<BluetoothDeviceInfo>();
        public delegate void UpdateDiscoverBox(List<BluetoothDeviceInfo> list);
        public delegate void AddListItem();
        public AddListItem myDelegate;
        Thread finderThread;

        public Form1()
        {
            InitializeComponent();
            myDelegate = new AddListItem(UpdateGUI);
            finderThread = new Thread(FindBluetoothDevices);
            finderThread.Start();
            try
            {
                IFormatter formatter = new SoapFormatter();
                Stream stream = new FileStream("Base.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
                while (true)
                {
                    try
                    {
                        Clients read_client = (Clients)formatter.Deserialize(stream);
                        if (read_client.deviceName == null) break;

                        UpdateRegisterList(read_client, false);
                    }
                    catch (System.Xml.XmlException)
                    {
                        break;
                    }
                }
                stream.Close();
            }
            catch (FileNotFoundException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Cursor = Cursors.WaitCursor;
            button1.Enabled = false;
        }

        public void UpdateGUI()
        {
            finderThread.Suspend();

            foreach (BluetoothDeviceInfo device in AvailableDevicesList)
            {
                bool bFound = false;
                bool fg = false;
                string name = "test";
                string surname = "test";
                string regnumber = "test";
                ListViewItem item;

                foreach (ClientsStock stock in ClientsOnStock)
                { 
                    if (device.DeviceName == stock.deviceName)
                    {
                        bFound = true;
                        regnumber = stock.clientRegNumber;
                        foreach (ListViewItem itm in listView3.Items)
                        {
                            if (itm.Text == device.DeviceName)
                            {
                                listView3.Items.Remove(itm);
                                fg = true;
                            }
                        }
                        if (!fg)
                            foreach (ListViewItem itm in listView1.Items)
                            {
                                if (itm.Text == device.DeviceName)
                                    listView1.Items.Remove(itm);
                            }
                        ClientsOnStock.Remove(stock);
                        break;
                    }
                }

                if (bFound)
                {
                    item = new ListViewItem(new string[] { device.DeviceName, regnumber, "Выезд", DateTime.Now.ToString() });
                    listView2.Items.Insert(0, item);
                }
                else
                {
                    foreach (Clients client in RegisterClients)
                    {
                        if (device.DeviceName == client.deviceName)
                        {
                            bFound = true;
                            name = client.clientName;
                            surname = client.clientSurname;
                            regnumber = client.clientRegNumber;
                        }
                    }
                    if (bFound)
                    {
                        item = new ListViewItem(new string[] { device.DeviceName, surname, name, regnumber });
                        listView1.Items.Add(item);
                    }
                    else
                    {
                        NewClient actForm = new NewClient(device.DeviceName.ToString(), this);
                        if (actForm.ShowDialog() == DialogResult.OK)
                        {
                            regnumber = actForm.TheValue;
                            item = new ListViewItem(new string[] { device.DeviceName, regnumber });
                            listView3.Items.Add(item);
                        }
                        regnumber = actForm.TheValue;
                    }
                    item = new ListViewItem(new string[] { device.DeviceName, regnumber, "Заезд", DateTime.Now.ToString() });
                    listView2.Items.Insert(0, item);
                    ClientsStock newStock = new ClientsStock();
                    newStock.deviceName = device.DeviceName;
                    newStock.clientRegNumber = regnumber;
                    ClientsOnStock.Add(newStock);
                }
            }
            finderThread.Resume();
        }

        public void UpdateBase()
        {
            IFormatter formatter = new SoapFormatter();
            //IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("Base.xml", FileMode.Create, FileAccess.Write, FileShare.None);
            foreach (Clients client in RegisterClients)
                formatter.Serialize(stream, client);
            stream.Close();
        }

        public void UpdateRegisterList(Clients client, bool f)
        {
            RegisterClients.Add(client);
            if (f)
            {
                foreach (ListViewItem itm in listView1.Items)
                {
                    if (itm.Text == client.deviceName)
                        listView3.Items.Remove(itm);
                }
                ListViewItem item = new ListViewItem(new string[] { client.deviceName, client.clientSurname, client.clientName, client.clientRegNumber });
                listView1.Items.Add(item);
                item = new ListViewItem(new string[] { client.deviceName, client.clientRegNumber, "Регистрация", DateTime.Now.ToString() });
                listView2.Items.Insert(0, item);
            }
        }

        public void UpdateRegisterClient(Clients update_client)
        {
            Clients clientInList = RegisterClients.Find(item2 => item2.deviceName == update_client.deviceName);
            RegisterClients.Remove(clientInList);
            RegisterClients.Add(update_client);

            ListViewItem item = new ListViewItem(new string[] { update_client.deviceName, update_client.clientRegNumber, "Изменен", DateTime.Now.ToString() });
            listView2.Items.Insert(0, item);

            UpdateBase();
            UpdateGUI();
            MessageBox.Show("Изменения сохранены", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DeleteRegisterClientList(Clients delete_client)
        {
            Clients clientInList = RegisterClients.Find(item2 => item2.deviceName == delete_client.deviceName);
            RegisterClients.Remove(clientInList);
            ListViewItem item = new ListViewItem(new string[] { delete_client.deviceName, delete_client.clientRegNumber, "Удален", DateTime.Now.ToString() });
            listView2.Items.Insert(0, item);
            UpdateBase();
            UpdateGUI();
            MessageBox.Show("Клиента удален из базы", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void FindBluetoothDevices()
        {
            while(true)
            {

            // bool find = false;
            bool find2 = false;
            BluetoothClient bluc = new BluetoothClient();
            BluetoothDeviceInfo[] DevList = bluc.DiscoverDevices(5);
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
                    find2 = true;
                foreach (BluetoothDeviceInfo oldDevice in copyDevicesList)
                {
                    if (device.DeviceName == oldDevice.DeviceName)
                        if (!find2)
                            find2 = true;
               }
            }
            //find = !find2;

            if (find2)
            {

                    AvailableDevicesList = ThreadList.GetRange(0, ThreadList.Count);

                    if (this.IsHandleCreated)
                        this.Invoke(myDelegate);
            }
            }
        }

        private void listView3_DoubleClick(object sender, EventArgs e)
        {
            string devName = listView3.SelectedItems[0].Text;
            RegistrationForm regForm = new RegistrationForm(devName, this);
            regForm.ShowDialog();
        }
        private void listView3_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0)
                button2.Enabled = false;
            else button2.Enabled = true;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            string devName = listView1.SelectedItems[0].Text;
            foreach (Clients client in RegisterClients)
                if (client.deviceName == devName)
                {
                    ChangeUserForm userForm = new ChangeUserForm(client, this);
                    userForm.ShowDialog();
                    break;
                }
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

        private void ToolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                string devName = listView3.SelectedItems[0].Text;
                RegistrationForm regForm = new RegistrationForm(devName, this);
                regForm.ShowDialog();
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
