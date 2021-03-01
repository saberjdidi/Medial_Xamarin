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
    public partial class CustomsDutyPage : ContentPage
    {
        public CustomsDutyPage()
        {
            InitializeComponent();
            BindingContext = new CustomDutyViewModel();
        }
        private async void Update_CustomsDuty(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var customsDuty = mi.CommandParameter as CustomsDuty;
            await PopupNavigation.Instance.PushAsync(new UpdateCustomDutyPage(customsDuty));
        }

        private async void Add_CustomsDuty(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewCustomDutyPage());
        }
    }
}