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
using System.Collections.Generic;
using System.Xml.Linq;

namespace QuickBloxSDK_Silverlight.Core
{
    public class ValidateErrorElement
    {
        private ValidateErrorElement(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }


        public static ValidateErrorElement[] LoadErrorList(string Scheme)
        {
            if (string.IsNullOrEmpty(Scheme))
                return null;

            try
            {
                List<ValidateErrorElement> elements = new List<ValidateErrorElement>();
                XElement xml = XElement.Parse(Scheme);
                foreach (var t in xml.Elements("error"))
                    elements.Add(new ValidateErrorElement(t.Value));
                
                return elements.ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Сообщение об ошибки
        /// </summary>
        public string ErrorMessage
        { get; private set; }

    }
}
