using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class CreateProjectForm : Form
    {

        public CreateProjectForm()
        {
            InitializeComponent();
        }

        //объект класса ProjectEntity
        public ProjectEntity a = new ProjectEntity();

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (!Directory.Exists("C:\\Projects\\Xml_Projects\\"))
                Directory.CreateDirectory("C:\\Projects\\Xml_Projects\\");
            DirectoryInfo dir = new DirectoryInfo("C:\\Projects\\Xml_Projects\\");
            string str = textBox1.Text + ".xml";
            foreach (FileInfo file in dir.GetFiles())
            {      
                if (str == file.Name)
                {
                    MessageBox.Show("Имя проекта уже занято, введите другое");
                    return;
                }                         
            }

            if (textBox1.Text != "")
            {
                a.ProjectName = textBox1.Text;
            }
            if (textBox2.Text != "")
            {
                a.DirectorySource = textBox2.Text;
            }
            if (textBox3.Text != "")
            {
                a.FTPHost = textBox3.Text;
            }
            if (textBox4.Text != "")
            {
                a.FTPLogin = textBox4.Text;
            }
            if (textBox5.Text != "")
            {
                a.FTPPass = textBox5.Text;
            }
            if (textBox6.Text != "")
            {
                a.ProjectSource = textBox6.Text;
            }
            if (textBox7.Text != "")
            {
                a.FtpPath = textBox7.Text;
            }

            RootForm temp = (RootForm)this.MdiParent;
            temp.project = a;
            this.Result = a;

            if (textBox2.Text != "" && textBox6.Text != "")
            {
                if (!Directory.Exists(textBox6.Text))
                    Directory.CreateDirectory(textBox6.Text);
                if (!Directory.Exists(textBox6.Text + "\\" + textBox1.Text + "_ProjectDir"))
                    Directory.CreateDirectory(textBox6.Text + "\\" + textBox1.Text + "_ProjectDir");
             /*   if (!Directory.Exists(textBox6.Text + "\\" + textBox1.Text + "_ProjectDir\\" + textBox1.Text))
                    Directory.CreateDirectory(textBox6.Text + "\\" + textBox1.Text + "_ProjectDir\\" + textBox1.Text );
                if (!Directory.Exists(textBox6.Text + "\\" + textBox1.Text + "_ProjectDir\\" + textBox1.Text + "_Temp"))
                    Directory.CreateDirectory(textBox6.Text + "\\" + textBox1.Text + "_ProjectDir\\" + textBox1.Text + "_Temp");*/
                
                DirectoryInfo soursDir = new DirectoryInfo(textBox2.Text); //папка из которой копировать
                DirectoryInfo destDir = new DirectoryInfo(textBox6.Text + "\\" + textBox1.Text + "_ProjectDir\\" + textBox1.Text ); //куда копировать
                CopyDir(soursDir, destDir);

                DirectoryInfo soursDirTemp = new DirectoryInfo(textBox2.Text); //папка из которой копировать
                DirectoryInfo destDirTemp = new DirectoryInfo(textBox6.Text + "\\" + textBox1.Text + "_ProjectDir\\" + textBox1.Text + "_Temp"); //куда копировать
                CopyDir(soursDirTemp, destDirTemp);
            }
            if (!Directory.Exists("C:\\Projects\\Xml_Projects\\"))
                Directory.CreateDirectory("C:\\Projects\\Xml_Projects\\");
            if (!File.Exists("C:\\Projects\\Xml_Projects\\" + textBox1.Text + ".xml"))
                CreateXMLDocument("C:\\Projects\\Xml_Projects\\" + textBox1.Text + ".xml");
            temp.project.SaveProject(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text);
            //Write(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text);

            this.Close();
            Cursor = Cursors.Default;
            Project projform = new Project();
            projform.treenodeproj = a;
            projform.MdiParent = temp;
            projform.Text = temp.project.ProjectName;
            projform.Show();
        }



        // копирование директории
        private void CopyDir(DirectoryInfo soursDir, DirectoryInfo destDir)
        {
            while (true)
            {
                CreateDir(soursDir, destDir);
                //проверка наличия папок в директории
                DirectoryInfo[] dirs = soursDir.GetDirectories();
                if (dirs.Length > 0)
                {
                    foreach (DirectoryInfo di in dirs)
                    {
                        DirectoryInfo dir = new DirectoryInfo(destDir.FullName.ToString() + "\\" + di.Name.ToString());
                        CopyDir(di, dir);
                    }
                    break;
                }
                else break;
            }
        }
        //создание директории, если её нету
        private void CreateDir(DirectoryInfo soursDir, DirectoryInfo destDir)
        {
            if (!destDir.Exists) destDir.Create();
            //проверка наличия файлов
            FileInfo[] fls = soursDir.GetFiles();
            if (fls.Length > 0) //копируем
                foreach (FileInfo fi in fls)
                {
                    fi.CopyTo(destDir.FullName.ToString() + "\\" + fi.Name.ToString(), true);
                }
        }



        public ProjectEntity Result
        {
            get { return a;}
            set { a = value;}
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

        //создание xml файла
        private void CreateXMLDocument(string filepath)
        {
            XmlTextWriter xtw = new XmlTextWriter(filepath, Encoding.UTF8);
            xtw.WriteStartDocument();
            xtw.WriteStartElement("Project");
            xtw.WriteEndDocument();
            xtw.Close();
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
                    string dir =  form.dir();
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
