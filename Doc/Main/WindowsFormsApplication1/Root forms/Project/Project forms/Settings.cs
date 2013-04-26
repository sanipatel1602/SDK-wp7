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
    public partial class Settings6 : Form
    {
        public ProjectEntity project;

        public Settings6()
        {
            InitializeComponent();
        }

        private void LoadTextBox()
        {
            textBox1.Text = project.ProjectName;
            textBox2.Text = project.DirectorySource;
            textBox3.Text = project.FTPHost;
            textBox4.Text = project.FTPLogin;
            textBox5.Text = project.FTPPass;
            textBox6.Text = project.ProjectSource;
            textBox7.Text = project.FtpPath;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            LoadTextBox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo("C:\\Projects\\Xml_Projects");
          /*  string str = textBox1.Text + ".xml";
            foreach (FileInfo file in dir.GetFiles())
            {
                if (str == file.Name)
                {
                    MessageBox.Show("Имя проекта уже занято, введите другое");
                    return;
                }
               
            }*/

            if (!File.Exists("C:\\Projects\\Xml_Projects" + project.ProjectName + ".xml"))
                CreateXMLDocument("C:\\Projects\\Xml_Projects" + textBox1.Text + ".xml");
            else
            {
                File.Delete("C:\\Projects\\Xml_Projects" + project.ProjectName + ".xml");
                CreateXMLDocument("C:\\Projects\\Xml_Projects" + textBox1.Text + ".xml");
            }


            if (textBox1.Text != "")
            {
                project.ProjectName = textBox1.Text;
            }
            if (textBox2.Text != "")
            {
                project.DirectorySource = textBox2.Text;
            }
            if (textBox3.Text != "")
            {
                project.FTPHost = textBox3.Text;
            }
            if (textBox4.Text != "")
            {
                project.FTPLogin = textBox4.Text;
            }
            if (textBox5.Text != "")
            {
                project.FTPPass = textBox5.Text;
            }
            if (textBox6.Text != "")
            {
                project.ProjectSource = textBox6.Text;
            }
            if (textBox7.Text != "")
            {
                project.FtpPath = textBox7.Text;
            }
            
            if (textBox2.Text != "" && textBox6.Text != "")
            {
                if (!Directory.Exists(textBox6.Text))
                    Directory.CreateDirectory(textBox6.Text);
                if (!Directory.Exists(textBox6.Text + "\\" + textBox1.Text + "_ProjectDir"))
                    Directory.CreateDirectory(textBox6.Text + "\\" + textBox1.Text + "_ProjectDir");
            }

            project.SaveProject(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text);
            this.Close();

        }

        private void CreateXMLDocument(string filepath)
        {
            XmlTextWriter xtw = new XmlTextWriter(filepath, Encoding.UTF8);
            xtw.WriteStartDocument();
            xtw.WriteStartElement("Project");
            xtw.WriteEndDocument();
            xtw.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Foldersource = new FolderBrowserDialog();
            if (Foldersource.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = Foldersource.SelectedPath.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Foldersource = new FolderBrowserDialog();
            if (Foldersource.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = Foldersource.SelectedPath.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "" && textBox5.Text != "" && textBox3.Text != "")
            {
                Cursor = Cursors.WaitCursor;
                ChooseFTPDir form = new ChooseFTPDir();

                form.login = textBox4.Text;
                form.password = textBox5.Text;
                form.host = textBox3.Text;
                form.ShowDialog();
                Cursor = Cursors.Default;
                string dir = form.dir();
                textBox7.Text = dir;
            }
            else
                MessageBox.Show("Не все поля заполнены!(ftppass, ftplogin и ftphost)", "Error");
        }

        private void TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox2.Text) && !String.IsNullOrEmpty(textBox6.Text) && !String.IsNullOrEmpty(textBox7.Text))
            {
                button2.Enabled = true;
            }

        }  

    }
}
