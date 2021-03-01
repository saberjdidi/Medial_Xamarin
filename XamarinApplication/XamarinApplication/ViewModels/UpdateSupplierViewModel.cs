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
    public class UpdateSupplierViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Supplier supplier;
        #endregion

        #region Constructors
        public UpdateSupplierViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            ListContainerAutoComplete();
            ListCountryAutoComplete();
            ListCustomAutoComplete();
        }
        #endregion

        #region Properties
        public Supplier Supplier
        {
            get { return supplier; }
            set
            {
                supplier = value;
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
        public async void EditSupplier()
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

            if (string.IsNullOrEmpty(Supplier.code) || string.IsNullOrEmpty(Supplier.description))
            {
                Value = true;
                return;
            }
            if (Supplier.container == null || Supplier.customsDuty == null)
            {
                Value = true;
                return;
            }

            var supplier = new Supplier
            {
                id = Supplier.id,
                code = Supplier.code,
                description = Supplier.description,
                country = Supplier.country,
                container = Supplier.container,
                customsDuty = Supplier.customsDuty,
                note = Supplier.note,
                european = Supplier.european,
                exporter = Supplier.exporter,
                dutyFree = Supplier.dutyFree
            };
            var response = await apiService.Put<Supplier>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/supplier",
                  supplier);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            SuppliersViewModel.GetInstance().Update(supplier);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Supplier Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateSupplier
        {
            get
            {
                return new Command(() =>
                {
                    EditSupplier();
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
        //Country
        private List<Country> _countryAutoComplete;
        public List<Country> CountryAutoComplete
        {
            get { return _countryAutoComplete; }
            set
            {
                _countryAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Country>> ListCountryAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<Country>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/country/search?sortedBy=name&order=asc",
                  _searchRequest);
            CountryAutoComplete = (List<Country>)response.Result;
            return CountryAutoComplete;
        }

        //Container
        private List<Containner> _containerAutoComplete;
        public List<Containner> ContainerAutoComplete
        {
            get { return _containerAutoComplete; }
            set
            {
                _containerAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Containner>> ListContainerAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<Containner>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/container/search?sortedBy=description&order=asc",
                  _searchRequest);
            ContainerAutoComplete = (List<Containner>)response.Result;
            return ContainerAutoComplete;
        }

        //Customs Duty
        private List<CustomsDuty> _customAutoComplete;
        public List<CustomsDuty> CustomAutoComplete
        {
            get { return _customAutoComplete; }
            set
            {
                _customAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<CustomsDuty>> ListCustomAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<CustomsDuty>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/config/search?sortedBy=code&order=asc",
                  _searchRequest);
            CustomAutoComplete = (List<CustomsDuty>)response.Result;
            return CustomAutoComplete;
        }
        #endregion
    }
}
