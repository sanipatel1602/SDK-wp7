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
using QuickBloxSDK_Silverlight.Core;
using System.Collections.Generic;
using System.Xml.Linq;

namespace QuickBloxSDK_Silverlight.users
{
    public class UserService
    {
         public delegate void UserServiceHeandler(UserServiceEventArgs Args);

      
        public event UserServiceHeandler UserServiceEvent;

       
        public bool IsOnline
        {
            get;
            set;
        }

        public int OwnerId
        { get; set; }

        public User user
        { get; set; }


         /// <summary>
        /// Контекст подключения
        /// </summary>
        private ConnectionContext Сontext;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        public UserService(ConnectionContext context)
        {
            Сontext = context;
            this.Сontext.RequestResult +=new ConnectionContext.Main((Result result)=>{

                if (result.ServerName != Helper.PartToServerName(Part.users))
                    return;

                switch (result.Verbs)
                {
                    case AcceptVerbs.GET:
                        {

                            if (result.ControllerName.Contains("/owners/") && result.ControllerName.Contains("/users.xml"))
                            {
                                this.GetUsersByOwner_Response(result);
                                return;
                            }


                            if (result.ControllerName.Contains("/users/") && result.ControllerName.Contains(".xml"))
                            {
                                this.GetUser_Response(result);
                                return;
                            }
                            if (result.ControllerName.Contains("users/email/"))
                            {
                                this.EmailVerification_Response(result);
                                return;
                            }

                            if (result.ControllerName.Contains("users/identify"))
                            {
                                this.Identify_Response(result);
                                return;
                            }

                            if (result.ControllerName.Contains("users/logout"))
                            {
                                this.Logout_Response(result);
                                return;
                            }

                            if (result.ControllerName.Contains("/users/resetmypasswordbyemail"))
                            {
                                this.Resetmypasswordbyemail_Response(result);
                                return;
                            }

                            if (result.ControllerName.Contains("/users/resetpassword/"))
                            {
                                this.Resetpassword_Response(result);
                                return;
                            }

                            break;
                        }
                    case AcceptVerbs.DELETE:
                        {
                            if (result.ControllerName.Contains("/users/") && result.ControllerName.Contains(".xml"))
                            {
                                this.DeleteUser_Response(result);
                                return;
                            }
                            break;
                        }
                    case AcceptVerbs.PUT:
                        {
                            if (result.ControllerName.Contains("/users/") && result.ControllerName.Contains(".xml"))
                            {
                                this.EditUser_Response(result);
                                return;
                            }
                            break;
                        }
                    case AcceptVerbs.POST:
                        {
                            if (result.ControllerName.Contains("/users/authenticate"))
                            {
                                this.Authenticate_Response(result);
                                return;
                            }
                            if (result.ControllerName.Contains("/users/") && result.ControllerName.Contains("/update_password.xml"))
                            {
                                this.SetNewPassword_Response(result);
                                return;
                            }
                            if (result.ControllerName.Contains("/users"))
                            {
                                this.AddUser_Response(result);
                                return;
                            }
                            break;
                        }
                }

               


                
            
            });
        }


        private void GetUser_Response(Result result)
        {
            if (UserServiceEvent != null) // если привязан обработчик
            {
                if (result.ResultStatus == Status.OK) // если всё хорошо и пришол контент
                {
                    try // Распарсиваем
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = new User((string)result.Content),
                            t = typeof(User),
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.GetUser,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.GetUser,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.GetUser,
                        errorMessage = result.ErrorMessage
                    });
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.GetUser,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }


        /// <summary>
        /// Gets the  quickblox  user
        /// </summary>
        /// <param name="id">identifier or external identifire</param>
        /// <param name="IsExternalID">identifire main or external</param>
        public void GetUser(int id, bool IsExternalID)
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.SendAsyncRequest("users/" + (IsExternalID ? "external/" : string.Empty) + id.ToString() + ".xml", AcceptVerbs.GET);

        }


