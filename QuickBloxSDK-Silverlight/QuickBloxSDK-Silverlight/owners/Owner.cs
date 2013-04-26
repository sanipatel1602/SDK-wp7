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
using System.Linq;

namespace QuickBloxSDK_Silverlight.owners
{
    public class Owner
    {
         #region поля
        /// <summary>
        /// Идентификатор пользователя в базе данных.
        /// </summary>
        public int id
        { get; private set; }

        /// <summary>
        /// Редактируемое: Нет
       /// Обязательное: Да, если type_type = Application
        /// </summary>
        public int? ApplicationId
        { get;  set; }

        /// <summary>
        /// Обязательное: Да
        /// </summary>
        public AuthorizationType AuthorizationType
        { get; set; }

        /// <summary>
        /// Обязательное: Да
        /// </summary>
        public string EmailConfirmMask
        { get; set; }

        /// <summary>
        /// Обязательное: Да
        /// </summary>
        public EmailEditType EmailEditType
        { get; set; }


        public int? FbAppId
        { get;  set; }


        public string FbAppSecret
        { get; set; }

        /// <summary>
        /// Обязательное: Да
        /// </summary>
        public string PasswordResetMask
        { get; set; }
        /// <summary>
        /// Обязательное: Да
        /// </summary>
        public PasswordResetType PasswordResetType
        { get; set; }
        /// <summary>
        /// Обязательное: Да
        /// </summary>
        public RegistrationConfirmType RegistrationConfirmType
        { get; set; }

        /// <summary>
        /// Редактируемое: Нет
        /// Обязательное: Да, если type_type = Service
        /// </summary>
        public int ServiceId
        { get; set; }


        public int? TwitterAppId
        { get;  set; }


        public string TwitterAppSecret
        { get; set; }

        /// <summary>
        /// Обязательное
        /// Не редактируемое
        /// </summary>
        public TypeType TypeType
        { get; set; }

        public string CustomFields
        { get; set; }

        /// <summary>
        /// Обязательное: Да
        /// </summary>
        public string EditableFields
        { get; set; }

        /// <summary>
        /// Обязательное: Да
        /// Редактируемое: Нет
        /// </summary>
        public string RequiredFields
        { get; set; }


        public DateTime? CreatedDate
        {
            get;
            private set;
        }


        public string[] CustomFieldsArr
        { get; set; }

        public string[] EditableFieldsArr
        { get; set; }


        public string[] RequiredFieldsArr
        { get; set; }
        
        public DateTime? UpdatedDate
        {
            get;
            private set;
        }

       
        

        #endregion
        #region конструкторы
        public Owner()
        {
            
        }


        public Owner(string Xml)
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
            return id.ToString();
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
                //----
                this.CreatedDate = DateTime.Parse(xmlResult.Element("created-at").Value);
                this.UpdatedDate = (string.IsNullOrEmpty(xmlResult.Element("updated-at").Value) ? (DateTime?)null : DateTime.Parse(xmlResult.Element("updated-at").Value));
                //----

                this.ApplicationId = string.IsNullOrEmpty(xmlResult.Element("application-id").Value) ? (int?)null : int.Parse(xmlResult.Element("application-id").Value);
                this.FbAppId = string.IsNullOrEmpty(xmlResult.Element("fb-app-id").Value) ? 0 : int.Parse(xmlResult.Element("fb-app-id").Value);
                this.TwitterAppId = string.IsNullOrEmpty(xmlResult.Element("twitter-app-id").Value) ? 0 : int.Parse(xmlResult.Element("twitter-app-id").Value);
                this.ServiceId = string.IsNullOrEmpty(xmlResult.Element("service-id").Value) ? 0 : int.Parse(xmlResult.Element("service-id").Value);
                
               //------
                this.AuthorizationType = OwnerServiceHelper.StringToAuthorizationType(xmlResult.Element("authorization-type").Value);
                this.EmailConfirmMask = xmlResult.Element("email-confirm-mask").Value;
                this.FbAppSecret = xmlResult.Element("fb-app-secret").Value;
                this.PasswordResetMask = xmlResult.Element("password-reset-mask").Value;
                this.PasswordResetType = OwnerServiceHelper.StringToPasswordResetType(xmlResult.Element("password-reset-type").Value);
                this.RegistrationConfirmType = OwnerServiceHelper.StringToRegistrationConfirmType(xmlResult.Element("registration-confirm-type").Value);
                this.TwitterAppSecret = xmlResult.Element("twitter-app-secret").Value;
                this.TypeType = OwnerServiceHelper.StringToTypeType(xmlResult.Element("type-type").Value);
                this.CustomFields = xmlResult.Element("custom-fields").Value;
                this.EditableFields = xmlResult.Element("editable-fields").Value;
                this.RequiredFields = xmlResult.Element("required-fields").Value;
                this.EmailEditType = OwnerServiceHelper.StringToEmailEditType(xmlResult.Element("email-edit-type").Value);

                string temp = string.Empty;
                try
                {

                    if (!string.IsNullOrEmpty(this.CustomFields) && !this.CustomFields.Contains("[]"))
                    {
                        temp = this.CustomFields;
                        temp = temp.Trim().Replace("\n",string.Empty).Replace(" ",string.Empty);
                        while(true)
                        {
                            if (!temp.Contains("--"))
                                break;
                            else
                                temp = temp.Replace("--", "-");
                        }

                        this.CustomFieldsArr = temp.Split('-').Where(t => !string.IsNullOrEmpty(t)).ToArray();
 
                    }

                    if (!string.IsNullOrEmpty(this.EditableFields) && !this.EditableFields.Contains("[]"))
                    {
                        temp = this.EditableFields;
                        temp = temp.Trim().Replace("\n", string.Empty).Replace(" ", string.Empty);
                        while (true)
                        {
                            if (!temp.Contains("--"))
                                break;
                            else
                              temp = temp.Replace("--", "-");
                        }

                        this.EditableFieldsArr = temp.Split('-').Where(t => !string.IsNullOrEmpty(t)).ToArray();
                    }


                    if (!string.IsNullOrEmpty(this.RequiredFields) && !this.RequiredFields.Contains("[]"))
                    {
                        temp = this.RequiredFields;
                        temp = temp.Trim().Replace("\n", string.Empty).Replace(" ", string.Empty);
                        while (true)
                        {
                            if (!temp.Contains("--"))
                                break;
                            else
                                temp = temp.Replace("--", "-");
                        }

                        this.RequiredFieldsArr = temp.Split('-').Where(t => !string.IsNullOrEmpty(t)).ToArray();
                    }

                }
                catch
                {

                }
                

            }
            catch
            {
                throw new Exception("Content error");
            }
        }
        #endregion
    }
}
