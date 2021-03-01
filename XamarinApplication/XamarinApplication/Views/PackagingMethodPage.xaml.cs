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
    public partial class PackagingMethodPage : ContentPage
    {
        public PackagingMethodPage()
        {
            InitializeComponent();
            BindingContext = new PackagingMethodViewModel();
        }
        private async void Add_PackagingMethod(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewPackagingMethodPage());
        }
        private async void Update_PackagingMethod(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var packagingMethod = mi.CommandParameter as PackagingMethod;
            await PopupNavigation.Instance.PushAsync(new UpdatePackagingMethodPage(packagingMethod));
        }
    }
}