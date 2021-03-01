using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class NewSupplierViewModel : INotifyPropertyChanged
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        bool _isRunning;
        bool _isEnabled;
        #endregion

        #region Constructor
        public NewSupplierViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();
            IsEnabled = true;
            IsRunning = true;

            ListContainerAutoComplete();
            ListCountryAutoComplete();
            ListCustomAutoComplete();
        }
        #endregion

        #region Properties
        public string Code { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        private Country country = null;
        public Country Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged();
            }
        }
        private Containner containner = null;
        public Containner Containner
        {
            get { return containner; }
            set
            {
                containner = value;
                OnPropertyChanged();
            }
        }
        private CustomsDuty customsDuty = null;
        public CustomsDuty CustomsDuty
        {
            get { return customsDuty; }
            set
            {
                customsDuty = value;
                OnPropertyChanged();
            }
        }
        private bool european = false;
        public bool European
        {
            get { return european; }
            set
            {
                european = value;
                OnPropertyChanged();
            }
        }
        private bool exporter = false;
        public bool Exporter
        {
            get { return exporter; }
            set
            {
                exporter = value;
                OnPropertyChanged();
            }
        }
        private bool dutyFree = false;
        public bool DutyFree
        {
            get { return dutyFree; }
            set
            {
                dutyFree = value;
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
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }
        #endregion

        #region Methods
        public async void AddSupplier()
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
            if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Description))
            {
                //IsVisible = true;
                Value = true;
                //IsEnabled = true;
                //await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.UsernameValidation, Languages.Ok);
                return;
            }
            if (Containner == null || CustomsDuty == null)
            {
                Value = true;
                return;
            }

            IsRunning = false;
            IsEnabled = false;

            var supplier = new AddSupplier
            {
                code = Code,
                description = Description,
                country = Country,
                container = Containner,
                customsDuty = CustomsDuty,
                note = Note,
                european = European,
                exporter = Exporter,
                dutyFree = DutyFree
            };
            var response = await apiService.Save<AddSupplier>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/supplier",
                  supplier);
            Debug.WriteLine("********responseIn ViewModel*************");
            Debug.WriteLine(response);
            if (!response.IsSuccess)
            {
                IsRunning = true;
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            IsRunning = true;
            IsEnabled = true;
            MessagingCenter.Send((App)Application.Current, "OnSaved");
            DependencyService.Get<INotification>().CreateNotification("Medial", "Supplier Added");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand SaveSupplier
        {
            get
            {
                return new Command(() =>
                {
                    AddSupplier();
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
