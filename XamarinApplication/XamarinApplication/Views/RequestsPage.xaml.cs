using Rg.Plugins.Popup.Extensions;
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
    public partial class RequestsPage : ContentPage
    {
        public RequestsPage()
        {
            InitializeComponent();
            BindingContext = new RequestsViewModel(Navigation);
        }
       /* protected override void OnAppearing()
        {
            (this.BindingContext as RequestsViewModel).GetRequests();

        }*/

        private async void Request_Detail(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var request = mi.CommandParameter as Request;
            //await Navigation.PushAsync(new RequestDetailPage(request));
            await PopupNavigation.Instance.PushAsync(new RequestDetailPage(request));
        }
        private void MenuItem_Clicked(object sender, EventArgs e)
        {
           // var button = sender as Button;
           // var request = button.BindingContext as Request;
            //var menuItem = sender as Button;
            var mi = ((MenuItem)sender);
            var selectedItem = mi.CommandParameter as Request;
            Navigation.PushAsync(new RequestDetailPage(selectedItem));
        }

        /*private void Search_Request(object o, EventArgs e)
        {
            var pop = new SearchRequestPage();
            pop.OnDialogClosed += (s, args) =>
            {
               // ResultTxt.Text = args.Message;
               // ResultLtvw.ItemsSource = args.RequestsPopup;  //x:Name="ResultLtvw"
            };
            App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            //PopupNavigation.Instance.PushAsync(new SearchRequestPage());
        }*/

    }
}