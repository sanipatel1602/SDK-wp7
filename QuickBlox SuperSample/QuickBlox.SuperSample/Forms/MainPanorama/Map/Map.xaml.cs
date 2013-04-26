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
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using System.Threading;
using QuickBloxSDK_Silverlight.Geo;
using QuickBlox.SuperSample.Model;
using Microsoft.Phone.Controls.Maps.Platform;
using QuickBloxSDK_Silverlight.users;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using System.ComponentModel;

//-------
namespace QuickBlox.SuperSample.Forms
{
    public partial class Map : PhoneApplicationPage
    {

        private enum MapStatus
        {
            MyPlaces,
            I,
            MyFreands
        }
        private Accelerometer accelerometer;
        public QuickBloxSDK_Silverlight.QuickBlox QBlox
        {
            get;
            private set;
        }
        private System.Windows.Threading.DispatcherTimer timer;
        private GeoData[] geoData;
        private List<User> users = new List<User>();
        private MapStatus CurrentMapStatus;

        private int Id
        { get; set; }
        bool ZoomFlag = false;
        private Vector3 currentValues;
       
        public Map()
        {
            InitializeComponent();
            this.BackKeyPress += new EventHandler<CancelEventArgs>(MainPage_BackKeyPress);   
            this.timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0,0,5);
            var MainContext = App.Current as App;
            this.CurrentMapStatus = MapStatus.MyFreands;
            this.QBlox = MainContext.QBlox;
            accelerometer = new Accelerometer();
            accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
            this.QBlox.geoService.GeoServiceEvent += new QuickBloxSDK_Silverlight.Geo.GeoService.GeoServiceHeandler(geoService_GeoServiceEvent);
            this.QBlox.userService.UserServiceEvent += new QuickBloxSDK_Silverlight.users.UserService.UserServiceHeandler(userService_UserServiceEvent);
       
        }

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
           /* if (accelerometer.IsDataValid)
            {
                float deltaZ = (currentValues - e.SensorReading.Acceleration).Z;
                float Z = e.SensorReading.Acceleration.Z;

                currentValues = e.SensorReading.Acceleration;

                if (Z < 0 && deltaZ > 0)
                {
                    Dispatcher.BeginInvoke(() => MainMap.ZoomLevel++);
                }
                if (Z > 0 && deltaZ < 0)
                {
                    Dispatcher.BeginInvoke(() => MainMap.ZoomLevel++);
                }
            }*/

        }

        private void ReloadMap()
        {
            ((App)App.Current).RootFrame.Dispatcher.BeginInvoke(new Action(() =>
            {

               
           
            MainMap.Children.Clear();
            switch (CurrentMapStatus)
            {
                case MapStatus.MyPlaces:
                    {

                        if (!ZoomFlag)
                        {
                            MainMap.ZoomLevel = 3;
                            ZoomFlag = true;
                        }

                        if (this.QBlox.GeoData == null)
                            return;

                        if (this.QBlox.GeoData.Length < 1)
                            return;

                        Title.Text = "My visited places...";
                        var m1 = MessageManager.GetVisitedPlaces(this.QBlox.GeoData, QBlox.QBUser.id);
                        if (m1 == null)
                            return;

                        foreach (var t in m1)
                        {
                            Pushpin pushpin = new Pushpin();
                            Location location = new Location();
                            location.Latitude = (double)t.Latitude;
                            location.Longitude = (double)t.Longitude;
                            pushpin.Location = location;
                            pushpin.Background = new SolidColorBrush(Colors.Orange);
                            pushpin.Content = t.Text;
                            pushpin.FontSize = 30;

                            MainMap.Children.Add(pushpin);
                        }
                       
                        break;
                    }
                case MapStatus.MyFreands:
                    {

                        if (!ZoomFlag)
                        {
                            MainMap.ZoomLevel = 3;
                            ZoomFlag = true;
                        }

                        if (this.QBlox.GeoData == null)
                            return;

                        if (this.QBlox.GeoData.Length < 1)
                            return;


                        Title.Text = "My...";
                        var m1 = MessageManager.GetLastLocations(this.QBlox.GeoData);
                        if (m1 == null)
                            return;

                        foreach (var t in m1)
                        {
                            try
                            {
                                if (t.user.Username != this.QBlox.QBUser.Username)
                                {
                                    Pushpin pushpin = new Pushpin();
                                    Location location = new Location();
                                    location.Latitude = (double)t.Latitude;
                                    location.Longitude = (double)t.Longitude;
                                    pushpin.Location = location;
                                    pushpin.Background = new SolidColorBrush(Colors.Orange);
                                    pushpin.Content = t.user.Username;
                                    pushpin.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(pushpin_Tap);
                                    pushpin.FontSize = 30;
                                    MainMap.Children.Add(pushpin);
                                }
                            }
                            catch(Exception ex)
                            {
 
                            }

                           /* if (CurrentLocation.Status == GeoPositionStatus.Ready)
                            {*/
                                Pushpin pushpin1 = new Pushpin();
                                Location location1 = new Location();
                                location1.Latitude = (double)this.QBlox.Latitude;
                                location1.Longitude = (double)this.QBlox.Longitude;
                                pushpin1.Location = location1;
                                pushpin1.Background = new SolidColorBrush(Colors.Blue);
                                pushpin1.Content = QBlox.QBUser.Username;
                                pushpin1.FontSize = 30;
                                MainMap.Children.Add(pushpin1);
                           // }
                           


                        }
                        
                        break;
                    }
                case MapStatus.I:
                    {
                        //Title.Text = "Only i";
                        MainMap.Center = new GeoCoordinate(double.Parse(QBlox.Latitude.ToString()), double.Parse(QBlox.Longitude.ToString()));

                        Pushpin pushpin = new Pushpin();
                        Location location = new Location();
                        location.Latitude = (double)QBlox.Latitude;
                        location.Longitude = (double)QBlox.Longitude;
                        pushpin.Location = location;
                        pushpin.Background = new SolidColorBrush(Colors.Blue);
                        pushpin.Content = QBlox.QBUser.Username;
                        pushpin.FontSize = 30;
                        MainMap.Children.Add(pushpin);
                        if (!ZoomFlag)
                        {
                            MainMap.ZoomLevel = 17;
                            ZoomFlag = true;
                        }
                        break;
                    }
            }
            }));

        }

        void pushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Pushpin pr = sender as Pushpin;
                if (pr == null)
                    return;

                (App.Current as App).RootViewModel.CurrentFriend = new SuperSampleUser(QBlox.QBUsers.SingleOrDefault(t=>t.Username == (string)pr.Content));
                (App.Current as App).RootViewModel.LoadPrivateChatWithUser((App.Current as App).RootViewModel.CurrentFriend);
                this.NavigationService.Navigate(new Uri("/Forms/MainPanorama/UserDetail/UserDetail.xaml", UriKind.Relative));

            }
            catch
            {
 
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            new Thread(this.ReloadMap).Start();
            
        }

        void userService_UserServiceEvent(QuickBloxSDK_Silverlight.users.UserServiceEventArgs Args)
        {
            switch (Args.currentCommand)
            {
                case UserServiceCommand.GetUser:
                    {
                        if (Args.status == QuickBloxSDK_Silverlight.Core.Status.OK)
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                User u = (User)Args.result;
                                if (!this.users.Contains(u))
                                    this.users.Add(u);
                            }));
                        }
                        break;
                    }
            }
        }

        void geoService_GeoServiceEvent(QuickBloxSDK_Silverlight.Geo.GeoServiceEventArgs Args)
        {
           switch(Args.currentCommand)
           {
               case GeoServiceCommand.GetGeoLocationsForApp:
                   {
                       if (Args.status == QuickBloxSDK_Silverlight.Core.Status.OK)
                       {
                           Dispatcher.BeginInvoke(new Action(() =>
                           {
                               if (Args.status == QuickBloxSDK_Silverlight.Core.Status.OK)
                               {
                                   this.geoData = ((GeoPage)Args.result).GeoLocations;
                                   this.ReloadMap();
                               }
                           }));
                       }
                       break;
                   }
                   
                   case GeoServiceCommand.AddGeoLocation:
                   {
                       if (Args.status == QuickBloxSDK_Silverlight.Core.Status.OK)
                       {
                           Dispatcher.BeginInvoke(new Action(() =>
                           {
                               MessageBox.Show("My current position setting succeded");

                           }));
                       }
                       else
                       {
                           Dispatcher.BeginInvoke(new Action(() =>
                           {
                               MessageBox.Show("My current position setting not succeded");

                           }));
                       }
                       break;
                   }
           }
           
        }





        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            MainMap.Mode = new AerialMode(true);
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            MainMap.Mode = new RoadMode();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            MainMap.Mode = new AerialMode(true);
            //accelerometer.Start();
            new Thread(StartTimer).Start();
            

            this.CurrentMapStatus = MapStatus.MyFreands;
            ZoomFlag = false;
            this.ReloadMap();

        }
        void StartTimer()
        {
            ((App)App.Current).RootFrame.Dispatcher.BeginInvoke(new Action(() =>
           {

               timer.Start();
           }));
        }


        private void LablesItem_Click(object sender, EventArgs e)
        {
            this.CurrentMapStatus = MapStatus.MyPlaces;
            ZoomFlag = false;
            
            this.ReloadMap();
        }

        private void FreandsItem_Click(object sender, EventArgs e)
        {
            this.CurrentMapStatus = MapStatus.MyFreands;
            ZoomFlag = false;
            
            this.ReloadMap();
        }

        private void IItem_Click(object sender, EventArgs e)
        {
            this.CurrentMapStatus = MapStatus.I;
            ZoomFlag = false;
            
            this.ReloadMap();
        }

        private void AddPlace_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Forms/AddPlace.xaml?id=" + QBlox.QBUser.id + "&Latitude=" + this.QBlox.Latitude + "&Longitude=" + this.QBlox.Longitude, UriKind.Relative));
         }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Tick -= new EventHandler(timer_Tick);
            this.QBlox.geoService.GeoServiceEvent -= new QuickBloxSDK_Silverlight.Geo.GeoService.GeoServiceHeandler(geoService_GeoServiceEvent);
            this.QBlox.userService.UserServiceEvent -= new QuickBloxSDK_Silverlight.users.UserService.UserServiceHeandler(userService_UserServiceEvent);
           
        }

        private void ApplicationBarMenuItem_Click_2(object sender, EventArgs e)
        {
            GeoData data = new GeoData(this.QBlox.QBUser.id, this.QBlox.Latitude, this.QBlox.Longitude, MessageManager.CreateMyLocation());
            QBlox.geoService.AddGeoLocation(data);
        }


        public void MainPage_BackKeyPress(object sender, CancelEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Forms/MainPanorama/MainPanoramaPage.xaml", UriKind.Relative));
        }        
    }
}