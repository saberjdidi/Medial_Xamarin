using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class ProductDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public ProductDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public Product Product { get; set; }
    }
}
