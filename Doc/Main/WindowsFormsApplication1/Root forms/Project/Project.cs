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
    public partial class Project : Form
    {
       
        

        public Project()
        {
            InitializeComponent();           
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
            string path = "C:\\Projects\\Xml_Projects\\" + treenodeproj.ProjectName + "_DeleteFtp.txt";
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

        private void check(DirectoryInfo dir)
        {
            foreach (var t in dir.GetDirectories())
            {
                NamePath = temp.Last().path;
                createFolderFtp(NamePath,t.Name);
                NamePath += t.Name + "/";
                if (t.GetFiles().Count() != 0)
                {
                    foreach (FileInfo file in t.GetFiles())
                    {
                        uploadfiles(NamePath, file.FullName);
                    }
                }

                if (t.GetDirectories().Count() != 0)
                {
                    pair2.dir = t;
                    pair2.path = NamePath;
                    temp.Add(pair2);
                   
                    check(t);                   
                }

            }
            temp.Remove(temp.Last());
        }


        private void check2(DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.GetFiles())
            {
                NamePath = "/";
                uploadfiles(NamePath, file.FullName);
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
            string path = "C:\\Projects\\Xml_Projects\\" + treenodeproj.ProjectName + "_DeleteFtp.txt"; ;
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
            checkandcreatefolderftp();
            richTextBox1.AppendText("\n" + "Копируется содержимое папки " + treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName + " на FTP ...");
            richTextBox1.Update();
            FolderBrowserDialog Foldersource = new FolderBrowserDialog();          
            TempParent tmp = new TempParent();
            tmp.dir = null;
            tmp.path = "/";
            temp.Add(tmp);
            check(new DirectoryInfo(treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName));
            check2(new DirectoryInfo(treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName));

            richTextBox1.AppendText("\n" + "Содержимое папки " + treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName + " успешно скопировано на FTP");
            MessageBox.Show("Ready!");           
        }

        public struct TempParentDelete
        {
            public string folderpathes;
            public string path;
        }
        List<string> folderpathes = new List<string>();

        private void button2_Click(object sender, EventArgs e)
        {
            string path = "C:\\Projects\\" + treenodeproj.ProjectName + @"_ProjectDir\" + treenodeproj.ProjectName + "_DeleteFtp.txt";
            if (File.Exists(path))
            {
                string[] readText = File.ReadAllLines(path, Encoding.UTF8);
                Array.Reverse(readText);
                richTextBox1.AppendText("\n" + "Удаляем с FTP папку" + treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName);
                richTextBox1.Update();
                foreach (string s in readText)
                {
                    DeleteFTPTXT(s);
                }
                File.Delete(path);
                File.Create(path);

                richTextBox1.AppendText("\n" + treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName + " успешно удалена с FTP");
            }
            else
                return;
        }

        private void DelAll()
        {
            List<string> fileList = null;
            fileList = ListFolders(NamePath);
            foreach (var s in fileList)
            {
                string str;
                str = s.Substring(s.IndexOf("/") + 1);
                DeleteFTP(NamePath,str);
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


        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("\n" + "Перезаливаем папку " + treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" +treenodeproj.ProjectName + " нашего проекта");
            richTextBox1.Update();
            DeleteProj(treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName);
            if ( treenodeproj.DirectorySource != "" && treenodeproj.ProjectSource != "")
            {
                DirectoryInfo soursDir = new DirectoryInfo(treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName + "_Temp"); //папка из которой копировать
                DirectoryInfo destDir = new DirectoryInfo(treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" +treenodeproj.ProjectName); //куда копировать
                CopyDir(soursDir, destDir);
            }
            richTextBox1.AppendText("\n" + "Папка " + treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName + " восстановлена из исходника");
        }

        public ProjectEntity treenodeproj;

        private void button5_Click(object sender, EventArgs e)
        {
            TreeOfProject form1 = new TreeOfProject();
            form1.tempor = treenodeproj;
            form1.MdiParent = RootForm.ActiveForm;
            form1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangeCSS form = new ChangeCSS();
            form.project = treenodeproj;
            form.MdiParent = RootForm.ActiveForm;
            form.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Settings6 form = new Settings6();
            form.project = treenodeproj;
            form.MdiParent = RootForm.ActiveForm;     
            form.Show();
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
                string path = "C:\\Projects\\Xml_Projects\\" + treenodeproj.ProjectName + "_DeleteFtp.txt";
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
            catch(Exception ex)
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
                    createUnExistFolderFtp(tempofpathes.Last(),s);
                tempofpathes.Add(ftppathrazdel);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            BackGroundForm8 form = new BackGroundForm8();
            form.treenodeproj = treenodeproj;
            richTextBox1.AppendText("\n" + "Копируется содержимое папки " + treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName + " на FTP ...");
            richTextBox1.Update();
            form.ShowDialog();
            richTextBox1.AppendText("\n" + "Содержимое папки " + treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName + " успешно скопировано на FTP");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Images form = new Images();
            form.MdiParent = RootForm.ActiveForm;
            form.project = treenodeproj;
            form.Show();
            if (!Directory.Exists(treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName +"_Images"))
                Directory.CreateDirectory(treenodeproj.ProjectSource + "\\" + treenodeproj.ProjectName + "_ProjectDir" + "\\" + treenodeproj.ProjectName + "_Images");
        
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ftpFileExist(@"ftp://tdf7.1gb.ru/http");
        }


        //proverka sushestvovania file on ftp server
        public bool ftpFileExist(string fileName)
        {
            WebClient wc = new WebClient();
            try
            {
                wc.Credentials = new NetworkCredential("1gb_tdf7", "59cb06da");
                byte[] fData = wc.DownloadData(fileName);
                if (fData.Length > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                // Debug here?
            }
            return false;
        }


    }
}
