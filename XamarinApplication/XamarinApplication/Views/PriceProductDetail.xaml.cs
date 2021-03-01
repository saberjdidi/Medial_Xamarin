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
    public partial class PriceProductDetail : PopupPage
    {
        public PriceProductDetail(PriceProduct priceProduct)
        {
            InitializeComponent();
            var productViewModel = new PriceProductDetailViewModel(Navigation);
            productViewModel.PriceProduct = priceProduct;
            BindingContext = productViewModel;
        }
        private async void Close_Price(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as ProductDTO;
            await PopupNavigation.Instance.PushAsync(new PriceProductCost(product));
        }
    }
}