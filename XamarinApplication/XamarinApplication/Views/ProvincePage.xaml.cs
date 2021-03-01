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
    public partial class ProvincePage : ContentPage
    {
        public ProvincePage()
        {
            InitializeComponent();
            BindingContext = new ProvinceViewModel();
        }
        private async void Update_Province(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var province = mi.CommandParameter as Province;
            await PopupNavigation.Instance.PushAsync(new UpdateProvincePage(province));
        }

        private async void Add_Province(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewProvincePage());
        }

        void close(System.Object sender, System.EventArgs e)
        {
            contentPage.Opacity = 1;
            menuitems.IsVisible = false;
        }
        async void TapGestureRecognizer_Tapped_3(System.Object sender, System.EventArgs e)
        {
            Image image = sender as Image;
            string filename = image.Source.ToString();
            if (filename == "File: close.png")
            {
                contentPage.Opacity = 1;
                menu.Source = "add.png";
                await menulist.FadeTo(0);
                menulist.IsVisible = false;
                contentPage.InputTransparent = false;
            }
            else if (filename == "File: add.png")
            {
                contentPage.Opacity = 0.3;
                menu.Source = "close.png";
                await menulist.FadeTo(1, 0, Easing.SinIn);
                menulist.IsVisible = true;
                contentPage.InputTransparent = true;
            }
        }
    }
}