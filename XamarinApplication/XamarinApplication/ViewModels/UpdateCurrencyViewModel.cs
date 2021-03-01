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
   public class UpdateCurrencyViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Currency _currency;
        #endregion

        #region Constructors
        public UpdateCurrencyViewModel()
        {

        }
        #endregion

        #region Properties
        public Currency Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
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
        public async void EditCurrency()
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

            if (string.IsNullOrEmpty(Currency.entity) || string.IsNullOrEmpty(Currency.currency) ||
                string.IsNullOrEmpty(Currency.alphabeticCode) || string.IsNullOrEmpty(Currency.numericCode) || string.IsNullOrEmpty(Currency.minorUnit))
            {
                Value = true;
                return;
            }

            var category = new Currency
            {
                id = Currency.id,
                entity = Currency.entity,
                currency = Currency.currency,
                alphabeticCode = Currency.alphabeticCode,
                numericCode = Currency.numericCode,
                minorUnit = Currency.minorUnit
            };
            var response = await apiService.Put<Currency>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/currency",
                  category);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            CurruncyViewModel.GetInstance().Update(Currency);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Currency Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateCurrency
        {
            get
            {
                return new Command(() =>
                {
                    EditCurrency();
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
