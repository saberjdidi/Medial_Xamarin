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
    public class ProductsViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private InfiniteScrollCollection<Product> products;
        private bool isRefreshing;
        private bool isVisible;
        private string filter;
        private List<Product> productsList;
        private bool _isBusy;
        private const int _maxResult = 8;
        int _offset = 0;
        public int TotalCount { get; private set; }
        private bool _showHide = false;
        #endregion

        #region Properties
        private ObservableCollection<Product> productsCollection;
        public ObservableCollection<Product> ProductsCollection
        {
            get { return productsCollection; }
            set
            {
                productsCollection = value;
                OnPropertyChanged();
            }
        }
        public InfiniteScrollCollection<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
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
        public ProductsViewModel()
        {
            apiService = new ApiServices();
            instance = this;
            // Navigation = _navigation;
            /*
                Products = new InfiniteScrollCollection<Product>
                {
                    OnLoadMore = async () =>
                    {
                        IsBusy = true;
                        IsRefreshing = true;
                        // load the next page
                        var page = (Products.Count/_maxResult)+8;
                        var _searchRequest = new SearchRequest
                        {
                            code = "",
                            description = ""
                        };
                        var response = await apiService.SearchRequestProduct<Product>(
                        "https://app.smart-path.it",
                        "/md-core",
                        "/medial/product/search?sortedBy=code&order=asc&maxResult=",
                        _maxResult,
                        "&offset=",
                        page,
                        _searchRequest);
                        if (!response.IsSuccess)
                        {
                            //IsVisible = true;
                            IsRefreshing = true;
                            await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                        }
                        productsList = (List<Product>)response.Result;

                        //Products.AddRange(productsList);
                        IsBusy = false;
                        IsRefreshing = false;
                        return productsList;
                    },
                    OnCanLoadMore = () =>
                    {
                        //return Products.Count < _maxResult * PageSize;
                        return Products.Count < 1000;
                        // return Requests.Count < TotalCount;
                    }
                };
            */
            GetProducts();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetProducts();
            });
        }
        #endregion

        #region Sigleton
        static ProductsViewModel instance;
        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }

            return instance;
        }

        public void Update(Product product)
        {
            IsRefreshing = true;
            var oldProduct = productsList
                .Where(p => p.id == product.id)
                .FirstOrDefault();
            oldProduct = product;
            ProductsCollection = new ObservableCollection<Product>(productsList);
            IsRefreshing = false;
        }
        public async Task Delete(Product product)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Product>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/product",
                product.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            productsList.Remove(product);
            ProductsCollection = new ObservableCollection<Product>(productsList);

            IsRefreshing = false;
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
                code="",
                description=""
            };

            /*  var response = await apiService.SearchRequestProduct<Product>(
              "https://app.smart-path.it",
              "/md-core",
              "/medial/product/search?sortedBy=code&order=asc&maxResult=100",
              pageSize: _maxResult,
              "&offset=",
              pageIndex: 0,
              _searchRequest);
            var response = await apiService.SeviceProduct<Product>(
            "https://app.smart-path.it",
            "/md-core",
            "/medial/product/search?sortedBy=code&order=asc&maxResult=8",
            _searchRequest);
            var response = await apiService.SearchRequestProduct<Product>(
                        "https://app.smart-path.it",
                        "/md-core",
                        "/medial/product/search?sortedBy=code&order=asc&maxResult=",
                        _maxResult,
                        "&offset=",
                        0,
                        _searchRequest);*/
            var response = await apiService.Post<Product>(
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
            productsList = (List<Product>)response.Result;
            ProductsCollection = new ObservableCollection<Product>(productsList);
            //Products.AddRange(productsList);
            IsVisible = false;
            IsRefreshing = false;
        }

        public async void LoadMoreItems(Product currentItem)
        {
            int itemIndex = ProductsCollection.IndexOf(currentItem);

            _offset = ProductsCollection.Count;

            if (ProductsCollection.Count - 50 == itemIndex)
            {
                IsBusy = true;
                IsRefreshing = true;
                var _searchRequest = new SearchRequest
                {
                    code = "",
                    description = ""
                };
                var response = await apiService.LoadMoreData<Product>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/search?sortedBy=code&order=asc&maxResult=10&offset="+_offset,
                  _searchRequest);
                if (!response.IsSuccess)
                {
                    //IsRefreshing = true;
                    await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                    return;
                }
                productsList = (List<Product>)response.Result;
                foreach (Product item in productsList)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsBusy = false;
                        IsRefreshing = false;
                        ProductsCollection.Add(item);

                        //MessagingCenter not working
                        MessagingCenter.Subscribe<DialogResultProduct>(this, "PopUpMoreDataProduct", (value) =>
                        {
                            ProductsCollection = value.ProductsPopup;
                        });
                    });
                }

               /* ProductsCollection = new ObservableCollection<Product>(productsList);

                string sql = $"SELECT * FROM MyData LIMIT 20 OFFSET {_offset}";
                List<MyData> data = conn.Query<MyData>(sql);
                foreach (MyData item in data)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ListData.Add(item);
                    });
                }*/
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
                   
                    MessagingCenter.Subscribe<DialogResultProduct>(this, "PopUpDataProduct", (value) =>
                    {
                        /// string receivedData = value.RequestsPopup;
                        /// MyLabel.Text = receivedData;
                        //Products = value.ProductsPopup;
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
                    await PopupNavigation.Instance.PushAsync(new SearchProductPage());
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
        /*
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
                ProductsCollection = new ObservableCollection<Product>(productsList);
                IsVisible = false;
            }
            else
            {
                ProductsCollection = new ObservableCollection<Product>(
                      productsList.Where(
                          l => l.supplier.description.ToLower().StartsWith(Filter.ToLower())));

                if (ProductsCollection.Count() == 0)
                {
                    IsVisible = true;
                }
                else
                {
                    IsVisible = false;
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
        */
        #endregion
    }
}
