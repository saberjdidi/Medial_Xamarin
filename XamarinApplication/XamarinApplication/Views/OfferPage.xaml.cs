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
    public partial class OfferPage : ContentPage
    {
        public OfferPage()
        {
            InitializeComponent();
            BindingContext = new OfferViewModel();
        }

        private async void Offer_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var offer = mi.CommandParameter as Offer;
            await PopupNavigation.Instance.PushAsync(new OfferDetailPage(offer));
        }
        private async void Update_Offer(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var offer = mi.CommandParameter as Offer;
            await PopupNavigation.Instance.PushAsync(new UpdateOfferPage(offer));
        }
        private async void Add_Offer(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewOfferPage());
        }
    }
}