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
using System.Xml.Linq;
using QuickBloxSDK_Silverlight.users;

namespace QuickBloxSDK_Silverlight.Geo
{
    /// <summary>
    /// Местоположения.
    /// Если схема по которой будет создаватся объект будет пустой
    /// или не правильной то идентификатор будет -1
    /// </summary>
    public class GeoData
    {

        #region Конструкторы
        public GeoData(int UserId, decimal Latitude, decimal Longitude, string Status)
        {
            if (UserId < 1)
                throw new ArgumentException();

            this.UserId = UserId;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.Status = Status;
        }
        /// <summary>
        /// Создание местоположения по схеме
        /// </summary>
        /// <param name="Scheme">XML схема</param>
        public GeoData(string Scheme)
        {
            if (string.IsNullOrEmpty(Scheme))
                throw new Exception("Content error");

            try
            {
                XElement xmlResult = XElement.Parse(Scheme);
                this.Id = int.Parse(xmlResult.Element("id").Value);
                //----
                this.CreatedDate = DateTime.Parse(xmlResult.Element("created-at").Value);
                this.UpdatedDate = DateTime.Parse(xmlResult.Element("updated-at").Value);
                //----
                this.UserId = int.Parse(xmlResult.Element("user-id").Value);
                this.AppId = int.Parse(xmlResult.Element("app-id").Value);
                try
                {
                    this.user = new User(xmlResult.Element("user").ToString());
                }
                catch { }                
                try
                {
                    this.CreatedAtTimestamp = string.IsNullOrEmpty(xmlResult.Element("created-at-timestamp").Value) ? 0 : int.Parse(xmlResult.Element("created-at-timestamp").Value);
                }
                catch
                {
                    this.CreatedAtTimestamp = 0;
                }
                //------
                this.Status = xmlResult.Element("status").Value;
               //------------

                try
                {
                    this.Longitude = decimal.Parse(xmlResult.Element("longitude").Value);
                }
                catch
                {
                    try{
                        this.Longitude = decimal.Parse(xmlResult.Element("longitude").Value.Replace('.', ','));
                    }
                    catch
                    {
 
                    }
                }
                try
                {
                    this.Latitude = decimal.Parse(xmlResult.Element("latitude").Value);
                }
                catch
                {
                    try
                    {
                        this.Latitude = decimal.Parse(xmlResult.Element("latitude").Value.Replace('.', ','));
                    }
                    catch
                    {

                    }
                }
                
                
            }
            catch(Exception ex)
            {
                throw new Exception("Content error");
            }

 
        }
        #endregion
        #region Поля
        /// <summary>
        /// Идентификатор локации
        /// </summary>
        public int Id
        { get; private set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит это местоположение
        /// </summary>
        public int UserId
        { get; private set; }

        /// <summary>
        /// Дата создания точки местоположения
        /// </summary>
        public DateTime CreatedDate
        { get; private set; }


        public int AppId
        { get; private set; }

        /// <summary>
        /// Дата обновления местоположения
        /// в документации я видел что местоположение не редактируется))) 
        /// так что существование данного поля считаю странным
        /// </summary>
        public DateTime UpdatedDate
        { get; private set; }

        /// <summary>
        /// Широта
        /// </summary>
        public decimal Latitude
        { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public decimal Longitude
        { get; set; }

        /// <summary>
        /// Статус - не могу понять что это))
        /// как я понял всё что угодно... 
        /// </summary>
        public string Status
        { get; set; }

        /// <summary>
        /// Пока не пойму что это))))
        /// может время жизни метки)) пока загадка))
        /// </summary>
        public int CreatedAtTimestamp
        { get; private set; }


        public User user
        { get; private set; }

        #endregion
    }
}
