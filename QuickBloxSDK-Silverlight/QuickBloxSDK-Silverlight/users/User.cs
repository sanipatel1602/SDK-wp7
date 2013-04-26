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

namespace QuickBloxSDK_Silverlight.users
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        #region поля
        /// <summary>
        /// Идентификатор пользователя в базе данных.
        /// </summary>
        public int id
        { get; private set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username
        { get; private set; }

        public int OwnerId
        { get; private set; }

        /// <summary>
        /// Идентификатор устройства
        /// </summary>
        public string DeviceId
        { get; private set; }


        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email
        { get; private set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime? CreatedDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Када мыло подтвердилось
        /// </summary>
        public DateTime? EmailVerificationCodeCreatedOn
        {
            get;
            private set;
        }

        /// <summary>
        /// Последнее обращение к серверу
        /// </summary>
        public DateTime? LastRequestDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Дата создения ключа востановления пароля
        /// </summary>
        public DateTime? PasswordResetCodeCreatedDate
        {
            get;
            private set;
        }

        public DateTime? UpdatedDate
        {
            get;
            private set;
        }

        

        public int? ExternalUserId
        { get; set; }

        

        public string TwitterId
        { get; private set; }



        public string CryptedPassword
        { get; private set; }

        


        public string EmailNotValidated
        { get; private set; }


        public string EmailVerificationCode
        { get; private set; }


        public string FacebookId
        { get; private set; }


        public string FullName
        { get;  set; }


        public string PasswordResetCode
        { get; private set; }

        public string PasswordSalt
        { get; private set; }

        public string PersistenceToken
        { get; private set; }

        public string Phone
        { get; set; }

        public string Website
        { get; set; }

        #endregion
        #region конструкторы
        public User(string Username, int Owner, string Email, string Device)
        {
            if (string.IsNullOrEmpty(Username) || Owner < 0)
                throw new ArgumentException("Argument error");
            this.OwnerId = Owner;
            this.Email = Email;
            this.Username = Username;
            this.DeviceId = Device;
        }

        public User(string Username, int Owner, string Email, string Device, string FacebookId)
        {
            if (string.IsNullOrEmpty(Username) || Owner < 0 || string.IsNullOrEmpty(FacebookId) || string.IsNullOrEmpty(Email))
                throw new ArgumentException("Argument error");
            this.OwnerId = Owner;
            this.Email = Email;
            this.Username = Username;
            this.DeviceId = Device;
            this.FacebookId = FacebookId;
        }


        public User(string Username, int Owner, string Email, string Device, string FacebookId, string TwitterId)
        {
            if (string.IsNullOrEmpty(Username) || Owner < 0 || string.IsNullOrEmpty(FacebookId) || string.IsNullOrEmpty(Email))
                throw new ArgumentException("Argument error");
            this.OwnerId = Owner;
            this.Email = Email;
            this.Username = Username;
            this.DeviceId = Device;
            this.FacebookId = FacebookId;
            this.TwitterId = TwitterId;
        }

        public User(int id, string Username, int Owner, string Email, string Device)
        {
            if (string.IsNullOrEmpty(Username) || id < 0 || Owner < 0)
                throw new ArgumentException("Argument error");

            this.id = id;
            this.OwnerId = Owner;
            this.Email = Email;
            this.Username = Username;
            this.DeviceId = Device;
        }


        public User(string Xml)
        {
            this.Parse(Xml);

        }
        #endregion
        #region Методы 
         
        /// <summary>
        /// Преабразует объект в строку
        /// </summary>
        /// <returns>Имя пользователя</returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Username) ? string.Empty : Username;
        }

        /// <summary>
        /// Распарсивает прищедшего от сервера пользователя
        /// </summary>
        /// <param name="xml"></param>
        private void Parse(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new Exception("Content error");
            try
            {
                XElement xmlResult = XElement.Parse(xml);
                this.id = int.Parse(xmlResult.Element("id").Value);
                this.Username = xmlResult.Element("login").Value;
                //----
                this.CreatedDate = DateTime.Parse(xmlResult.Element("created-at").Value);
                this.EmailVerificationCodeCreatedOn = (string.IsNullOrEmpty(xmlResult.Element("email-verification-code-created-on").Value) ? (DateTime?)null : DateTime.Parse(xmlResult.Element("email-verification-code-created-on").Value));
                this.UpdatedDate = (string.IsNullOrEmpty(xmlResult.Element("updated-at").Value) ? (DateTime?)null : DateTime.Parse(xmlResult.Element("updated-at").Value));
                this.LastRequestDate = (string.IsNullOrEmpty(xmlResult.Element("last-request-at").Value) ? (DateTime?)null : DateTime.Parse(xmlResult.Element("last-request-at").Value));
                this.PasswordResetCodeCreatedDate = (string.IsNullOrEmpty(xmlResult.Element("password-reset-code-created-on").Value) ? (DateTime?)null : DateTime.Parse(xmlResult.Element("password-reset-code-created-on").Value));
                //----
                this.DeviceId = xmlResult.Element("device-id").Value; // string.IsNullOrEmpty(xmlResult.Element("device-id").Value) ? (int?)null : int.Parse(xmlResult.Element("device-id").Value);
                this.ExternalUserId = string.IsNullOrEmpty(xmlResult.Element("external-user-id").Value) ? (int?)null : int.Parse(xmlResult.Element("external-user-id").Value);
                this.OwnerId = string.IsNullOrEmpty(xmlResult.Element("owner-id").Value) ? 0 : int.Parse(xmlResult.Element("owner-id").Value);
                
               //------
                this.CryptedPassword = xmlResult.Element("crypted-password").Value;
                this.Email = xmlResult.Element("email").Value;
                this.EmailNotValidated = xmlResult.Element("email-not-validated").Value;
                this.EmailVerificationCode = xmlResult.Element("email-verification-code").Value;
                this.FacebookId = xmlResult.Element("facebook-id").Value;
                this.TwitterId = xmlResult.Element("twitter-id").Value;
                this.FullName = xmlResult.Element("full-name").Value;
                this.PasswordResetCode = xmlResult.Element("password-reset-code").Value;
                this.PasswordSalt = xmlResult.Element("password-salt").Value;
                this.PersistenceToken = xmlResult.Element("persistence-token").Value;
                this.Phone = xmlResult.Element("phone").Value;
                this.Website = xmlResult.Element("website").Value;
                

            }
            catch
            {
                throw new Exception("Content error");
            }
        }
        #endregion
      
    }
}