        private void GetUsersByOwner_Response(Result result)
        {
            if (UserServiceEvent != null) // если привязан обработчик
            {
                if (result.ResultStatus == Status.OK) // если всё хорошо и пришол контент
                {
                    try
                    {
                        List<User> users = new List<User>();
                        XElement xml = XElement.Parse((string)result.Content);
                        foreach (var t in xml.Descendants("user"))
                        {
                            try
                            {

                                users.Add(new User(t.ToString()));
                            }
                            catch
                            {
 
                            }
                        }

                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = users.ToArray(),
                            t = typeof(User[]),
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.GetUsersByOwner,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.GetUsersByOwner,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.GetUsersByOwner,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.GetUsersByOwner,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.GetUsersByOwner,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }
        public void GetUsersByOwner(int OwnerID)
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.SendAsyncRequest("owners/" + OwnerID + "/users.xml", AcceptVerbs.GET);

        }
        public void GetUsersByOwner()
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.SendAsyncRequest("owners/" + this.OwnerId + "/users.xml", AcceptVerbs.GET);

        }




        private void DeleteUser_Response(Result result)
        {
            if (UserServiceEvent != null) // если привязан обработчик
            {
                if (result.ResultStatus == Status.OK) // если всё хорошо и пришол контент
                {
                    try
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.DeleteUser,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.DeleteUser,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.DeleteUser,
                        errorMessage = result.ErrorMessage
                    });

            }
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя в базе</param>
        public void DeleteUser(int id, bool IsExternalID)
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.SendAsyncRequest("users/" + (IsExternalID ? "external/" : string.Empty) + id.ToString() + ".xml", AcceptVerbs.DELETE);
            
        }


