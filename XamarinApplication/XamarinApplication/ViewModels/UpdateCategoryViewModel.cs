using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class UpdateCategoryViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Category _category;
        #endregion

        #region Constructors
        public UpdateCategoryViewModel()
        {

        }
        #endregion

        #region Properties
        public Category Category
        {
            get { return _category; }
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }
        private bool value = false;
        public bool Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Methods
        public async void EditCategory()
        {
            Value = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Warning,
                    Languages.CheckConnection,
                    Languages.Ok);
                return;
            }

            if (string.IsNullOrEmpty(Category.code) || string.IsNullOrEmpty(Category.description))
            {
                Value = true;
                return;
            }

            var category = new Category
            {
                id = Category.id,
                code = Category.code,
                description = Category.description
            };
            var response = await apiService.Put<Category>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/category",
                  category);
            if (!response.IsSuccess)
             {
                 await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                 return;
             }
            Value = false;
            CategorieViewModel.GetInstance().Update(category);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Category Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateCategory
        {
            get
            {
                return new Command(() =>
                {
                    EditCategory();
                });
            }
        }
        public Command ClosePopup
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PopPopupAsync();
                    Debug.WriteLine("********Close*************");
                });
            }
        }
        #endregion
    }
}
