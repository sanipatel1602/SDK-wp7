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

namespace QuickBloxSDK_Silverlight.users
{
    /// <summary>
    /// Команда
    /// </summary>
    public enum UserServiceCommand
    {
        GetUser,
        GetUserByExternalId,
        GetUserByEmail,
        AddUser,
        DeleteUser,
        EditUser,
        SetNewPassword,
        EmailVerification,
        Authenticate,
        Identify,
        Logout,
        Resetmypasswordbyemail,
        Resetpassword,
        GetUsersByOwner

    }
}
