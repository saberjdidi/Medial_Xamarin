using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using XamarinApplication.EntryAutoComplete;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class SearchClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        private InfiniteScrollCollection<Client> clients;
        private ObservableCollection<Client> clientsCollection;
        private bool isRefreshing;
        private bool isVisible;
        private List<Client> clientsList;
        private bool _isBusy;
        private const int _maxResult = 100;
        public EventHandler<DialogResultClient> OnDialogClosed;
        private string _description = "";
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
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
        #endregion

        #region Constructors
        public SearchClientViewModel()
        {
            //SearchClients();
            apiService = new ApiServices();
          /*
            Task.Run(async () =>
            {
                Clients = new InfiniteScrollCollection<Client>
                {
                    OnLoadMore = async () =>
                    {
                        IsBusy = true;
                        IsRefreshing = true;
                        // load the next page
                        var page = Clients.Count / _maxResult;


                        var _searchRequest = new SearchRequest
                        {
                            code = "",
                            description = Description
                            //description = SearchClient
                        };

                        var response = await apiService.SearchRequest<Client>(
                        "https://app.smart-path.it",
                        "/md-core",
                        "/medial/client/search?sortedBy=code&order=asc&maxResult=" + 0,
                        "&offset=" + page,
                        _searchRequest);
                        Debug.WriteLine("********response In ViewModel*************");
                        Debug.WriteLine(response);
                        if (!response.IsSuccess)
                        {
                            //IsVisible = true;
                            IsRefreshing = true;
                            await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                        }
                        clientsList = (List<Client>)response.Result;

                        Clients.AddRange(clientsList);
                        // productsList = (List<Product>)response.Result;
                        //MessagingCenter.Send(new DialogResultClient() { ClientsPopup = Clients }, "PopUpDataClient");
                        //await App.Current.MainPage.Navigation.PopPopupAsync(true);
                        IsBusy = false;
                        IsRefreshing = false;
                        return clientsList;
                    },
                    OnCanLoadMore = () =>
                    {
                        //return Products.Count < _maxResult * PageSize;
                        return Clients.Count < 10000;
                        // return Requests.Count < TotalCount;
                    }
                };
                await Clients.LoadMoreAsync();

            });
            RefreshCommand = new Command(() =>
            {
                Clients.Clear();
                Clients.LoadMoreAsync();
            });
            */
            /*SearchCommand = new Command(() =>
            {
                // clear and start again
                //Clients.Clear();
                Clients.LoadMoreAsync();
            });
          */

            ListClientsAutoComplete();
            ListCategoryAutoComplete();
        }
        #endregion

        #region Properties
        public ObservableCollection<Client> ClientsCollection
        {
            get { return clientsCollection; }
            set
            {
                clientsCollection = value;
                OnPropertyChanged();
            }
        }
        public InfiniteScrollCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
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
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                this.isVisible = value;
                //this.RaisePropertyChanged("IsVisible");
                OnPropertyChanged();
            }
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Methods
        public async void SearchClients()
        {
            IsRefreshing = true;
            //IsVisible = true;
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
            var _searchRequest = new SearchRequestByCategory
            {
                code = "",
                description = Description,
                category = Category
                //description = SearchClient
            };

           /* var response = await apiService.SearchRequest<Client>(
            "https://app.smart-path.it",
            "/md-core",
            "/medial/client/search?sortedBy=code&order=asc&maxResult=" + _maxResult,
            "&offset=" + 0,
            _searchRequest);*/
            var response = await apiService.SearchClientByCategory<Client>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client/search?sortedBy=code&order=asc&maxResult=100",
                  _searchRequest);
            clientsList = (List<Client>)response.Result;

            //Clients.AddRange(clientsList);
            ClientsCollection = new ObservableCollection<Client>(clientsList);

            MessagingCenter.Send(new DialogResultClient() { ClientsPopup = ClientsCollection }, "PopUpDataClient");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);

            IsVisible = false;
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    SearchClients();
                });
            }
        }
        public ICommand RefreshCommand
        {

            get
            {
                return new Command(() =>
                {
                    SearchClients();
                });
            }
        }
        #endregion

        #region Autocomplete
        //First Method with Syncfusion
        private List<Client> _clientAutoComplete;
        public List<Client> ClientAutoComplete
        {
            get { return _clientAutoComplete; }
            set
            {
                _clientAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Client>> ListClientsAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.AutocompleteClient<Client>(
                        "https://app.smart-path.it",
                        "/md-core",
                        "/medial/client/search?sortedBy=code&order=asc&maxResult=300",
                        _searchRequest);
            ClientAutoComplete = (List<Client>)response;
            return ClientAutoComplete;
        }
        //List of Category
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
            var response = await apiService.AutocompleteClient<Category>(
                        "https://app.smart-path.it",
                        "/md-core",
                        "/medial/category/search?sortedBy=code&order=asc",
                        _searchRequest);
            CategoryAutoComplete = (List<Category>)response;
            return CategoryAutoComplete;
        }
        //Second Method with EntryAutoComplete
        /*
        private string _searchCountry = string.Empty;
        private bool _customSearchFunctionSwitchIsToggled;
        private SearchMode _searchMode = SearchMode.Contains;

        public string SearchClient
        {
            get => _searchCountry;
            set
            {
                _searchCountry = value;
                OnPropertyChanged();
            }
        }
        public List<string> ClientsAutocomplete { get; } = new List<string>
        {
            "1000 COSE PER TUTTI di MARINO ITALO",
            "2D SNC  DI  LUIGGIA MARTINIA & CO (PULIR",
            "2M ITALIA S.R.L.",
            "2M FORNITURE MILITARI SRL",
            "2M ITALIA SRL",
            "2M SRL",
            "2P LINOLEUM SRL"
        };

        private ObservableCollection<Client> _clientAutocomplete;
        private List<Client> clientAutocompleteList;
        public ObservableCollection<Client> ClientAutocomplete
        {
            get { return _clientAutocomplete; }
            set
            {
                _clientAutocomplete = value;
                OnPropertyChanged();
            }
        }
        public async void ClientsAutocompleteList()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };

            var response = await apiService.Post<Client>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client/search?sortedBy=code&order=asc&maxResult=10",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            clientAutocompleteList = (List<Client>)response.Result;
            Debug.WriteLine("********clientAutocompleteList*************");
            Debug.WriteLine(clientAutocompleteList);
            ClientAutocomplete = new ObservableCollection<Client>(clientAutocompleteList);
        }

        public SearchMode SearchMode
        {
            get => _searchMode;
            set
            {
                _searchMode = value;
                OnPropertyChanged();
            }
        }
        public bool CustomSearchFunctionSwitchIsToggled
        {
            get => _customSearchFunctionSwitchIsToggled;
            set
            {
                _customSearchFunctionSwitchIsToggled = value;
                OnPropertyChanged();
                UpdateCustomSearchFunction();
            }
        }
        private void UpdateCustomSearchFunction()
        {
            SearchMode = CustomSearchFunctionSwitchIsToggled
                ? SearchMode.Using((text, obj) => obj.ToString().Length % 2 == 0 && obj.ToString().ToLower().StartsWith(text.ToLower()))
                : SearchMode.Contains;
        }*/
        #endregion
    }

    public class DialogResultClient
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ObservableCollection<Client> ClientsPopup { get; set; }
    }
}
