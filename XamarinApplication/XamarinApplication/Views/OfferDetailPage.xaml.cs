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
    public partial class OfferDetailPage : PopupPage
    {
        public OfferDetailPage(Offer offer)
        {
            InitializeComponent();
            var offerViewModel = new OfferDetailViewModel(Navigation);
            offerViewModel.Offer = offer;
            BindingContext = offerViewModel;
        }

        private async void Close_Popup_Offer(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}