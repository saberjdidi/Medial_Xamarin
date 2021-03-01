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
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage()
        {
            InitializeComponent();
            Device.SetFlags(new[] { "Expander_Experimental" });
            BindingContext = new ProductsViewModel();
        }
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            (BindingContext as ProductsViewModel).LoadMoreItems(e.Item as Product);
        }

        private async void Products_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as Product;
            await PopupNavigation.Instance.PushAsync(new ProductDetailPage(product));
        }
        private async void Product_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var product = mi.CommandParameter as Product;
            //await Navigation.PushAsync(new RequestDetailPage(request));
            await PopupNavigation.Instance.PushAsync(new ProductDetailPage(product));
        }

        private async void Add_Product(object sender, EventArgs e)
        {
            //await PopupNavigation.Instance.PushAsync(new NewProductPage());
            await Navigation.PushAsync(new NewProductPage());
        }
        private async void Update_Product(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var product = mi.CommandParameter as Product;
            //await PopupNavigation.Instance.PushAsync(new UpdateClientPage(client));
            await Navigation.PushAsync(new UpdateProductPage(product));
        }
        private async void Component_Product(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var product = mi.CommandParameter as Product;
            //await Navigation.PushAsync(new SupplierProductsPage(supplier));
            await PopupNavigation.Instance.PushAsync(new ComponentProductPage(product));
        }
        private async void Show_Price(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new PriceProductPage());
        }
    }
}