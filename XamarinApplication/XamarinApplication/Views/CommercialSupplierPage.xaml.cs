using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Models;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommercialSupplierPage : ContentPage
    {
        public CommercialSupplierPage()
        {
            InitializeComponent();
            BindingContext = new CommercialSupplierViewModel();
        }

        private async void Product_Details(object sender, ItemTappedEventArgs e)
        {
           // var mi = ((MenuItem)sender);
           // var commercial = mi.CommandParameter as CommercialSupplier;
            var commercial = e.Item as CommercialSupplier;
            await PopupNavigation.Instance.PushAsync(new CommercialSupplierDetails(commercial));
        }
    }
}