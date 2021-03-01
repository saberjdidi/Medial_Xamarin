using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class UpdatePackagingMethodViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private PackagingMethod _packagingMethod;
        #endregion

        #region Constructors
        public UpdatePackagingMethodViewModel()
        {

        }
        #endregion

        #region Properties
        public PackagingMethod PackagingMethod
        {
            get { return _packagingMethod; }
            set
            {
                _packagingMethod = value;
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
        public async void EditPackagingMethod()
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

            if (string.IsNullOrEmpty(PackagingMethod.code) || string.IsNullOrEmpty(PackagingMethod.description))
            {
                Value = true;
                return;
            }

            var packagingMethod = new PackagingMethod
            {
                id = PackagingMethod.id,
                code = PackagingMethod.code,
                description = PackagingMethod.description
            };
            var response = await apiService.Put<PackagingMethod>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/packagingMethod",
                  packagingMethod);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            PackagingMethodViewModel.GetInstance().Update(packagingMethod);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Packaging Method Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdatePackagingMethod
        {
            get
            {
                return new Command(() =>
                {
                    EditPackagingMethod();
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
                });
            }
        }
        #endregion
    }
}