        private void EmailVerification_Response(Result result)
        {
            if (UserServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    try
                    {

                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.EmailVerification,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.EmailVerification,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.EmailVerification,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.EmailVerification,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.EmailVerification,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }

        public void EmailVerification(User user, bool authenticate)
        {
            #region base validation
           if (user == null)
                return;
            #endregion
            #region form
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.Add("authenticate", authenticate ? "true" : "false");
            #endregion
            #region Request
            this.Сontext.SendAsyncRequest("users/email/" + user.EmailVerificationCode, AcceptVerbs.GET);
            #endregion
        }


        private void Authenticate_Response(Result result)
        {
            if (UserServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    try
                    {
                        User quser = new User(result.Content);
                        this.user = quser;
                        this.IsOnline = true;
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = quser,
                            t = typeof(User),
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.Authenticate,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.Authenticate,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.Authenticate,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.Authenticate,
                        errorMessage = result.ErrorMessage
                    });
                }
                else if (result.ResultStatus == Status.AuthenticationError)
                {
                    this.user = null;
                    this.IsOnline = false;
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = Status.AuthenticationError,
                        currentCommand = UserServiceCommand.Authenticate,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.Authenticate,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }
        public void Authenticate(string Username, int OwnerId, string Password)
        {
            #region base validation
            if ( OwnerId < 0 || string.IsNullOrEmpty(Password))
                return;
            #endregion
            #region form
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.Add("login", Username);
            this.Сontext.Add("owner_id", OwnerId.ToString());
            this.Сontext.Add("password", Password);
            #endregion
            #region Request
            this.Сontext.SendAsyncRequest("users/authenticate", AcceptVerbs.POST);
            #endregion
        }



        private void Identify_Response(Result result)
        {
            if (UserServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    try
                    {
                        User quser = new User(result.Content);
                        this.user = quser;
                        this.IsOnline = true;
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = quser,
                            t = typeof(User),
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.Identify,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.Identify,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.Identify,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.Identify,
                        errorMessage = result.ErrorMessage
                    });
                }
                else if (result.ResultStatus == Status.AuthenticationError)
                {
                    this.user = null;
                    this.IsOnline = false;
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = Status.AuthenticationError,
                        currentCommand = UserServiceCommand.Identify,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.Identify,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }
        public void Identify()
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.SendAsyncRequest("users/identify", AcceptVerbs.GET);
           
        }


        private void Resetmypasswordbyemail_Response(Result result)
        {
            if (UserServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    try
                    {
                       
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.Resetmypasswordbyemail,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.Resetmypasswordbyemail,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.Resetmypasswordbyemail,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.Resetmypasswordbyemail,
                        errorMessage = result.ErrorMessage
                    });
                }
                else if (result.ResultStatus == Status.AuthenticationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = Status.AuthenticationError,
                        currentCommand = UserServiceCommand.Resetmypasswordbyemail,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.Resetmypasswordbyemail,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }
        public void Resetmypasswordbyemail(string Email)
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.Add("email", Email);
            this.Сontext.SendAsyncRequest("users/resetmypasswordbyemail", AcceptVerbs.GET);

        }
        private void Resetpassword_Response(Result result)
        {
            if (UserServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    try
                    {
                        ResultMessage mess = new ResultMessage(result.Content);
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = mess.Password,
                            t = typeof(string),
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.Resetpassword,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.Resetpassword,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.Resetpassword,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.Resetpassword,
                        errorMessage = result.ErrorMessage
                    });
                }
                else if (result.ResultStatus == Status.AuthenticationError)
                {
                    
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = Status.AuthenticationError,
                        currentCommand = UserServiceCommand.Resetpassword,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.Resetpassword,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }
        public void Resetpassword(string ResetCode)
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.SendAsyncRequest("users/resetpassword/" + ResetCode, AcceptVerbs.GET);

        }

        public void Resetpassword(User user)
        {
            if (user == null)
                return;

            this.Сontext.CurrentPart = Part.users;
            this.Сontext.SendAsyncRequest("users/resetpassword/" + user.PasswordResetCode, AcceptVerbs.GET);

        }


        private void Logout_Response(Result result)
        {
            if (UserServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    try
                    {
                        
                        this.IsOnline = false;
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.Logout,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.Logout,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.Logout,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.Logout,
                        errorMessage = result.ErrorMessage
                    });
                }
                else if (result.ResultStatus == Status.AuthenticationError)
                {
                    this.user = null;
                    this.IsOnline = false;
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = Status.AuthenticationError,
                        currentCommand = UserServiceCommand.Logout,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.Logout,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }
        public void Logout()
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.SendAsyncRequest("users/logout", AcceptVerbs.GET);

        }


        private void SetNewPassword_Response(Result result)
        {
            if (UserServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    try
                    {

                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.SetNewPassword,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.SetNewPassword,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.SetNewPassword,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.SetNewPassword,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.SetNewPassword,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }
        public void SetNewPassword(int UserId, int OwnerId, string Password)
        {
            #region base validation
            if (UserId < 0 || OwnerId < 0 || string.IsNullOrEmpty(Password))
                return;
            #endregion
            #region form
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.Add("owner_id", OwnerId.ToString());
            this.Сontext.Add("password", Password);
            #endregion
            #region Request
            this.Сontext.SendAsyncRequest("users/" + UserId.ToString() + "/update_password.xml", AcceptVerbs.POST);
            #endregion
        }

        private void AddUser_Response(Result result)
        {
            if (UserServiceEvent != null) // если привязан обработчик
            {
                if (result.ResultStatus == Status.OK) // если всё хорошо и пришол контент
                {
                    try
                    {

                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = new User(result.Content),
                            t = typeof(User),
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.AddUser,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = UserServiceCommand.AddUser,
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
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.AddUser,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.AddUser,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.AddUser,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }
        public void AddUser(User user)
        {
            if (string.IsNullOrEmpty(user.Username))
                return;

            this.Сontext.CurrentPart = Part.users;
            this.Сontext.Add("user[owner_id]", user.OwnerId.ToString());
           // this.Сontext.Add("user[device_id]", user.DeviceId);
            this.Сontext.Add("user[email]", user.Email);
            this.Сontext.Add("user[login]", user.Username);
            this.Сontext.Add("user[full_name]", user.FullName);
            this.Сontext.SendAsyncRequest("users", AcceptVerbs.POST);
        }



        private void EditUser_Response(Result result)
        {
            if (UserServiceEvent != null)
            {
                if (result.ResultStatus == Status.OK)
                {
                    
                        this.UserServiceEvent(new UserServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = result.ResultStatus,
                            currentCommand = UserServiceCommand.EditUser,
                            errorMessage = result.ErrorMessage
                        });
                    
                   
                }
                // нет контента из за ошибок
                else if (result.ResultStatus == Status.StreamError
                    || result.ResultStatus == Status.TimeoutError
                    || result.ResultStatus == Status.UnknownError
                    || result.ResultStatus == Status.NotFoundError
                    || result.ResultStatus == Status.AccessDenied
                    || result.ResultStatus == Status.ConnectionError
                    || result.ResultStatus == Status.AuthenticationError)
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.EditUser,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = UserServiceCommand.EditUser,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.UserServiceEvent(new UserServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = UserServiceCommand.EditUser,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }
        /// <summary>
        /// Редактировать пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        public void EditUser(User user)
        {
            if (string.IsNullOrEmpty(user.Username))
                return;

            this.Сontext.CurrentPart = Part.users;

            if (user.ExternalUserId.HasValue)
           // this.Сontext.Add("user[external_user_id]", user.ExternalUserId.Value.ToString());
            this.Сontext.Add("user[email]", user.Email);
            this.Сontext.Add("user[full_name]", user.FullName);
            this.Сontext.Add("user[phone]", user.Phone);
            this.Сontext.Add("user[website]", user.Website);
            this.Сontext.SendAsyncRequest("users/" + user.id.ToString() + ".xml", AcceptVerbs.PUT);
        }
    }
}
