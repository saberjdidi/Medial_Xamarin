using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;

namespace XamarinApplication.Converters
{
   public class ProductVisibleConverter : IValueConverter
    {
        public User User { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Username = Settings.Username;
            User = JsonConvert.DeserializeObject<User>(Username);

            bool visible = (bool)value;
            if(User.roles.Select(r => r.name).FirstOrDefault().Equals("ROLE_ADMIN"))
            {
                visible = true;
            } else if(User.roles.Select(r => r.name).FirstOrDefault().Equals("ROLE_AGENT"))
            {
                visible = false;
            }
            return visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
