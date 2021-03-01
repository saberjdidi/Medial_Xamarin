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
    public partial class CategoryPage : ContentPage
    {
        public CategoryPage()
        {
            InitializeComponent();
            BindingContext = new CategorieViewModel();
        }
        private async void Add_Category(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewCategoryPage());
        }
        private async void Update_Category(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var category = mi.CommandParameter as Category;
            await PopupNavigation.Instance.PushAsync(new UpdateCategoryPage(category));
        }
    }
}