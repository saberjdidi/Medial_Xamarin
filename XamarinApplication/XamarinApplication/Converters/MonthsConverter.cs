using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XamarinApplication.Converters
{
   public class MonthsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is int && value != null)
            {
                int s = (int)value;
                switch (s)
                {
                    case 1:
                        return "January";
                    case 2:
                        return "Saved";
                    case 3:
                        return "Sent";
                    case 4:
                        return "To Be Completed";
                    case 5:
                        return "Validated";
                    case 6:
                        return "Signed";
                    case 7:
                        return "Non Selected";
                    case 8:
                        return "None";
                    case 9:
                        return "Validated";
                    case 10:
                        return "Signed";
                    case 11:
                        return "Non Selected";
                    case 12:
                        return "None";
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
