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
    public partial class CurrencyPage : ContentPage
    {
        public CurrencyPage()
        {
            InitializeComponent();
            BindingContext = new CurruncyViewModel();
        }
        private async void Add_Curruncy(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewCurrencyPage());
        }
        private async void Update_Curruncy(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var currency = mi.CommandParameter as Currency;
            await PopupNavigation.Instance.PushAsync(new UpdateCurrencyPage(currency));
        }
    }
}