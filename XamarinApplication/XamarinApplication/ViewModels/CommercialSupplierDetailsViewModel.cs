using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;

namespace XamarinApplication.ViewModels
{
   public class CommercialSupplierDetailsViewModel
    {
        public INavigation Navigation { get; set; }
        public CommercialSupplierDetailsViewModel()
        {
            
        }
        public CommercialSupplier CommercialSupplier { get; set; }
    }
}
