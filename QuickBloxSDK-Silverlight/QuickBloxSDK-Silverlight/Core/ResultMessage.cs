using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace QuickBloxSDK_Silverlight.Core
{
    public class ResultMessage
    {

        public ResultMessage(bool IsOk, string Password)
        {
            this.IsOK = IsOk;
            this.Password = Password;
 
        }



        public ResultMessage(string Scheme)
        {


            XElement xmlResult = XElement.Parse(Scheme);
            this.IsOK = bool.Parse(xmlResult.Element("result").Value);
            

            try
            {
                this.Password = xmlResult.Element("password").Value;
            }
            catch
            {
 
            }

        }


        public bool IsOK
        { get; private set; }

        public string Password
        { get; private set; }
    }
}
