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
    public class ProvinceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<Province> provinces;
        private bool isRefreshing;
        private string filter;
        private List<Province> provincesList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<Province> Provinces
        {
            get { return provinces; }
            set
            {
                provinces = value;
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
        public ProvinceViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetProvinces();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetProvinces();
            });
        }
        #endregion

        #region Sigleton
        static ProvinceViewModel instance;
        public static ProvinceViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProvinceViewModel();
            }

            return instance;
        }

        public void Update(Province province)
        {
            IsRefreshing = true;
            var oldprovince = provincesList
                .Where(p => p.id == province.id)
                .FirstOrDefault();
            oldprovince = province;
            Provinces = new ObservableCollection<Province>(provincesList);
            IsRefreshing = false;
        }
        public async Task Delete(Province province)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Province>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/province",
                province.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            provincesList.Remove(province);
            Provinces = new ObservableCollection<Province>(provincesList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetProvinces()
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

            var response = await apiService.Post<Province>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/province/search?sortedBy=code&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            provincesList = (List<Province>)response.Result;
            Provinces = new ObservableCollection<Province>(provincesList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetProvinces);
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
                Provinces = new ObservableCollection<Province>(provincesList);
                IsVisibleStatus = false;
            }
            else
            {
                Provinces = new ObservableCollection<Province>(
                      provincesList.Where(l => l.code.ToLower().StartsWith(Filter.ToLower()) ||
                                               l.description.ToLower().StartsWith(Filter.ToLower())));

                if (Provinces.Count() == 0)
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
