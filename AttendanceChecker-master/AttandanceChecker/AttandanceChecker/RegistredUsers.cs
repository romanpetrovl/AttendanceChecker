using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttandanceChecker
{
    public partial class RegistredUsers : Form
    {
        public RegistredUsers(List<Clients> cl)
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.Columns.Add("№", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Name", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Surname", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Device name", -2, HorizontalAlignment.Left);
            int i = 1;
            foreach (Clients client in cl)
            {
                string[] info = { client.clientName, client.clientSurname, client.deviceName };
                listView1.Items.Add(i.ToString()).SubItems.AddRange(info);
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
