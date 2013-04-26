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

namespace QuickBloxSDK_Silverlight.Core
{
    public class MessageBase : Object
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsOK
        { get; set; }

        /// <summary>
        /// if IsOK= False ErrorMessage not empty
        /// </summary>
        public string ErrorMessage
        { get; set; }
    }
}
