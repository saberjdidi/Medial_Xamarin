using Rg.Plugins.Popup.Pages;
using Syncfusion.SfAutoComplete.XForms;
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
    public partial class SearchProductPage : PopupPage
    {
        public SearchProductPage()
        {
            InitializeComponent();
            BindingContext = new SearchProductViewModel();

        }
    }
}