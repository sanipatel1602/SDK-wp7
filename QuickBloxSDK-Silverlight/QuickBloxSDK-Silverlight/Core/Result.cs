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
    /// <summary>
    /// Данный класс используется классом ConnectionContext
    /// </summary>
    public class Result : MessageBase
    {
        /// <summary>
        /// Контент который пришол от сервера
        /// </summary>
        public string Content
        { get; set; }


        public string ControllerName
        { get; set; }

        public AcceptVerbs Verbs
        { get; set; }

        public string URI
        { get; set; }

        public string ServerName
        { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public Status ResultStatus
        { get; set; }

        
    }
}
