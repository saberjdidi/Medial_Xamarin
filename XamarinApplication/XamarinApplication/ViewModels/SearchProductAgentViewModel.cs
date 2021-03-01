using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
   public class SearchProductAgentViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion
        #region Attributes
        public INavigation Navigation { get; set; }
        private bool isRefreshing;
        private bool isVisible;
        private List<ProductAgent> productsList;
        private bool _isBusy;
        private const int _maxResult = 8;
        int _offset = 0;
        public EventHandler<DialogResultProductAgent> OnDialogClosed;
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
        public SearchProductAgentViewModel()
        {
            apiService = new ApiServices();
            ListProductsAutoComplete();
            ListSuppliersAutoComplete();
        }
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
            var response = await apiService.SearchProductBySupplier<ProductAgent>(
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
                productsList = (List<ProductAgent>)response.Result;
                ProductsCollection = new ObservableCollection<ProductAgent>(productsList);
                //Products.AddRange(productsList);

                // MessagingCenter.Send(new DialogResultProduct() { ProductsPopup = Products }, "PopUpDataProduct");
                MessagingCenter.Send(new DialogResultProductAgent() { ProductsPopup = ProductsCollection }, "PopUpDataProductAgent");
                await App.Current.MainPage.Navigation.PopPopupAsync(true);
                IsVisible = false;
                IsRefreshing = false;
            }

        }
        public async void LoadMoreItems(ProductAgent currentItem)
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
                var response = await apiService.LoadMoreData<ProductAgent>(
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
                productsList = (List<ProductAgent>)response.Result;
                foreach (ProductAgent item in productsList)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        IsBusy = false;
                        IsRefreshing = false;
                        ProductsCollection.Add(item);
                        MessagingCenter.Send(new DialogResultProductAgent() { ProductsPopup = ProductsCollection }, "PopUpMoreDataProductAgent");
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
        #endregion
    }

    public class DialogResultProductAgent
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        // public InfiniteScrollCollection<Product> ProductsPopup { get; set; }
        public ObservableCollection<ProductAgent> ProductsPopup { get; set; }
    }
}
