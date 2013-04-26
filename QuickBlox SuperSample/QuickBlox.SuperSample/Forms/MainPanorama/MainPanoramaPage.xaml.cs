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
using System.ComponentModel;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using QuickBlox.SuperSample.ViewModel;
using Microsoft.Phone.Shell;
using System.Collections;
using QuickBlox.SuperSample.Model;
using QuickBloxSDK_Silverlight.Geo;
//--------------
namespace QuickBlox.SuperSample
{
	public partial class MainPanoramaPage : PhoneApplicationPage
	{
		SuperSampleViewModel viewModel;
		
		ApplicationBarIconButton[] tempBar;

        ApplicationBarIconButton btnSendMessage;
        private System.Windows.Threading.DispatcherTimer timer;
        private QuickBloxSDK_Silverlight.QuickBlox QBlox;

       

		public MainPanoramaPage()
		{
			InitializeComponent();
			//Set Up handlers of Page LifeTime
			this.Loaded += new RoutedEventHandler(MainPage_Loaded);
			this.Unloaded += new RoutedEventHandler(MainPage_UnLoaded);
			this.BackKeyPress += new EventHandler<CancelEventArgs>(MainPage_BackKeyPress);
			this.QBlox = (App.Current as App).QBlox;

            timer = new System.Windows.Threading.DispatcherTimer();            
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 5);
		}

        void timer_Tick(object sender, EventArgs e)
        {
            this.RenderMap();
        }

        private void RenderMap()
        {
            PanoramaMap.Children.Clear();
            Pushpin pushpin = new Pushpin();
            Location location = new Location();
            location.Latitude = (double)this.QBlox.Latitude;
            location.Longitude = (double)this.QBlox.Longitude;
            pushpin.Location = location;
            pushpin.Background = new SolidColorBrush(Colors.Blue);
            if (QBlox.IsQBUserLoaded)
                pushpin.Content = QBlox.QBUser.Username;
            pushpin.FontSize = 30;
            PanoramaMap.Children.Add(pushpin);
            PanoramaMap.Center = new GeoCoordinate((double)this.QBlox.Latitude, (double)this.QBlox.Longitude);
            if (!(App.Current as App).IsGeo)
            {
                try
                {
                    QBlox.geoService.AddGeoLocation(new GeoData(QBlox.QBUser.id, this.QBlox.Latitude, this.QBlox.Longitude, ""));
                    (App.Current as App).IsGeo = true;
                }
                catch
                { }
            }
        }

		
		





        /// <summary>
        /// Open Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void TestModeBtn_Click(object sender, System.EventArgs e)
		{
			this.NavigationService.Navigate(new Uri("/Forms/temp/TestMainPage.xaml", UriKind.Relative));			
		}

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogout_Click(object sender, System.EventArgs e)
		{
			App.lastValidatedUser = null;
			App.SaveSettings();
			this.viewModel.removeHandlers();
			this.NavigationService.Navigate(new Uri("/Forms/MainPage.xaml", UriKind.Relative));
		}
		
        /// <summary>
        /// Open Map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnMap_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            NavigationService.Navigate(new Uri("/Forms/MainPanorama/Map/Map.xaml", UriKind.Relative));			
		}
		
        /// <summary>
        /// Tap on the one the friends Icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void userBtn_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			MessageBox.Show(sender.ToString());
		}        
		
		private void chatField_GotFocus(object sender, System.Windows.RoutedEventArgs e)
		{
            tempBar = new ApplicationBarIconButton[] { ApplicationBar.Buttons[0] as ApplicationBarIconButton, ApplicationBar.Buttons[1] as ApplicationBarIconButton };
            ApplicationBar.Buttons.Clear();
            btnSendMessage = new ApplicationBarIconButton();
            btnSendMessage.IconUri = new Uri("/icons/appbar.check.rest.png", UriKind.Relative);
            btnSendMessage.Text = "send";
            ApplicationBar.Buttons.Add(btnSendMessage);
            btnSendMessage.Click += new EventHandler(btnSendMessage_Click);           
		}
		
		private void chatField_LostFocus(object sender, System.Windows.RoutedEventArgs e)
		{
			ApplicationBar.Buttons.Clear();
            ApplicationBar.Buttons.Add(tempBar[0]);
            ApplicationBar.Buttons.Add(tempBar[1]);
		}	

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            this.viewModel.sendMessage(chatField);
            this.Focus();
			ApplicationBar.Buttons.Clear();
            ApplicationBar.Buttons.Add(tempBar[0]);
            ApplicationBar.Buttons.Add(tempBar[1]);
            chatMessages.SelectedIndex = (chatMessages.Items.Count - 1);
			chatField.Text = String.Empty;
        }
		

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textField_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.viewModel != null)
                this.viewModel.UpdateUser(sender as TextBox);
        }

        private void ListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {            
            this.viewModel.CurrentFriend = (sender as ListBox).SelectedItem as SuperSampleUser;
            this.viewModel.LoadPrivateChatWithUser(this.viewModel.CurrentFriend);
            this.NavigationService.Navigate(new Uri("/Forms/MainPanorama/UserDetail/UserDetail.xaml", UriKind.Relative));

        }		

		#region Page LifeTime Handlers
		/// <summary>
		/// Handler for Page Load event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void MainPage_Loaded(object sender, RoutedEventArgs e)
		{          
			(Application.Current as App).RootViewModel = this.viewModel = new ViewModel.SuperSampleViewModel();
			this.viewModel.setHandlers();
			DataContext = this.viewModel;
			PanoramaMap.Mode = new AerialMode(true);
            this.viewModel.LoadData();
            timer.Start();
            this.RenderMap();
		}
		/// <summary>
		/// Handler for Page Unload (Closed) event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void MainPage_UnLoaded(object sender, RoutedEventArgs e)
		{            
			App.SaveSettings();
            timer.Tick -= new EventHandler(timer_Tick);
			this.viewModel.removeHandlers();
		}
		/// <summary>
		/// Handler for BackKey Press event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void MainPage_BackKeyPress(object sender, CancelEventArgs e)
		{            
			App.SaveSettings();
			this.viewModel.removeHandlers();
            timer.Tick -= new EventHandler(timer_Tick);
			this.NavigationService.Navigate(new Uri("/Forms/MainPage.xaml", UriKind.Relative));
		}

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string pageUri = String.Empty;
            PhoneApplicationFrame frame = (PhoneApplicationFrame)Application.Current.RootVisual;
            if (frame.CanGoBack)
            {
                if(frame.BackStack.First().Source.Equals("/Forms/MainPanorama/UserDetail/UserDetail.xaml"))            
                    myPanorama.DefaultItem = myPanorama.Items[3];
                else
                    myPanorama.DefaultItem = myPanorama.Items[0];
            }         
            base.OnNavigatedTo(e);
        }

		#endregion  
	}
}
