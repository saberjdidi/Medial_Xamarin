using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
   public class ProductsAgentViewModel : BaseViewModel
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
        private List<ProductAgent> productsList;
        private bool _isBusy;
        private const int _maxResult = 8;
        int _offset = 0;
        public int TotalCount { get; private set; }
        private bool _showHide = false;
        #endregion

        #region Properties
        private ObservableCollection<ProductAgent> productsCollection;
        public ObservableCollection<ProductAgent> ProductsCollection
        {
            get { return productsCollection; }
            set
            {
                productsCollection = value;
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
        public ProductsAgentViewModel()
        {
            apiService = new ApiServices();
           
            GetProducts();
        }
        #endregion

        #region Methods
        public async void GetProducts()
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

            var response = await apiService.Post<ProductAgent>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/search?sortedBy=code&order=asc&maxResult=60",
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
            productsList = (List<ProductAgent>)response.Result;
            ProductsCollection = new ObservableCollection<ProductAgent>(productsList);
            //Products.AddRange(productsList);
            IsVisible = false;
            IsRefreshing = false;
        }

        public async void LoadMoreItems(ProductAgent currentItem)
        {
            int itemIndex = ProductsCollection.IndexOf(currentItem);

            _offset = ProductsCollection.Count;

            if (ProductsCollection.Count - 50 == itemIndex)
            {
                IsBusy = true;
                IsRefreshing = true;
                var _searchRequest = new SearchRequest
                {
                    order = "asc",
                    sortedBy = "code"
                };
                var response = await apiService.LoadMoreData<ProductAgent>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/search?sortedBy=code&order=asc&maxResult=10&offset=" + _offset,
                  _searchRequest);
                if (!response.IsSuccess)
                {
                    //IsRefreshing = true;
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                    return;
                }
                productsList = (List<ProductAgent>)response.Result;
                foreach (ProductAgent item in productsList)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsBusy = false;
                        IsRefreshing = false;
                        ProductsCollection.Add(item);

                        //MessagingCenter not working
                        MessagingCenter.Subscribe<DialogResultProductAgent>(this, "PopUpMoreDataProductAgent", (value) =>
                        {
                            ProductsCollection = value.ProductsPopup;
                        });
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

                    MessagingCenter.Subscribe<DialogResultProductAgent>(this, "PopUpDataProductAgent", (value) =>
                    {
                        
                        ProductsCollection = value.ProductsPopup;
                        IsBusy = false;
                        IsRefreshing = false;
                        if (ProductsCollection.Count() == 0)
                        {
                            IsVisible = true;
                        }
                        else
                        {
                            IsVisible = false;
                        }
                    });
                    await PopupNavigation.Instance.PushAsync(new SearchProductAgent());
                });
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetProducts);
            }
        }
        #endregion
    }
}
