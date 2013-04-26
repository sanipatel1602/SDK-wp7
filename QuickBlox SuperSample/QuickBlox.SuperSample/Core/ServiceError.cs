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
using QuickBloxSDK_Silverlight.Geo;
using QuickBloxSDK_Silverlight.Core;
using System.Text;
namespace QuickBlox.SuperSample.Core
{
    public class ServiceError
    {
        public static string GetErrorMessage(Status status, string errorMessage, object result = null)
        {
            switch (status)
            {
                case Status.ValidationError:
                    {
                        StringBuilder message = new StringBuilder();
                        foreach (var text in (ValidateErrorElement[])result)
                        {
                            message.Append(text.ErrorMessage);
                            message.Append("\n");
                        }
                        return message.ToString();
                    }
                default:
                    {
                        return errorMessage;
                    }
            }
        }

    }
}
