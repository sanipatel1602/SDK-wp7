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
using System.ComponentModel;
using System.Collections.ObjectModel;

using System.Linq;
using System.Windows.Threading;
using System.Threading;
using System.Text;
using System.Collections.Generic;

///QuickBlox SuperSample
using QuickBlox.SuperSample.Core;
using QuickBlox.SuperSample.Model;
///-------

///QuickBlox SDK
using QuickBloxSDK_Silverlight;
using QuickBloxSDK_Silverlight.Core;
using QuickBloxSDK_Silverlight.Geo;
using QuickBloxSDK_Silverlight.users;
//-------

namespace QuickBlox.SuperSample.ViewModel
{
	public class SuperSampleViewModel : INotifyPropertyChanged
	{

		private int friendsTickCount;

		/// <summary>
		/// Service Context
		/// </summary>
		public QuickBloxSDK_Silverlight.QuickBlox QBlox;

		/// <summary>
		/// Current Application Context
		/// </summary>
		public App appContext;

		#region Properties
		private ObservableCollection<SuperSampleUser> myFriends;
		/// <summary>
		/// Collection of my friends (User)
		/// </summary>
		public ObservableCollection<SuperSampleUser> MyFriends
		{
			get { return myFriends; }
			set
			{
				if (myFriends == value) return;
				myFriends = value;
				RaisePropertyChanged("MyFriends");
			}
		}

		private ObservableCollection<Message> commonMessages;
		/// <summary>
		/// Messages for Common Chat
		/// </summary>
		public ObservableCollection<Message> CommonMessages
		{
			get { return commonMessages; }
			set
			{
				if (commonMessages == value) return;
				commonMessages = value;
				RaisePropertyChanged("CommonMessages");
			}
		}

		private ObservableCollection<Message> privateMessages;
		/// <summary>
		/// Messages for Private Chat with Some User
		/// </summary>
		public ObservableCollection<Message> PrivateMessages
		{
			get { return privateMessages; }
			set
			{
				if (privateMessages == value) return;
				privateMessages = value;
				RaisePropertyChanged("PrivateMessages");
			}
		}

		private SuperSampleUser me;
		/// <summary>
		/// Current User account
		/// </summary>
		public SuperSampleUser Me
		{
			get { return me; }
			set
			{
				if (me == value) return;
				me = value;
				RaisePropertyChanged("Me");
			}
		}

		public SuperSampleUser currentFriend;
		/// <summary>
		/// Selected Friend
		/// </summary>
		public SuperSampleUser CurrentFriend
		{
			get { return currentFriend; }
			set
			{
				if (currentFriend == value) return;
				currentFriend = value;
				RaisePropertyChanged("CurrentFriend");
			}
		}
		#endregion

		/// <summary>
		/// The base constructor
		/// </summary>
		public SuperSampleViewModel()
		{
			var MainContext = App.Current as App;
			this.QBlox = MainContext.QBlox;           

			Me = (App.lastValidatedUser != null)? App.lastValidatedUser: new SuperSampleUser();

			CurrentFriend = new SuperSampleUser();

			MyFriends = new ObservableCollection<SuperSampleUser>();

			CommonMessages = new ObservableCollection<Message>();

			PrivateMessages = new ObservableCollection<Message>();
		}  

		#region Operations of ViewModel
		/// <summary>
		/// Load Some Data
		/// </summary>
		public void LoadData()
		{
			if (QBlox.QBUsers != null)
			{
				ObservableCollection<SuperSampleUser> tempArray = new ObservableCollection<SuperSampleUser>();
				QBlox.QBUsers.Where(t => t.id != Me.ID).ToList().ForEach(item => tempArray.Add(new SuperSampleUser(item)));
				MyFriends = tempArray;
			}
			else
				QBlox.userService.GetUsersByOwner();
			QBlox.userService.GetUser(Me.ID, false);			
		}

