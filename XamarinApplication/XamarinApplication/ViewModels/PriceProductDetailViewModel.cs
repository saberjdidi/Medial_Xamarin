using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class PriceProductDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public PriceProductDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public PriceProduct PriceProduct { get; set; }
    }
}
