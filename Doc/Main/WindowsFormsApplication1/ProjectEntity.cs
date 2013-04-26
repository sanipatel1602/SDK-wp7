using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace WindowsFormsApplication1
{
    public class ProjectEntity
    {
        //название проекта
        private string projname;
        public string ProjectName
        {
            get
            {
                return projname;
            }
            set
            {
                projname = value;
            }
        }

        //каталог источника
        private string dirsource;
        public string DirectorySource
        {
            get
            { 
                return dirsource;
            }
            set
            {
                dirsource =value;
            }
        }
        
        //путь к папке проекта
        private string projsource;
        public string ProjectSource
        {
            get
            {
                return projsource;
            }
            set
            {
                projsource = value;
            }
        }

        // фтп настройки
        private string ftphost;
        public string FTPHost
        {
            get
            {
                return ftphost;
            }
            set
            {
                ftphost = value;
            }
        }

        private string ftppass;
        public string FTPPass
        {
            get
            {
                return ftppass;
            }
            set
            {
                ftppass = value;
            }
        }

        private string ftplog;
        public string FTPLogin
        {
            get
            {
                return ftplog;
            }
            set
            {
                ftplog = value;
            }
        }

        // статус, состояние проекта (шаг на котором находимся)
        private string state;
        public string Status
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        //название проекта
        private string ftppath;
        public string FtpPath
        {
            get
            {
                return ftppath;
            }
            set
            {
                ftppath = value;
            }
        }
        //ссылка на шаблон
        private string cssstyle;
        public string Cssstyle
        {
            get
            {
                return cssstyle;
            }
            set
            {
                cssstyle = value;
            }
        }


        public class Book
        {
            public String projname;
        }

        public void SaveProject(string projname, string foldersource, string ftphost, string ftplogin, string ftppass, string projsource, string ftpdir)
        {
            string type = "";
            int lineInt = 0;
            string source = "C:\\Projects\\Xml_Projects\\" + projname +".xml";
            int.TryParse(projname, out lineInt);
            if (!Directory.Exists("C:\\Projects\\Xml_Projects\\"))
                Directory.CreateDirectory("C:\\Projects\\Xml_Projects\\");
            // Cоздаем экземпляр класса
            XmlDocument Document = new XmlDocument();
     
            // Загружаем XML файл
            Document.Load(source);

            XmlNode root = Document.DocumentElement;

            //  для List<T> XmlNode t = Document.SelectNodes();

            XmlElement entryElement = Document.CreateElement("entry");
            entryElement.SetAttribute("type", type);

            XmlElement nod = Document.CreateElement("ProjectName");
            nod.InnerText = projname;
            root.AppendChild(nod);

            XmlElement nod1 = Document.CreateElement("FolderSource");
            nod1.InnerText = foldersource;
            root.AppendChild(nod1);

            XmlElement nod2 = Document.CreateElement("FTP-Host");
            nod2.InnerText = ftphost;
            root.AppendChild(nod2);

            XmlElement nod3 = Document.CreateElement("FTPLogin");
            nod3.InnerText = ftplogin;
            root.AppendChild(nod3);

            XmlElement nod4 = Document.CreateElement("FTPPass");
            nod4.InnerText = ftppass;
            root.AppendChild(nod4);

            XmlElement nod5 = Document.CreateElement("ProjectSource");
            nod5.InnerText = projsource;
            root.AppendChild(nod5);

            XmlElement nod6 = Document.CreateElement("FTPDir");
            nod6.InnerText = ftpdir;
            root.AppendChild(nod6);
            //CSS
            XmlNode node1 = Document.CreateNode(XmlNodeType.Element, "CSS-Temps", "");
            root.AppendChild(node1);

            XmlNode node2 = Document.CreateNode(XmlNodeType.Element, "Element", "");
            node1.AppendChild(node2);
            //CSS-attributes
            XmlElement nod7 = Document.CreateElement("NameOfTextBox");
            nod7.InnerText = ftpdir;
            node2.AppendChild(nod7);

            XmlElement nod8 = Document.CreateElement("ValueOfAttribute");
            nod8.InnerText = projsource;
            node2.AppendChild(nod8);

            Document.Save(source);
        }


        //Конструкторы
        public ProjectEntity()
        {
        }

        public string pathdir;
        public ProjectEntity(string path)
        {

            // Объявляем и забиваем файл в документ   
            XmlDocument xd = new XmlDocument();
            FileStream fs = new FileStream(path, FileMode.Open);
            xd.Load(fs);
            XmlNodeList nodes = xd.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "Project")
                    foreach (XmlElement attr in node)
                    {
                        switch(attr.Name)
                        {
                            case "ProjectName": 
                                {
                                    this.projname = attr.InnerText;
                                    break;
                                }
                            case "FolderSource":
                                {
                                    this.dirsource = attr.InnerText;
                                    break;
                                }
                            case "FTP-Host":
                                {
                                    this.ftphost = attr.InnerText;
                                    break;
                                }
                            case "FTPLogin":
                                {
                                    this.ftplog = attr.InnerText;
                                    break;
                                }
                            case "FTPPass":
                                {
                                    this.ftppass = attr.InnerText;
                                    break;
                                }
                            case "ProjectSource":
                                {
                                    this.projsource = attr.InnerText;
                                    break;
                                }
                            case "FTPDir":
                                {
                                    this.ftppath = attr.InnerText;
                                    break;
                                }

                            default: 
                                break;
                        }
                    }
            }
            // Закрываем поток   
            fs.Close();
        }

        //переобьявление метода ToString()
        public override string ToString()
        {
            return projname;
        }


        //CSS - temps
        private List<string> CssTemps;
        public List<string> CssTemp
        {
            get
            {
                return CssTemps;
            }
            set
            {
                CssTemps = value;
            }
        }

        public struct TempCSS
        {
            public string txtboxname;
            public string value;
        }

    }   
    
}
