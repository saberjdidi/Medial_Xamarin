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
    public partial class SupplierProductsPage : PopupPage
    {
        public SupplierProductsPage(Supplier supplier)
        {
            InitializeComponent();
            var supplierProducts = new SupplierProductsViewModel(supplier);
            supplierProducts.Supplier = supplier;
            BindingContext = supplierProducts;
        }

        private async void Close_Product_Supplier(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}