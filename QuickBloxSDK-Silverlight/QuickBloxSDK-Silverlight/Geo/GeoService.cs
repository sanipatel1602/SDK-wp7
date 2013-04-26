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
using System.Collections.Generic;
using QuickBloxSDK_Silverlight.Core;
using System.Xml.Linq;

namespace QuickBloxSDK_Silverlight.Geo
{

    /// <summary>
    /// Гео служба))
    /// </summary>
    public class GeoService
    {
        /// <summary>
        /// Обработчик событий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="result"></param>
        /// <param name="StatusCode"></param>
        public delegate void GeoServiceHeandler(GeoServiceEventArgs Args);

        /// <summary>
        /// Событие
        /// </summary>
        public event GeoServiceHeandler GeoServiceEvent;


         /// <summary>
        /// Контекст подключения
        /// </summary>
        private ConnectionContext Сontext;


        public GeoService(ConnectionContext context)
        {
            Сontext = context;
            this.Сontext.RequestResult += new ConnectionContext.Main((Result result) =>
            {

                if (result.ServerName != Helper.PartToServerName(Part.geopos))
                    return;

                switch (result.Verbs)
                {
                    case AcceptVerbs.GET:
                        {
                            

                            
                            if (result.ControllerName.Contains("/geodata/find") && result.URI.Contains("app.id") && result.URI.Contains("user.id"))
                            {
                                this.GetGeoLocationsForUser_Response(result);
                                return;
                            }
                            if (result.ControllerName.Contains("/geodata/find") && result.URI.Contains("app.id") && !result.URI.Contains("user.id"))
                            {
                                this.GetGeoLocationsForApp_Response(result);
                                return;
                            }
                            break;
                        }
                    case AcceptVerbs.DELETE:
                        {
                            
                            break;
                        }
                    case AcceptVerbs.PUT:
                        {
                            
                            break;
                        }
                    case AcceptVerbs.POST:
                        {
                            
                            if (result.ControllerName.Contains("/geodata"))
                            {
                                this.AddGeoLocation_Response(result);
                                return;
                            }
                            break;
                        }
                }


            });
        }



