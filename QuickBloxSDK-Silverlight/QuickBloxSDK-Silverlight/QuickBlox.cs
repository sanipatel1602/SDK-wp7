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
using QuickBloxSDK_Silverlight.users;
using QuickBloxSDK_Silverlight.Core;
using QuickBloxSDK_Silverlight.Geo;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using QuickBloxSDK_Silverlight.owners;

namespace QuickBloxSDK_Silverlight
{
    public class QuickBlox: IQuickBlox
    {
        /// <summary>
        /// Контекст подключения
        /// </summary>
        private ConnectionContext Сontext;



        public bool IsOnlyOneEventHandler
        {
            get
            {
                return this.Сontext.IsOnlyOneEventHandler;
            }
            set
            {
                this.Сontext.IsOnlyOneEventHandler = value;
            }
        }


        /// <summary>
        /// Гео сервис
        /// </summary>
        public GeoService geoService
        {
            get;
            set;
        }

        public OwnersService ownerService
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public UserService userService
        { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public QuickBlox(int AppId, int OwnerId , string AuthenticationKey, string AuthenticationSecret)
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            this._ApplicationId = AppId;
            this.IsOnline = false;
            this._AuthenticationKey = string.IsNullOrEmpty(AuthenticationKey) ? string.Empty : AuthenticationKey;
            this._AuthenticationSecret = string.IsNullOrEmpty(AuthenticationSecret) ? string.Empty : AuthenticationSecret;
            this.Сontext = new ConnectionContext(AppId);
            this.BackgroundConnection = new ConnectionContext(AppId);
            this.BackgroundConnection.RequestResult += new ConnectionContext.Main(BackgroundConnection_RequestResult);
            this.geoService = new GeoService(this.Сontext);
            this.userService = new UserService(this.Сontext);
            this.ownerService = new OwnersService(this.Сontext);
            this.userService.user = this.QBUser;
            this.userService.IsOnline = this.IsOnline;
            this.OwnerId = OwnerId;
            this.userService.OwnerId = this.OwnerId;
            this.IsQBUserLoaded = false;
            this.IsQBUsersLoaded = false;
            this.GetBgRequest();

        }

       

       


        
        private string _Username, _Password, _AuthenticationSecret, _AuthenticationKey;
        private int _UserId, _GeoUserId, _ApplicationId;

        public decimal Latitude
        {get; set;}

        public decimal Longitude
        { get; set; }

        public int OwnerId
        { get; private set; }


        public bool IsQBUserLoaded
        { get; set; }

        public User QBUser
        { get; set; }

        public bool IsQBUsersLoaded
        { get; set; }

        public User[] QBUsers
        { get; set; }

        public bool IsGeoDataLoad
        {
            get;
            set;
        }

