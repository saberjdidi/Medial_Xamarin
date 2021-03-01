using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class ClientsViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private InfiniteScrollCollection<Client> clients;
        private bool isRefreshing;
        private bool isVisible;
        private string filter;
        private List<Client> clientsList;
        private bool _isBusy;
        private const int _maxResult = 8;
        int _offset = 0;
        public int TotalCount { get; private set; }
        #endregion

        #region Properties
        private ObservableCollection<Client> clientsCollection;
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

        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
                //Search();
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

        #region Constructors
        public ClientsViewModel()
        {
            apiService = new ApiServices();
            instance = this;
            GetClients();
            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetClients();
            });
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
                            description = ""
                        };

                        var response = await apiService.SearchRequest<Client>(
                        "https://app.smart-path.it",
                        "/md-core",
                        "/medial/client/search?sortedBy=code&order=asc&maxResult=" + _maxResult,
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

                        //Clients.AddRange(clientsList);
                        // productsList = (List<Product>)response.Result;

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
                // clear and start again
                Clients.Clear();
                Clients.LoadMoreAsync();
            });
            */
        }
        #endregion

        #region Sigleton
        static ClientsViewModel instance;
        public static ClientsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ClientsViewModel();
            }

            return instance;
        }

        public void Update(Client client)
        {
            IsRefreshing = true;
            var oldClient = clientsList
                .Where(p => p.id == client.id)
                .FirstOrDefault();
            oldClient = client;
            ClientsCollection = new ObservableCollection<Client>(clientsList);
            IsRefreshing = false;
        }
        public async Task Delete(Client client)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Client>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/client",
                client.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            clientsList.Remove(client);
            ClientsCollection = new ObservableCollection<Client>(clientsList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetClients()
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
            var _searchRequest = new SearchRequest
            {
                order = "asc",
                sortedBy = "code"
            };
            /*var response = await apiService.SearchRequest<Client>(
            "https://app.smart-path.it",
            "/md-core",
            "/medial/client/search?sortedBy=code&order=asc&maxResult=" + 10,
            "&offset=" + 0,
            _searchRequest);*/
            var response = await apiService.Post<Client>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client/search?sortedBy=code&order=asc&maxResult=60",
                  _searchRequest);
            Debug.WriteLine("********responseIn ViewModel*************");
            Debug.WriteLine(response);
            if (!response.IsSuccess)
            {
                //IsVisible = true;
                IsRefreshing = true;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            clientsList = (List<Client>)response.Result;

            //Clients.AddRange(clientsList);
            ClientsCollection = new ObservableCollection<Client>(clientsList);
            IsVisible = false;
            IsRefreshing = false;
        }

        public async void LoadMoreItems(Client currentItem)
        {
            int itemIndex = ClientsCollection.IndexOf(currentItem);

            _offset = ClientsCollection.Count;

            if (ClientsCollection.Count - 50 == itemIndex)
            {
                IsBusy = true;
                IsRefreshing = true;
                var _searchRequest = new SearchRequest
                {
                    order = "asc",
                    sortedBy = "code"
                };
                var response = await apiService.LoadMoreData<Client>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client/search?sortedBy=code&order=asc&maxResult=10&offset=" + _offset,
                  _searchRequest);
                if (!response.IsSuccess)
                {
                    //IsVisible = true;
                    IsRefreshing = true;
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                    return;
                }
                clientsList = (List<Client>)response.Result;
                foreach (Client item in clientsList)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsBusy = false;
                        IsRefreshing = false;
                        ClientsCollection.Add(item);
                    });
                }
            }
        }
        #endregion
        #region Commands
        public ICommand SearchPopup
        {
             get
             {
                 return new Command(async () =>
                 {

                     MessagingCenter.Subscribe<DialogResultClient>(this, "PopUpDataClient", (value) =>
                     {
                         //Clients = value.ClientsPopup;
                         ClientsCollection = value.ClientsPopup;
                         IsBusy = false;
                         IsRefreshing = false;
                         if (ClientsCollection.Count() == 0)
                         {
                             IsVisible = true;
                         }
                         else
                         {
                             IsVisible = false;
                         }
                     });
                     await PopupNavigation.Instance.PushAsync(new SearchClientPage());
                 });
             }
         }
        public ICommand RefreshCommand
        {
            //get;
             get
             {
                 return new RelayCommand(GetClients);
             }
        }
        #endregion
        //Author method InfiniteScroll 
        /*
        #region Constructor
            private bool _showLoadingText;
            public bool ShowLoadingText
        {
            get { return _showLoadingText; }
            set
            {
                if (_showLoadingText == value)
                    return;

                _showLoadingText = value;
                OnPropertyChanged(nameof(ShowLoadingText));
            }
        }
        public ClientsViewModel(INavigation _navigation)
        {
            apiService = new ApiServices();
            Navigation = _navigation;
            Task.Run(async () =>
            {
                Clients = new InfiniteScrollCollection<Client>
                {
                    OnLoadMore = async () =>
                    {
                        var page = Clients.Count / PageSize;
                        var items = await Get(_maxResult, page);
                        return items;
                    },
                    OnCanLoadMore = () =>
                    {
                        ShowLoadingText = (Clients.Count < TotalCount);
                        return ShowLoadingText;
                    },
                };
                await Clients.LoadMoreAsync();
            });
        }
        #endregion
        #region methods
        public async Task<IEnumerable<Client>> Get(int maxResult, int page)
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            //(int count, clientsList) = await apiService.SearchClient(searchTxt, page, _pageSize);
            (int count, var response) = await apiService.SearchClient<Client>(
                        "https://app.smart-path.it",
                        "/md-core",
                        "/medial/client/search?sortedBy=code&order=asc&maxResult=" + _maxResult,
                        page,
                        PageSize,
                        _searchRequest);
            TotalCount = count;
            return response;
        }

        //-------------------------------------------------------------//
         public async void GetNextClients()
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
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.SearchRequest<Client>(
            "https://app.smart-path.it",
            "/md-core",
            "/medial/client/search?sortedBy=code&order=asc&maxResult=10",
            "&offset=" + 8,
            _searchRequest);
            Debug.WriteLine("********responseIn ViewModel*************");
            Debug.WriteLine(response);
            if (!response.IsSuccess)
            {
                //IsVisible = true;
                IsRefreshing = true;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            clientsList = (List<Client>)response.Result;

            //Clients.AddRange(clientsList);
            ClientsCollection = new ObservableCollection<Client>(clientsList);
            IsVisible = false;
            IsRefreshing = false;
        }
        public ICommand NextPage
        {
            get
            {
                return new Command(() =>
                {
                    GetNextClients();
                });
            }
        }
        #endregion
        */
    }
}
