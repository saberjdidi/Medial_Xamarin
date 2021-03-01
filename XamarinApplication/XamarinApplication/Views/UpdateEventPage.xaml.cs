using Rg.Plugins.Popup.Pages;
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
    public partial class UpdateEventPage : PopupPage
    {
        public UpdateEventPage(Events events)
        {
            InitializeComponent();
            var ViewModel = new UpdateEventViewModel();
            ViewModel.Events = events;
            BindingContext = ViewModel;
        }
    }
}