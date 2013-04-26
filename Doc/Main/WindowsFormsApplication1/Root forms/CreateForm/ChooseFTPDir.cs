using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class ChooseFTPDir : Form
    {
        public ChooseFTPDir()
        {

            InitializeComponent();
        }

        public string host = "";
        public string login = "";
        public string password = "";
        public string ftpdir = "";
        
        List<string> files2 = new List<string>();

        public string[] GetFileList()
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + host + "/http/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(login, password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                downloadFiles = null;
                return downloadFiles;
            }
        }

        public List<string> getFileList(string FTPAddress, string username, string password)
        {
           
            List<string> files = new List<string>();
            try
            {
                //Create FTP request
                FtpWebRequest request = FtpWebRequest.Create(FTPAddress) as FtpWebRequest;

                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                FtpWebResponse response = request.GetResponse() as FtpWebResponse;
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        while (!reader.EndOfStream)
                        {
                            string a = reader.ReadLine();
                            a = a.Replace("./","");
                            files.Add(a);
                        }
                    }
                }
                check = true;  
            }
            catch (Exception ex)
            {
                // write to log
                check = false;               
            }
            return files;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            SelectAllSubnodes(e.Node);
        }

        void SelectAllSubnodes(TreeNode treeNode)
        {
            // Ставим или убираем отметку со всех подузлов.
            foreach (TreeNode treeSubNode in treeNode.Nodes)
            {
                treeSubNode.Checked = treeNode.Checked;
            }

        }

        void DeleteNodes(TreeNode treeNode)
        {
            // Ставим или убираем отметку со всех подузлов.
            foreach (TreeNode treeSubNode in treeNode.Nodes)
            {
                treeSubNode.Checked = treeNode.Checked;
            }
        }


        private void DirectoryRecursive2(TreeNode node, List<string> folders)
        {
            try
            {
                foreach (var t in folders)
                {
                    node.Nodes.Add(t);
                }
            }
            catch
            {
            }
        }

        private void DirectoryRecursive(TreeNode node, List<string> folders)
        {
            try
            {
                foreach (var t in files2)
                {
                    treeView1.Nodes.Add(t);
                }
            }
            catch
            {
            }
        }



        private TreeNode AddNode(TreeNode node, string text)
        {
            return node.LastNode.Nodes.Add(text);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        List<string> temp = new List<string>();
        bool check = false;
         
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            string papka,papkapath;
            Cursor = Cursors.WaitCursor;
            files2.Clear();
            FTPPath();
            files2 = getFileList(@"ftp://tdf7.1gb.ru/" + temppath, login, password);
            temp.Clear();
            foreach(var t in files2)
            { 
                papka = t;
                papka = papka.Substring(t.IndexOf("/") + 1);
                getFileList(@"ftp://tdf7.1gb.ru/" + temppath + "/" + papka + "/", login, password);
                if (check == true)
                {
                    temp.Add(papka);
                }

            }
            DirectoryRecursive2(treeView1.SelectedNode, temp);
            Cursor = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FTPPath();
            this.Close();
        }

        string temppath;
        // путь по папкам
        private void FTPPath()
        {
            string path = "";
            TreeNode tree;
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Parent != null)
            {
                path = treeView1.SelectedNode.Text;
                tree = treeView1.SelectedNode;
                do
                {
                    tree = tree.Parent;
                    path = tree.Text + "/" + path;

                } while (tree.Parent != null);
                temppath = path;
            }
            else
            {
                if (treeView1.SelectedNode != null)
                {
                    path = treeView1.SelectedNode.Text;
                    temppath = path;
                }
            }
            
        }

        public List<string> getFileList2(string FTPAddress, string username, string password)
        {

            List<string> files = new List<string>();
            try
            {
                //Create FTP request
                FtpWebRequest request = FtpWebRequest.Create(FTPAddress) as FtpWebRequest;

                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                FtpWebResponse response = request.GetResponse() as FtpWebResponse;
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        while (!reader.EndOfStream)
                        {
                            string a = reader.ReadLine();
                            a = a.Replace("./", "");
                            files.Add(a);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Close();
                MessageBox.Show(ex.Message);
                // write to log                
            }
            return files;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            files2 = getFileList2("ftp://" + host, login, password);
            DirectoryRecursive(treeView1.TopNode, files2);
            Cursor = Cursors.Default;          
        }

        public string dir()
        {
            ftpdir = temppath;
            return ftpdir;
        }
    }
}
