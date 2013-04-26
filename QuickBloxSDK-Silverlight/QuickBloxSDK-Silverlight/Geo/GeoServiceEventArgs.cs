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
using QuickBloxSDK_Silverlight.Core;

namespace QuickBloxSDK_Silverlight.Geo
{
    /// <summary>
    /// Objects from Geo Service action Response
    /// </summary>
    public class GeoServiceEventArgs
    {
        /// <summary>
        /// Result Object
        /// </summary>
        public object result
        { get; set; }

        /// <summary>
        /// Result Object Type
        /// </summary>
       public Type t
        { get; set; }

        /// <summary>
        /// Completed Operation Status
        /// </summary>
       public Status status
       { get; set; }

        /// <summary>
        /// Current Command Name
        /// </summary>
       public GeoServiceCommand currentCommand
       { get; set; }

        /// <summary>
        /// Error message (based on current status)
        /// </summary>
       public string errorMessage
       { get; set; }
    }
}
