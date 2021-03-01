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
    public partial class ProductAgentDetail : PopupPage
    {
        public ProductAgentDetail(ProductAgent product)
        {
            InitializeComponent();
            var productViewModel = new ProductAgentDetailViewModel(Navigation);
            productViewModel.Product = product;
            BindingContext = productViewModel;
        }
        private async void Close_Popup_Product(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}