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
    public partial class RegistrationForm : Form
    {
        Form1 pForm;
        public RegistrationForm(string devName, Form1 parentForm)
        {
            InitializeComponent();
            textBox3.Text = devName;
            pForm = parentForm;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clients new_client = new Clients();
            new_client.clientName = textBox1.Text;
            new_client.clientSurname = textBox2.Text;
            new_client.deviceName = textBox3.Text;

            pForm.UpdateRegisterList(new_client);
            this.Close();
        }
    }
}
