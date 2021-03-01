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
    public partial class ClientProductsPage : PopupPage
    {
        public ClientProductsPage(Client client)
        {
            InitializeComponent();
            var clientProducts = new ClientProductsViewModel(client);
            clientProducts.Client = client;
            BindingContext = clientProducts;
        }

        private async void SellingDetails_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var sellingDetails = e.Item as SellingDetails;
            await PopupNavigation.Instance.PushAsync(new ClientProductsDetailPage(sellingDetails));
        }

        private async void Close_Product_Client(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}