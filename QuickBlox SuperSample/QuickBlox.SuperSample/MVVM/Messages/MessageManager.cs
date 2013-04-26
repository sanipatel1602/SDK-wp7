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
using System.Linq;
using QuickBloxSDK_Silverlight.Geo;
using System.Collections.Generic;

namespace QuickBlox.SuperSample.Model
{
    public class MessageManager
    {


        public static string CreateMyLocation()
        {
            return new Message(MessageType.Location, 0, 0, string.Empty).ToString();
        }


        public static string CreateChatMessage(string Message)
        {
            return new Message(MessageType.Message, 0, 0, Message).ToString();
        }


        public static string CreatePrivateMessage(int to, int from, string Message)
        {
            return new Message(MessageType.PrivateMessage, from, to, Message).ToString();
        }


        public static string CreatePlace(string Message)
        {
            return new Message(MessageType.VisitedPlaces, 0, 0, Message).ToString();
        }



        public static Message[] GetMyLocations(GeoData[] geoData, int MyGeoId)
        {
            if (geoData == null)
                return null;

            if (geoData.Length < 1)
                return null;

            List<Message> result = new List<Message>();
            foreach (var t in geoData)
            {
                if (t == null || string.IsNullOrEmpty(t.Status))
                    continue;
                try
                {
                    Message mes = new Message(t.Status);
                    if (!mes.IsLoad)
                        continue;

                    if (mes.Type == MessageType.Location && t.UserId == MyGeoId)
                    {
                        mes.Latitude = t.Latitude;
                        mes.Longitude = t.Longitude;
                        mes.Date = t.CreatedDate;
                        mes.From = MyGeoId;
                        result.Add(mes);
                    }
                }
                catch { }

            }

            return result.Where(t => t != null).OrderBy(t => t.Date).ToArray();

        }

        public static GeoData[] GetLastLocations(GeoData[] geoData)
        {
            if (geoData == null)
                return null;
            if (geoData.Length < 1)
                return null;
            
            
            List<GeoData> result = new List<GeoData>();
            var re = geoData.Where(t => t != null).Select(t => t.UserId).Distinct().ToArray();
            foreach (var t1 in geoData.Where(t => t != null).Select(t => t.UserId).Distinct().ToArray())
            {
                var tr = geoData.Where(t3 => t3.UserId == t1).OrderBy(t3 => t3.CreatedDate).ToArray();
                if (tr.Length != 0)
                    result.Add(tr[tr.Length-1]);
                
            }
            

           return result.ToArray();
        }

        public static Message[] GetAllLastLocations(GeoData[] geoData)
        {
            if (geoData == null)
                return null;

            
            List<Message> result = new List<Message>();
            foreach (var t in geoData)
            {
                if (t == null || string.IsNullOrEmpty(t.Status))
                    continue;
                try
                {
                    Message mes = new Message(t.Status);
                    if (!mes.IsLoad)
                        continue;

                    if (mes.Type == MessageType.Location)
                    {
                        mes.Latitude = t.Latitude;
                        mes.Longitude = t.Longitude;
                        mes.Date = t.CreatedDate;
                        mes.From = t.Id;
                        result.Add(mes);
                    }
                }
                catch { }

            }

            List<Message> fullresult = new List<Message>();

            foreach (var t1 in result)
            {

                if (!result.Select(t2 => t2.From).ToArray().Contains(t1.From))
                {
                    try
                    {
                        fullresult.Add(result.Where(t3 => t3.From == t1.From).OrderBy(t3 => t3.Date).ToArray()[0]);
                    }
                    catch
                    {
 
                    }
                }
            }


            return fullresult.Where(t => t != null).OrderBy(t => t.Date).ToArray();

        }




        public static Message[] GetMyChatMessages(GeoData[] geoData, int MyGeoId)
        {
            if (geoData == null)
                return null;

            if (geoData.Length < 1)
                return null;

            List<Message> result = new List<Message>();
            foreach (var t in geoData)
            {
                if (t == null || string.IsNullOrEmpty(t.Status))
                    continue;
                try
                {
                    Message mes = new Message(t.Status);
                    if (!mes.IsLoad)
                        continue;

                    if (mes.Type == MessageType.Message && t.UserId == MyGeoId)
                    {
                        mes.Date = t.CreatedDate;
                        mes.From = MyGeoId;
                        result.Add(mes);
                    }
                }
                catch { }

            }

            return result.Where(t => t != null).OrderBy(t => t.Date).ToArray();

        }


        public static Message[] GetVisitedPlaces(GeoData[] geoData, int GeoId)
        {
            if (geoData == null)
                return null;

            if (geoData.Length < 1)
                return null;

            List<Message> result = new List<Message>();
            foreach (var t in geoData)
            {
                if (t != null && !string.IsNullOrEmpty(t.Status))
                {
                    try
                    {
                        Message mes = new Message(t.Status);

                        if (mes.IsLoad)
                            if (mes.Type == MessageType.VisitedPlaces && t.UserId == GeoId)
                        {
                            mes.Latitude = t.Latitude;
                            mes.Longitude = t.Longitude;
                            mes.Date = t.CreatedDate;
                            mes.From = GeoId;
                            result.Add(mes);
                        }
                    }
                    catch { }
                }

            }



            return result.Where(t=>t!=null).OrderBy(t => t.Date).ToArray();

        }


