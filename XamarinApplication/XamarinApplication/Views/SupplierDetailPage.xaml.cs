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
    public partial class SupplierDetailPage : PopupPage
    {
        public SupplierDetailPage(Supplier supplier)
        {
            InitializeComponent();
            var supplierViewModel = new SupplierDetailViewModel(Navigation);
            supplierViewModel.Supplier = supplier;
            BindingContext = supplierViewModel;
        }

        private async void Close_Popup_Supplier(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}