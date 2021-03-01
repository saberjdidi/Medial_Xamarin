using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XamarinApplication.Converters
{
    public class BackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is string && value != null)
            {
                string s = (string)value;
                switch (s)
                {
                    case "0":
                        return Color.FromHex("#9FCC2E");
                    case "2":
                        return Color.FromHex("#D64933");
                    default:
                        return Color.FromHex("#00B2CA");
                }

            }
            return Color.FromHex("#00B2CA");

            /*  //Return boolean True or False
            return ((bool)value ? Color.FromHex("#548687") : Color.Green);
            */
            /*
             * switch (value.ToString().ToLower())
            {
                case "CH":
                    return Color.Green;
                case "SV":
                    return Color.FromHex("#548687");
                case "SE":
                    return Color.FromHex("#95C623");
            }

            *return Color.FromHex("#F3CA40");


            string s = (string)value;
            if (s.Equals("CH"))
            {
                return Color.FromHex("#548687");
            }
            else if (s.Equals("SE"))
            {
                return Color.FromHex("#95C623");
            }
            else
            {
                return Color.Black;
            }

           
            */
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
