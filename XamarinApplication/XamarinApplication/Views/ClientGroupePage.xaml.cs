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
    public partial class ClientGroupePage : ContentPage
    {
        public ClientGroupePage()
        {
            InitializeComponent();
            BindingContext = new ClientGroupeViewModel();
        }
        private async void Add_Groupe(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewClientGroupePage());
        }
        private async void Update_Groupe(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var groupe = mi.CommandParameter as Groupe;
            await PopupNavigation.Instance.PushAsync(new UpdateGroupeClientPage(groupe));
        }
    }
}