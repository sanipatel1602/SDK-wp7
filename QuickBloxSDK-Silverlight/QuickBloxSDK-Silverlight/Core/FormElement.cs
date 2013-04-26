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
using System.Text;

namespace QuickBloxSDK_Silverlight.Core
{
    /// <summary>
    /// Поле формы
    /// </summary>
    public class FormElement
    {
        /// <summary>
        /// Название поля
        /// </summary>
        public string key
        { get; set; }

        /// <summary>
        /// Значение поля
        /// </summary>
        public string value
        { get; set; }


        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.key))
                return string.Empty;

            StringBuilder result = new StringBuilder();
            result.Append(this.key);
            result.Append("=");
            result.Append(string.IsNullOrEmpty(this.value)?string.Empty:this.value);
            return result.ToString();
        }
    }
}
