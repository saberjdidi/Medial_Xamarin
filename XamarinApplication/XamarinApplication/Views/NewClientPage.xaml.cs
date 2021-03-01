using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewClientPage : TabbedPage
    {
        public NewClientPage()
        {
            InitializeComponent();
            BindingContext = new NewClientViewModel(Navigation);

            NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
        }
    }
}