		/// <summary>
		/// Load a new Chat for you and another user
		/// </summary>
		/// <param name="user"></param>
		public void LoadPrivateChatWithUser(SuperSampleUser user)
		{            
			var messages = MessageManager.GetPrivateChatTwoUsers(this.QBlox.GeoData, Me.ID, CurrentFriend.ID);
			if (PrivateMessages != null)
				if(PrivateMessages.Count > 0)
					PrivateMessages.Clear();

			if(messages != null)
				foreach (var tmp in messages)
					PrivateMessages.Add(tmp);			       
		}
		/// <summary>
		/// Update info about Me
		/// </summary>
		public void UpdateUser(TextBox field)
		{
			User temp = (App.Current as App).QBlox.QBUser;

			switch (field.Name)
			{
				case "txtMyFullName":
					{
						temp.FullName = field.Text;
						break;
					}
				case "txtMyPhone":
					{
						temp.Phone =  field.Text;
						break;
					}
				case "txtMyWebSite":
					{
						temp.Website = field.Text;                       
						break;
					}
				default:
					break;
			}

			//Special checks in order to prevent null data
			if (string.IsNullOrEmpty(temp.Website))
				temp.Website = "http://testwebsiteforquickblox.com";
			if(string.IsNullOrEmpty(temp.Phone))
				temp.Website = "00000000.com";          

			QBlox.userService.EditUser(temp);               
		}
		/// <summary>
		/// Send Message to Common Chat
		/// </summary>
		/// <param name="box"></param>
		public void sendMessage(TextBox box)
		{
			this.QBlox.geoService.AddGeoLocation(new GeoData(Me.ID, 0, 0, MessageManager.CreateChatMessage(box.Text)));    
			CommonMessages.Add(new Message(MessageType.Message,me.ID,0,box.Text));
		}
		/// <summary>
		/// Send Private Message to User (Friend)
		/// </summary>
		/// <param name="box"></param>
		public void sendPrivateMessage(TextBox box)
		{ 
			this.QBlox.geoService.AddGeoLocation(new GeoData(Me.ID,0,0,MessageManager.CreatePrivateMessage(CurrentFriend.ID,Me.ID, box.Text)));
			PrivateMessages.Add(new Message(MessageType.Message, me.ID, CurrentFriend.ID, box.Text));
		}
		#endregion      

		#region QB Services Event Handlers

		/// <summary>
		/// Background events handler
		/// </summary>
		/// <param name="Command"></param>
		/// <param name="Result"></param>
		void QBlox_Background_EventHandler(string Command, object Result)
		{
			((App)App.Current).RootFrame.Dispatcher.BeginInvoke(new Action(() =>
			{
				switch (Command)
				{
					case "users":
						{
							//Update only after 1 minute
							if (friendsTickCount == 12)
							{
								ObservableCollection<SuperSampleUser> tempArray = new ObservableCollection<SuperSampleUser>();
								QBlox.QBUsers.Where(t => t.id != Me.ID).ToList().ForEach(item => tempArray.Add(new SuperSampleUser(item)));
								myFriends = tempArray;
								RaisePropertyChanged("MyFriends");
								friendsTickCount = 0;
							}
							else
								friendsTickCount++;
							break;
						}
					case "geodata":
						{             
							var res = MessageManager.GetChatMessages(Result as GeoData[]);                            
							if (res.Count() != CommonMessages.Count())							
								for (int i = CommonMessages.Count(); i < res.Count(); i++)                           
									CommonMessages.Add(res[i]);                                                            					
							
							res = MessageManager.GetPrivateChatTwoUsers(Result as GeoData[], Me.ID, CurrentFriend.ID);                            
							if (res.Count() != PrivateMessages.Count())							
								for (int i = PrivateMessages.Count(); i < res.Count(); i++)                           
									PrivateMessages.Add(res[i]);							
							break;
						}
					default:
						break;
				}
			}));

		}

