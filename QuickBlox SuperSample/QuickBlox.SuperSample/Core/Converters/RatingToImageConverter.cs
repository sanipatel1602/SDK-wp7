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
//--------------
namespace QuickBlox.SuperSample.Converters
{
    public class RatingToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string imageUrl = "/graphics/RatingImages/rating0.jpg";
            int rating = 0;

            if(value != null && int.TryParse(value.ToString(), out rating))
            {
                imageUrl = string.Format("/graphics/RatingImages/rating{0}.jpg", rating);
            }

            return imageUrl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
