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
//---------
namespace QuickBlox.SuperSample.Forms.temp
{
    public partial class User : PhoneApplicationPage
    {
        QuickBloxSDK_Silverlight.QuickBlox blox;
        public User()
        {
            InitializeComponent();
            var MainContext = App.Current as App;
            this.blox = MainContext.QBlox;
            blox.userService.UserServiceEvent += new QuickBloxSDK_Silverlight.users.UserService.UserServiceHeandler(userService_UserServiceEvent);
        }

        void userService_UserServiceEvent(QuickBloxSDK_Silverlight.users.UserServiceEventArgs Args)
        {
            ;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.Id = int.Parse(NavigationContext.QueryString["id"]);

        }

        private int Id;

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            blox.userService.GetUser(this.Id, true);
        }
    }
}