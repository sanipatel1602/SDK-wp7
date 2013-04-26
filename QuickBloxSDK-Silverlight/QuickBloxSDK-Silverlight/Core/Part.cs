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
    /// Разделы апи.
    /// От разделов зависит конфигурация запросов к серверам.
    /// </summary>
    public enum Part
    {
        users,
        geopos,
        messaging,
        chat,
        blobs,
        ratings
    }
}
