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
    public partial class NewClient : Form
    {
        Form1 pForm;
        string devName;
        public NewClient(string device, Form1 parentForm)
        {
            InitializeComponent();
            pForm = parentForm;
            devName = device;
        }

        public string TheValue
        {
            get { return textBox1.Text; }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                button2.Enabled = false;
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            RegistrationForm regForm = new RegistrationForm(devName, textBox1.Text, pForm);
            if (regForm.ShowDialog() == DialogResult.OK)
                this.Close();
        }
    }
}
