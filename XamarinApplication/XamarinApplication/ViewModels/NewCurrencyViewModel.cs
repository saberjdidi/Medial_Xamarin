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
    public class NewCurrencyViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public NewCurrencyViewModel()
        {
            apiService = new ApiServices();
        }
        #endregion

        #region Properties
        public string Entity { get; set; }
        public string Currency { get; set; }
        public string AlphabeticCode { get; set; }
        public string NumericCode { get; set; }
        public string MinorUnit { get; set; }
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
        public async void AddCurrency()
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
            if (string.IsNullOrEmpty(Entity) || string.IsNullOrEmpty(Currency)||
                string.IsNullOrEmpty(AlphabeticCode) || string.IsNullOrEmpty(NumericCode) || string.IsNullOrEmpty(MinorUnit))
            {
                Value = true;
                return;
            }

            var currency = new AddCurrency
            {
                entity = Entity,
                currency = Currency,
                alphabeticCode = AlphabeticCode,
                numericCode = NumericCode,
                minorUnit = MinorUnit
            };
            var response = await apiService.Save<AddCurrency>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/currency",
                  currency);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            MessagingCenter.Send((App)Application.Current, "OnSaved");
            DependencyService.Get<INotification>().CreateNotification("Medial", "Currency Added");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand SaveCurrency
        {
            get
            {
                return new Command(() =>
                {
                    AddCurrency();
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
                    //App.Current.MainPage.Navigation.PopPopupAsync(true);
                });
            }
        }
        #endregion
    }
}
