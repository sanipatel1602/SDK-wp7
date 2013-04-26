using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace WindowsFormsApplication1
{
    public partial class BackGroundForm8 : Form
    {
        public ProjectEntity treenodeproj;

        public BackGroundForm8()
        {
            InitializeComponent();
            bw1.WorkerSupportsCancellation = true;
            bw1.WorkerReportsProgress = true;

        }



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



        private void createFolderFtp(string namepath, string filename)
        {
            string ftphost = treenodeproj.FTPHost + "/";
            string ftpfullpath = "ftp://" + ftphost + treenodeproj.FtpPath + namepath + filename;
            //запись пути создания папки
            string path = "C:\\Projects\\Xml_Projects" + treenodeproj.ProjectName + "_DeleteFtp.txt";
            StreamWriter sw = File.AppendText(path);
            sw.WriteLine(ftpfullpath);
            sw.Close();
            FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(ftpfullpath);
            ftp.Credentials = new NetworkCredential(treenodeproj.FTPLogin, treenodeproj.FTPPass);
            ftp.KeepAlive = false;
            ftp.UseBinary = true;
            ftp.UsePassive = false;
            ftp.Proxy = null;
            ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
            FtpWebResponse resp = (FtpWebResponse)ftp.GetResponse();
            resp.Close();
        }

        public struct TempParent
        {
            public DirectoryInfo dir;
            public string path;
        }


        List<TempParent> temp = new List<TempParent>();
        string NamePath = "/";
        TempParent pair2 = new TempParent();
        int i = 0;

        private void check(DirectoryInfo dir, object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            foreach (var t in dir.GetDirectories())
            {

                if (worker.CancellationPending == true)
                    return;
                NamePath = temp.Last().path;
                createFolderFtp(NamePath, t.Name);
                NamePath += t.Name + "/";
                if (t.GetFiles().Count() != 0)
                {
                    foreach (FileInfo file in t.GetFiles())
                    {
                        if (worker.CancellationPending == true)
                        {
                            e.Cancel = true;
                            return;
                        }
                        uploadfiles(NamePath, file.FullName);
                        
                        i++;
                        worker.ReportProgress(i);

                    }
                }

                if (t.GetDirectories().Count() != 0)
                {
                    pair2.dir = t;
                    pair2.path = NamePath;
                    temp.Add(pair2);
                    check(t, sender, e);
                }

            }
            temp.Remove(temp.Last());
        }


        private void check2(DirectoryInfo dir, object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            foreach (FileInfo file in dir.GetFiles())
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
                NamePath = "/";
                uploadfiles(NamePath, file.FullName);
                i++;
                worker.ReportProgress(i);
            }
        }
        public void uploadfiles(string fileway, string filename)
        {

            string ftpHostIP = treenodeproj.FTPHost;
            string ftpUserID = treenodeproj.FTPLogin;
            string ftpPassword = treenodeproj.FTPPass;
            FileInfo fileInf = new FileInfo(filename);
            string uri = "ftp://" + treenodeproj.FTPHost + "/" + treenodeproj.FtpPath + "/" + fileway + fileInf.Name;
            //запись пути создания файла
            string path = "C:\\Projects\\Xml_Projects" + treenodeproj.ProjectName + "_DeleteFtp.txt"; ;
            StreamWriter sw = File.AppendText(path);
            sw.WriteLine(uri);
            sw.Close();

            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + treenodeproj.FTPHost + "/" + treenodeproj.FtpPath + "/" + fileway + fileInf.Name));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;

            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Upload Error");
            }
        }

        public void DeleteFTP(string dir, string filename)
        {
            string ftpUserID = treenodeproj.FTPLogin;
            string ftpPassword = treenodeproj.FTPPass;
            string path = "ftp://" + treenodeproj.FTPHost + "/" + treenodeproj.FtpPath + filename;
            string folderpath = dir + "/" + filename;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                folderpathes.Add(folderpath);
            }

        }


        public List<string> ListFolders(string path)
        {
            List<string> fileList = new List<string>();
            string ftphost = treenodeproj.FTPHost + "/" + treenodeproj.FtpPath + "/";
            string ftpfullpath = "ftp://" + ftphost;
            FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(ftpfullpath);
            ftp.Credentials = new NetworkCredential(treenodeproj.FTPLogin, treenodeproj.FTPPass);
            ftp.KeepAlive = false;
            ftp.UseBinary = true;
            ftp.UsePassive = false;
            ftp.Proxy = null;
            ftp.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse resp = (FtpWebResponse)ftp.GetResponse();
            Stream responseStream = null;
            StreamReader readStream = null;
            try
            {

                responseStream = resp.GetResponseStream();

                readStream = new StreamReader(responseStream, System.Text.Encoding.Default);
                if (readStream != null)
                {
                    string line;
                    fileList = new List<string>();
                    do
                    {
                        line = readStream.ReadLine();
                        if (line != null)
                            fileList.Add(line);
                    } while (line != null);
                }
            }
            finally
            {
                if (readStream != null)
                {
                    readStream.Close();
                }
                if (resp != null)
                {
                    resp.Close();
                }
            }
            return fileList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (bw1.IsBusy != true)
            {
                bw1.RunWorkerAsync();
            }
            
        }

        public struct TempParentDelete
        {
            public string folderpathes;
            public string path;
        }
        List<string> folderpathes = new List<string>();

        private void DelAll()
        {
            List<string> fileList = null;
            fileList = ListFolders(NamePath);
            foreach (var s in fileList)
            {
                string str;
                str = s.Substring(s.IndexOf("/") + 1);
                DeleteFTP(NamePath, str);
                foreach (var t in folderpathes)
                {
                    str += "/" + t;
                    DelAll();
                }
                folderpathes.Remove(folderpathes.Last());
            }

        }
        //удаление файлов и папок, записанных в txt-шнике
        private void ReadText()
        {
            string path = @"F:\WORKING NAH\рабочая - копия\11.txt";
            string[] readText = File.ReadAllLines(path, Encoding.UTF8);
            Array.Reverse(readText);
            foreach (string s in readText)
            {
                DeleteFTPTXT(s);
            }
            File.Delete(path);
            File.Create(path);
        }

        public void DeleteFTPTXT(string path)
        {
            string ftpUserID = treenodeproj.FTPLogin;
            string ftpPassword = treenodeproj.FTPPass;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void DeleteProj(string RemDir)
        {
            try
            {
                foreach (string subDir in Directory.GetDirectories(RemDir))
                {
                    Directory.Delete(Path.Combine(RemDir, subDir), true);
                    foreach (string currFile in Directory.GetFiles(RemDir))
                        File.Delete(Path.Combine(RemDir, currFile));
                }
                Directory.Delete(RemDir);
                Directory.CreateDirectory(RemDir);
            }
            catch
            {

            }
        }

        //проверка на существование directory
        private bool FtpDirectoryExists(string directory, string username, string password)
        {
            try
            {
                var request = (FtpWebRequest)WebRequest.Create(directory);
                request.Credentials = new NetworkCredential(username, password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {

                return false;
            }
            return true;
        }


        private void createUnExistFolderFtp(string namepath, string filename)
        {
            try
            {
                string ftphost = treenodeproj.FTPHost + "/";
                string ftpfullpath = "ftp://" + ftphost + namepath + filename;
                //запись пути создания папки
                string path = "C:\\Projects\\" + treenodeproj.ProjectName + @"_ProjectDir\" + treenodeproj.ProjectName + "_DeleteFtp.txt";
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(ftpfullpath);
                sw.Close();
                FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential(treenodeproj.FTPLogin, treenodeproj.FTPPass);
                ftp.KeepAlive = false;
                ftp.UseBinary = true;
                ftp.UsePassive = false;
                ftp.Proxy = null;
                ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse resp = (FtpWebResponse)ftp.GetResponse();
                resp.Close();
            }
            catch (Exception ex)
            {
                
            }
        }


        private void checkandcreatefolderftp()
        {
            List<string> tempofpathes = new List<string>();
            bool check;
            string ftppath = treenodeproj.FtpPath;
            string ftppathrazdel = "";
            char razdel = '/';
            string[] words = ftppath.Split(razdel);
            foreach (string s in words)
            {
                ftppathrazdel += s + "/";
                string path = "ftp://" + treenodeproj.FTPHost + "/" + ftppathrazdel;
                check = FtpDirectoryExists(path, treenodeproj.FTPLogin, treenodeproj.FTPPass);
                if (check == false)
                    createUnExistFolderFtp(tempofpathes.Last(), s);
                tempofpathes.Add(ftppathrazdel);
            }

        }

        private void download(object sender, DoWorkEventArgs e)
        {
            lock(this)
            {
            checkandcreatefolderftp();
            FolderBrowserDialog Foldersource = new FolderBrowserDialog();
            TempParent tmp = new TempParent();
            tmp.dir = null;
            tmp.path = "/";
            temp.Add(tmp);
            check(new DirectoryInfo(treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName), sender, e);
            check2(new DirectoryInfo(treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName), sender, e);
          //  MessageBox.Show("Ready!");
        }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bw1.WorkerSupportsCancellation == true)
            {
                bw1.CancelAsync();

            }
            button1.Enabled = true;
        }


        private void bw1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            download(sender, e);
        }

        private void bw1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
            if (e.Cancelled == true)
            {
                MessageBox.Show("Canceled!");
            }
            else if (e.Error != null)
            {
               MessageBox.Show("Error: " + e.Error.Message);
            }
            else
            {
               MessageBox.Show("Done!");
            }
        }

        private void bw1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.PerformStep();
            label2.Text = e.ProgressPercentage.ToString();          
        }

        private void kolichestvofilov()
        {
            int a = Directory.GetFiles(treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName, "*.*" , SearchOption.AllDirectories).Count();
            label5.Text = a.ToString();
            progressBar1.Maximum = a;

        }

        private void BackGroundForm8_Load(object sender, EventArgs e)
        {
            kolichestvofilov();
        }

    }
}
