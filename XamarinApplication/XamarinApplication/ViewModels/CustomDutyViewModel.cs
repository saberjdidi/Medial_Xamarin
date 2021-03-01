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
    public class CustomDutyViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<CustomsDuty> _customsDuty;
        private bool isRefreshing;
        private string filter;
        private List<CustomsDuty> customsDutyList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<CustomsDuty> CustomsDuty
        {
            get { return _customsDuty; }
            set
            {
                _customsDuty = value;
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
        public CustomDutyViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetCustomsDuty();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetCustomsDuty();
            });
        }
        #endregion

        #region Sigleton
        static CustomDutyViewModel instance;
        public static CustomDutyViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CustomDutyViewModel();
            }

            return instance;
        }

        public void Update(CustomsDuty customsDuty)
        {
            IsRefreshing = true;
            var oldcustomsDuty = customsDutyList
                .Where(p => p.id == customsDuty.id)
                .FirstOrDefault();
            oldcustomsDuty = customsDuty;
            CustomsDuty = new ObservableCollection<CustomsDuty>(customsDutyList);
            IsRefreshing = false;
        }
        public async Task Delete(CustomsDuty customsDuty)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<CustomsDuty>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/config",
                customsDuty.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            customsDutyList.Remove(customsDuty);
            CustomsDuty = new ObservableCollection<CustomsDuty>(customsDutyList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetCustomsDuty()
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

            var response = await apiService.Post<CustomsDuty>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/config/search?sortedBy=code&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            customsDutyList = (List<CustomsDuty>)response.Result;
            CustomsDuty = new ObservableCollection<CustomsDuty>(customsDutyList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetCustomsDuty);
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
                CustomsDuty = new ObservableCollection<CustomsDuty>(customsDutyList);
                IsVisibleStatus = false;
            }
            else
            {
                CustomsDuty = new ObservableCollection<CustomsDuty>(
                      customsDutyList.Where(
                          l => l.description.ToLower().StartsWith(Filter.ToLower())));

                if (CustomsDuty.Count() == 0)
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
