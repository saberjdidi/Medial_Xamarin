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
    public partial class UpdateClientPage : TabbedPage
    {
        public UpdateClientPage(Client client )
        {
            InitializeComponent();

            var updateClientViewModel = new UpdateClientViewModel(Navigation);
            updateClientViewModel.Client = client;
            BindingContext = updateClientViewModel;

            NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
        }
        /* private async void Add_Event_Client(object sender, EventArgs e)
         {
             var mi = ((MenuItem)sender);
             var client = mi.CommandParameter as Client;
             await PopupNavigation.Instance.PushAsync(new NewEventPage(client));
         }*/
        private async void Update_Event(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            Events events = ((UpdateClientViewModel)BindingContext).Events.Where(ser => ser.id == (int)tappedEventArgs.Parameter).FirstOrDefault();
            await PopupNavigation.Instance.PushAsync(new UpdateEventPage(events));
        }
    }
}