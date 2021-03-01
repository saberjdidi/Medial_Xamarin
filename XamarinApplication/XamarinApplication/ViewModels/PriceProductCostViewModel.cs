using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class PriceProductCostViewModel
    {
        public INavigation Navigation { get; set; }
        public PriceProductCostViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public ProductDTO ProductDTO { get; set; }
    }
}
