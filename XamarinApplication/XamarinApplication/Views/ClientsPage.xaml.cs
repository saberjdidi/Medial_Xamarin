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
    public partial class ClientsPage : ContentPage
    {
        public ClientsPage()
        {
            InitializeComponent();
            Device.SetFlags(new[] { "Expander_Experimental" });
            BindingContext = new ClientsViewModel();

        }

        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            (BindingContext as ClientsViewModel).LoadMoreItems(e.Item as Client);
        }

        private async void Client_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var client = e.Item as Client;
            long id = 2219;
            await Navigation.PushAsync(new CalendarEventsPage(client));
        }
        private async void Client_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var client = mi.CommandParameter as Client;
            //await Navigation.PushAsync(new RequestDetailPage(request));
            await PopupNavigation.Instance.PushAsync(new ClientDetailPage(client));
        }
        private async void Calendar_Event(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var client = mi.CommandParameter as Client;
            await Navigation.PushAsync(new CalendarEventsPage(client));
        }
        private async void Client_Products(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var client = mi.CommandParameter as Client;
            //await Navigation.PushAsync(new SupplierProductsPage(supplier));
            await PopupNavigation.Instance.PushAsync(new ClientProductsPage(client));
        }
        private async void Update_Client(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var client = mi.CommandParameter as Client;
            //await PopupNavigation.Instance.PushAsync(new UpdateClientPage(client));
            await Navigation.PushAsync(new UpdateClientPage(client));
        }

        private async void Add_Client(object sender, EventArgs e)
        {
            // await PopupNavigation.Instance.PushAsync(new NewClientPage());
            await Navigation.PushAsync(new NewClientPage());
        }

        private async void Regional_Report(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new RegionalReportClient());
        }
        private async void Article_Region_Report(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new ArticleRegionalReport());
        }
    }
}