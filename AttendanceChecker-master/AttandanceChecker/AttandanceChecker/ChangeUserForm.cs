using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttandanceChecker
{
    public partial class ChangeUserForm : Form
    {
        Form1 pForm;
        public ChangeUserForm(Clients client, Form1 parentForm)
        {
            InitializeComponent();
            textBox1.Text = client.clientName;
            textBox2.Text = client.clientSurname;
            textBox3.Text = client.clientAvtoName;
            textBox4.Text = client.clientRegNumber;
            textBox5.Text = client.deviceName;
            dateTimePicker1.Value = DateTime.Parse(client.clientDateBirth);
            pForm = parentForm;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Clients update_client = new Clients();
            update_client.clientName = textBox1.Text;
            update_client.clientSurname = textBox2.Text;
            update_client.deviceName = textBox5.Text;
            update_client.clientAvtoName = textBox3.Text;
            update_client.clientRegNumber = textBox4.Text;
            update_client.clientDateBirth = dateTimePicker1.Value.ToString("dd-MM-yyyy");

            pForm.UpdateRegisterClient(update_client);
            /*IFormatter formatter = new SoapFormatter();
            //IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("Base.xml", FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, update_client);
            stream.Close();*/
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Clients delete_client = new Clients();
            delete_client.deviceName = textBox5.Text;
            pForm.DeleteRegisterClientList(delete_client);
        }
    }
}
