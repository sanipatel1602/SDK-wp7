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
using System.IO;
using QuickBloxSDK_Silverlight;
using QuickBloxSDK_Silverlight.Core;
using QuickBloxSDK_Silverlight.Geo;
using Microsoft.Phone.Shell;
using System.Text;
using System.Device.Location;
using QuickBloxSDK_Silverlight.users;
using QuickBlox.SuperSample.Model;
//---------
namespace QuickBlox.SuperSample
{
    public partial class TestMainPage : Microsoft.Phone.Controls.PhoneApplicationPage
    {
        public QuickBloxSDK_Silverlight.QuickBlox QBlox
        {
            get;
            private set;
        }
        private GeoCoordinateWatcher CurrentLocation;
        private decimal Latitude, Longitude;
        private int Id
        { get; set; }

        public TestMainPage()
        {
            InitializeComponent();            
            var MainContext = App.Current as App;
            this.QBlox = MainContext.QBlox;
            this.QBlox.geoService.GeoServiceEvent+=new GeoService.GeoServiceHeandler(geoService_GeoServiceEvent);
            this.QBlox.userService.UserServiceEvent += new QuickBloxSDK_Silverlight.users.UserService.UserServiceHeandler(userService_UserServiceEvent);
            this.CurrentLocation = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            CurrentLocation.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(CurrentLocation_PositionChanged);
        }


       


        void userService_UserServiceEvent(QuickBloxSDK_Silverlight.users.UserServiceEventArgs Args)
        {
            switch (Args.currentCommand)
            {
                case UserServiceCommand.GetUser:
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (Args.status == Status.OK)
                            {
                                User user = Args.result as User;
                                if (user == null)
                                    this.QBlox.userService.GetUser(161, false);

                                this.QBlox.userService.EmailVerification(user, false);
                            }
                            else
                            {
                                this.QBlox.userService.GetUser(161, false);
                            }
                        }));
                        break;
                    }
                case UserServiceCommand.EmailVerification:
                    {
                        if (Args.status == Status.OK)
                        {
               
                            QBlox.LogOn();
                        }
                        else
                        {
                            MessageBox.Show(Args.errorMessage);
                        }
                        break;
                    }
                case UserServiceCommand.Authenticate:
                    {
                        if (Args.status == Status.OK)
                        {
                            QBlox.Ping();
                        }
                        else
                        {
                            MessageBox.Show(Args.errorMessage);
                        }
                        break;
                    }
                case UserServiceCommand.Identify:
                    {
                        if (Args.status == Status.OK)
                        {
                            QBlox.LogOff();
                        }
                        else
                        {
                            MessageBox.Show(Args.errorMessage);
                        }
                        break;
                    }
                case UserServiceCommand.Logout:
                    {
                        if (Args.status == Status.OK)
                        {

                        }
                        else
                        {
                            MessageBox.Show(Args.errorMessage);
                        }
                        break;
                    }
                case UserServiceCommand.SetNewPassword:
                    {
                        if (Args.status == Status.OK)
                        {
                            QBlox.userService.EmailVerification(QBlox.QBUser, true);
                        }
                        else
                        {
                            MessageBox.Show(Args.errorMessage);
                        }
                        break;
                    }
                case UserServiceCommand.AddUser:
                    {
                        if (Args.status == Status.OK)
                        {
                            QBlox.UserId = ((User)Args.result).id;
                            QBlox.Username = ((User)Args.result).Username;
                            QBlox.QBUser = (User)Args.result;
                            QBlox.Password = "fsHJ@#jd123";
                            QBlox.userService.SetNewPassword(QBlox.UserId, QBlox.OwnerId, QBlox.Password);
                        }
                        else
                        {
                            MessageBox.Show(Args.errorMessage);
                        }
                        break;
                    }
                case UserServiceCommand.Resetmypasswordbyemail:
                    {
                        if (Args.status == Status.OK)
                        {
                            
                        }
                        else
                        {
                            MessageBox.Show(Args.errorMessage);
                        }
                        break;
                    }
                case UserServiceCommand.Resetpassword:
                    {
                        if (Args.status == Status.OK)
                        {

                        }
                        else
                        {
                            MessageBox.Show(Args.errorMessage);
                        }
                        break;
                    }
                case UserServiceCommand.GetUsersByOwner:
                    {
                        if (Args.status == Status.OK)
                        {

                        }
                        else
                        {
                            MessageBox.Show(Args.errorMessage);
                        }
                        break;
                    }
            }
        }


        void CurrentLocation_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            this.Latitude = new decimal(e.Position.Location.Latitude);
            this.Longitude = new decimal(e.Position.Location.Longitude);

        }

        /// <summary>
        /// Обработчик событий
        /// </summary>
        /// <param name="result"></param>
        /// <param name="t"></param>
        /// <param name="status"></param>
        /// <param name="currentCommand"></param>
        void geoService_GeoServiceEvent(GeoServiceEventArgs args)
        {
            switch (args.currentCommand)
            {
               

                

                case GeoServiceCommand.AddGeoLocation:
                    {
                        if (args.status == Status.OK)
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                GeoData data = (GeoData)args.result;
                                Message mes = new Message(data.Status);
                                MessageBox.Show("OK", "Send message", MessageBoxButton.OK);

                            }));
                        else if (args.status == Status.ValidationError)
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                MessageBox.Show("Validate error", "Send message", MessageBoxButton.OK);

                            }));
                        }
                        else
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                MessageBox.Show(args.errorMessage, "Send message", MessageBoxButton.OK);

                            }));
                        }
                        break;
                    }
                case GeoServiceCommand.GetGeoLocationsForApp:
                    {
                        if (args.status == Status.OK)
                        {
                            GeoPage page =  (GeoPage)args.result;
                            GeoData[] data = page.GeoLocations;
                            var m1 = MessageManager.GetAllMyPrivateMessages(data, 295);
                            var m2 = MessageManager.GetAllPrivateMessagesForMe(data, 295);
                            var m3 = MessageManager.GetMyChatMessages(data, 295);
                            
                        }
                        break;
                    }
                



        }
        }

      

        private void tr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Reload();
            this.CurrentLocation.Start();
        }

        public void Reload()
        {
            this.QBlox.userService.GetUsersByOwner(1);
            this.QBlox.userService.GetUser(2, false);
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationBarMenuItem CurrentItem = (ApplicationBarMenuItem)sender;
            
            if (tr.SelectedItem == null) return;
            NavigationService.Navigate(new Uri("/Forms/temp/Map.xaml?id=" + this.Id, UriKind.Relative));

        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
           /* ApplicationBarIconButton btn = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
            this.Reload();*/

           
            
        }



        private void MenuAdd_Click(object sender, EventArgs e)
        {

        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            if (tr.SelectedItem == null) return;
           // this.QBlox.geoService.DeleteGeoUser(((GeoUser)tr.SelectedItem).id);
        }

        private void MenuEdit_Click(object sender, EventArgs e)
        {
           
            this.Reload();
        }

     
    }
}