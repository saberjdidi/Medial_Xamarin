using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class UpdateProductViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Product _product;
        #endregion

        #region Constructors
        public UpdateProductViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            ListPackagingMethodAutoComplete();
            ListMeasureUnitAutoComplete();
            ListSupplierAutoComplete();
            ListCurrencyAutoComplete();
            ListCustomAutoComplete();

            ListStatus = GetStatus().OrderBy(t => t.Value).ToList();
        }
        #endregion

        #region Properties
        public Product Product
        {
            get { return _product; }
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }
        private bool value = false;
        public bool Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Methods
        public async void EditProduct()
        {
            Value = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Warning,
                    Languages.CheckConnection,
                    Languages.Ok);
                return;
            }
            if (string.IsNullOrEmpty(Product.code) || string.IsNullOrEmpty(Product.description))
            {
                Value = true;
                return;
            }
            if (Product.supplier == null || Product.customsDuty == null || Product.measureUnit == null)
            {
                Value = true;
                return;
            }
            if(SelectedStatus == null)
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Select Availability", "ok");
                return;
            }
            var purchaseCost = new PurchaseCost
            {
                id = Product.purchaseCost.id,
                currency = Product.purchaseCost.currency,
                value = Product.purchaseCost.value
            };
            var currencyStatic = new Currency
            {
                id = 2,
                entity = "ÅLAND ISLANDS",
                currency = "Euro",
                alphabeticCode = "EUR",
                numericCode = "978",
                minorUnit = "2",
                withdrawalDate = null,
                remark = null
            };
            var packagingCost = new PackagingCost
            {
                id = Product.packagingCost.id,
                currency = currencyStatic,
                value = Product.packagingCost.value
            };

            var product = new Product
            {
                id = Product.id,
                code = Product.code,
                availability = SelectedStatus.Key,
                description = Product.description,
                purchaseCost = purchaseCost,
                packagingCost = packagingCost,
                updateCostDate = Product.updateCostDate,
                costChange = Product.costChange,
                quantityPerPackage = 1,
                active = Product.active,
                fob = Product.fob,
                supplier = Product.supplier,
                packagingMethod = Product.packagingMethod,
                measureUnit = Product.measureUnit,
                composed = Product.composed,
                component = Product.component,
                importVolume = Product.importVolume,
                importQuantity = Product.importQuantity,
                customsDuty = Product.customsDuty,
                note = Product.note,
                width = Product.width,
                height = Product.height,
                length = Product.length,
                cartonVolume = 1,
                useCarton = Product.useCarton
            };

            var eubasiccost = new PackagingCost
            {
                currency = currencyStatic,
                value = Product.purchaseCost.value
            };
            var euddpcost = new PackagingCost
            {
                currency = currencyStatic,
                value = Product.purchaseCost.value
            };
            var productJson = new ProductJson
            {
                // prices = [],
                product = product,
                username = "admin",
                eubasicpcost = eubasiccost,
                euddpcost = euddpcost,
                //piecesPerContainer = 30
            };
            var response = await apiService.PutProduct<Product>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product",
                  productJson);            
           /* if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }*/
            Value = false;
            //ProductsViewModel.GetInstance().Update(productJson);
            MessagingCenter.Send((App)Application.Current, "OnSaved");
            DependencyService.Get<INotification>().CreateNotification("Medial", "Product Updated");
            await Navigation.PopAsync();
        }
        #endregion

        #region Commands
        public ICommand UpdateProduct
        {
            get
            {
                return new Command(() =>
                {
                    EditProduct();
                });
            }
        }
        public Command ClosePopup
        {
            get
            {
                return new Command(() =>
                {
                    //Navigation.PopPopupAsync();
                    Navigation.PopAsync();
                    Debug.WriteLine("********Close*************");
                });
            }
        }
        #endregion

        #region Autocomplete
        //PackagingMethod
        private List<PackagingMethod> _packagingMethodAutoComplete;
        public List<PackagingMethod> PackagingMethodAutoComplete
        {
            get { return _packagingMethodAutoComplete; }
            set
            {
                _packagingMethodAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<PackagingMethod>> ListPackagingMethodAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<PackagingMethod>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/packagingMethod/search?sortedBy=code&order=asc",
                  _searchRequest);
            PackagingMethodAutoComplete = (List<PackagingMethod>)response.Result;
            return PackagingMethodAutoComplete;
        }
        //MeasureUnit
        private List<MeasureUnit> _measureUnitAutoComplete;
        public List<MeasureUnit> MeasureUnitAutoComplete
        {
            get { return _measureUnitAutoComplete; }
            set
            {
                _measureUnitAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<MeasureUnit>> ListMeasureUnitAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<MeasureUnit>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/measureUnit/search?sortedBy=code&order=asc",
                  _searchRequest);
            MeasureUnitAutoComplete = (List<MeasureUnit>)response.Result;
            return MeasureUnitAutoComplete;
        }
        //Supplier
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
        public async Task<List<Supplier>> ListSupplierAutoComplete()
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
        //Currency
        private List<Currency> _currencyAutoComplete;
        public List<Currency> CurrencyAutoComplete
        {
            get { return _currencyAutoComplete; }
            set
            {
                _currencyAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Currency>> ListCurrencyAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<Currency>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/currency/search?sortedBy=entity&order=asc",
                  _searchRequest);
            CurrencyAutoComplete = (List<Currency>)response.Result;
            return CurrencyAutoComplete;
        }
        //Customs Duty
        private List<CustomsDuty> _customAutoComplete;
        public List<CustomsDuty> CustomAutoComplete
        {
            get { return _customAutoComplete; }
            set
            {
                _customAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<CustomsDuty>> ListCustomAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                code = "",
                description = ""
            };
            var response = await apiService.Post<CustomsDuty>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/config/search?sortedBy=code&order=asc",
                  _searchRequest);
            CustomAutoComplete = (List<CustomsDuty>)response.Result;
            return CustomAutoComplete;
        }
        #endregion

        #region Status
        public List<Language> ListStatus { get; set; }
        private Language _selectedStatus { get; set; }
        public Language SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<Language> GetStatus()
        {
            var languages = new List<Language>()
            {
                new Language(){Key =  "0", Value= "Dispo"},
                new Language(){Key =  "2", Value= "Non Dispo"}
            };

            return languages;
        }
        #endregion
    }
}
