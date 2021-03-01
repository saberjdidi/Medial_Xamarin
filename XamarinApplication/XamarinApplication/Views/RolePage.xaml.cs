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
    public partial class RolePage : ContentPage
    {
        public RolePage()
        {
            InitializeComponent();
            BindingContext = new RoleViewModel();
        }
        private async void Add_Role(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewRolePage());
        }
        private async void Update_Role(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var role = mi.CommandParameter as RoleUser;
            //TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            //RoleUser role = ((RoleViewModel)BindingContext).Roles.Where(ser => ser.id == (int)tappedEventArgs.Parameter).FirstOrDefault();
            await PopupNavigation.Instance.PushAsync(new UpdateRolePage(role));
        }
       
    }
}