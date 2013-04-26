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
//--------
namespace QuickBlox.SuperSample.Model
{
    public enum MessageType
    {
        /// <summary>
        ///  User's location         
        /// </summary>
        Location,
        /// <summary>
        /// Message for common Chat
        /// </summary>
        Message,
        /// <summary>
        /// Private message for some User
        /// </summary>
        PrivateMessage,
        /// <summary>
        /// My visited places
        /// </summary>
        VisitedPlaces
    }
}
