using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
   public class ProductAgentDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public ProductAgentDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public ProductAgent Product { get; set; }
    }
}
