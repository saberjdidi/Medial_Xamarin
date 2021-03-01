using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class ContainerViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<Containner> containers;
        private bool isRefreshing;
        private string filter;
        private List<Containner> containersList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<Containner> Containners
        {
            get { return containers; }
            set
            {
                containers = value;
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

        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
                Search();
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
        public bool ShowHide
        {
            get => _showHide;
            set
            {
                _showHide = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public ContainerViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetContainer();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetContainer();
            });
        }
        #endregion

        #region Sigleton
        static ContainerViewModel instance;
        public static ContainerViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ContainerViewModel();
            }

            return instance;
        }

        public void Update(Containner containner)
        {
            IsRefreshing = true;
            var oldcontainer = containersList
                .Where(p => p.id == containner.id)
                .FirstOrDefault();
            oldcontainer = containner;
            Containners = new ObservableCollection<Containner>(containersList);
            IsRefreshing = false;
        }
        public async Task Delete(Containner containner)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Containner>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/container",
                containner.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            containersList.Remove(containner);
            Containners = new ObservableCollection<Containner>(containersList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetContainer()
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

            var _searchRequest = new SearchRequest
            {
                order = "asc",
                sortedBy = "description"
            };

            var response = await apiService.Post<Containner>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/container/search?sortedBy=description&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            containersList = (List<Containner>)response.Result;
            Containners = new ObservableCollection<Containner>(containersList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetContainer);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                ShowHide = false;
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                Containners = new ObservableCollection<Containner>(containersList);
                IsVisibleStatus = false;
            }
            else
            {
                Containners = new ObservableCollection<Containner>(
                      containersList.Where(
                          l => l.description.ToLower().StartsWith(Filter.ToLower())));

                if (Containners.Count() == 0)
                {
                    IsVisibleStatus = true;
                }
                else
                {
                    IsVisibleStatus = false;
                }
            }
        }

        public ICommand OpenSearchBar
        {
            get
            {
                return new Command(() =>
                {
                    if (ShowHide == false)
                    {
                        ShowHide = true;
                    }
                    else
                    {
                        ShowHide = false;
                    }
                });
            }
        }
        #endregion
    }
}
