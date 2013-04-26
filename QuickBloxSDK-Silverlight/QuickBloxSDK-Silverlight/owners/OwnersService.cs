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

namespace QuickBloxSDK_Silverlight.owners
{
    public class OwnersService
    {
        public delegate void OwnerServiceHeandler(OwnerServiceEventArgs Args);


        public event OwnerServiceHeandler OwnerServiceEvent;


        private ConnectionContext Сontext;



        public OwnersService(ConnectionContext context)
        {
            Сontext = context;
            this.Сontext.RequestResult +=new ConnectionContext.Main((Result result)=>{

                if (result.ServerName != Helper.PartToServerName(Part.users))
                    return;

                switch (result.Verbs)
                {
                    case AcceptVerbs.GET:
                        {

                           if (result.ControllerName.Contains("/owners/") && result.ControllerName.Contains(".xml"))
                           {
                               this.GetOwner_Response(result);
                               return;
                           }
                            break;
                        }
                    case AcceptVerbs.DELETE:
                        {
                           /* if (result.ControllerName.Contains("/owners/") && result.ControllerName.Contains(".xml"))
                            {
                                this.DeleteOwner_Response(result);
                                return;
                            }*/
                            break;
                        }
                    case AcceptVerbs.PUT:
                        {
                           /* /*if (result.ControllerName.Contains("/owners/") && result.ControllerName.Contains(".xml"))
                            {
                                this.EditOwner_Response(result);
                                return;
                            }*/
                            break;
                        }
                    case AcceptVerbs.POST:
                        {

                           /* if (result.ControllerName.Contains("/owners"))
                            {
                                this.AddOwner_Response(result);
                                return;
                            }*/
                            break;
                        }
                }

               


                
            
            });
        }






      /*  private void DeleteOwner_Response(Result result)
        {
            if (OwnerServiceEvent != null) // если привязан обработчик
            {
                if (result.ResultStatus == Status.OK) // если всё хорошо и пришол контент
                {
                    try
                    {
                        this.OwnerServiceEvent(new OwnerServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = result.ResultStatus,
                            currentCommand = OwnerServiceCommand.DeleteOwner,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.OwnerServiceEvent(new OwnerServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = OwnerServiceCommand.DeleteOwner,
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
                    this.OwnerServiceEvent(new OwnerServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = OwnerServiceCommand.DeleteOwner,
                        errorMessage = result.ErrorMessage
                    });

            }
        }
        public void DeleteOwner(int id)
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.SendAsyncRequest("owners/" +  id.ToString() + ".xml", AcceptVerbs.DELETE);

        }*/




        private void GetOwner_Response(Result result)
        {
            //if (OwnerServiceEvent != null) // если привязан обработчик
            //{
                if (result.ResultStatus == Status.OK) // если всё хорошо и пришол контент
                {
                    try // Распарсиваем
                    {
                        this.OwnerServiceEvent(new OwnerServiceEventArgs
                        {
                            result = new Owner((string)result.Content),
                            t = typeof(Owner),
                            status = result.ResultStatus,
                            currentCommand =  OwnerServiceCommand.GetOwner,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.OwnerServiceEvent(new OwnerServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = OwnerServiceCommand.GetOwner,
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
                    this.OwnerServiceEvent(new OwnerServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = OwnerServiceCommand.GetOwner,
                        errorMessage = result.ErrorMessage
                    });
                else
                {
                    this.OwnerServiceEvent(new OwnerServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = OwnerServiceCommand.GetOwner,
                        errorMessage = result.ErrorMessage
                    });
                }
            //}
        }
        public void GetOwner(int id)
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.SendAsyncRequest("owners/" + id.ToString() + ".xml", AcceptVerbs.GET);

        }



/*
        private void AddOwner_Response(Result result)
        {
            if (OwnerServiceEvent != null) // если привязан обработчик
            {
                if (result.ResultStatus == Status.OK) // если всё хорошо и пришол контент
                {
                    try
                    {

                        this.OwnerServiceEvent(new OwnerServiceEventArgs
                        {
                            result = new Owner(result.Content),
                            t = typeof(Owner),
                            status = result.ResultStatus,
                            currentCommand = OwnerServiceCommand.AddOwner,
                            errorMessage = result.ErrorMessage
                        });
                    }
                    catch (Exception ex) // ошибка распарсивания
                    {
                        this.OwnerServiceEvent(new OwnerServiceEventArgs
                        {
                            result = null,
                            t = null,
                            status = Status.ContentError,
                            currentCommand = OwnerServiceCommand.AddOwner,
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
                    this.OwnerServiceEvent(new OwnerServiceEventArgs
                    {
                        result = null,
                        t = null,
                        status = result.ResultStatus,
                        currentCommand = OwnerServiceCommand.AddOwner,
                        errorMessage = result.ErrorMessage
                    });
                else if (result.ResultStatus == Status.ValidationError)
                {
                    this.OwnerServiceEvent(new OwnerServiceEventArgs
                    {
                        result = ValidateErrorElement.LoadErrorList(result.Content),
                        t = typeof(ValidateErrorElement[]),
                        status = result.ResultStatus,
                        currentCommand = OwnerServiceCommand.AddOwner,
                        errorMessage = result.ErrorMessage
                    });
                }
                else
                {
                    this.OwnerServiceEvent(new OwnerServiceEventArgs
                    {
                        result = result.Content,
                        t = null,
                        status = Status.UnknownError,
                        currentCommand = OwnerServiceCommand.AddOwner,
                        errorMessage = result.ErrorMessage
                    });
                }
            }
        }
        public void AddOwner(Owner owner)
        {
            this.Сontext.CurrentPart = Part.users;
            this.Сontext.Add("type_type", OwnerServiceHelper.TypeTypeToString(owner.TypeType));

            if(owner.TypeType == TypeType.Application)
                this.Сontext.Add("application_id ", owner.ApplicationId.ToString());
            else
                this.Сontext.Add("service_id", owner.ServiceId.ToString());

            this.Сontext.Add("custom-fields", owner.CustomFields);
            this.Сontext.Add("required_fields", owner.RequiredFields);
            this.Сontext.Add("editable_fields", owner.EditableFields);
            this.Сontext.Add("password_reset_type", OwnerServiceHelper.PasswordResetTypeToString(owner.PasswordResetType));
            this.Сontext.Add("email_edit_type", OwnerServiceHelper.EmailEditTypeToString(owner.EmailEditType));
            this.Сontext.Add("registration_confirm_type", OwnerServiceHelper.RegistrationConfirmTypeToString(owner.RegistrationConfirmType));
            this.Сontext.Add("authorization_type", OwnerServiceHelper.AuthorizationTypeToString(owner.AuthorizationType));
            this.Сontext.Add("email_confirm_mask", owner.EmailConfirmMask);
            this.Сontext.Add("password_reset_mask", owner.PasswordResetMask);
            this.Сontext.Add("email_confirm_mask", owner.EmailConfirmMask);




            this.Сontext.SendAsyncRequest("owners", AcceptVerbs.POST);
        }*/
    }
}
