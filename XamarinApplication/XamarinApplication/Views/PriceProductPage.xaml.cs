using Rg.Plugins.Popup.Pages;
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
    public partial class PriceProductPage : PopupPage
    {
        public PriceProductPage()
        {
            InitializeComponent();
            BindingContext = new PriceProductViewModel();
        }

        private async void Close_Price(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as PriceProduct;
           // await Navigation.PushAsync(new PriceProductDetail(product));
            await PopupNavigation.Instance.PushAsync(new PriceProductDetail(product));
        }
    }
}