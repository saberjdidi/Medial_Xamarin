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
    public class UpdateContainerViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Containner _container;
        #endregion

        #region Constructors
        public UpdateContainerViewModel()
        {
            ListCurrencyAutoComplete();
        }
        #endregion

        #region Properties
        public Containner Container
        {
            get { return _container; }
            set
            {
                _container = value;
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
        public async void EditContainer()
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

            if (string.IsNullOrEmpty(Container.description) || string.IsNullOrEmpty(Container.value.ToString()))
            {
                Value = true;
                return;
            }
            var _cost = new Cost
            {
                id = Container.seaFreight.id,
                currency = Container.seaFreight.currency,
                value = Container.seaFreight.value
            };
            var container = new Containner
            {
                id = Container.id,
                description = Container.description,
                value = Container.value,
                seaFreight = _cost
            };
            var response = await apiService.Put<Containner>(
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
            ContainerViewModel.GetInstance().Update(container);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Container Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateContainer
        {
            get
            {
                return new Command(() =>
                {
                    EditContainer();
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
