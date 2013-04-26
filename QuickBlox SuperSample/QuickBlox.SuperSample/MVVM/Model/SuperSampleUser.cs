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
using QuickBloxSDK_Silverlight.users;
//--------
namespace QuickBlox.SuperSample.Model
{
    /// <summary>
    /// Application User with all aspects
    /// </summary>
    public class SuperSampleUser : INotifyPropertyChanged
    {
        public enum RegistrationStatus
        {
            /// <summary>
            /// Ok
            /// </summary>
            Ok = 0,
            /// <summary>
            /// User Was created (without password for some type of owners)
            /// </summary>
            Creation = 1,
            /// <summary>
            /// Password for current user was accepted
            /// </summary>
            Password = 2,
            /// <summary>
            /// User's mail was verificated
            /// </summary>
            EmailVerification = 3,
            /// <summary>
            /// User was autentificated
            /// </summary>
            Authentification = 4,
            /// <summary>
            /// User was identified
            /// </summary>
            Identification = 5,
            /// <summary>
            /// Geo User was created (with external id = current User Id)
            /// </summary>
            GeoUserCreation = 6,
        }

        private int id;
        /// <summary>
        /// User ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set
            {
                if (id == value) return;
                id = value;
                RaisePropertyChanged("ID");
            }
        }
        /// <summary>
        /// login name
        /// </summary>
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                if (userName == value) return;
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        private int ownerID;
        /// <summary>
        /// Owner's ID
        /// </summary>
        public int OwnerId
        {
            get { return ownerID; }
            set
            {
                if (ownerID == value) return;
                ownerID = value;
                RaisePropertyChanged("OwnerId");
            }
        }
        ///
        private string deviceId;
        /// <summary>
        /// Device indentificator
        /// </summary>
        public string DeviceId
        {
            get { return deviceId; }
            set
            {
                if (deviceId == value) return;
                deviceId = value;
                RaisePropertyChanged("DeviceId");
            }
        }

