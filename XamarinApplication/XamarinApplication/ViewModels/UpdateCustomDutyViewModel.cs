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
    class UpdateCustomDutyViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private CustomsDuty _customDuty;
        #endregion

        #region Constructors
        public UpdateCustomDutyViewModel()
        {

        }
        #endregion

        #region Properties
        public CustomsDuty CustomsDuty
        {
            get { return _customDuty; }
            set
            {
                _customDuty = value;
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
        public async void EditCustomsDuty()
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

            if (string.IsNullOrEmpty(CustomsDuty.code) || string.IsNullOrEmpty(CustomsDuty.description) || string.IsNullOrEmpty(CustomsDuty.taxRate))
            {
                Value = true;
                return;
            }

            var customsDuty = new CustomsDuty
            {
                id = CustomsDuty.id,
                code = CustomsDuty.code,
                description = CustomsDuty.description,
                taxRate = CustomsDuty.taxRate
            };
            var response = await apiService.Put<CustomsDuty>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/config",
                  customsDuty);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            CustomDutyViewModel.GetInstance().Update(customsDuty);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Custom Duty Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateCustomsDuty
        {
            get
            {
                return new Command(() =>
                {
                    EditCustomsDuty();
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
