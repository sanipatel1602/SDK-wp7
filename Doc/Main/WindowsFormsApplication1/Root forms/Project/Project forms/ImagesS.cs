using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Images : Form
    {
        public Images()
        {
            InitializeComponent();
        }

        private string path;
        public ProjectEntity project;
        string dialogfilename;
        int a = 1;
        bool proverka;

        private void button1_Click(object sender, EventArgs e)
        {          
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string otkatname = NameOfFile();
                    dialogfilename = dialog.FileName;
                                if (!Directory.Exists(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\")) ;
            Directory.CreateDirectory(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\");
                    File.Copy(dialogfilename, project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\" + otkatname + ".jpg");
                    listBox1.Update();
                }
                Refresh();
            }
            catch(Exception ex)
            {
                   MessageBox.Show(ex.Message);
            }
        }

        private void Refresh()
        {
            if (!Directory.Exists(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\")) ;
                Directory.CreateDirectory(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\");
            DirectoryInfo dir = new DirectoryInfo(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\");
            FileInfo[] files = dir.GetFiles();
            listBox1.Items.Clear();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(".jpg"))
                {
                    string fil = file.FullName;
                    listBox1.Items.Add(fil);

                }
            }
        }

        private string NameOfFile()
        {
            string name = "0";
            List<int> names = new List<int>();
            if (!Directory.Exists(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\")) ;
            Directory.CreateDirectory(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\");
            DirectoryInfo dir = new DirectoryInfo(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\");
            if(Directory.GetFiles(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\").Count()<1)
            {
                name = "1";
                return name;
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(".jpg"))
                {
                        name = file.Name;
                        name = name.Remove(name.IndexOf(".jpg"), 4);
                        names.Add(Convert.ToInt32(name));
                }
            }

            name = (names.Max()+1).ToString();
            return name;

        }


        private void Images_Load(object sender, EventArgs e)
        {
            Refresh();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (File.Exists(listBox1.SelectedItem.ToString()))
                    File.Delete(listBox1.SelectedItem.ToString());
                listBox1.Items.Clear();
            }
            Refresh();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0 && listBox1.SelectedItem != null)
            {
                pictureBox1.ImageLocation = listBox1.SelectedItem.ToString();
                pictureBox1.Update();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DirectoryInfo dir = new DirectoryInfo(dialog.SelectedPath);
                    FileInfo[] files = dir.GetFiles();
                    listBox1.Items.Clear();
                    foreach (FileInfo file in files)
                    {
                        if (file.Name.Contains(".jpg"))
                        {
                            string otkatname = NameOfFile();
                                        if (!Directory.Exists(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\")) ;
            Directory.CreateDirectory(project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\");
            File.Copy(file.FullName, project.ProjectSource + "\\" + project.ProjectName + "_ProjectDir" + "\\" + project.ProjectName + "\\Images\\" + otkatname + ".jpg");
                        }
                    }

                    Refresh();
                }
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                proverka = false;
                Cursor = Cursors.WaitCursor;
                DirectoryInfo dir = new DirectoryInfo(project.ProjectSource + @"\" + project.ProjectName + @"_ProjectDir\" + project.ProjectName + @"\html\");
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Name.Contains(".htm"))
                    {
                        string tago = "<ROFL>", tagz = "</ROFL>";
                        string tag = "", newtag = "", newstr = "";
                        List<string> readTextnew = new List<string>();
                        string path = file.FullName;
                        string[] readText = File.ReadAllLines(path, Encoding.UTF8);
                        foreach (string str in readText)
                        {
                            if (str.Contains(tago))
                            {
                                string temp = str;
                                do
                                {
                                    tag = temp.Substring(temp.IndexOf(tago), (temp.IndexOf(tagz) - temp.IndexOf(tago) + tagz.Count()));
                                    newtag = tag;
                                    newtag = "<img src = " + "\"" + "../Images/" + newtag.Substring(newtag.IndexOf(tago) + tago.Count(), newtag.IndexOf(tagz) - newtag.IndexOf(tago) - tago.Count()) + ".jpg" + "\"" + " height = 100 weight = 100 ></img>";
                                    newstr = temp.Replace(tag, newtag);
                                    temp = newstr;
                                    proverka = true;
                                }
                                while (temp.Contains(tago));
                            }
                            else
                            {
                                newstr = str;
                            }
                            readTextnew.Add(newstr);
                        }
                        if (proverka = true)
                        {
                            File.Delete(file.FullName);
                            File.WriteAllLines(file.FullName, readTextnew);
                            proverka = false;
                        }
                    }
                }
                Cursor = Cursors.Default;
                MessageBox.Show("Тэги заменены");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

          /*  OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string tagz = "</ROFL>", tago = "<ROFL>";
                string tag = "", newtag = "",newstr="";
                List<string> readTextnew = new List<string>();
                string path = dialog.FileName;
                string[] readText = File.ReadAllLines(path, Encoding.UTF8);
                foreach (string str in readText)
                {
                    if (str.Contains(tago))
                    {
                        string temp = str;
                        do
                        {
                            tag = temp.Substring(temp.IndexOf(tago), (temp.IndexOf(tagz) - temp.IndexOf(tago) + tagz.Count()));
                            newtag = tag;
                            newtag = "<img src = " + "\""  + newtag.Substring(newtag.IndexOf(tago) + tago.Count(), newtag.IndexOf(tagz) - newtag.IndexOf(tago) - tago.Count()) + ".jpg" + "" + "></img>";
                            newstr = temp.Replace(tag, newtag);
                            temp = newstr;
                        }
                        while (temp.Contains(tago));
                    }
                    else
                    {
                        newstr = str;
                    }
                    readTextnew.Add(newstr);
                }  */
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NameOfFile();
        }


    }
}
