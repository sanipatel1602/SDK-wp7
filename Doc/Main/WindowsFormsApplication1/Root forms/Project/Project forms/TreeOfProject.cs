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
    public partial class TreeOfProject : Form
    {

        public ProjectEntity tempor;

        public TreeOfProject()
        {
            InitializeComponent();
           
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // Ставим галочку на всех подузлах.
            SelectAllSubnodes(e.Node);
        }

        // Метод для установки галочки для всех подузлов.
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

        private void DirectoryRecursive(TreeNode node, DirectoryInfo dir)
        {
            try
            {
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo subdir in dirs)
                {
                    DirectoryRecursive(AddNode(node, subdir.Name), subdir);
                    foreach (FileInfo file in subdir.GetFiles())
                    {
                        AddNode(node.LastNode, file.Name);
                    }
                }
                foreach (FileInfo file in dir.GetFiles())
                {
                    AddNode(node, file.Name);
                }
            }
            catch
            {
            }
        }

        private TreeNode AddNode(TreeNode node, string text)
        {
            return node.Nodes.Add(text);
        }


        private void TreenodeForm_Load(object sender, EventArgs e)
        {
            DirectoryRecursive(treeView1.Nodes.Add(tempor.ProjectSource + "\\" + tempor.ProjectName + "_ProjectDir" + "\\" + tempor.ProjectName), new DirectoryInfo(tempor.ProjectSource + "\\" + tempor.ProjectName + "_ProjectDir" + "\\" + tempor.ProjectName));
        }


        private TreeNode[] qwe(TreeNode nodes)
        {
            try
            {
                List<TreeNode> temp = new List<TreeNode>();
                foreach (var t in nodes.Nodes)
                    temp.Add((TreeNode)t);

                return temp.ToArray();
            }
            catch
            { return null; }
        }

        List<string> gtemp = new List<string>();

        private static void CheckRecurs(TreeNodeCollection node, string projname)
        {
            foreach (TreeNode t in node)
            {
                if (t.Checked)
                {
                    string RemDir = t.FullPath.ToString() + t.Name.ToString();
                    SaveText(RemDir, projname);
                }
                if (t.Nodes != null)
                    TreeOfProject.CheckRecurs(t.Nodes, projname);
            }
            return;
        }

        private static void SaveText(string RemDir, string ProjectName)
        {
            string path = @"C:\\Projects\XML_ProjectEntyties\" + ProjectName + "_treenode.txt";
            StreamWriter sw = File.AppendText(path);
            sw.WriteLine(RemDir);
            sw.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CheckRecurs(treeView1.Nodes, tempor.ProjectName);
            
        }

        private void DeleteText()
        {
            try
            {
                string path = @"C:\\Projects\XML_ProjectEntyties\" + tempor.ProjectName + "_treenode.txt";
                string[] readText = File.ReadAllLines(path, Encoding.UTF8);
                foreach (string str in readText)
                {
                    if (System.IO.Directory.Exists(str))
                    {
                        Directory.Delete(Path.Combine(str), true);
                        treeView1.Nodes.Clear();
                        DirectoryRecursive(treeView1.Nodes.Add(tempor.ProjectSource + "\\" + tempor.ProjectName + "_ProjectDir" + "\\" + tempor.ProjectName), new DirectoryInfo(tempor.ProjectSource + "\\" + tempor.ProjectName + "_ProjectDir" + "\\" + tempor.ProjectName));
                    }
                    else if (System.IO.File.Exists(str))
                    {
                        File.Delete(str);
                        treeView1.Nodes.Clear();
                        DirectoryRecursive(treeView1.Nodes.Add(tempor.ProjectSource + "\\" + tempor.ProjectName + "_ProjectDir" + "\\" + tempor.ProjectName), new DirectoryInfo(tempor.ProjectSource + "\\" + tempor.ProjectName + "_ProjectDir" + "\\" + tempor.ProjectName));
                    }
                }

            }
            catch
            {
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckRecurs(treeView1.Nodes, tempor.ProjectName);
            DeleteText();           
        }

    }
}
