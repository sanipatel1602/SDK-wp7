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
using QuickBloxSDK_Silverlight.users;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

namespace QuickBloxSDK_Silverlight.Core
{
    /// <summary>
    ///Контекст подключения
    /// </summary>
    public class ConnectionContext
    {

        #region Конструкторы
        /// <summary>
        /// Конструктор
        /// </summary>
        public ConnectionContext(int ApplicationId)
        {
            this.ApplicationId = ApplicationId;
            this.Form = new List<FormElement>();
            this.Cookie = new CookieContainer();
        }
        #endregion


        #region Поля


        public bool IsOnlyOneEventHandler
        {
            get;
            set;
        }


        /// <summary>
        /// Адрес текущего сервера к которому происходит подключение.
        /// </summary>
        public string CurrentServerAdr
        {  get; private set; }

        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        public int ApplicationId
        { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public User Username
        {
            get;
            set;
        }

        private Part currentPart;

        /// <summary>
        /// Раздел апи с которым в данный момент работает программа
        /// </summary>
        public Part CurrentPart
        {
            get
            {
                return currentPart;
            }
            set
            {
                this.currentPart = value;
                this.CurrentServerAdr = Helper.PartToServerName(value);
            }
        }

        /// <summary>
        /// Строка идентификации клиента
        /// пример: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string AgentName
        { get; set; }

        /// <summary>
        /// Контент, который может принимать клиент
        /// пример: Accept:text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept
        { get; set; }

        
        /// <summary>
        /// Заголовки
        /// </summary>
        public Header[] Headers
        {
            get;
            private set;
        }

        private CookieContainer Cookie; 

        /// <summary>
        /// Делегат 
        /// </summary>
        /// <param name="result"></param>
        public delegate void Main(Result result);

        private Main _RequestResult;

        public event Main RequestResult
        {
            add
            {
                this._RequestResult += value;
            }
            remove
            {
                this._RequestResult -= value;
            }
        }

        #endregion

        #region Form
        /// <summary>
        /// Форма
        /// </summary>
        private List<FormElement> Form;

        /// <summary>
        /// Добавить элемент формы
        /// </summary>
        public void Add(string key, string value)
        {
            if (this.Form == null)
                this.Form = new List<FormElement>();

            if (string.IsNullOrEmpty(key))
                return;

            this.Form.Add(new FormElement { key =key, value = value });
        }

        /// <summary>
        /// Очистить форму
        /// </summary>
        public void Clear()
        {
            if (this.Form == null)
                this.Form = new List<FormElement>();

            this.Form.Clear();
        }

        /// <summary>
        /// Удалить элемент формы
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            if (this.Form == null)
            {
                this.Form = new List<FormElement>();
                return;
            }

            if (string.IsNullOrEmpty(key))
                return;

            try
            {
                FormElement element = null;
                foreach (var t in this.Form)
                    if (t.key == key)
                        element = t;

                if (element != null)
                    this.Form.Remove(element);
            }
            catch
            { }
        }
        /// <summary>
        /// Рендерит форму
        /// </summary>
        /// <param name="IsPost"></param>
        /// <returns></returns>
        private string RenderForm(bool IsPost)
        {
            if (this.Form == null)
                return string.Empty;

            if (this.Form.Count < 1)
                return string.Empty;

            StringBuilder result = new StringBuilder();
            result.Append(IsPost?string.Empty:"?");
            foreach (var t in this.Form)
            {
                result.Append(t.ToString());
                result.Append("&");
            }

            string formresult = result.ToString();
            if (formresult[result.ToString().Length - 1] == '&')
                formresult = formresult.Remove(formresult.Length - 1);

            return formresult;
        }
        #endregion
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        #region Запрос
        /// <summary>
        /// Выполнить асинхронный запрос
        /// </summary>
        public void SendAsyncRequest(string ControllerName, AcceptVerbs method)
        {
            
            StringBuilder urlMaker = new StringBuilder();
            urlMaker.Append("http://");
            urlMaker.Append(this.CurrentServerAdr);
            urlMaker.Append("/");
            urlMaker.Append(ControllerName);

            HttpWebRequest request;

            if (method == AcceptVerbs.GET || method == AcceptVerbs.DELETE)
                request = WebRequest.CreateHttp(new Uri(urlMaker.ToString() + this.RenderForm(false)));
            else 
                request = WebRequest.CreateHttp(new Uri(urlMaker.ToString()));


            request.Method = Helper.AcceptVerbsToString(method);
            request.AllowAutoRedirect = false;

            if (request == null)
                return;

            try
            {
                request.Headers["Referer"] = "-";
                request.Headers["Accept-Language"] = "en;q=0.8";
                request.Headers["Cache-Control"] = "no-cache";
                request.Headers["Accept-Encoding"] = "windows-1251,utf-8;q=0.7,*;q=0.7";
                request.Accept = string.IsNullOrEmpty(this.Accept) ? "*/*;q=0.1" : this.Accept;
                request.UserAgent = string.IsNullOrEmpty(this.AgentName) ? "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)" : string.Empty;
            }
            catch
            { }
                request.CookieContainer = this.Cookie;
            switch (method)
            {
                case AcceptVerbs.GET:
                    {
                        IAsyncResult asyncResult = (IAsyncResult)request.BeginGetResponse(new AsyncCallback(ResponseCallback), request);
                        //allDone.WaitOne(3000);
                        this.Clear();
                        break;
                    }
                case AcceptVerbs.POST:
                    {
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.BeginGetRequestStream(new AsyncCallback(CreateRequestStreamCallback), request);
                       
                        break;
                    }
                case AcceptVerbs.PUT:
                    {
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.BeginGetRequestStream(new AsyncCallback(CreateRequestStreamCallback), request);
                       
                        break;
                    }
                case AcceptVerbs.DELETE:
                    {
                        request.BeginGetResponse(new AsyncCallback(ResponseCallback), request);
                        //allDone.WaitOne();
                        this.Clear();
                        break;
                    }
                case AcceptVerbs.HEAD:
                    {
                        break;
                    }
                case AcceptVerbs.OPTIONS:
                    {
                        break;
                    }
                case AcceptVerbs.TRACE:
                    {
                        break;
                    }
            }
        }


