using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
    public class SupplierDetailViewModel
    {
        public INavigation Navigation { get; set; }
        public SupplierDetailViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }
        public Supplier Supplier { get; set; }
    }
}
