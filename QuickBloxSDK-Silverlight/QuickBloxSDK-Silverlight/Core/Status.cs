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
    /// <summary>
    /// Статус операции
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 200,201,Всё ништяк
        /// </summary>
        OK,
        /// <summary>
        /// 422, Ошибка валидации полей
        /// </summary>
        ValidationError,
        /// <summary>
        /// 404, не найден запрашиваемый сервис
        /// </summary>
        NotFoundError,

        /// <summary>
        /// 405, Такого рода операция не допускается
        /// </summary>
        MethodNotAllowed,
        /// <summary>
        /// Ошибка подключения к серверу(неверные настройки)
        /// или проблемы сети
        /// </summary>
        ConnectionError,
        /// <summary>
        /// Ошибка по таймауту
        /// </summary>
        TimeoutError,
        /// <summary>
        /// 401, Ошибка аутентификации
        /// </summary>
        AuthenticationError,
        /// <summary>
        /// Проблемы с выходным потоком
        /// </summary>
        StreamError,
        /// <summary>
        /// 403, Доступ запрещён
        /// </summary>
        AccessDenied,
        /// <summary>
        /// Неизвесная ошибка
        /// </summary>
        UnknownError,
        /// <summary>
        /// Ничего не произошло, или результат не подходит под под извесное описание
        /// </summary>
        none,
        /// <summary>
        /// Полученный от сервера контент не может быть приведён к заданному типу
        /// (не распарсился, битый итд)
        /// </summary>
        ContentError,
        /// <summary>
        /// Пустой список или пустой объект
        /// </summary>
        NullContent,
        NotAcceptable,
        Unauthorized

    }
}
