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
    public class CurruncyViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<Currency> _curruncy;
        private bool isRefreshing;
        private string filter;
        private List<Currency> curruncyList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<Currency> Currency
        {
            get { return _curruncy; }
            set
            {
                _curruncy = value;
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
        public CurruncyViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetCurrency();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetCurrency();
            });
        }
        #endregion

        #region Sigleton
        static CurruncyViewModel instance;
        public static CurruncyViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CurruncyViewModel();
            }

            return instance;
        }

        public void Update(Currency currency)
        {
            IsRefreshing = true;
            var oldcurrency = curruncyList
                .Where(p => p.id == currency.id)
                .FirstOrDefault();
            oldcurrency = currency;
            Currency = new ObservableCollection<Currency>(curruncyList);
            IsRefreshing = false;
        }
        public async Task Delete(Currency currency)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Currency>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/currency",
                currency.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            curruncyList.Remove(currency);
            Currency = new ObservableCollection<Currency>(curruncyList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetCurrency()
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
                sortedBy = "entity"
            };

            var response = await apiService.Post<Currency>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/currency/search?sortedBy=entity&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            curruncyList = (List<Currency>)response.Result;
            Currency = new ObservableCollection<Currency>(curruncyList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetCurrency);
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
                Currency = new ObservableCollection<Currency>(curruncyList);
                IsVisibleStatus = false;
            }
            else
            {
                Currency = new ObservableCollection<Currency>(
                      curruncyList.Where(
                          l => l.entity.ToLower().StartsWith(Filter.ToLower())||
                          l.alphabeticCode.ToLower().StartsWith(Filter.ToLower())));

                if (Currency.Count() == 0)
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
