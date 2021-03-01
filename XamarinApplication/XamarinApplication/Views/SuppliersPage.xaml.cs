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
    public partial class SuppliersPage : ContentPage
    {
        public SuppliersPage()
        {
            Device.SetFlags(new[] { "Expander_Experimental" });
            InitializeComponent();
            BindingContext = new SuppliersViewModel();
        }

        protected override void OnAppearing()
        {
            (this.BindingContext as SuppliersViewModel).GetSuppliers();
        }

        private async void Supplier_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var supplier = e.Item as Supplier;
            //await Navigation.PushAsync(new SupplierDetailViewModel(supplier));
            await PopupNavigation.Instance.PushAsync(new SupplierDetailPage(supplier));
        }
        private async void Supplier_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var supplier = mi.CommandParameter as Supplier;
            await PopupNavigation.Instance.PushAsync(new SupplierDetailPage(supplier));
        }
        private async void Supplier_Products(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var supplier = mi.CommandParameter as Supplier;
            //await Navigation.PushAsync(new SupplierProductsPage(supplier));
            await PopupNavigation.Instance.PushAsync(new SupplierProductsPage(supplier));
        }
        private async void Update_Supplier(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var supplier = mi.CommandParameter as Supplier;
            //await Navigation.PushAsync(new SupplierProductsPage(supplier));
            await PopupNavigation.Instance.PushAsync(new UpdateSupplierPage(supplier));
        }

        private async void Add_Supplier(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewSupplierPage());
        }
    }
}