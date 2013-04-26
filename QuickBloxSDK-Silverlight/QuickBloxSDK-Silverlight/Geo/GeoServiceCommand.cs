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

namespace QuickBloxSDK_Silverlight.Geo
{
    /// <summary>
    /// Команда, которая была выполнина в данный момент
    /// </summary>
    public enum GeoServiceCommand
    {
        
        GetGeoLocations,
        AddGeoLocation,
        GetGeoLocation,
        GetGeoLocationsForApp,
        GetGeoLocationsForUser
    }
}
