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
using QuickBloxSDK_Silverlight.Geo;
using QuickBlox.SuperSample.Model;
//-----------
namespace QuickBlox.SuperSample.Forms
{
    public partial class AddPlace : PhoneApplicationPage
    {
        public QuickBloxSDK_Silverlight.QuickBlox QBlox
        {
            get;
            private set;
        }
        public AddPlace()
        {
            InitializeComponent();
            var MainContext = App.Current as App;
            this.QBlox = MainContext.QBlox;
            this.QBlox.geoService.GeoServiceEvent += new QuickBloxSDK_Silverlight.Geo.GeoService.GeoServiceHeandler(geoService_GeoServiceEvent);
        }



        void geoService_GeoServiceEvent(QuickBloxSDK_Silverlight.Geo.GeoServiceEventArgs Args)
        {
            switch (Args.currentCommand)
            {
                case GeoServiceCommand.AddGeoLocation:
                    {
                        if (Args.status == QuickBloxSDK_Silverlight.Core.Status.OK)
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (Args.status == QuickBloxSDK_Silverlight.Core.Status.OK)
                                {
                                    this.NavigationService.GoBack();

                                }
                                else
                                {
                                    MessageBox.Show(Args.errorMessage);
                                }
                            }));
                        }
                        break;
                    }
               
              
            }
        }

       
        private decimal Latitude, Longitude;

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text.Text))
                return;
            
            GeoData data = new GeoData(Id, this.Latitude, this.Longitude, MessageManager.CreatePlace(Text.Text));
            QBlox.geoService.AddGeoLocation(data);
           
        }

        int Id;

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.Id = int.Parse(NavigationContext.QueryString["id"]);
            this.Latitude = decimal.Parse(NavigationContext.QueryString["Latitude"]);
            this.Longitude = decimal.Parse(NavigationContext.QueryString["Longitude"]);
            
            
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Text.Text = string.Empty;
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            this.QBlox.geoService.GeoServiceEvent -= new QuickBloxSDK_Silverlight.Geo.GeoService.GeoServiceHeandler(geoService_GeoServiceEvent);
       
        }

    }
}