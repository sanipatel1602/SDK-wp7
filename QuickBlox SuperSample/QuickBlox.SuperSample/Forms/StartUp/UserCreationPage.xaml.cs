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
using System.ComponentModel;
//--------
///QuickBlox SDK
using QuickBloxSDK_Silverlight;
using QuickBloxSDK_Silverlight.Core;
using QuickBloxSDK_Silverlight.Geo;
using QuickBloxSDK_Silverlight.users;
using QuickBlox.SuperSample.Core;
using QuickBlox.SuperSample.Model;
//

namespace QuickBlox.SuperSample
{
    public partial class UserCreationPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        /// <summary>
        /// Service Context
        /// </summary>
        public QuickBloxSDK_Silverlight.QuickBlox QBlox;

        /// <summary>
        /// Current Application Context
        /// </summary>
        public App appContext;

        public UserCreationPage()
        {
            InitializeComponent();

            //Set up View Model for UI
            DataContext = this;

            //The flag for fields (validation)
            txtUserName.Tag = txtUserEmail.Tag = txtPasswordBox.Tag = txtPasswordBoxRepeat.Tag = false;

            //Set up service Context for this Page
            this.appContext = App.Current as App;
            this.QBlox = this.appContext.QBlox;

            //Set Up handlers of Page LifeTime
            this.Loaded += new RoutedEventHandler(UserCreationPage_Loaded);
            this.Unloaded +=new RoutedEventHandler(UserCreationPage_Unloaded);
            this.BackKeyPress +=new EventHandler<CancelEventArgs>(UserCreationPage_BackKeyPress);
        }

