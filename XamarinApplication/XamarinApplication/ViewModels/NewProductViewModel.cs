using Rg.Plugins.Popup.Extensions;
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
    public class NewProductViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public NewProductViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();
            IsEnabled = true;

            ListPackagingMethodAutoComplete();
            ListMeasureUnitAutoComplete();
            ListSupplierAutoComplete();
            ListCurrencyAutoComplete();
            ListCustomAutoComplete();

            ListStatus = GetStatus().OrderBy(t => t.Value).ToList();
        }
        #endregion

        #region Properties
        bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                this._isEnabled = value;
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
        public string Code { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        private DateTime updateCostDate = System.DateTime.Today;  //DateTime.Today.Date;
        public DateTime UpdateCostDate
        {
            get { return updateCostDate; }
            set
            {
                updateCostDate = value;
                OnPropertyChanged();
            }
        }
        private PackagingMethod _packagingMethod = null;
        public PackagingMethod PackagingMethod
        {
            get { return _packagingMethod; }
            set
            {
                _packagingMethod = value;
                OnPropertyChanged();
            }
        }
        private MeasureUnit _measureUnit = null;
        public MeasureUnit MeasureUnit
        {
            get { return _measureUnit; }
            set
            {
                _measureUnit = value;
                OnPropertyChanged();
            }
        }
        private Supplier _supplier = null;
        public Supplier Supplier
        {
            get { return _supplier; }
            set
            {
                _supplier = value;
                OnPropertyChanged();
            }
        }
        private bool _active = true;
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                OnPropertyChanged();
            }
        }
        private bool _composed = false;
        public bool Composed
        {
            get { return _composed; }
            set
            {
                _composed = value;
                OnPropertyChanged();
            }
        }
        private bool _fob = false;
        public bool FOB
        {
            get { return _fob; }
            set
            {
                _fob = value;
                OnPropertyChanged();
            }
        }
        private Currency _currency = null;
        public Currency Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
                OnPropertyChanged();
            }
        }
        private CustomsDuty customsDuty = null;
        public CustomsDuty CustomsDuty
        {
            get { return customsDuty; }
            set
            {
                customsDuty = value;
                OnPropertyChanged();
            }
        }
        public decimal ValuePurchaseCost { get; set; }
        private decimal _costChange = 1;
        public decimal CostChange
        {
            get { return _costChange; }
            set
            {
                _costChange = value;
                OnPropertyChanged();
            }
        }
        public decimal ValuePackagingCost { get; set; }
        public decimal ImportVolume { get; set; }
        public decimal ImportQuantity { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        #endregion

        #region Methods
        public async void AddNewProduct()
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
            
            IsEnabled = false;

            var PurchaseCost = new AddPurchaseCost
            {
                currency = Currency,
                value = ValuePurchaseCost
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
            var PackagingCost = new AddPackagingCost
            {
                currency = currencyStatic,
                value = ValuePackagingCost
            };

            if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Description))
            {
                Value = true;
                return;
            }
            if (MeasureUnit == null || Supplier == null || PackagingMethod == null || PurchaseCost == null || CustomsDuty == null)
            {
                Value = true;
                return;
            }
            /* if(SelectedStatus.Key == null)
            {
                return;
            }*/
            var addProduct = new AddProduct
            {
                code = Code,
                //availability = SelectedStatus.Key,
                description = Description,
                purchaseCost = PurchaseCost,
                packagingCost = PackagingCost,
                updateCostDate = UpdateCostDate,
                costChange = CostChange,
                quantityPerPackage = 1,
                active = Active,
                fob = FOB,
                supplier = Supplier,
                packagingMethod = PackagingMethod,
                measureUnit = MeasureUnit,
                composed = Composed,
                importVolume = ImportVolume,
                importQuantity = ImportQuantity,
                customsDuty = CustomsDuty,
                note = Note,
                width = Width,
                height = Height,
                length = Length,
                cartonVolume = 0.198
            };
            var productJson = new AddProductJson
            {
               // prices = [],
                product = addProduct,
                username = "admin"
            };

            var response = await apiService.Save<AddProductJson>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product",
                  productJson);
            Debug.WriteLine("********responseIn ViewModel*************");
            Debug.WriteLine(response);
            if (!response.IsSuccess)
            {
                IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            IsEnabled = true;
            MessagingCenter.Send((App)Application.Current, "OnSaved");
            DependencyService.Get<INotification>().CreateNotification("Medial", "Product Added");
            //await Navigation.PopModalAsync(); //use for Popup
            await Navigation.PopAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveProduct
        {
            get
            {
                return new Command(() =>
                {
                    AddNewProduct();
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
