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
    public partial class ClientDetailPage : PopupPage
    {
        public ClientDetailPage(Client client)
        {
            InitializeComponent();
            var clientViewModel = new ClientDetailViewModel(Navigation);
            clientViewModel.Client = client;
            BindingContext = clientViewModel;
        }

        private async void Close_Popup_Client(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}