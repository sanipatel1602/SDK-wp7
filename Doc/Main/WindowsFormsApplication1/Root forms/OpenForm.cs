using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;


namespace WindowsFormsApplication1
{
    public partial class OpenForm : Form
    {
        public OpenForm()
        {

            InitializeComponent();
            ListLoad();

        }

        public void ListLoad()
        {
            RootForm temp = (RootForm)this.MdiParent;
            if (!Directory.Exists("C:\\Projects\\Xml_Projects\\"))
                Directory.CreateDirectory("C:\\Projects\\Xml_Projects\\");
            DirectoryInfo dir = new DirectoryInfo("C:\\Projects\\Xml_Projects\\");
            FileInfo[] files = dir.GetFiles();
            listBox1.Items.Clear();
                foreach (FileInfo file in files)
                {
                    if(file.Name.Contains(".xml"))
                    {
                        ProjectEntity load = new ProjectEntity(file.FullName);
                        listBox1.Items.Add(load);
                    }
                }
            if (listBox1.Items.Count < 1)
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            RootForm temp = (RootForm)this.MdiParent;
            if (listBox1.SelectedItem != null)
            {
                temp.project = (ProjectEntity)listBox1.SelectedItem;

                Project form = new Project();
                form.MdiParent = RootForm.ActiveForm;
                form.treenodeproj = temp.project;
                form.Text = temp.project.ProjectName;
                form.Show();
                this.Close();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                ProjectEntity deleteproj = (ProjectEntity)listBox1.SelectedItem;
                File.Delete("C:\\Projects\\Xml_Projects\\" + deleteproj.ProjectName + ".xml");

                foreach (string subDir in Directory.GetDirectories(deleteproj.ProjectSource + "\\" + deleteproj.ProjectName + "_ProjectDir"))
                    Directory.Delete(Path.Combine(deleteproj.ProjectSource + "\\" + deleteproj.ProjectName + "_ProjectDir", subDir), true);

                foreach (string currFile in Directory.GetFiles(deleteproj.ProjectSource + "\\" + deleteproj.ProjectName + "_ProjectDir"))
                    File.Delete(Path.Combine(deleteproj.ProjectSource + "\\" + deleteproj.ProjectName + "_ProjectDir", currFile));
                Directory.Delete(deleteproj.ProjectSource + "\\" + deleteproj.ProjectName + "_ProjectDir");

                foreach (string subDir in Directory.GetDirectories("C:\\Projects\\CSS\\" + deleteproj.ProjectName + "_CSS\\"))
                    Directory.Delete(Path.Combine("C:\\Projects\\CSS\\" + deleteproj.ProjectName + "_CSS\\", subDir), true);

                foreach (string currFile in Directory.GetFiles("C:\\Projects\\CSS\\" + deleteproj.ProjectName + "_CSS\\"))
                    File.Delete(Path.Combine("C:\\Projects\\CSS\\" + deleteproj.ProjectName + "_CSS\\", currFile));
                Directory.Delete("C:\\Projects\\CSS\\" + deleteproj.ProjectName + "_CSS\\");

                ListLoad();
            }
        }

    }
}
