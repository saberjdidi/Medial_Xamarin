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
    public partial class ComponentProductPage : PopupPage
    {
        public ComponentProductPage(Product product)
        {
            InitializeComponent();
            var componentProduct = new ComponentProductViewModel();
            componentProduct.Product = product;
            BindingContext = componentProduct;
        }

        private async void Close_Component(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}