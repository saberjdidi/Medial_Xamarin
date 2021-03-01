using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XamarinApplication.Converters
{
    public class StringStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is string && value != null)
            {
                string s = (string)value;
                switch (s)
                {
                    case "CH":
                        return "Checked";
                    case "SV":
                        return "Saved";
                    case "SE":
                        return "Sent";
                    case "TC":
                        return "To Be Completed";
                    case "VL":
                        return "Validated";
                    case "SI":
                        return "Signed";
                    case "NS":
                        return "Non Selected";
                    case "":
                        return "None";
                    default:
                        return "Auther";
                }

            }
            return "Auther";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
