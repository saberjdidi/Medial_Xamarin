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
    public partial class ClientsAgentPage : ContentPage
    {
        public ClientsAgentPage()
        {
            Device.SetFlags(new[] { "Expander_Experimental" });
            InitializeComponent();
            BindingContext = new ClientsAgentViewModel();
        }
        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            (BindingContext as ClientsAgentViewModel).LoadMoreItems(e.Item as Client);
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
        private async void Add_Client(object sender, EventArgs e)
        {
            // await PopupNavigation.Instance.PushAsync(new NewClientPage());
            await Navigation.PushAsync(new NewClientPage());
        }
    }
}