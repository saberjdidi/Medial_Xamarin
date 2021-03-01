using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
   public class CommercialSupplierViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        private ObservableCollection<CommercialSupplier> product;
        private List<CommercialSupplier> productList;
        private bool isVisible;
        private bool isRefreshing;
        private bool _showHide = false;
        private string filter;
        private bool isEnabled = true;
        #endregion

        #region Properties
        public ObservableCollection<CommercialSupplier> Products
        {
            get { return product; }
            set
            {
                product = value;
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
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                this.isVisible = value;
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
        public bool ShowHide
        {
            get => _showHide;
            set
            {
                _showHide = value;
                OnPropertyChanged();
            }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                this.isEnabled = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public CommercialSupplierViewModel()
        {
            //GetCosts();
            ListSupplierAutoComplete();
        }
        #endregion

        #region Methods
        public async void GetCosts()
        {
            if (Supplier == null)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Warning", "Select Supplier", "ok");
                return;
            }
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var _searchRequest = new SearchCommercialSupplier
            {
                maxResult = 100,
                order = "asc",
                sortedBy = "code",
                criteria = "",
                exchangeRate = 0.86,
                supplierId = Supplier.id
            };

            var response = await apiService.CommercialSupplier<CommercialSupplier>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/costs",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            productList = (List<CommercialSupplier>)response.Result;
            Products = new ObservableCollection<CommercialSupplier>(productList);
            IsRefreshing = false;
            if (Products.Count() == 0)
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
            }
        }
        #endregion

        #region Commands
        public ICommand GetData
        {
            get
            {
               /* if (Supplier == null)
                {
                    IsEnabled = false;
                }*/
                return new Command(() =>
                {
                    GetCosts();
                });
            }
        }
        public Command ExportExcel
        {
            get
            {
               /* if (Supplier == null)
                {
                    IsEnabled = false;
                }*/
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                    if (Supplier == null)
                    {
                        IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert("Warning", "Select Supplier", "ok");
                        return;
                    }
                    var _searchRequest = new SearchCommercialSupplier
                    {
                        order = "asc",
                        sortedBy = "code",
                        exchangeRate = 0.86,
                        supplierId = Supplier.id
                    };

                    var requestJson = JsonConvert.SerializeObject(_searchRequest);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(requestJson);
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/product/attachment";
                    Debug.WriteLine("********url*************");
                    Debug.WriteLine(url);
                    var response = await httpClient.PostAsync(url, content);
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("********result*************");
                    Debug.WriteLine(result);
                    IsRefreshing = false;
                    if (!response.IsSuccessStatusCode)
                    {
                        IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                        return;
                    }
                    var pdf = JsonConvert.DeserializeObject<PdfClient>(result);

                    byte[] bytes = Convert.FromBase64String(pdf.report);
                    MemoryStream stream = new MemoryStream(bytes);

                    if (stream == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Data is Empty", "ok");
                        return;
                    }

                    await DependencyService.Get<ISave>().SaveAndView(Supplier.description+"_products-"+dateNow+ ".xlsx", pdf.defaultExtention, stream);
                });
            }
        }
        public Command GeneratePdf
        {
            get
            {
                /*if (Supplier == null)
                {
                    IsEnabled = false;
                }*/
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                    if (Supplier == null)
                    {
                        IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert("Warning", "Select Supplier", "ok");
                        return;
                    }
                    var _searchRequest = new SearchCommercialSupplier
                    {
                        order = "asc",
                        sortedBy = "code",
                        exchangeRate = 0.86,
                        supplierId = Supplier.id,
                        format = "PDF"
                    };

                    var requestJson = JsonConvert.SerializeObject(_searchRequest);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(requestJson);
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/product/generate";
                    Debug.WriteLine("********url*************");
                    Debug.WriteLine(url);
                    var response = await httpClient.PostAsync(url, content);
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("********result*************");
                    Debug.WriteLine(result);
                    IsRefreshing = false;
                    if (!response.IsSuccessStatusCode)
                    {
                        IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                        return;
                    }
                    var pdf = JsonConvert.DeserializeObject<PdfClient>(result);

                    byte[] bytes = Convert.FromBase64String(pdf.report);
                    MemoryStream stream = new MemoryStream(bytes);

                    if (stream == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Data is Empty", "ok");
                        return;
                    }

                    await DependencyService.Get<ISave>().SaveAndView(Supplier.description + "_products-" + dateNow + ".pdf", pdf.defaultExtention, stream);
                });
            }
        }
        public Command GenerateExcel
        {
            get
            {
               /* if (Supplier == null)
                {
                    IsEnabled = false;
                }*/
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                    if (Supplier == null)
                    {
                        IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert("Warning", "Select Supplier", "ok");
                        return;
                    }
                    var _searchRequest = new SearchCommercialSupplier
                    {
                        order = "asc",
                        sortedBy = "code",
                        exchangeRate = 0.86,
                        supplierId = Supplier.id,
                        format = "XLSX"
                    };

                    var requestJson = JsonConvert.SerializeObject(_searchRequest);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(requestJson);
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/product/generate";
                    Debug.WriteLine("********url*************");
                    Debug.WriteLine(url);
                    var response = await httpClient.PostAsync(url, content);
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("********result*************");
                    Debug.WriteLine(result);
                    IsRefreshing = false;
                    if (!response.IsSuccessStatusCode)
                    {
                        IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                        return;
                    }
                    var pdf = JsonConvert.DeserializeObject<PdfClient>(result);

                    byte[] bytes = Convert.FromBase64String(pdf.report);
                    MemoryStream stream = new MemoryStream(bytes);

                    if (stream == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Data is Empty", "ok");
                        return;
                    }

                    await DependencyService.Get<ISave>().SaveAndView(Supplier.description + "_products-" + dateNow + ".xlsx", pdf.defaultExtention, stream);
                });
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetCosts);
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
                Products = new ObservableCollection<CommercialSupplier>(productList);
                IsVisible = false;
            }
            else
            {
                Products = new ObservableCollection<CommercialSupplier>(
                      productList.Where(
                          l => l.product.code.ToLower().StartsWith(Filter.ToLower()) ||
                             l.product.description.ToLower().StartsWith(Filter.ToLower())));

            }
            if (Products.Count() == 0)
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
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

        #region Autocomplete
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
                order = "asc",
                sortedBy = "code"
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
}
