using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class NewContainerViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public NewContainerViewModel()
        {
            apiService = new ApiServices();
            ListCurrencyAutoComplete();
        }
        #endregion

        #region Properties
        public decimal Valuem3 { get; set; }
        public decimal ValueSeaFreight { get; set; }
        public string Description { get; set; }
        private Currency _currency = null;
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
        public async void AddContainer()
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
            if (string.IsNullOrEmpty(Description))
            {
                Value = true;
                return;
            }
            if(Valuem3 == 0 || ValueSeaFreight == 0)
            {
                Value = true;
                return;
            }
            if(Currency == null)
            {
                Value = true;
                return;
            }
            var _seaFreight = new SeaFreight
            {
                value = ValueSeaFreight,
                currency = Currency
            };
            var container = new AddContainer
            {
                description = Description,
                value = Valuem3,
                seaFreight = _seaFreight
            };
            var response = await apiService.Save<AddContainer>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/container",
                  container);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            MessagingCenter.Send((App)Application.Current, "OnSaved");
            DependencyService.Get<INotification>().CreateNotification("Medial", "Container Added");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand SaveContainer
        {
            get
            {
                return new Command(() =>
                {
                    AddContainer();
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
                    Debug.WriteLine("********Close*************");
                });
            }
        }
        #endregion

        #region Autocomplete
        //Currency
        private List<Currency> _currencyAutoComplete;
        public List<Currency> CurrencyAutoComplete
        {
            get { return _currencyAutoComplete; }
            set
            {
                _currencyAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Currency>> ListCurrencyAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<Currency>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/currency/search?sortedBy=entity&order=asc",
                  _searchRequest);
            CurrencyAutoComplete = (List<Currency>)response.Result;
            return CurrencyAutoComplete;
        }
        #endregion
    }
}