		/// <summary>
		/// Geo Service Event Handler
		/// </summary>
		/// <param name="args"></param>
		void GeoService_EventHandler(GeoServiceEventArgs args)
		{
			((App)App.Current).RootFrame.Dispatcher.BeginInvoke(new Action(() =>
			{
				switch (args.currentCommand)
				{
					case GeoServiceCommand.AddGeoLocation:
						{
							if (args.status == Status.OK)				
							{
								GeoData res = (args.result as GeoData);                                							
							}								
							else							
								MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));
							break;
						}
					case GeoServiceCommand.GetGeoLocations:
						{
							if (args.status == Status.OK)
							{
								GeoPage page = args.result as GeoPage;

								//Update messages for Common Chat
								var res = MessageManager.GetChatMessages(page.GeoLocations);
								if (res.Count() != CommonMessages.Count())	
									for (int i = CommonMessages.Count(); i < res.Count(); i++)
										CommonMessages.Add(res[i]); 

								//Update Messages for Private Chat
								var resForPrivate = MessageManager.GetPrivateChatTwoUsers(page.GeoLocations, Me.ID, CurrentFriend.ID);
								if (resForPrivate.Count() != PrivateMessages.Count())
									for (int i = PrivateMessages.Count(); i < resForPrivate.Count(); i++)
										PrivateMessages.Add(resForPrivate[i]);
							}
							else
								MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));
							break;
						}
					default:
						break;
				}
			}));           
		}

		/// <summary>
		/// User Service Event Handler
		/// </summary>
		/// <param name="args"></param>
		void UserService_EventHandler(UserServiceEventArgs args)
		{
			((App)App.Current).RootFrame.Dispatcher.BeginInvoke(new Action(() =>
			{
				switch (args.currentCommand)
				{
					//Get Info about User
					case UserServiceCommand.GetUser:
						{
							if (args.status == Status.OK)
							{
								User result = (User)args.result;
								Me = new SuperSampleUser(result);
								this.QBlox.QBUser = result;
								QBlox.IsQBUserLoaded = true;                        
								App.SaveSettings();                                
							}
							else
								MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));       
							break;   
						}
					//Get All users of current Owner
					case UserServiceCommand.GetUsersByOwner:
					{
						if (args.status == Status.OK)
						{
							User[] result = args.result as User[];
							ObservableCollection<SuperSampleUser> tempArray = new ObservableCollection<SuperSampleUser>();
							result.Where(t => t.id != Me.ID).ToList().ForEach(item => tempArray.Add(new SuperSampleUser(item)));
							myFriends = tempArray;
							RaisePropertyChanged("MyFriends");
						}
						else
							MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));       
								break;   
					}
					//Edit user
					case UserServiceCommand.EditUser:
						{
							if (args.status == Status.OK)
							{                                
							   User result = (User)args.result;
							   MessageBox.Show("My User info update was succeded!");
							}
							else							
								MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));       							
							break;
						}
					//Authenticate
					case UserServiceCommand.Authenticate:
						{
							if (args.status != Status.OK)
								MessageBox.Show(ServiceError.GetErrorMessage(args.status, args.errorMessage, args.result));							
							break;
						}                       
					default:
						break;
				}
			}));
		}
		#endregion         

		#region Operations with Handlers
		/// <summary>
		/// Set up all service event handlers 
		/// </summary>
		public void setHandlers()
		{
			//Set Up event handler for User Service
			this.QBlox.userService.UserServiceEvent += new UserService.UserServiceHeandler(UserService_EventHandler);

			//Set Up event hanndler for Geo Service (in order to make geoUsers)
			this.QBlox.geoService.GeoServiceEvent += new GeoService.GeoServiceHeandler(GeoService_EventHandler);

			this.QBlox.BackgroundEvent += new QuickBloxSDK_Silverlight.QuickBlox.BGR(QBlox_Background_EventHandler);
		}
		/// <summary>
		/// Remove all service event handlers 
		/// </summary>
		public void removeHandlers()
		{
			//Remove event handler for User Service
			this.QBlox.userService.UserServiceEvent -= new UserService.UserServiceHeandler(UserService_EventHandler);

			//Remove event hanndler for Geo Service (in order to make geoUsers)
			this.QBlox.geoService.GeoServiceEvent -= new GeoService.GeoServiceHeandler(GeoService_EventHandler);

			this.QBlox.BackgroundEvent -= new QuickBloxSDK_Silverlight.QuickBlox.BGR(QBlox_Background_EventHandler);
		}
		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaisePropertyChanged(string name)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}

		#endregion
	}
}
