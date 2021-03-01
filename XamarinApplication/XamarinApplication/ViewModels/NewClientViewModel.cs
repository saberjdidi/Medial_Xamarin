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
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class NewClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public NewClientViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();

            ListCountryAutoComplete();
            ListCategoryAutoComplete();
            ListRegionAutoComplete();
            ListAgentAutoComplete();
            ListGroupeAutoComplete();
            ListProvinceAutoComplete();
        }
        #endregion

        #region Properties
        public string Code { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FiscalCode { get; set; }
        public string IVA { get; set; }
        public string Note { get; set; }
        public string HeadQuarter { get; set; }
        public string Representant { get; set; }
        public string PaymentCondition { get; set; }
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
        private Category category = null;
        public Category Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged();
            }
        }
        private Reggion region = null;
        public Reggion Reggion
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged();
            }
        }
        private User agent = null;
        public User Agent
        {
            get { return agent; }
            set
            {
                agent = value;
                OnPropertyChanged();
            }
        }
        private Groupe groupe = null;
        public Groupe Groupe
        {
            get { return groupe; }
            set
            {
                groupe = value;
                OnPropertyChanged();
            }
        }
        private Province province = null;
        public Province Province
        {
            get { return province; }
            set
            {
                province = value;
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
        public async void AddClient()
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
            if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(HeadQuarter))
            {
                Value = true;
                return;
            }
            if (Agent == null || Reggion == null)
            {
                Value = true;
                return;
            }
            var commercial = new AddCommercialDetails
            {
                agent = Agent,
                region = Reggion,
                representant = Representant,
                paymentCondition = PaymentCondition,
                headQuarter = HeadQuarter
            };
            var client = new AddClient
            {
                code = Code,
                description = Description,
                district = City,
                address = Address,
                email = Email,
                phoneNumber = PhoneNumber,
                fiscalCode = FiscalCode,
                iva = IVA,
                note = Note,
                commercialDetails = commercial,
                country = Country,
                category = Category,
                groupe = Groupe,
                province = Province
            };
            var response = await apiService.Save<AddClient>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client",
                  client);
            Debug.WriteLine("********responseIn ViewModel*************");
            Debug.WriteLine(response);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            MessagingCenter.Send((App)Application.Current, "OnSaved");
            DependencyService.Get<INotification>().CreateNotification("Medial", "Client Added");
            //await Navigation.PopModalAsync(); //use for Popup
            await Navigation.PopAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveClient
        {
            get
            {
                return new Command(() =>
                {
                    AddClient();
                });
            }
        }
        public Command ClosePopup
        {
            get
            {
                return new Command(() =>
                {
                    // Navigation.PopPopupAsync();
                    //App.Current.MainPage.Navigation.PopPopupAsync(true);
                    Navigation.PopAsync();
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
        //Category
        private List<Category> _categoryAutoComplete;
        public List<Category> CategoryAutoComplete
        {
            get { return _categoryAutoComplete; }
            set
            {
                _categoryAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Category>> ListCategoryAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<Category>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/category/search?sortedBy=code&order=asc",
                  _searchRequest);
            CategoryAutoComplete = (List<Category>)response.Result;
            return CategoryAutoComplete;
        }
        //Region
        private List<Reggion> _regionAutoComplete;
        public List<Reggion> RegionAutoComplete
        {
            get { return _regionAutoComplete; }
            set
            {
                _regionAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Reggion>> ListRegionAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<Reggion>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/region/search?sortedBy=code&order=asc",
                  _searchRequest);
            RegionAutoComplete = (List<Reggion>)response.Result;
            return RegionAutoComplete;
        }
        //Agent
        private List<User> _agentAutoComplete;
        public List<User> AgentAutoComplete
        {
            get { return _agentAutoComplete; }
            set
            {
                _agentAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<User>> ListAgentAutoComplete()
        {
            var response = await apiService.GetList<User>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/user/agents");
            AgentAutoComplete = (List<User>)response.Result;
            return AgentAutoComplete;
        }
        //Groupe
        private List<Groupe> _groupeAutoComplete;
        public List<Groupe> GroupeAutoComplete
        {
            get { return _groupeAutoComplete; }
            set
            {
                _groupeAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Groupe>> ListGroupeAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<Groupe>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client_groupe/search?sortedBy=code&order=asc",
                  _searchRequest);
            GroupeAutoComplete = (List<Groupe>)response.Result;
            return GroupeAutoComplete;
        }
        //Province
        private List<Province> _provinceAutoComplete;
        public List<Province> ProvinceAutoComplete
        {
            get { return _provinceAutoComplete; }
            set
            {
                _provinceAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Province>> ListProvinceAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<Province>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/province/search?sortedBy=code&order=asc",
                  _searchRequest);
            ProvinceAutoComplete = (List<Province>)response.Result;
            return ProvinceAutoComplete;
        }
        #endregion
    }
}
