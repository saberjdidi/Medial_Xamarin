using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class ClientProductsDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public ClientProductsDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public SellingDetails SellingDetails { get; set; }
    }
}
