using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
    public class UpdateClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Client client;
        private ObservableCollection<Events> events;
        private ObservableCollection<Reference> references;
        private List<Events> eventsList;
        private List<Reference> referencesList;
        private bool isRefreshing;
        bool _isVisibleStatus;
        #endregion

        #region Constructors
        public UpdateClientViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            instance = this;
            GetEvents();
            GetReferences();

            ListCountryAutoComplete();
            ListCategoryAutoComplete();
            ListRegionAutoComplete();
            ListAgentAutoComplete();
            ListGroupeAutoComplete();
            ListProvinceAutoComplete();
        }
        #endregion

        #region Properties
        public Client Client
        {
            get { return client; }
            set
            {
                client = value;
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
        public ObservableCollection<Events> Events
        {
            get { return events; }
            set
            {
                events = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Reference> References
        {
            get { return references; }
            set
            {
                references = value;
                OnPropertyChanged();
            }
        }
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public bool IsVisibleStatus
        {
            get { return _isVisibleStatus; }
            set
            {
                _isVisibleStatus = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Sigleton Events
        static UpdateClientViewModel instance;
        public static UpdateClientViewModel GetInstance()
        {
            if (instance == null)
            {
                //return new UpdateClientViewModel(INavigation _navigation);
            }

            return instance;
        }

        public void Update(Events events)
        {
            IsRefreshing = true;
            var oldevents = eventsList
                .Where(p => p.id == events.id)
                .FirstOrDefault();
            oldevents = events;
            Events = new ObservableCollection<Events>(eventsList);
            IsRefreshing = false;
        }
        public async Task DeleteEvent(Events events)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }
            var model = new Events
            {
                id = events.id,
                title = events.title,
                client = events.client,
                colorPrimary = events.colorPrimary,
                startDate = events.startDate,
                endDate = events.endDate,
                status = events.status,
                type = events.type,
                note = events.note,
                createdBy = events.createdBy
            };
            var httpClient = new HttpClient();
            var jsonRequest = JsonConvert.SerializeObject(model);
            Debug.WriteLine("********jsonRequest*************");
            Debug.WriteLine(jsonRequest);
            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json"),
                Method = HttpMethod.Delete,
                RequestUri = new Uri("https://app.smart-path.it/md-core/medial/client/" + events.client.id + "/admin/events")
            };
            var response = await httpClient.SendAsync(request);
            Debug.WriteLine("********response*************");
            Debug.WriteLine(response);
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("********result*************");
            Debug.WriteLine(result);

           /* var response = await apiService.DeleteEvent<Events>(
                  "https://app.smart-path.it",
                  "/md-core",
                  "/medial/client",
                  events.client.id,
                  "/admin/events",
                  model);

             if (!response.IsSuccess)
             {
                 IsRefreshing = false;
                 await dialogService.ShowMessage(
                     "Error",
                     response.Message);
                 return;
             }*/

            eventsList.Remove(events);
            Events = new ObservableCollection<Events>(eventsList);

            IsRefreshing = false;
        }
        #endregion

        #region Sigleton References
        public async Task DeleteReference(Reference reference) 
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }
            var model1 = new Reference
            {
                id = reference.id,
                firstName = reference.firstName,
                lastName = reference.lastName,
                email = reference.email,
                phoneNumber = reference.phoneNumber,
                role = reference.role,
                note = reference.note,
                client = reference.client
            };
             var httpClient = new HttpClient();
             var jsonRequest = JsonConvert.SerializeObject(model1);
             Debug.WriteLine("********jsonRequest*************");
             Debug.WriteLine(jsonRequest);
             HttpRequestMessage request = new HttpRequestMessage
             {
                 Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json"),
                 Method = HttpMethod.Delete,
                 RequestUri = new Uri("https://app.smart-path.it/md-core/medial/client/"+ reference.client.id + "/references")
             };
            var response = await httpClient.SendAsync(request);
             Debug.WriteLine("********response*************");
             Debug.WriteLine(response);
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("********result*************");
            Debug.WriteLine(result);

            referencesList.Remove(reference);
            References = new ObservableCollection<Reference>(referencesList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void EditClient()
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
            if (string.IsNullOrEmpty(Client.code) || string.IsNullOrEmpty(Client.description) || string.IsNullOrEmpty(Client.commercialDetails.headQuarter))
            {
                Value = true;
                return;
            }
            if (Client.commercialDetails.agent == null || Client.commercialDetails.region == null)
            {
                Value = true;
                return;
            }
            var commercial = new CommercialDetails
            {
                id = Client.commercialDetails.id,
                agent = Client.commercialDetails.agent,
                region = Client.commercialDetails.region,
                representant = Client.commercialDetails.representant,
                paymentCondition = Client.commercialDetails.paymentCondition,
                headQuarter = Client.commercialDetails.headQuarter
            };
            var client = new Client
            {
                id = Client.id,
                code = Client.code,
                description = Client.description,
                district = Client.district,
                address = Client.address,
                email = Client.email,
                phoneNumber = Client.phoneNumber,
                fiscalCode = Client.fiscalCode,
                iva = Client.iva,
                note = Client.note,
                commercialDetails = commercial,
                country = Client.country,
                category = Client.category,
                groupe = Client.groupe,
                province = Client.province
            };
            var response = await apiService.Put<Client>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client",
                  client);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            ClientsViewModel.GetInstance().Update(client);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Client Updated");
            //await App.Current.MainPage.Navigation.PopPopupAsync(true);
            await Navigation.PopAsync();
        }
        public async void GetEvents()
        {
            //IsRefreshing = true;
            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var response = await apiService.GetEvents<Events>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client",
                   Client.id,
                 "/events");
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            eventsList = (List<Events>)response.Result;
            Events = new ObservableCollection<Events>(eventsList);
            
            if (Events.Count() == 0)
            {
                IsVisibleStatus = true;
            }
            else
            {
                IsVisibleStatus = false;
            }
            IsRefreshing = false;
        }
        public async void GetReferences()
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var response = await apiService.GetEvents<Reference>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client",
                   Client.id,
                 "/references");
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            referencesList = (List<Reference>)response.Result;
            References = new ObservableCollection<Reference>(referencesList);

            if (References.Count() == 0)
            {
                IsVisibleStatus = true;
            }
            else
            {
                IsVisibleStatus = false;
            }
            IsRefreshing = false;
        }
        
        #endregion

        #region Commands
        public ICommand UpdateClient
        {
            get
            {
                return new Command(() =>
                {
                    EditClient();
                });
            }
        }
        public Command ClosePopup
        {
            get
            {
                return new Command(() =>
                {
                    //Navigation.PopPopupAsync();
                    Navigation.PopAsync();
                    Debug.WriteLine("********Close*************");
                });
            }
        }
        public ICommand RefreshCommandEvent
        {
            get
            {
                return new RelayCommand(GetEvents);
            }
        }
        public ICommand RefreshCommandReferences
        {
            get
            {
                return new RelayCommand(GetReferences);
            }
        }
        public Command AddEvent
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new NewEventPage(Client));
                });
            }
        }
        public Command AddReference
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new NewReferencePage(Client));
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
