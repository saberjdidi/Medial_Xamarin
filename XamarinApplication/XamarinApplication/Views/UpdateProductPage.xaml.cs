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
    public partial class UpdateProductPage : TabbedPage
    {
        public UpdateProductPage(Product product)
        {
            InitializeComponent();

            var updateProductViewModel = new UpdateProductViewModel(Navigation);
            updateProductViewModel.Product = product;
            BindingContext = updateProductViewModel;

            NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
        }
    }
}