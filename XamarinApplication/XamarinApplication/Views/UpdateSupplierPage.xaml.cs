using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class UpdateSupplierPage : PopupPage
    {
        public UpdateSupplierPage(Supplier supplier)
        {
            InitializeComponent();

            var updateSupplierViewModel = new UpdateSupplierViewModel(Navigation);
            updateSupplierViewModel.Supplier = supplier;
            Debug.WriteLine("********supplier*************");
            Debug.WriteLine(supplier);
            BindingContext = updateSupplierViewModel;
        }
    }
}