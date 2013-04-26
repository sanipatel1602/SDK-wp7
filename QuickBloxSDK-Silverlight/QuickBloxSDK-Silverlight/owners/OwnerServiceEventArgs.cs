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
    public class OwnerServiceEventArgs
    {
        /// <summary>
        /// Возвращаемый объект
        /// </summary>
        public object result
        { get; set; }

        /// <summary>
        /// Тип возвращаемого объекта
        /// </summary>
        public Type t
        { get; set; }

        /// <summary>
        /// Статус выполненой операции
        /// </summary>
        public Status status
        { get; set; }

        /// <summary>
        /// Команда которая была в данный момент выполнена
        /// </summary>
        public OwnerServiceCommand currentCommand
        { get; set; }

        /// <summary>
        /// Сообщение об ошибке если таковая имеется.
        /// Орентироватся нужно по статусу.
        /// 
        /// </summary>
        public string errorMessage
        { get; set; }
    }
}
