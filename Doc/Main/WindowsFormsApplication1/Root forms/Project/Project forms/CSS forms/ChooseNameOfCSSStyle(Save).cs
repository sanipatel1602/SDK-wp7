using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class NameOFCSSStyle10 : Form
    {
        public NameOFCSSStyle10()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OkCan = "Cancel";
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                OkCan = "OK";
                this.Close();
            }
            else
                MessageBox.Show("Имя стиля не может быть пустым!");
        }

        public string OkCan;
        public string nameofcss;
        public string style()
        {
            nameofcss = textBox1.Text;
            return nameofcss;
        }


    }
}
