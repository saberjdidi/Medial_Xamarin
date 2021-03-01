using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class RequestDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public RequestDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public Request Request { get; set; }
    }
}