        #region QB Services Event Handlers

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
                    case UserServiceCommand.AddUser:
                        {
                            if (args.status == Status.OK)
                            {
                                User resultUser = (User)args.result;
                                #region Save process status
                                App.lastValidatedUser = new SuperSampleUser(resultUser);
                                App.lastValidatedUser.Password = Password;
                                App.lastValidatedUser.isPasswordSaved = true;
                                App.lastValidatedUser.status = SuperSampleUser.RegistrationStatus.Creation;
                                App.SaveSettings();                                
                                CurrentRegistrationProcessStatus.Value = 0.3;
                                #endregion
                                #region Save data to current service context
                                QBlox.UserId = resultUser.id;
                                QBlox.Username = resultUser.Username;
                                QBlox.Password = Password;
                                QBlox.QBUser = resultUser;
                                #endregion
                                //Start the next operation - set password
                                QBlox.userService.SetNewPassword(App.lastValidatedUser.ID, QBlox.OwnerId, App.lastValidatedUser.Password);
                            }
                            else
                            {
                                VisualStateManager.GoToState(this, "NormalState", true);
                                MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));
                            }
                            break;
                        }
                    case UserServiceCommand.SetNewPassword:
                        {
                            if (args.status == Status.OK)
                            {
                                #region Save process status
                                App.lastValidatedUser.status = SuperSampleUser.RegistrationStatus.Password;
                                App.SaveSettings();                                
                                CurrentRegistrationProcessStatus.Value = 0.6;
                                #endregion
                                //Try to make email verification
                                QBlox.userService.EmailVerification(QBlox.QBUser, true);
                            }
                            else
                            {
                                VisualStateManager.GoToState(this, "NormalState", true);
                                this.QBlox.userService.DeleteUser(App.lastValidatedUser.ID, false);
                                MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));
                            }
                            break;
                        }
                    case UserServiceCommand.EmailVerification:
                        {
                            if (args.status == Status.OK)
                            {
                                #region Save process status
                                App.lastValidatedUser.status = SuperSampleUser.RegistrationStatus.EmailVerification;
                                App.SaveSettings();                                
                                CurrentRegistrationProcessStatus.Value = 0.9;
                                #endregion
                                //This is QBlox.userService.Authenticate(UserName, OwnerID, Password) with data from current context
                                QBlox.LogOn();
                            }
                            else
                            {
                                VisualStateManager.GoToState(this, "NormalState", true);
                                this.QBlox.userService.DeleteUser(App.lastValidatedUser.ID, false);
                                MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));
                            }
                            break;
                        }
                    case UserServiceCommand.Authenticate:
                        {

                            if (args.status == Status.OK)
                            {
                                #region Save process status
                                App.lastValidatedUser.status = SuperSampleUser.RegistrationStatus.Authentification;
                                App.SaveSettings();                                
                                CurrentRegistrationProcessStatus.Value = 1;

                                this.NavigationService.Navigate(new Uri("/Forms/MainPanorama/MainPanoramaPage.xaml", UriKind.Relative));

                                #endregion

                            }
                            else
                            {
                                VisualStateManager.GoToState(this, "NormalState", true);
                                this.QBlox.userService.DeleteUser(App.lastValidatedUser.ID, false);
                                MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));
                            }
                            break;
                        }
                    case UserServiceCommand.DeleteUser:
                        {
                            if (args.status == Status.OK)
                            {
                                CurrentRegistrationProcessStatus.Value = 0;                                
                            }
                            else
                                VisualStateManager.GoToState(this, "NormalState", true);
                            break;
                        }
                    default:
                        break;
                }
            }));
        }
        #endregion

        #region ViewModel for this Page (login, name, email, password, process states)

        private string email = String.Empty;
        /// <summary>
        /// Email for user account
        /// </summary>
        public string Email
        {
            get { return email; }
            set
            {
                if (Validators.Validate(value, Validators.ValidationPatterns.email))
                {
                    email = value;
                    RaisePropertyChanged("Email");
                }
                else
                    throw new Exception("Email validation error!");
            }
        }
        private string userName = String.Empty;
        /// <summary>
        /// Login name for account
        /// </summary>
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
        private string fullName = String.Empty;
        /// <summary>
        /// User's full name
        /// </summary>
        public string FullName
        {
            get { return fullName; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    fullName = value;
                    RaisePropertyChanged("FullName");
                }
                else
                    throw new Exception("FullName validation error!");
            }
        }
        private string password = String.Empty;
        /// <summary>
        /// Password to set for account
        /// </summary>
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
        private string passwordRepeat = String.Empty;
        /// <summary>
        /// The copy of password to check
        /// </summary>
        public string PasswordRepeat
        {
            get { return passwordRepeat; }
            set
            {
                if (this.Password == value)
                {
                    passwordRepeat = value;
                    RaisePropertyChanged("PasswordRepeat");
                }
                else
                    throw new Exception("Passwords are not the same!");
            }
        }
        /// <summary>
        /// Filed validator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            switch (e.Error.ErrorContent.ToString())
            {
                case "Email validation error!":
                    {
                        txtUserEmail.Tag = (e.Action == ValidationErrorEventAction.Added) ? true : false;
                        break;
                    }
                case "Name validation error!":
                    {
                        txtUserName.Tag = (e.Action == ValidationErrorEventAction.Added) ? true : false;
                        break;
                    }
                case "FullName validation error!":
                    {
                        txtUserFullName.Tag = (e.Action == ValidationErrorEventAction.Added) ? true : false;
                        break;
                    }
                case "Password validation error!":
                    {
                        txtPasswordBox.Tag = (e.Action == ValidationErrorEventAction.Added) ? true : false;
                        break;
                    }
                case "Passwords are not the same!":
                    {
                        txtPasswordBoxRepeat.Tag = (e.Action == ValidationErrorEventAction.Added) ? true : false;
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
        /// AppBar button delete handlers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteUserBtn_Click(object sender, System.EventArgs e)
        {
            this.QBlox.userService.DeleteUser(50, false);
            VisualStateManager.GoToState(this, "WaitState", true);
            CurrentRegistrationProcessStatus.Value = 0.2;
        }
        /// <summary>
        /// AppBar button register event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewUserBtn_Click(object sender, System.EventArgs e)
        {
            this.Focus();

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(PasswordRepeat) || (bool)txtUserName.Tag || (bool)txtUserEmail.Tag || (bool)txtPasswordBox.Tag || (this.Password != txtPasswordBoxRepeat.Password))
            {
                MessageBox.Show("Please fill in fields with correct values!");
            }
            else
            {
                //Change State for "Waiting"
                VisualStateManager.GoToState(this, "WaitState", true);

                //Create service User
                User tmpUser = new User(UserName, appContext.OwnerID, Email, appContext.Device);//
                tmpUser.FullName = FullName;

                //Start the registration process by adding the user
                this.QBlox.userService.AddUser(tmpUser);
            }
        }
        #endregion

        #region Page LifeTime Handlers
        /// <summary>
        /// Handler for Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UserCreationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Set Up event handler for User Service
            this.QBlox.userService.UserServiceEvent += new UserService.UserServiceHeandler(UserService_EventHandler);            
        }
        /// <summary>
        /// Handler for Page Unload (Closed) event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UserCreationPage_Unloaded(object sender, RoutedEventArgs e)
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
        public void UserCreationPage_BackKeyPress(object sender, CancelEventArgs e)
        {
            App.SaveSettings();

            //Remove event handler for User Service
            this.QBlox.userService.UserServiceEvent -= new UserService.UserServiceHeandler(UserService_EventHandler);          
        }
        #endregion  

    }
}
