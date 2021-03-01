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
    public partial class RegionPage : ContentPage
    {
        public RegionPage()
        {
            InitializeComponent();
            BindingContext = new RegionViewModel();
        }
        private async void Add_Region(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewRegionPage());
        }
        private async void Update_Region(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var region = mi.CommandParameter as Reggion;
            await PopupNavigation.Instance.PushAsync(new UpdateRegionPage(region));
        }
    }
}