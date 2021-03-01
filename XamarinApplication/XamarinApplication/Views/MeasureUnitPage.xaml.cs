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
    public partial class MeasureUnitPage : ContentPage
    {
        public MeasureUnitPage()
        {
            InitializeComponent();
            BindingContext = new MeasureUnitViewModel();
        }
        private async void Add_MeasureUnit(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewMeasureUnitPage());
        }
        private async void Update_MeasureUnit(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var measureUnit = mi.CommandParameter as MeasureUnit;
            await PopupNavigation.Instance.PushAsync(new UpdateMeasureUnitPage(measureUnit));
        }
    }
}