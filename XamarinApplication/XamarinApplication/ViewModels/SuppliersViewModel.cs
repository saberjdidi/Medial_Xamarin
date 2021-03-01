using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class SuppliersViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<Supplier> suppliers;
        private bool isRefreshing;
        private string filter;
        private List<Supplier> suppliersList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<Supplier> Suppliers
        {
            get { return suppliers; }
            set
            {
                suppliers = value;
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
        public SuppliersViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetSuppliers();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetSuppliers();
            });
        }
        #endregion

        #region Sigleton
        static SuppliersViewModel instance;
        public static SuppliersViewModel GetInstance()
        {
            if (instance == null)
            {
                return new SuppliersViewModel();
            }

            return instance;
        }

        public void Update(Supplier supplier)
        {
            IsRefreshing = true;
            var oldSupplier = suppliersList
                .Where(p => p.id == supplier.id)
                .FirstOrDefault();
            oldSupplier = supplier;
            Suppliers = new ObservableCollection<Supplier>(suppliersList);
            IsRefreshing = false;
        }
        public async Task Delete(Supplier supplier)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Supplier>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/supplier",
                supplier.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            suppliersList.Remove(supplier);
            Suppliers = new ObservableCollection<Supplier>(suppliersList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetSuppliers()
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
                code="",
                description=""
            };

            var response = await apiService.Post<Supplier>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/supplier/search?sortedBy=code&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            suppliersList = (List<Supplier>)response.Result;
            Suppliers = new ObservableCollection<Supplier>(suppliersList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetSuppliers);
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
                Suppliers = new ObservableCollection<Supplier>(suppliersList);
                IsVisibleStatus = false;
              }
              else
              {
                Suppliers = new ObservableCollection<Supplier>(
                      suppliersList.Where(
                          l => l.code.ToLower().StartsWith(Filter.ToLower()) ||
                             l.description.ToLower().StartsWith(Filter.ToLower())));

                if (Suppliers.Count() == 0)
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
