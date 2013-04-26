using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class ChooseCSSStyle9 : Form
    {
        public ChooseCSSStyle9()
        {
            InitializeComponent();
            ListLoad();
            
        }

        public void ListLoad()
        {
            RootForm temp = (RootForm)this.MdiParent;
            if (!Directory.Exists("C:\\Projects\\CSS\\Universal_Styles\\"))
                Directory.CreateDirectory("C:\\Projects\\CSS\\Universal_Styles\\");
            DirectoryInfo dir = new DirectoryInfo("C:\\Projects\\CSS\\Universal_Styles\\");
            FileInfo[] files = dir.GetFiles();
            listBox1.Items.Clear();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(".xml"))
                {
                    listBox1.Items.Add(file.Name.Remove(file.Name.Length - 4));
                }
            }
            if (listBox1.Items.Count < 1)
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }

        public string OkCan;


        private void button1_Click(object sender, EventArgs e)
        {
            OkCan = "OK";
            this.Close();            
        }

        public string style()
        {
            if (listBox1.SelectedItem != null)
            {
                string style = listBox1.SelectedItem.ToString();
                return style;
            }
            return null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = "";
            if (listBox1.SelectedItem != null)
            {               
                path = listBox1.SelectedItem.ToString();
            }

            if (path != null)
            {
                File.Delete("C:\\Projects\\CSS\\Universal_Styles\\" + path + @".xml");
            }
            ListLoad();
        }
    }
}
