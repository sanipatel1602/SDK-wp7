using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using QuickBloxSDK_Silverlight.Core;
using QuickBloxSDK_Silverlight.Geo;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
//------
namespace QuickBlox.SuperSample.Forms.temp
{
    public partial class Map : PhoneApplicationPage
    {
        QuickBloxSDK_Silverlight.QuickBlox blox;

        // Конструктор
        public Map()
        {
            InitializeComponent();
            var MainContext = App.Current as App;
            this.blox = MainContext.QBlox;
            blox.geoService.GeoServiceEvent += new QuickBloxSDK_Silverlight.Geo.GeoService.GeoServiceHeandler(geoService_GeoServiceEvent);
        }

        void geoService_GeoServiceEvent(QuickBloxSDK_Silverlight.Geo.GeoServiceEventArgs Args)
        {
            switch (Args.currentCommand)
            {
                case GeoServiceCommand.GetGeoLocations:
                    {

                        Dispatcher.BeginInvoke(new Action(() =>
                        {

                            if ((GeoData[])Args.result != null)
                            {
                                foreach (var t in (GeoData[])Args.result)
                                {
                                    Pushpin pushpin = new Pushpin();
                                    Location location = new Location();
                                    location.Latitude = (double)t.Latitude;
                                    location.Longitude = (double)t.Longitude;
                                    pushpin.Location = location;
                                    pushpin.Background = new SolidColorBrush(Colors.Orange);
                                    pushpin.Content = t.UserId.ToString();
                                    pushpin.FontSize = 30;
                                    
                                    map1.Children.Add(pushpin);
                                }
                            }

                        }));
                        break;
                    }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.Id = int.Parse(NavigationContext.QueryString["id"]);

        }

        private int Id;
    }
}