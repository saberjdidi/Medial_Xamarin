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
    public partial class ProductsAgentPage : ContentPage
    {
        public ProductsAgentPage()
        {
            InitializeComponent();
            BindingContext = new ProductsAgentViewModel();

        }
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            (BindingContext as ProductsAgentViewModel).LoadMoreItems(e.Item as ProductAgent);
        }

        private async void Products_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as ProductAgent;
            await PopupNavigation.Instance.PushAsync(new ProductAgentDetail(product));
        }
    }
}