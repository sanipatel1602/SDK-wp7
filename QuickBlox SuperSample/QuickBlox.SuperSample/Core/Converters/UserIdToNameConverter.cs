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
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

using QuickBlox.SuperSample.ViewModel;
//--------------
namespace QuickBlox.SuperSample.Converters
{
    public class UserIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string name = "test";
            int userId = 0;

            if (value != null && int.TryParse(value.ToString(), out userId))
            {
                QuickBloxSDK_Silverlight.QuickBlox qblox = (App.Current as App).QBlox;                
                try
                {
                   name  = qblox.QBUsers.Where(t => t.id == userId).Select(g => g.Username).First();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }               
            }
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