        public static Message[] GetAllVisitedPlaces(GeoData[] geoData)
        {
            if (geoData == null)
                return null;

            if (geoData.Length < 1)
                return null;

            List<Message> result = new List<Message>();
            foreach (var t in geoData)
            {
                if (t == null || string.IsNullOrEmpty(t.Status))
                    continue;
                try
                {
                    Message mes = new Message(t.Status);
                    if (!mes.IsLoad)
                        continue;

                    if (mes.Type == MessageType.VisitedPlaces)
                    {
                        mes.Latitude = t.Latitude;
                        mes.Longitude = t.Longitude;
                        mes.Date = t.CreatedDate;
                        mes.From = t.UserId;
                        result.Add(mes);
                    }
                }
                catch { }

            }



            return result.Where(t => t != null).OrderBy(t => t.Date).ToArray();

        }


        public static Message[] GetChatMessages(GeoData[] geoData)
        {
            if (geoData == null)
                return null;

            if (geoData.Length < 1)
                return null;

            List<Message> result = new List<Message>();
            foreach (var t in geoData)
            {
                if (t == null || string.IsNullOrEmpty(t.Status))
                    continue;
                try
                {
                    Message mes = new Message(t.Status);
                    if (!mes.IsLoad)
                        continue;

                    if (mes.Type == MessageType.Message)
                    {
                        mes.Date = t.CreatedDate;
                        mes.From = t.UserId;
                        result.Add(mes);
                    }
                }
                catch { }

            }

            return result.Where(t => t != null).OrderBy(t => t.Date).ToArray();

        }



        public static Message[] GetAllMyPrivateMessages(GeoData[] geoData, int MyGeoId)
        {
            if (geoData == null)
                return null;

            if (geoData.Length < 1)
                return null;

            List<Message> result = new List<Message>();
            foreach (var t in geoData)
            {
                if (t == null || string.IsNullOrEmpty(t.Status))
                    continue;
                try
                {
                    Message mes = new Message(t.Status);
                    if (!mes.IsLoad)
                        continue;

                    if (mes.Type == MessageType.PrivateMessage && t.UserId == MyGeoId)
                    {
                        mes.Date = t.CreatedDate;
                        mes.From = t.UserId;
                        result.Add(mes);
                    }
                }
                catch { }

            }

            return result.Where(t => t != null).OrderBy(t => t.Date).ToArray();

        }




        public static Message[] GetAllPrivateMessagesForMe(GeoData[] geoData, int MyGeoId)
        {
            if (geoData == null)
                return null;

            if (geoData.Length < 1)
                return null;

            List<Message> result = new List<Message>();
            foreach (var t in geoData)
            {
                if (t == null || string.IsNullOrEmpty(t.Status))
                    continue;
                try
                {
                    Message mes = new Message(t.Status);
                    if (!mes.IsLoad)
                        continue;

                    if (mes.Type == MessageType.PrivateMessage && mes.To == MyGeoId)
                    {
                        mes.Date = t.CreatedDate;
                        mes.From = t.UserId;
                        result.Add(mes);
                    }
                }
                catch { }

            }

            return result.Where(t => t != null).OrderBy(t => t.Date).ToArray();

        }



        public static Message[] GetPrivateMessagesForUser(GeoData[] geoData, int FromGeoUserId, int ToGeoUserId)
        {
            if (geoData == null)
                return null;

            if (geoData.Length < 1)
                return null;

            List<Message> result = new List<Message>();
            foreach (var t in geoData)
            {
                if (t == null || string.IsNullOrEmpty(t.Status))
                    continue;
                try
                {
                    Message mes = new Message(t.Status);
                    if (!mes.IsLoad)
                        continue;

                    if (mes.Type == MessageType.PrivateMessage && t.UserId == FromGeoUserId && mes.To == ToGeoUserId)
                    {
                        mes.Date = t.CreatedDate;
                        mes.From = t.UserId;
                        result.Add(mes);
                    }
                }
                catch { }

            }

            return result.Where(t => t != null).OrderBy(t => t.Date).ToArray();

        }



        public static Message[] GetPrivateChatTwoUsers(GeoData[] geoData, int FirstUserId, int SecondUserId)
        {
            if (geoData == null)
                return null;

            var arr0 = MessageManager.GetPrivateMessagesForUser(geoData, FirstUserId, SecondUserId);
            var arr1 = MessageManager.GetPrivateMessagesForUser(geoData, SecondUserId, FirstUserId);

            if ((arr0 == null && arr0.Length < 1) && (arr1 != null && arr1.Length > 0))
                return arr1;

            if ((arr1 == null && arr1.Length < 1) && (arr0 != null && arr0.Length > 0))
                return arr0;

            return arr0.Union(arr1).Where(t => t != null).OrderBy(t => t.Date).ToArray();

        }
    }
}
