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

namespace QuickBloxSDK_Silverlight.owners
{
    public static class OwnerServiceHelper
    {
        public static string TypeTypeToString(TypeType typeType)
        {
            switch (typeType)
            {
                case TypeType.Application:
                    {
                        return "Application";
                    }
                case TypeType.Service:
                    {
                        return "Service";
                    }
            }
            return null;
        }
        public static TypeType StringToTypeType(string text)
        {
            switch (text)
            {
                case "Application":
                    {
                        return TypeType.Application;
                       
                    }
                case "Service":
                    {
                        return TypeType.Service;
                    }
            }

            return TypeType.Service;
            
        }


        public static string EmailEditTypeToString(EmailEditType type)
        {
            switch (type)
            {
                case EmailEditType.email_сode:
                    {
                        return "email_сode";
                    }
                case EmailEditType.none:
                    {
                        return "none";
                    }
            }
            return null;
        }
        public static EmailEditType StringToEmailEditType(string text)
        {
            switch (text)
            {
                case "email_code":
                    {
                        return EmailEditType.email_сode;

                    }
                case "none":
                    {
                        return EmailEditType.none;
                    }
            }

            return EmailEditType.none;

        }


        public static string RegistrationConfirmTypeToString(RegistrationConfirmType type)
        {
            switch (type)
            {
                case RegistrationConfirmType.email_сode:
                    {
                        return "email_сode";
                    }
                case RegistrationConfirmType.none:
                    {
                        return "none";
                    }
            }
            return null;
        }
        public static RegistrationConfirmType StringToRegistrationConfirmType(string text)
        {
            switch (text)
            {
                case "email_code":
                    {
                        return RegistrationConfirmType.email_сode;

                    }
                case "none":
                    {
                        return RegistrationConfirmType.none;
                    }
            }

            return RegistrationConfirmType.none;

        }


        public static string PasswordResetTypeToString(PasswordResetType passwordResetType)
        {
            switch (passwordResetType)
            {
                case PasswordResetType.email:
                    {
                        return "email";
                    }
                case PasswordResetType.email_code:
                    {
                        return "email_code";
                    }
                case PasswordResetType.email_code_email:
                    {
                        return "email_code_email";
                    }
                case PasswordResetType.none:
                    {
                        return "none";
                    }
            }
            return null;
        }
        public static PasswordResetType StringToPasswordResetType(string text)
        {
            switch (text)
            {
                case "email":
                    {
                        return PasswordResetType.email;
                       
                    }
                case "email_code":
                    {
                        return PasswordResetType.email_code;
                    }
                case "email_code_email":
                    {
                        return PasswordResetType.email_code_email;
                    }
                case "none":
                    {
                        return PasswordResetType.none;
                    }
            }

            return PasswordResetType.none;
            
        }


        public static string AuthorizationTypeToString(AuthorizationType type)
        {
            switch (type)
            {
                case AuthorizationType.login:
                    {
                        return "login";
                    }
                case AuthorizationType.login_password:
                    {
                        return "login_password";
                    }
                case AuthorizationType.email_password:
                    {
                        return "email_password";
                    }
                case AuthorizationType.device:
                    {
                        return "device";
                    }
            }
            return null;
        }
        public static AuthorizationType StringToAuthorizationType(string text)
        {
            switch (text)
            {
                case "login":
                    {
                        return AuthorizationType.login;

                    }
                case "login_password":
                    {
                        return AuthorizationType.login_password;
                    }
                case "email_password":
                    {
                        return AuthorizationType.email_password;
                    }
                case "device":
                    {
                        return AuthorizationType.device;
                    }
            }

            return AuthorizationType.login;

        }





    }
}
