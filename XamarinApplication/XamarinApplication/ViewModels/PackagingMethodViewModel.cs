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
    public class PackagingMethodViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<PackagingMethod> packagingMethods;
        private bool isRefreshing;
        private string filter;
        private List<PackagingMethod> packagingMethodsList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<PackagingMethod> PackagingMethods
        {
            get { return packagingMethods; }
            set
            {
                packagingMethods = value;
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
        public PackagingMethodViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetPackagingMethods();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetPackagingMethods();
            });
        }
        #endregion

        #region Sigleton
        static PackagingMethodViewModel instance;
        public static PackagingMethodViewModel GetInstance()
        {
            if (instance == null)
            {
                return new PackagingMethodViewModel();
            }

            return instance;
        }

        public void Update(PackagingMethod packagingMethod)
        {
            IsRefreshing = true;
            var oldpackagingMethod = packagingMethodsList
                .Where(p => p.id == packagingMethod.id)
                .FirstOrDefault();
            oldpackagingMethod = packagingMethod;
            PackagingMethods = new ObservableCollection<PackagingMethod>(packagingMethodsList);
            IsRefreshing = false;
        }
        public async Task Delete(PackagingMethod packagingMethod)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<PackagingMethod>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/packagingMethod",
                packagingMethod.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            packagingMethodsList.Remove(packagingMethod);
            PackagingMethods = new ObservableCollection<PackagingMethod>(packagingMethodsList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetPackagingMethods()
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

            var response = await apiService.Post<PackagingMethod>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/packagingMethod/search?sortedBy=code&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            packagingMethodsList = (List<PackagingMethod>)response.Result;
            PackagingMethods = new ObservableCollection<PackagingMethod>(packagingMethodsList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetPackagingMethods);
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
                PackagingMethods = new ObservableCollection<PackagingMethod>(packagingMethodsList);
                IsVisibleStatus = false;
            }
            else
            {
                PackagingMethods = new ObservableCollection<PackagingMethod>(
                      packagingMethodsList.Where(
                          l => l.code.ToLower().StartsWith(Filter.ToLower()) ||
                             l.description.ToLower().StartsWith(Filter.ToLower())));

                if (PackagingMethods.Count() == 0)
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
