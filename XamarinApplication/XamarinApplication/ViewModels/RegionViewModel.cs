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
   public class RegionViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<Reggion> regions;
        private bool isRefreshing;
        private string filter;
        private List<Reggion> regionsList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<Reggion> Regions
        {
            get { return regions; }
            set
            {
                regions = value;
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
        public RegionViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetRegions();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetRegions();
            });
        }
        #endregion

        #region Sigleton
        static RegionViewModel instance;
        public static RegionViewModel GetInstance()
        {
            if (instance == null)
            {
                return new RegionViewModel();
            }

            return instance;
        }

        public void Update(Reggion region)
        {
            IsRefreshing = true;
            var oldregion = regionsList
                .Where(p => p.id == region.id)
                .FirstOrDefault();
            oldregion = region;
            Regions = new ObservableCollection<Reggion>(regionsList);
            IsRefreshing = false;
        }
        public async Task Delete(Reggion region)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Reggion>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/region",
                region.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            regionsList.Remove(region);
            Regions = new ObservableCollection<Reggion>(regionsList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetRegions()
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
                sortedBy = "code"
            };

            var response = await apiService.Post<Reggion>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/region/search?sortedBy=code&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            regionsList = (List<Reggion>)response.Result;
            Regions = new ObservableCollection<Reggion>(regionsList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetRegions);
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
                Regions = new ObservableCollection<Reggion>(regionsList);
                IsVisibleStatus = false;
            }
            else
            {
                Regions = new ObservableCollection<Reggion>(
                      regionsList.Where(l => l.code.ToLower().StartsWith(Filter.ToLower()) ||
                                               l.description.ToLower().StartsWith(Filter.ToLower())));

                if (Regions.Count() == 0)
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
