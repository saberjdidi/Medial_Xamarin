using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
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
    public partial class CommercialProductPage : ContentPage
    {
        public CommercialProductPage()
        {
            InitializeComponent();
            BindingContext = new CommercialProductViewModel();
        }
        private async void Product_Details(object sender, ItemTappedEventArgs e)
        {
            var commercial = e.Item as CommercialSupplier;
            await PopupNavigation.Instance.PushAsync(new CommercialSupplierDetails(commercial));
        }
    }
}