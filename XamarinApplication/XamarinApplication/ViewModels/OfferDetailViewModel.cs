using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class OfferDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public OfferDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public Offer Offer { get; set; }
    }
}
