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
using Microsoft.Phone.Shell;
using System.Text;
///QuickBlox SDK
using QuickBloxSDK_Silverlight;
using QuickBloxSDK_Silverlight.Core;
using QuickBloxSDK_Silverlight.Geo;
using QuickBloxSDK_Silverlight.users;
//---------
using QuickBlox.SuperSample.Core;
using System.ComponentModel;
using QuickBlox.SuperSample.Model;
using Microsoft.Phone.Tasks;

namespace QuickBlox.SuperSample
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        /// <summary>
        /// Service Context
        /// </summary>
        public QuickBloxSDK_Silverlight.QuickBlox QBlox;

        /// <summary>
        /// Current Application Context
        /// </summary>
        public App appContext;

        /// <summary>
        /// timer for splashScreen
        /// </summary>
        System.Windows.Threading.DispatcherTimer _splashTimer;

        public MainPage()
        {
            //Set up service Context for this Page
            this.appContext = App.Current as App;
            this.QBlox = this.appContext.QBlox;

            InitializeComponent();
            DataContext = this;

            _splashTimer = new System.Windows.Threading.DispatcherTimer();

            //Set Up handlers of Page LifeTime
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            this.Unloaded += new RoutedEventHandler(MainPage_UnLoaded);
            this.BackKeyPress += new EventHandler<CancelEventArgs>(MainPage_BackKeyPress);                                    
        }

        /// <summary>
        /// User Service Event Handler
        /// </summary>
        /// <param name="args"></param>
        void UserService_EventHandler(UserServiceEventArgs args)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                switch (args.currentCommand)
                {
                    case UserServiceCommand.Authenticate:
                        {
                            if (args.status == Status.OK)
                            {
                                #region Save State
                                User result = (User) args.result;
                                App.lastValidatedUser = new SuperSampleUser(result);
                                App.lastValidatedUser.Password = Password;
                                App.lastValidatedUser.isPasswordSaved = (bool)chkBoxSavePassword.IsChecked;
                                App.SaveSettings();
                                #endregion

                                #region Save data to current service context
                                QBlox.UserId = result.id;
                                QBlox.Username = result.Username;
                                QBlox.Password = Password;
                                QBlox.QBUser = result;
                                #endregion                               

                                this.NavigationService.Navigate(new Uri("/Forms/MainPanorama/MainPanoramaPage.xaml", UriKind.Relative));
                            }
                            else
                            {
                                MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));
                            }
                            VisualStateManager.GoToState(this, "NormalState", true);
                            break;
                        }

                    default:
                        break;
                }
            }));
        }
        

        #region ViewModel for this Page (login, name, email, password, process states)

        private string userName = String.Empty;
        public string UserName
        {
            get { return userName; }
            set
            {
                if (Validators.Validate(value, Validators.ValidationPatterns.name))
                {
                    userName = value;
                    RaisePropertyChanged("UserName");
                }
                else
                    throw new Exception("Name validation error!");
            }
        }

        private string password = String.Empty;
        public string Password
        {
            get { return password; }
            set
            {
                if (Validators.Validate(value, Validators.ValidationPatterns.name))
                {
                    password = value;
                    RaisePropertyChanged("Password");
                }
                else
                    throw new Exception("Password validation error!");

            }
        }

        private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            switch (e.Error.ErrorContent.ToString())
            {
                case "Name validation error!":
                    {                        
                        break;
                    }
                case "Password validation error!":
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #endregion

        #region Button's event handlers
        /// <summary>
        /// Check for key down event (enter)
        /// </summary>
        /// <param name="sender">txtPassword, txtPasswordBoxRepeat, txtUserEmail, txtUserFullName, txtUserName</param>
        /// <param name="e"></param>
        private void field_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                this.Focus();
        }

        /// <summary>
        /// Register new User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void registerBtn_Click(object sender, System.EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Forms/StartUp/UserCreationPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Open the QUICKBLOX Site
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	WebBrowserTask browserTask = new WebBrowserTask();
            Uri qbUri;
            System.Uri.TryCreate("http://quickblox.com", UriKind.Absolute, out qbUri);
            browserTask.Uri = qbUri;
            browserTask.Show();
        }

        /// <summary>
        /// Try to login with current User Name and Password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginBtn_Click(object sender, System.EventArgs e)
        {
            if (!Validators.Validate(txtPasswordBox.Password, Validators.ValidationPatterns.name) || !Validators.Validate(txtUserName.Text, Validators.ValidationPatterns.name))
            {
                MessageBox.Show("Please fill fields with correct values!");
            }           
            else
            {
                string login = txtUserName.Text;
                string pass = txtPasswordBox.Password;
                //Check for having authentificated user
                if (App.lastValidatedUser != null)
                {
                    if (App.lastValidatedUser.Password == pass)
                    {
                        App.lastValidatedUser.isPasswordSaved = (bool)chkBoxSavePassword.IsChecked;
                        App.SaveSettings();                        

                        this.NavigationService.Navigate(new Uri("/Forms/MainPanorama/MainPanoramaPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        //Change State for "Waiting"
                        VisualStateManager.GoToState(this, "WaitState", true);

                        //Try to authenitificate user with current User Name and Password
                        this.QBlox.userService.Authenticate(login, appContext.OwnerID, pass);
                    }
                }
                else
                {
                    //Change State for "Waiting"
                    VisualStateManager.GoToState(this, "WaitState", true);
                    
                    //Try to authenitificate user with current User Name and Password
                    this.QBlox.userService.Authenticate(login, appContext.OwnerID, pass);
                }
            }
        }

        #endregion

        #region Page LifeTime Handlers

        /// <summary>
        /// Handler for Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            string pageUri = String.Empty;
            PhoneApplicationFrame frame = (PhoneApplicationFrame)Application.Current.RootVisual;
            if (frame.CanGoBack)
            {
                if (frame.BackStack.First().Source.Equals("/Forms/MainPanorama/MainPanoramaPage.xaml"))
                {
                    if (App.lastValidatedUser != null)
                    {
                        SuperSampleUser tmp = App.lastValidatedUser;
                        this.QBlox.QBUser = new User(tmp.ID.ToString(), tmp.OwnerId, tmp.Email, tmp.DeviceId);
                        UserName = App.lastValidatedUser.UserName;
                        if (App.lastValidatedUser.isPasswordSaved)
                            Password = App.lastValidatedUser.Password;
                    }
                }               
            }
            else
            {
                if (App.lastValidatedUser != null)
                {
                    SuperSampleUser tmp = App.lastValidatedUser;
                    this.QBlox.QBUser = new User(tmp.ID.ToString(), tmp.OwnerId, tmp.Email, tmp.DeviceId);

                    if (App.lastValidatedUser.isPasswordSaved)
                    {
                        if (_splashTimer!=null)
                            _splashTimer.Tick -= new EventHandler(_splashTimer_Tick);
                        this.NavigationService.Navigate(new Uri("/Forms/MainPanorama/MainPanoramaPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        UserName = App.lastValidatedUser.UserName;
                        //Set Up event handler for User Service
                        this.QBlox.userService.UserServiceEvent += new UserService.UserServiceHeandler(UserService_EventHandler);

                        if (_splashTimer != null)
                        {
                            _splashTimer.Interval = new TimeSpan(0, 0, 3);
                            _splashTimer.Tick += new EventHandler(_splashTimer_Tick);
                            _splashTimer.Start();
                        }
                    }
                }
                else
                {
                    //Set Up event handler for User Service
                    this.QBlox.userService.UserServiceEvent += new UserService.UserServiceHeandler(UserService_EventHandler);

                    if (_splashTimer != null)
                    {
                        _splashTimer.Interval = new TimeSpan(0, 0, 3);
                        _splashTimer.Tick += new EventHandler(_splashTimer_Tick);
                        _splashTimer.Start();
                    }
                }
            }                     
        }

        /// <summary>
        /// Handler for Timer Tick Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _splashTimer_Tick(object sender, EventArgs e)
        {
            _splashTimer.Stop();
            _splashTimer.Tick -= new EventHandler(_splashTimer_Tick);
            _splashTimer = null;
            splashScreenPanel.Visibility = Visibility.Collapsed;
            loginPanel.Visibility = Visibility.Visible;
            ApplicationBar.IsVisible = true;            
        }

        /// <summary>
        /// Handler for Page Unload (Closed) event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainPage_UnLoaded(object sender, RoutedEventArgs e)
        {
            App.SaveSettings();
            //Remove event handler for User Service
            this.QBlox.userService.UserServiceEvent -= new UserService.UserServiceHeandler(UserService_EventHandler);
        }
        /// <summary>
        /// Handler for BackKey Press event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        public void MainPage_BackKeyPress(object sender, CancelEventArgs e)
        {
            App.SaveSettings();
            //Remove event handler for User Service
            this.QBlox.userService.UserServiceEvent -= new UserService.UserServiceHeandler(UserService_EventHandler);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string pageUri = String.Empty;
            PhoneApplicationFrame frame = (PhoneApplicationFrame)Application.Current.RootVisual;
            if (frame.CanGoBack)
            {
                if (frame.BackStack.First().Source.Equals("/Forms/MainPanorama/MainPanoramaPage.xaml") || frame.BackStack.First().Source.Equals("/Forms/StartUp/UserCreationPage.xaml"))
                {
                    splashScreenPanel.Visibility = Visibility.Collapsed;
                    loginPanel.Visibility = Visibility.Visible;
                    ApplicationBar.IsVisible = true;
                    _splashTimer = null;
                }                
            }
            base.OnNavigatedTo(e);
        }

        #endregion  
       
    }
}
