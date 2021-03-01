using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
   public class ClientsAgentViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
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
        private User _user = null;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }
        public List<User> Users { get; set; }
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
        public ClientsAgentViewModel()
        {
            apiService = new ApiServices();
            GetClients();
            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetClients();
            });
        }
        #endregion

        #region Methods
        public async void GetClients()
        {
            IsRefreshing = true;
            var Username = Settings.Username;
            User = JsonConvert.DeserializeObject<User>(Username);
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
            var response = await apiService.Post<Client>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client/search?sortedBy=code&order=asc&maxResult=60&agent="+ User.username,
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
                 "/medial/client/search?sortedBy=code&order=asc&maxResult=10&offset=" + _offset+ "&agent=" + User.username,
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
        public Command DownloadClientReport
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                    var _searchRequest = new SearchRequest
                    {
                        order = "asc",
                        sortedBy = "code"
                    };

                    var requestJson = JsonConvert.SerializeObject(_searchRequest);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(requestJson);
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/client/generate/list/client?agent="+User.username;
                    Debug.WriteLine("********url*************");
                    Debug.WriteLine(url);
                    var response = await httpClient.PostAsync(url, content);
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("********result*************");
                    Debug.WriteLine(result);
                    if (!response.IsSuccessStatusCode)
                    {
                        IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                        return;
                    }
                    IsRefreshing = false;
                    var pdf = JsonConvert.DeserializeObject<PdfClient>(result);

                    byte[] bytes = Convert.FromBase64String(pdf.report);
                    MemoryStream stream = new MemoryStream(bytes);

                    if (stream == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Data is Empty", "ok");
                        return;
                    }

                    await DependencyService.Get<ISave>().SaveAndView("clients-"+ dateNow + ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", stream);
                });
            }
        }
        public Command DownloadPdf
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                    var _agents = new List<User> {
                        User
                    };
                    var _search = new SearchClientAgent
                    {
                        agentUserName = User.username,
                        agents = _agents
                    };

                    var requestJson = JsonConvert.SerializeObject(_search);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(requestJson);
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/client/eventsReport?format=pdf";
                    Debug.WriteLine("********url*************");
                    Debug.WriteLine(url);
                    var response = await httpClient.PostAsync(url, content);
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("********result*************");
                    Debug.WriteLine(result);
                    if (!response.IsSuccessStatusCode)
                    {
                        IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                        return;
                    }
                    IsRefreshing = false;
                    var pdf = JsonConvert.DeserializeObject<PdfClient>(result);

                    byte[] bytes = Convert.FromBase64String(pdf.report);
                    MemoryStream stream = new MemoryStream(bytes);

                    if (stream == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Data is Empty", "ok");
                        return;
                    }

                    await DependencyService.Get<ISave>().SaveAndView(pdf.name, pdf.defaultExtention, stream);
                });
            }
        }
        #endregion
    }
    #region Model
    public class SearchClientAgent
    {
        public string agentUserName { get; set; }
        public List<User> agents { get; set; }
    }
    #endregion
}
