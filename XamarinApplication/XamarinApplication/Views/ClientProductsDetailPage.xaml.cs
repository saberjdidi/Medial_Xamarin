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
    public partial class ClientProductsDetailPage : PopupPage
    {
        public ClientProductsDetailPage(SellingDetails sellingDetails)
        {
            InitializeComponent();
            var clientViewModel = new ClientProductsDetailViewModel(Navigation);
            clientViewModel.SellingDetails = sellingDetails;
            BindingContext = clientViewModel;
        }

        private async void Close_Popup_SellingDetails(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
        private async void Close_SellingDetails(object o, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}