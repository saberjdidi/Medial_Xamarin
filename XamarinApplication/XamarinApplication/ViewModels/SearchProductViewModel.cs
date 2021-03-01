using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class SearchProductViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion
        #region Attributes
        public INavigation Navigation { get; set; }
        private InfiniteScrollCollection<Product> products;
        private bool isRefreshing;
        private bool isVisible;
        private List<Product> productsList;
        private bool _isBusy;
        private const int _maxResult = 8;
        int _offset = 0;
        public EventHandler<DialogResultProduct> OnDialogClosed;
        private string _description = "";
        public string Code { get; set; }
         public string Description
         {
             get { return _description; }
             set
             {
                 _description = value;
                 OnPropertyChanged();
             }
         }
        private Supplier supplier = null;
        public Supplier Supplier
        {
            get { return supplier; }
            set
            {
                supplier = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Constructors
        public SearchProductViewModel()
        {
            apiService = new ApiServices();
           // SearchProducts();
            /*
                Products = new InfiniteScrollCollection<Product>
                {
                    OnLoadMore = async () =>
                    {
                        IsBusy = true;
                        IsRefreshing = true;
                        // load the next page
                        var page = Products.Count / _maxResult;
                       
                        var _searchRequest = new SearchRequest
                        {
                            code = Code,
                            description = Description
                            //description = SelectedProduct.description
                        };

                        var response = await apiService.SearchRequestProduct<Product>(
                        "https://app.smart-path.it",
                        "/md-core",
                        "/medial/product/search?sortedBy=code&order=asc&maxResult=",
                        _maxResult,
                        "&offset=",
                        page,
                        _searchRequest);
                        Debug.WriteLine("********response In ViewModel*************");
                        Debug.WriteLine(response);
                        if (!response.IsSuccess)
                        {
                            //IsVisible = true;
                            IsRefreshing = true;
                            await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                        }
                        productsList = (List<Product>)response.Result;

                        IsBusy = false;
                        IsRefreshing = false;
                        return productsList;
                    }
                };
                
            RefreshCommand = new Command(() =>
            {
                // clear and start again
                Products.Clear();
                Products.LoadMoreAsync();
            });
            */
            ListProductsAutoComplete();
            ListSuppliersAutoComplete();
        }
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
        #region Methods
        public async void SearchProducts()
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
            
            var _searchRequest = new SearchRequestBySupplier
            {
                code = "",
                description = Description,
                supplier = Supplier
               //description = SelectedProduct.description
            };

            /* var response = await apiService.SearchRequest<Product>(
             "https://app.smart-path.it",
             "/md-core",
             "/medial/product/search?sortedBy=code&order=asc&maxResult=" + _maxResult,
             "&offset=" + 0,
             _searchRequest); 
            var response = await apiService.SeviceProduct<Product>(
            "https://app.smart-path.it",
            "/md-core",
            "/medial/product/search?sortedBy=code&order=asc&maxResult=8",
            _searchRequest);*/
            var response = await apiService.SearchProductBySupplier<Product>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/search?sortedBy=code&order=asc&maxResult=100",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = true;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            if (response.IsSuccess)
            {
                productsList = (List<Product>)response.Result;
                ProductsCollection = new ObservableCollection<Product>(productsList);
                //Products.AddRange(productsList);

                // MessagingCenter.Send(new DialogResultProduct() { ProductsPopup = Products }, "PopUpDataProduct");
                MessagingCenter.Send(new DialogResultProduct() { ProductsPopup = ProductsCollection }, "PopUpDataProduct");
                await App.Current.MainPage.Navigation.PopPopupAsync(true);
                IsVisible = false;
                IsRefreshing = false;
            }
            
        }
        public async void LoadMoreItems(Product currentItem)
        {
            int itemIndex = ProductsCollection.IndexOf(currentItem);

            _offset = ProductsCollection.Count;

            if (ProductsCollection.Count - 20 == itemIndex)
            {
                IsBusy = true;
                IsRefreshing = true;
                var _searchRequest = new SearchRequest
                {
                    code = "",
                    description = Description
                };
                var response = await apiService.LoadMoreData<Product>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/search?sortedBy=code&order=asc&maxResult=10&offset=" + _offset,
                  _searchRequest);
                if (!response.IsSuccess)
                {
                    //IsVisible = true;
                    IsRefreshing = true;
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
                        MessagingCenter.Send(new DialogResultProduct() { ProductsPopup = ProductsCollection }, "PopUpMoreDataProduct");
                    });
                }
            }
        }

        #endregion
        #region Commands
        public ICommand SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    SearchProducts();
                });
            }
        }
        public ICommand RefreshCommand
        {
            get;
        }
        #endregion

        #region Autocomplete
        private List<Product> _productAutoComplete;
        public List<Product> ProductAutoComplete
        {
            get { return _productAutoComplete; }
            set
            {
                _productAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Product>> ListProductsAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.AutocompleteClient<Product>(
                        "https://app.smart-path.it",
                        "/md-core",
                        "/medial/product/search?sortedBy=code&order=asc&maxResult=100",
                        _searchRequest);
            ProductAutoComplete = (List<Product>)response;
            return ProductAutoComplete;
        }

        private List<Supplier> _supplierAutoComplete;
        public List<Supplier> SupplierAutoComplete
        {
            get { return _supplierAutoComplete; }
            set
            {
                _supplierAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Supplier>> ListSuppliersAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<Supplier>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/supplier/search?sortedBy=code&order=asc",
                  _searchRequest);
            SupplierAutoComplete = (List<Supplier>)response.Result;
            return SupplierAutoComplete;
        }
        /* private Product _selectedProduct { get; set; }
         public Product SelectedProduct
         {
             get { return _selectedProduct; }
             set
             {
                 _selectedProduct = value;
                 OnPropertyChanged();
             }
         }
         // use with => SelectedItem="{Binding SelectedProduct}"
             */
        #endregion
    }

    public class DialogResultProduct
    {
        public bool Success { get; set; }
        public string Message { get; set; }
       // public InfiniteScrollCollection<Product> ProductsPopup { get; set; }
        public ObservableCollection<Product> ProductsPopup { get; set; }
    }
}