        public GeoData[] GeoData
        {
            get;
            set;
        }
        
        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
               this._Username = value;
            }
        }

        public int UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                this._UserId = value;
            }
        }

        public int GeoUserId
        {
            get
            {
                return this._GeoUserId;
            }
            set
            {
                this._GeoUserId = value;
            }
        }

        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }

        public int ApplicationId
        {
            get
            {
                return this._ApplicationId;
            }
            set
            {
                this._ApplicationId = value;
            }
        }

        public string AuthenticationSecret
        {
            get
            {
                return this._AuthenticationSecret;
            }
            set
            {
                this._AuthenticationSecret = value;
            }
        }

        public string AuthenticationKey
        {
            get
            {
                return this._AuthenticationKey;
            }
            set
            {
                this._AuthenticationKey = value;
            }
        }

        public void LogOn()
        {
            this.userService.Authenticate(this.Username, this.OwnerId, this.Password);
        }

        public void LogOff()
        {
            this.userService.Logout();
        }


        public void Ping()
        {
            this.userService.Identify();
        }


        /// <summary>
        /// Is QuickBlox user online
        /// </summary>
        public bool IsOnline
        {
            get;
            private set;
        }


        #region DataUpdate

        /// <summary>
        /// Interval in seconds
        /// </summary>
        public int PingInterval
        {
            get
            {
                if (timer != null)
                    return  (int)timer.Interval.TotalSeconds;
                else
                    return -1;
            }
            set
            {
                if (timer != null)
                {
                    if (value < 1)
                    {
                        timer.Interval = new TimeSpan(0, 0, 1);
                        this.interval = 1;
                    }
                    else
                    {
                        timer.Interval = new TimeSpan(0, 0, value);
                        this.interval = value;
                    }
                }
            }

        }
       public List<string> UpdateErrorList;
       private System.Windows.Threading.DispatcherTimer timer;
       private int interval = 2;
       private ConnectionContext BackgroundConnection;
       int pageCount = 1;
       private void BackgroundConnection_RequestResult(Result result)
       {
           switch (result.ServerName)
           {
               case "location.quickblox.com":
                   {
                       if (result.ControllerName.Contains("/geodata/find") && result.URI.Contains("app.id") && !result.URI.Contains("user.id"))
                       {
                           if (result.ResultStatus == Status.OK)
                           {
                               try
                               {


                                   List<GeoData> users = new List<GeoData>();
                                   XElement xml = XElement.Parse((string)result.Content);
                                   foreach (var t in xml.Descendants("geo-datum"))
                                   {
                                       try
                                       {
                                           users.Add(new GeoData(t.ToString()));
                                       }
                                       catch { }                                   
                                   }

                                  //// var rtr = users.Where(t=>t!=null).Where(t => !(this.GeoData.Where(t1=>t1!=null).Select(t1 => t1.Id).ToArray().Contains(t.Id))).ToList();
                                  // if (this.GeoData == null)
                                  // {
                                  //     this.GeoData = users.Where(t => t != null).ToArray();
                                  // }
                                  // else
                                  //     this.GeoData = users.Where(t => !this.GeoData.Select(t1 => t1.Id).Contains(t.Id)).ToList().Union(this.GeoData).ToArray();


                                   this.GeoData = users.Where(t => t != null).OrderBy(t=>t.CreatedDate).ToArray();
                                   this.IsGeoDataLoad = true;

                                   try
                                   {
                                       pageCount = int.Parse(xml.Element("pages_count").Value);
                                   }
                                   catch
                                   {
                                       pageCount = 1;
                                   }
                                   if (this.BackgroundEvent != null)
                                       this.BackgroundEvent("geodata", GeoData); 
                                  
                               }
                               catch (Exception ex)
                               {
                                   this.IsGeoDataLoad = false;
                               }
                           }
                       }
                       break;
                   }
               case "users.quickblox.com":
                   {
                       if (result.ControllerName.Contains("/owners/") && result.ControllerName.Contains("/users.xml"))
                       {
                           if (result.ResultStatus == Status.OK)
                           {
                               try
                               {
                                   List<User> users = new List<User>();
                                   XElement xml = XElement.Parse((string)result.Content);
                                   foreach (var t in xml.Descendants("user"))
                                       users.Add(new User(t.ToString()));

                                   this.QBUsers = users.ToArray();
                                  
                                   



                                   this.IsQBUsersLoaded = true;
                                   if(this.BackgroundEvent !=null)
                                       this.BackgroundEvent("users", QBUsers);
                               }
                               catch (Exception ex)
                               {
                                   this.IsQBUsersLoaded = false;
                               }
                           }
                           return;
                       }
                       break;
                   }
           }
       }

       public void BackgroundUpdateStart()
        {
            try
            {
                this.UpdateErrorList = new List<string>();
                timer.Interval = new TimeSpan(0, 0, this.interval);
                this.timer.Start();
            }
            catch
            {

            }
        }

       public void BackgroundUpdateStop()
        {
            try
            {
                this.timer.Stop();
            }
            catch
            {

            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.GetBgRequest();
        }

        public int CounteLastGeoPage = 3;


        private void GetBgRequest()
        {
            this.BackgroundConnection.CurrentPart = Part.users;
            this.BackgroundConnection.SendAsyncRequest("owners/" + this.OwnerId + "/users.xml", AcceptVerbs.GET);


            this.BackgroundConnection.CurrentPart = Part.geopos;
            this.BackgroundConnection.Add("page", this.pageCount.ToString());
            this.BackgroundConnection.Add("page_size", "100");
            this.BackgroundConnection.Add("app.id", this.Сontext.ApplicationId.ToString());
            this.BackgroundConnection.SendAsyncRequest("geodata/find.xml", AcceptVerbs.GET);
           /* for (int i = 1; i <= this.CounteLastGeoPage; i++)
                this.GeoDataRequest(i);*/
        }

        private void GeoDataRequest(int LastPage)
        {
            if ((this.pageCount - LastPage) < 1)
                return;

            this.BackgroundConnection.CurrentPart = Part.geopos;
            this.BackgroundConnection.Add("page", (this.pageCount - LastPage).ToString());
            this.BackgroundConnection.Add("page_size", "100");
            this.BackgroundConnection.Add("app.id", this.Сontext.ApplicationId.ToString());
            this.BackgroundConnection.SendAsyncRequest("geodata/find.xml", AcceptVerbs.GET);
            
        }

        public delegate void BGR(string Command, object Result);

        public event BGR BackgroundEvent;

        #endregion

    }

   

}
