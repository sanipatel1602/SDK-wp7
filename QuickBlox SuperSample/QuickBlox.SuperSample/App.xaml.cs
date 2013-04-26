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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using QuickBloxSDK_Silverlight.Geo;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using QuickBlox.SuperSample.Core;

using Microsoft.Phone.Tasks;
using QuickBlox.SuperSample.ViewModel;
using QuickBlox.SuperSample.Model;
using QuickBloxSDK_Silverlight.owners;
using System.Device.Location;
using System.Threading;
//-------
namespace QuickBlox.SuperSample
{
    public partial class App : Application
    {
        /// <summary>
        /// Application ID
        /// </summary>
        public int AppID = 86;        

        /// <summary>
        /// Owner ID
        /// </summary>
        public int OwnerID = 4331;

        /// <summary>
        /// Device
        /// </summary>
        public string Device = "WindowsPhone7";
        

        public SuperSampleViewModel RootViewModel;
        /// <summary>
        /// Обеспечивает быстрый доступ к корневому кадру приложения телефона.
        /// </summary>
        /// <returns>Корневой кадр приложения телефона.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }
        /// <summary>
        /// Сервис геолокации
        /// </summary>
        private GeoCoordinateWatcher CurrentLocation;
        public static SuperSampleUser lastValidatedUser = null;

        /// <summary>
        /// Service Context
        /// </summary>
        public QuickBloxSDK_Silverlight.QuickBlox QBlox
        { get; set; }

        public Boolean IsGeo
        { get; set; }

        /// <summary>
        /// Конструктор объекта приложения.
        /// </summary>
        public App()
        {
            // Глобальный обработчик неперехваченных исключений. 
            UnhandledException += Application_UnhandledException;

            // Стандартная инициализация Silverlight
            InitializeComponent();

            // Инициализация телефона
            InitializePhoneApplication();
            this.QBlox = new QuickBloxSDK_Silverlight.QuickBlox(AppID, OwnerID, null, null);
            this.QBlox.PingInterval = 10;
            this.QBlox.BackgroundUpdateStart();
            this.IsGeo = false;


            this.CurrentLocation = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            CurrentLocation.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(CurrentLocation_PositionChanged);
            CurrentLocation.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(CurrentLocation_StatusChanged);
            new Thread(StartLocation).Start();

        }

        void StartLocation()
        {
            this.CurrentLocation.TryStart(false, TimeSpan.FromSeconds(60));
        }

        void CurrentLocation_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    if (CurrentLocation.Permission == GeoPositionPermission.Denied)
                        MessageBox.Show("Service is offline");

                    else
                        MessageBox.Show("Your device could not use this service");

                    break;
                case GeoPositionStatus.Initializing:
                   // Title.Text = "Service is initializing...";
                    break;
                case GeoPositionStatus.NoData:
                    //Title.Text = "Location data is not available";
                    break;
                case GeoPositionStatus.Ready:
                    //Title.Text = "Location data is available";
                    break;
            }
        }
        void CurrentLocation_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (CurrentLocation.Status == GeoPositionStatus.Ready)
            {
                this.QBlox.Latitude = new decimal(e.Position.Location.Latitude);
                this.QBlox.Longitude = new decimal(e.Position.Location.Longitude);
            }
        }



        // Код для выполнения при запуске приложения (например, из меню "Пуск")
        // Этот код не будет выполняться при повторной активации приложения
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            LoadSettings();            
        }

        // Код для выполнения при активации приложения (переводится в основной режим)
        // Этот код не будет выполняться при первом запуске приложения
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            LoadSettings();
        }

        // Код для выполнения при деактивации приложения (отправляется в фоновый режим)
        // Этот код не будет выполняться при закрытии приложения
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            SaveSettings();
        }

        // Код для выполнения при закрытии приложения (например, при нажатии пользователем кнопки "Назад")
        // Этот код не будет выполняться при деактивации приложения
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            SaveSettings();
        }

        // Код для выполнения в случае ошибки навигации
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Ошибка навигации; перейти в отладчик
                System.Diagnostics.Debugger.Break();
            }
        }

        // Код для выполнения на необработанных исключениях
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Произошло необработанное исключение; перейти в отладчик
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Инициализация приложения телефона

        // Избегайте двойной инициализации
        private bool phoneApplicationInitialized = false;

        // Не добавляйте в этот метод дополнительный код
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Создайте кадр, но не задавайте для него значение RootVisual; это позволит
            // экрану-заставке оставаться активным, пока приложение не будет готово для визуализации.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Обработка сбоев навигации
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Убедитесь, что инициализация не выполняется повторно
            phoneApplicationInitialized = true;
        }

        // Не добавляйте в этот метод дополнительный код
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Задайте корневой визуальный элемент для визуализации приложения
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;
            // Удалите этот обработчик, т.к. он больше не нужен
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }       
        #endregion

        public static void SaveSettings()
        {
            if (lastValidatedUser != null)
            {
                IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                settings["SuperSampleUser"] = lastValidatedUser;
                settings.Save();
            }
        }

        public static void LoadSettings()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("SuperSampleUser"))
            {
                lastValidatedUser = (SuperSampleUser)settings["SuperSampleUser"];
            }
        }

        private void btnPhone_Click(object sender, System.Windows.RoutedEventArgs e)
        {        	
            PhoneCallTask callTask = new PhoneCallTask();
            callTask.PhoneNumber = ((Button)sender).Content.ToString();
            callTask.Show();
        }

        private void btnWebSite_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	WebBrowserTask browserTask = new WebBrowserTask();
            Uri qbUri;
            Uri.TryCreate(((Button)sender).Content.ToString(), UriKind.Absolute, out qbUri); 
            browserTask.Uri = qbUri;
            browserTask.Show();
        }
    }
}