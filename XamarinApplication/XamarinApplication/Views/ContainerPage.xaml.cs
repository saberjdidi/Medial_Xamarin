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
    public partial class ContainerPage : ContentPage
    {
        public ContainerPage()
        {
            InitializeComponent();
            BindingContext = new ContainerViewModel();
        }
        private async void Update_Container(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var containner = mi.CommandParameter as Containner;
            await PopupNavigation.Instance.PushAsync(new UpdateContainer(containner));
        }

        private async void Add_Container(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewContainerPage());
        }
    }
}