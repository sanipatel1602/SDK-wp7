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

using QuickBlox.SuperSample.ViewModel;
using QuickBloxSDK_Silverlight;
using QuickBlox.SuperSample;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;


namespace QuickBlox_SuperSample
{
    public partial class UserDetail : PhoneApplicationPage
    {
        SuperSampleViewModel viewModel = (Application.Current as App).RootViewModel;

        private QuickBloxSDK_Silverlight.QuickBlox QBlox;

        public UserDetail()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            this.Unloaded += new RoutedEventHandler(MainPage_UnLoaded);
            this.BackKeyPress += new EventHandler<CancelEventArgs>(MainPage_BackKeyPress);            
            this.QBlox = (App.Current as App).QBlox;
        }

        #region Page LifeTime Handlers
        /// <summary>
        /// Handler for Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainPage_Loaded(object sender, RoutedEventArgs e)
        {            
            this.viewModel.setHandlers();
            DataContext = this.viewModel;            
        }
        /// <summary>
        /// Handler for Page Unload (Closed) event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainPage_UnLoaded(object sender, RoutedEventArgs e)
        {            
            this.viewModel.removeHandlers();
        }
        /// <summary>
        /// Handler for BackKey Press event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainPage_BackKeyPress(object sender, CancelEventArgs e)
        {            
            this.viewModel.removeHandlers();
            string pageUri = String.Empty;
            PhoneApplicationFrame frame = (PhoneApplicationFrame)Application.Current.RootVisual;
            if (frame.CanGoBack)
            {
                if (frame.BackStack.First().Source.Equals("/Forms/MainPanorama/Map/Map.xaml"))
                    this.NavigationService.Navigate(new Uri("/Forms/MainPanorama/Map/Map.xaml", UriKind.Relative));
                else
                    this.NavigationService.Navigate(new Uri("/Forms/MainPanorama/MainPanoramaPage.xaml", UriKind.Relative));
            }         
            
        }        
        #endregion  
		
		#region Buttons Events Handlers
		/// <summary>
		/// Click to Open thw webSite of current Friend
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnWebSite_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WebBrowserTask browserTask = new WebBrowserTask();
            Uri qbUri;
            System.Uri.TryCreate(this.viewModel.CurrentFriend.Website, UriKind.Absolute, out qbUri);
            browserTask.Uri = qbUri;
            browserTask.Show();
        }
		/// <summary>
		/// Click to call Current Friend
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void btnPhone_Click(object sender, System.Windows.RoutedEventArgs e)
        {        	
            PhoneCallTask callTask = new PhoneCallTask();
            callTask.DisplayName = this.viewModel.CurrentFriend.UserName;
            callTask.PhoneNumber = this.viewModel.CurrentFriend.Phone;
            callTask.Show();
        }
        /// <summary>
        /// Click to Send Mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmail_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	EmailComposeTask emailcomposer = new EmailComposeTask();
            emailcomposer.To = String.Format("<a href={0}>{0}</a>", this.viewModel.CurrentFriend.Website);
			emailcomposer.Subject = "quickblox";			
			emailcomposer.Show();
        }
        /// <summary>
        /// Open Up AppBar when field got focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chatField_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationBar.IsVisible = true;        	     
        }
        /// <summary>
        /// Show down AppBar when chatField Lost Focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chatField_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationBar.IsVisible = false;
        }

        /// <summary>
        /// Send Private Message To Friend
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendPrivateMessage_Click(object sender, System.EventArgs e)
        {
            this.Focus();
            ApplicationBar.IsVisible = false;
            this.viewModel.sendPrivateMessage(chatField);
            chatField.Text = String.Empty;
        }
		#endregion
    }
}
