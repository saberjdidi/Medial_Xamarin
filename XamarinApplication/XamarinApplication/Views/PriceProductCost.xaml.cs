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
    public partial class PriceProductCost : PopupPage
    {
        public PriceProductCost(ProductDTO productDTO)
        {
            InitializeComponent();
            var costViewModel = new PriceProductCostViewModel(Navigation);
            costViewModel.ProductDTO = productDTO;
            BindingContext = costViewModel;
        }
        private async void Close_Price(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}