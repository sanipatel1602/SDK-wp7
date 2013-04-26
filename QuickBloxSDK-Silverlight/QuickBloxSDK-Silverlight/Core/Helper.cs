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

namespace QuickBloxSDK_Silverlight.Core
{
    public class Helper
    {

        public static string AcceptVerbsToString(AcceptVerbs acceptVerbs)
        {
            switch (acceptVerbs)
            {
                case AcceptVerbs.DELETE:
                    {
                        return "DELETE";
                       
                    }
                case AcceptVerbs.GET:
                    {
                        return "GET";

                    }
                case AcceptVerbs.HEAD:
                    {
                        return "HEAD";

                    }
                case AcceptVerbs.OPTIONS:
                    {
                        return "OPTIONS";

                    }
                case AcceptVerbs.POST:
                    {
                        return "POST";

                    }
                case AcceptVerbs.PUT:
                    {
                        return "PUT";
                    }
                case AcceptVerbs.TRACE:
                    {
                        return "TRACE";
                    }
            }
            return null;
        }


        public static AcceptVerbs StringToAcceptVerbs( string acceptVerbs)
        {
            switch (acceptVerbs)
            {
                case "DELETE":
                    {
                        return AcceptVerbs.DELETE;

                    }
                case "GET":
                    {
                        return AcceptVerbs.GET;

                    }
                case "HEAD":
                    {
                        return AcceptVerbs.HEAD;

                    }
                case "OPTIONS":
                    {
                        return AcceptVerbs.OPTIONS;

                    }
                case "POST":
                    {
                        return AcceptVerbs.POST;

                    }
                case "PUT":
                    {
                        return AcceptVerbs.PUT;
                    }
                case "TRACE":
                    {
                        return AcceptVerbs.TRACE ;
                    }
            }
            return AcceptVerbs.GET;
        }


        /// <summary>
        /// Переводит раздел в имя сервера.
        /// Нужно привязать к сеттингу!!!!!
        /// </summary>
        /// <param name="part">Раздел</param>
        /// <returns></returns>
        public static string PartToServerName(Part part)
        {
            switch (part)
            {
                case Part.blobs:
                    {
                        return "";
                    }
                case Part.chat:
                    {
                        return "";
                    }
                case Part.geopos:
                    {
                        return "location.quickblox.com";
                    }
                case Part.messaging:
                    {
                        return "messaging.aws02.mob1serv.com";
                    }
                case Part.ratings:
                    {
                        return "ratings.aws02.mob1serv.com";
                    }
                case Part.users:
                    {
                        return "users.quickblox.com";
                    }
            }
            return null;
        }

        /// <summary>
        /// Находит в заголовках статус и переводит его в нормальный вид
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static Status HeaderToStatus(Header[] headers)
        {
            try
            {
                string statusCode = null;
                foreach (var t in headers)
                    if (t.Name == "Status")
                        statusCode = t.Value;

                return Helper.StringToStatus(statusCode);

            }
            catch
            {
                return Status.none;
            }
           
        }

        /// <summary>
        /// Получение статуса по строке
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static Status StringToStatus(string statusCode)
        {
            if (string.IsNullOrEmpty(statusCode))
                return Status.none;

            if (statusCode.IndexOf("20") != -1 )
                return Status.OK;

            if (statusCode.IndexOf("404") != -1 || statusCode.IndexOf("NotFound") != -1)
                return Status.NotFoundError;

            if (statusCode.IndexOf("401") != -1 || statusCode.IndexOf("Unauthorized") != -1)
                return Status.Unauthorized;

            if (statusCode.IndexOf("401") != -1)
                return Status.AuthenticationError;

            if (statusCode.IndexOf("405") != -1 || statusCode.IndexOf("MethodNotAllowed") != -1)
                return Status.MethodNotAllowed;

            if (statusCode.IndexOf("406") != -1 || statusCode.IndexOf("NotAcceptable") != -1)
                return Status.NotAcceptable;
            
            if (statusCode.IndexOf("403") != -1 )
                return Status.AccessDenied;

            if (statusCode.IndexOf("422") != -1)
                return Status.ValidationError;




            return Status.none;
        }



        

    }
}