        /// <summary>
        /// Добавяем в запрос поля формы и файлы
        /// </summary>
        /// <param name="asynchronousResult"></param>
       private void CreateRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            using (Stream postStream = webRequest.EndGetRequestStream(asynchronousResult))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(this.RenderForm(true));
                postStream.Write(byteArray, 0, byteArray.Length);
            }
            this.Clear();
            webRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), webRequest);
           // allDone.WaitOne();
            
        }


        /// <summary>
        /// Ответ от сервера
        /// </summary>
        /// <param name="asynchronousResult"></param>
       private void ResponseCallback(IAsyncResult result)
        {
            
            HttpWebRequest requestResult = result.AsyncState as HttpWebRequest;
            AcceptVerbs v = Helper.StringToAcceptVerbs(requestResult.Method);
            string ServerName = requestResult.RequestUri.Host;
            string ControllerName = requestResult.RequestUri.AbsolutePath;
            string URI = requestResult.RequestUri.OriginalString;
            
            try
            {
                if (requestResult == null) return;
                using (WebResponse response = requestResult.EndGetResponse(result))
                {
                    
                    #region Обработка ответа
                    if (response == null)
                        return;
                    //Переписываем хеадер
                    List<Header> headersList = new List<Header>();
                    for (int i = 0; i < response.Headers.Count; ++i)
                        headersList.Add(new Header { Name = response.Headers.AllKeys[i], Value = response.Headers[response.Headers.AllKeys[i]] });

                    try
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {

                            if (this._RequestResult != null)
                                this.CallResult(null, reader, string.Empty, Helper.HeaderToStatus(headersList.ToArray()), v, ServerName, ControllerName, URI);
                        }
                    }
                    catch (Exception ex)
                    {
                        //this.CallResult(null, null, ex.Message, Status.StreamError);
                    }
                    #endregion
                    response.Close();
                }
            }
            #region Обработка исключений
            
            catch (WebException ex) 
            {
                try
                {
                    using (HttpWebResponse ExceptionResponse = ((HttpWebResponse)ex.Response))
                    {
                         using (StreamReader reader = new StreamReader(ExceptionResponse.GetResponseStream()))
                        {
                            this.CallResult(null, reader, ExceptionResponse.StatusCode.ToString(), Helper.StringToStatus(ExceptionResponse.StatusCode.ToString()), v, ServerName, ControllerName, URI);
                         }
                    }
                   
                    ((HttpWebResponse)ex.Response).Close();
                    
       
                }
                catch (Exception e)
                {
                   // this.CallResult(null, null, e.Message, Status.UnknownError);
                    if (((HttpWebResponse)ex.Response) != null)
                    {
                         ((HttpWebResponse)ex.Response).Close();
                         ((HttpWebResponse)ex.Response).Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
               // this.CallResult(null, null, ex.Message, Status.UnknownError);
                if (requestResult != null)
                {
                    requestResult.Abort();
                    requestResult = null;
                }
            }
            #endregion

           // allDone.Set();
        }

       private void CallResult(string Content, StreamReader reader, string ErrMessage, Status status, AcceptVerbs v, string ServerName, string ControllerName, string URI)
       {

               string content = string.Empty;
               if (string.IsNullOrEmpty(ErrMessage))
                   ErrMessage = string.Empty;

               //Помойму бредовая конструкци
               if (string.IsNullOrEmpty(Content))
               {
                   if (reader != null)
                   {
                       content = reader.ReadToEnd();
                   }
               }
               else
               {
                   content = Content;
               }


           try
           {
               Delegate[] DelList = this._RequestResult.GetInvocationList();
               Delegate dele = DelList[0];
               
               int counte = DelList.Length;
               var tre = this._RequestResult.Method;

               this._RequestResult(new Result { 
                   IsOK = status == Status.OK ? true : false,
                   Content = content, 
                   ErrorMessage = ErrMessage, 
                   ResultStatus = status, 
                   Verbs = v, 
                   ControllerName = ControllerName,
                   URI = URI,
                   ServerName = ServerName});
             
              //// if(this.IsOnlyOneEventHandler)
              //     this._RequestResult = null;

              // /* if (this._RequestResult != null)
              //     if (this._RequestResult.GetInvocationList().Length >= 1)
              //         this._RequestResult = null;*/
           }
           catch
           {
               
           }
           
           
       }

        #endregion



       public void ClearEventHandlers()
       {
           this._RequestResult = null;
           
       }


    }




}
