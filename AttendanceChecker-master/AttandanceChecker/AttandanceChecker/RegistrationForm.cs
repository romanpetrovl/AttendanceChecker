using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttandanceChecker
{
    public partial class RegistrationForm : Form
    {
        Form1 pForm;
        public RegistrationForm(string devName, Form1 parentForm)
        {
            InitializeComponent();
            textBox3.Text = devName;
            pForm = parentForm;
            
        }

        public RegistrationForm(string devName, string regnumber, Form1 parentForm)
        {
            InitializeComponent();
            textBox3.Text = devName;
            textBox5.Enabled = false;
            textBox5.Text = regnumber;
            pForm = parentForm;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clients new_client = new Clients();
            new_client.clientName = textBox1.Text;
            new_client.clientSurname = textBox2.Text;
            new_client.deviceName = textBox3.Text;
            new_client.clientAvtoName = textBox4.Text;
            new_client.clientRegNumber = textBox5.Text;
            new_client.clientDateBirth = dateTimePicker1.Value.ToString("dd-MM-yyyy");

            IFormatter formatter = new SoapFormatter();
            //IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("Base.xml", FileMode.Append, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, new_client);
            stream.Close();

            pForm.UpdateRegisterList(new_client, true);
            this.Close();
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