        private void AddGeoLocation_Response(Result result)
        {
            if (GeoServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    try
                    {
                        this.GeoServiceEvent(new GeoServiceEventArgs
                        {
                            result = new GeoData(result.Content),
                            t = typeof(GeoData),
                            status = result.ResultStatus,
                            currentCommand = GeoServiceCommand.AddGeoLocation,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex)
                    {
                        this.GeoServiceEvent(new GeoServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = GeoServiceCommand.AddGeoLocation,
                            errorMessage = ex.Message
                        });
                    }
                }
                // нет контента из за ошибок
                else if (result.ResultStatus == Status.StreamError
                    || result.ResultStatus == Status.TimeoutError
                    || result.ResultStatus == Status.UnknownError
                    || result.ResultStatus == Status.NotFoundError
                    || result.ResultStatus == Status.AccessDenied
                    || result.ResultStatus == Status.ConnectionError
                    || result.ResultStatus == Status.NotAcceptable
                    || result.ResultStatus == Status.AuthenticationError)
                    this.GeoServiceEvent(new GeoServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = GeoServiceCommand.AddGeoLocation,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.GeoServiceEvent(new GeoServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = GeoServiceCommand.AddGeoLocation,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.GeoServiceEvent(new GeoServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = GeoServiceCommand.AddGeoLocation,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }


        public void AddGeoLocation(GeoData data)
        {
            if (data == null)
                return;


            this.Сontext.CurrentPart = Part.geopos;
            this.Сontext.Add("geo_data[user_id]", data.UserId.ToString());
            this.Сontext.Add("geo_data[app_id]", this.Сontext.ApplicationId.ToString());

            if (!string.IsNullOrEmpty(data.Status))
                this.Сontext.Add("geo_data[status]", data.Status);


            if (data.Latitude.ToString().Contains(","))
                this.Сontext.Add("geo_data[latitude]", data.Latitude.ToString().Replace(',', '.'));
            else
                this.Сontext.Add("geo_data[latitude]", data.Latitude.ToString());


            if (data.Longitude.ToString().Contains(","))
                this.Сontext.Add("geo_data[longitude]", data.Longitude.ToString().Replace(',', '.'));
            else
                this.Сontext.Add("geo_data[longitude]", data.Longitude.ToString());


            this.Сontext.SendAsyncRequest("geodata", AcceptVerbs.POST);
        }



        private void GetGeoLocation_Response(Result result)
        {
            if (GeoServiceEvent != null) 
            {
                if (result.ResultStatus == Status.OK) 
                {
                    try 
                    {
                        List<GeoData> users = new List<GeoData>();
                        XElement xml = XElement.Parse((string)result.Content);
                        foreach (var t in xml.Descendants("geo-data"))
                            users.Add(new GeoData(t.ToString()));

                        this.GeoServiceEvent(new GeoServiceEventArgs
                        {
                            result = users.Count > 0 ? users[0] : null,
                            t = typeof(GeoData),
                            status = result.ResultStatus,
                            currentCommand = GeoServiceCommand.GetGeoLocation,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.GeoServiceEvent(new GeoServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = GeoServiceCommand.GetGeoLocation,
                            errorMessage = ex.Message
                        });
                    }
                }
                // нет контента из за ошибок
                else if (result.ResultStatus == Status.StreamError
                    || result.ResultStatus == Status.TimeoutError
                    || result.ResultStatus == Status.UnknownError
                    || result.ResultStatus == Status.NotFoundError
                    || result.ResultStatus == Status.AccessDenied
                    || result.ResultStatus == Status.ConnectionError
                    || result.ResultStatus == Status.AuthenticationError)
                    this.GeoServiceEvent(new GeoServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = GeoServiceCommand.GetGeoLocation,
                        errorMessage = result.ErrorMessage
                    });

            }
        }

       
        public void GetGeoLocation(int id)
        {
            if (id < 0)
                return;

            this.Сontext.CurrentPart = Part.geopos;
            this.Сontext.Add("id", id.ToString());
            this.Сontext.SendAsyncRequest("geodata/find", AcceptVerbs.GET); // параметры запроса
           
        }

        




        private void GetGeoLocationsForUser_Response(Result result)
        {
            if (GeoServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    try
                    {


                        List<GeoData> users = new List<GeoData>();
                        XElement xml = XElement.Parse((string)result.Content);
                        foreach (var t in xml.Descendants("geo-datum"))
                            users.Add(new GeoData(t.ToString()));


                        int pageCount = 0;

                        try
                        {
                            pageCount = int.Parse(xml.Element("pages_count").Value);
                        }
                        catch
                        {
 
                        }
                       
                        this.GeoServiceEvent(new GeoServiceEventArgs
                        {
                            result = new GeoPage(pageCount,users.ToArray()),
                            t = typeof(GeoPage),
                            status = result.ResultStatus,
                            currentCommand = GeoServiceCommand.GetGeoLocationsForUser,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.GeoServiceEvent(new GeoServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = GeoServiceCommand.GetGeoLocationsForUser,
                            errorMessage = ex.Message
                        });
                    }
                }
                // нет контента из за ошибок
                else if (result.ResultStatus == Status.StreamError
                    || result.ResultStatus == Status.TimeoutError
                    || result.ResultStatus == Status.UnknownError
                    || result.ResultStatus == Status.NotFoundError
                    || result.ResultStatus == Status.AccessDenied
                    || result.ResultStatus == Status.ConnectionError
                    || result.ResultStatus == Status.NotAcceptable
                    || result.ResultStatus == Status.AuthenticationError)
                    this.GeoServiceEvent(new GeoServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = GeoServiceCommand.GetGeoLocationsForUser,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.GeoServiceEvent(new GeoServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = GeoServiceCommand.GetGeoLocationsForUser,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.GeoServiceEvent(new GeoServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = GeoServiceCommand.GetGeoLocationsForUser,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }

        public void GetGeoLocationsForUser(int UserId, int Page, SelectionSettings Settings)
        {
            SelectionSettings sett = Settings ?? new SelectionSettings();
            this.Сontext.CurrentPart = Part.geopos;
            this.Сontext.Add("page", Page.ToString());
            this.Сontext.Add("page_size", (sett.PageSize < 10 ? 100 : sett.PageSize).ToString());
            this.Сontext.Add("app.id", this.Сontext.ApplicationId.ToString());

            if(sett.sortType == SortType.SortAsc)
                this.Сontext.Add("sort_asc", "1");
            else
                this.Сontext.Add("sort_by", "1");


            switch (sett.sortField)
            {
                case SortField.Date:
                    {
                        this.Сontext.Add("created_at", "1");
                        break;
                    }
                case SortField.Distance:
                    {
                        this.Сontext.Add("distance", "1");
                        break;
                    }
                case SortField.Latitude:
                    {
                        this.Сontext.Add("latitude", "1");
                        break;
                    }
                case SortField.Longitude:
                    {
                        this.Сontext.Add("longitude", "1");
                        break;
                    }
            }


           if (sett.IsLastOnly)
                this.Сontext.Add("last_only", "1");

            this.Сontext.Add("user.id", UserId.ToString());
            this.Сontext.SendAsyncRequest("geodata/find.xml", AcceptVerbs.GET); // параметры запроса
           
        }




       

        private void GetGeoLocationsForApp_Response(Result result)
        {
            if (GeoServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    try
                    {


                        List<GeoData> users = new List<GeoData>();
                        XElement xml = XElement.Parse((string)result.Content);
                        foreach (var t in xml.Descendants("geo-data"))
                            users.Add(new GeoData(t.ToString()));


                        int pageCount = 0;

                        try
                        {
                            pageCount = int.Parse(xml.Element("pages_count").Value);
                        }
                        catch
                        {

                        }

                        this.GeoServiceEvent(new GeoServiceEventArgs
                        {
                            result = new GeoPage(pageCount, users.ToArray()),
                            t = typeof(GeoPage),
                            status = result.ResultStatus,
                            currentCommand = GeoServiceCommand.GetGeoLocationsForApp,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.GeoServiceEvent(new GeoServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = GeoServiceCommand.GetGeoLocationsForApp,
                            errorMessage = ex.Message
                        });
                    }
                }
                // нет контента из за ошибок
                else if (result.ResultStatus == Status.StreamError
                    || result.ResultStatus == Status.TimeoutError
                    || result.ResultStatus == Status.UnknownError
                    || result.ResultStatus == Status.NotFoundError
                    || result.ResultStatus == Status.AccessDenied
                    || result.ResultStatus == Status.ConnectionError
                    || result.ResultStatus == Status.NotAcceptable
                    || result.ResultStatus == Status.AuthenticationError)
                    this.GeoServiceEvent(new GeoServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = GeoServiceCommand.GetGeoLocationsForApp,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.GeoServiceEvent(new GeoServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = GeoServiceCommand.GetGeoLocationsForApp,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.GeoServiceEvent(new GeoServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = GeoServiceCommand.GetGeoLocationsForApp,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }

        public void GetGeoLocationsForApp(int Page, SelectionSettings Settings)
        {
            SelectionSettings sett = Settings ?? new SelectionSettings();
            this.Сontext.CurrentPart = Part.geopos;
            this.Сontext.Add("page", Page.ToString());
            this.Сontext.Add("page_size", (sett.PageSize < 10 ? 100 : sett.PageSize).ToString());

            if (sett.sortType == SortType.SortAsc)
                this.Сontext.Add("sort_asc", "1");
            else
                this.Сontext.Add("sort_by", "1");

            this.Сontext.Add("app.id", this.Сontext.ApplicationId.ToString());
            this.Сontext.SendAsyncRequest("geodata/find.xml", AcceptVerbs.GET); // параметры запроса

        }



        
        
    }
}