        private string email;
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email
        {
            get { return email; }
            set
            {
                if (email == value) return;
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        private DateTime? createdDate;
        /// <summary>
        /// Datetime of user creation
        /// </summary>
        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set
            {
                if (createdDate == value) return;
                createdDate = value;
                RaisePropertyChanged("CreatedDate");
            }
        }

        private DateTime? emailVerificationCodeCreatedOn;
        /// <summary>
        /// Date when email was checked
        /// </summary>
        public DateTime? EmailVerificationCodeCreatedOn
        {
            get { return emailVerificationCodeCreatedOn; }
            set
            {
                if (emailVerificationCodeCreatedOn == value) return;
                emailVerificationCodeCreatedOn = value;
                RaisePropertyChanged("EmailVerificationCodeCreatedOn");
            }
        }

        private DateTime? lastRequestDate;
        /// <summary>
        /// Last request to server datetime
        /// </summary>
        public DateTime? LastRequestDate
        {
            get { return lastRequestDate; }
            set
            {
                if (lastRequestDate == value) return;
                lastRequestDate = value;
                RaisePropertyChanged("LastRequestDate");
            }
        }

        private DateTime? passwordResetCodeCreatedDate;
        /// <summary>
        /// When the key to restore acccount was made
        /// </summary>
        public DateTime? PasswordResetCodeCreatedDate
        {
            get { return passwordResetCodeCreatedDate; }
            set
            {
                if (passwordResetCodeCreatedDate == value) return;
                passwordResetCodeCreatedDate = value;
                RaisePropertyChanged("PasswordResetCodeCreatedDate");
            }
        }

        private DateTime? updatedDate;
        /// <summary>
        /// User account update time
        /// </summary>
        public DateTime? UpdatedDate
        {
            get { return updatedDate; }
            set
            {
                if (updatedDate == value) return;
                updatedDate = value;
                RaisePropertyChanged("UpdatedDate");
            }
        }

        private int? externalUserId;
        /// <summary>
        /// External ID of User
        /// </summary>
        public int? ExternalUserId
        {
            get { return externalUserId; }
            set
            {
                if (externalUserId == value) return;
                externalUserId = value;
                RaisePropertyChanged("ExternalUserId");
            }
        }

        private string twitterId;
        /// <summary>
        /// Twitter ID
        /// </summary>
        public string TwitterId
        {
            get { return twitterId; }
            set
            {
                if (twitterId == value) return;
                twitterId = value;
                RaisePropertyChanged("TwitterId");
            }
        }

        private string cryptedPassword;
        /// <summary>
        /// The hash for cryped password
        /// </summary>
        public string CryptedPassword
        {
            get { return cryptedPassword; }
            set
            {
                if (cryptedPassword == value) return;
                cryptedPassword = value;
                RaisePropertyChanged("CryptedPassword");
            }
        }

        private string emailNotValidated;
        /// <summary>
        /// The reason or message of not validating email
        /// </summary>
        public string EmailNotValidated
        {
            get { return emailNotValidated; }
            set
            {
                if (emailNotValidated == value) return;
                emailNotValidated = value;
                RaisePropertyChanged("EmailNotValidated");
            }
        }

        private string emailVerificationCode;
        /// <summary>
        /// Code to send for verification to mail
        /// </summary>
        public string EmailVerificationCode
        {
            get { return emailVerificationCode; }
            set
            {
                if (emailVerificationCode == value) return;
                emailVerificationCode = value;
                RaisePropertyChanged("EmailVerificationCode");
            }
        }

        private string facebookId;
        /// <summary>
        /// Facebook ID
        /// </summary>
        public string FacebookId
        {
            get { return facebookId; }
            set
            {
                if (facebookId == value) return;
                facebookId = value;
                RaisePropertyChanged("FacebookId");
            }
        }

        private string fullName;
        /// <summary>
        /// User's full name
        /// </summary>
        public string FullName
        {
            get { return fullName; }
            set
            {
                if (fullName == value) return;
                fullName = value;
                RaisePropertyChanged("FullName");                
            }
        }

        private string passwordResetCode;
        /// <summary>
        /// The code for reseting password
        /// </summary>
        public string PasswordResetCode
        {
            get { return passwordResetCode; }
            set
            {
                if (passwordResetCode == value) return;
                passwordResetCode = value;
                RaisePropertyChanged("PasswordResetCode");
            }
        }

        private string passwordSalt;
        /// <summary>
        /// Password salt
        /// </summary>
        public string PasswordSalt
        {
            get { return passwordSalt; }
            set
            {
                if (passwordSalt == value) return;
                passwordSalt = value;
                RaisePropertyChanged("PasswordSalt");
            }
        }

        private string persistenceToken;
        /// <summary>
        /// Persistence Token
        /// </summary>
        public string PersistenceToken
        {
            get { return persistenceToken; }
            set
            {
                if (persistenceToken == value) return;
                persistenceToken = value;
                RaisePropertyChanged("PersistenceToken");
            }
        }

        private string phone;
        /// <summary>
        /// Phone number
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone == value || value.Equals("00000000")) return;
                phone = value;
                RaisePropertyChanged("Phone");                
            }
        }

        private string webSite;
        /// <summary>
        /// Website url
        /// </summary>
        public string Website
        {
            get { return webSite; }
            set
            {
                if (webSite == value || value.Equals("http://testwebsiteforquickblox.com")) return;
                webSite = value;
                RaisePropertyChanged("Website");                
            }
        }

        public SuperSampleUser(User user)
        {
            this.OwnerId = user.OwnerId;
            this.ID = user.id;
            this.UserName = user.Username;
            this.CreatedDate = user.CreatedDate;
            this.CryptedPassword = user.CryptedPassword;
            this.DeviceId = user.DeviceId;
            this.Email = user.Email;
            this.EmailNotValidated = user.EmailNotValidated;
            this.EmailVerificationCode = user.EmailVerificationCode;
            this.EmailVerificationCodeCreatedOn = user.EmailVerificationCodeCreatedOn;
            this.ExternalUserId = user.ExternalUserId;
            this.FacebookId = user.FacebookId;
            this.FullName = user.FullName;
            this.LastRequestDate = user.LastRequestDate;
            this.PasswordResetCode = user.PasswordResetCode;
            this.PasswordResetCodeCreatedDate = user.PasswordResetCodeCreatedDate;
            this.PasswordSalt = user.PasswordSalt;
            this.PersistenceToken = user.PersistenceToken;
            this.Phone = user.Phone;
            this.TwitterId = user.TwitterId;
            this.UpdatedDate = user.UpdatedDate;
            this.Website = user.Website;
        }
        public SuperSampleUser() { }

        /// <summary>
        /// User Password
        /// </summary>
        public string Password;

        /// <summary>
        /// Should we save password or not
        /// </summary>
        public bool isPasswordSaved;

        /// <summary>
        /// Status of current Registration Process
        /// </summary>
        public RegistrationStatus status;

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
