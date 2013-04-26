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
using QuickBloxSDK_Silverlight.Geo;
using QuickBloxSDK_Silverlight.users;

namespace QuickBloxSDK_Silverlight
{
    interface IQuickBlox
    {
       GeoService geoService
        {
            get;
            set;
        }

       UserService userService
        { get; set; }

       void LogOff();
       
       string Username
       { get; set; }

       int UserId
       { get; set; }


       int GeoUserId
       { get; set; }

       string Password
       { get; set; }

       int ApplicationId
       { get; set; }

       string AuthenticationSecret
       { get; set; }

       string AuthenticationKey
       { get; set; }

        void LogOn();


    }
}
