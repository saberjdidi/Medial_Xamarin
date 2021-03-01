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
   public class CommercialProductViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        private ObservableCollection<CommercialSupplier> product;
        private List<CommercialSupplier> productList;
        private SearchCommercialProduct _searchRequest;
        private bool isVisible;
        private bool isRefreshing;
        private bool _showHide = false;
        private string filter;
        private bool _isToggle = false;
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
        private Product _product = null;
        public Product Product
        {
            get { return _product; }
            set
            {
                _product = value;
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
        public bool IsToggle
        {
            get { return _isToggle; }
            set
            {
                this._isToggle = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public CommercialProductViewModel()
        {
            //GetCosts();
            ListProductAutoComplete();
        }
        #endregion

        #region Methods
        public async void GetCosts()
        {
            if (Product == null)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Warning", "Select Product", "ok");
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
            List<int> list2 = new List<int>();
            list2.Add(Product.id);
            if (IsToggle == true)
            {
                _searchRequest = new SearchCommercialProduct
                {
                    order = "asc",
                    sortedBy = "code",
                    exchangeRate = 0.86,
                    productList = list2
                };
            }
            else
            {
                _searchRequest = new SearchCommercialProduct
                {
                    order = "asc",
                    sortedBy = "code",
                    productList = list2
                };
            }

            var response = await apiService.CommercialProduct<CommercialSupplier>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/productList/costs",
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
                return new Command(() =>
                {
                    GetCosts();
                });
            }
        }
        public Command ExportPdf
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                    var _searchRequest = new SearchExportAll
                    {
                        order = "asc",
                        sortedBy = "code",
                        exchangeRate = 0.86,
                        format = "PDF"
                    };

                    var requestJson = JsonConvert.SerializeObject(_searchRequest);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(requestJson);
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(200); // this is double the default
                    var url = "https://app.smart-path.it/md-core/medial/product/generate/list/all";
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

                    await DependencyService.Get<ISave>().SaveAndView("products-" + dateNow + ".pdf", "application/pdf", stream);
                });
            }
        }
        public Command ExportExcel
          {
              get
              {
                  return new Command(async () =>
                  {
                      IsRefreshing = true;
                      var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                      var _searchRequest = new SearchExportAll
                      {
                          order = "asc",
                          sortedBy = "code",
                          exchangeRate = 0.86,
                          format = "XLSX"
                      };

                      var requestJson = JsonConvert.SerializeObject(_searchRequest);
                      Debug.WriteLine("********request*************");
                      Debug.WriteLine(requestJson);
                      var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                      var httpClient = new HttpClient();
                      httpClient.Timeout = TimeSpan.FromSeconds(200); // this is double the default
                      var url = "https://app.smart-path.it/md-core/medial/product/generate/list/all";
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

                      await DependencyService.Get<ISave>().SaveAndView("products-" + dateNow + ".xlsx", pdf.defaultExtention, stream);
                  });
              }
          }
        public Command GeneratePdf
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                    if (Product == null)
                    {
                        IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert("Warning", "Select Product", "ok");
                        return;
                    }
                    List<int> list2 = new List<int>();
                    list2.Add(Product.id);
                    if (IsToggle == true)
                    {
                        _searchRequest = new SearchCommercialProduct
                        {
                            order = "asc",
                            sortedBy = "code",
                            exchangeRate = 0.86,
                            productList = list2,
                            format = "PDF"
                        };
                    }
                    else
                    {
                        _searchRequest = new SearchCommercialProduct
                        {
                            order = "asc",
                            sortedBy = "code",
                            productList = list2,
                            format = "PDF"
                        };
                    }
                    var requestJson = JsonConvert.SerializeObject(_searchRequest);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(requestJson);
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/product/generate/list";
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

                    await DependencyService.Get<ISave>().SaveAndView("products-" + dateNow + ".pdf", pdf.defaultExtention, stream);
                });
            }
        }
        public Command GenerateExcel
        {
            get
            {
                
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                    if (Product == null)
                    {
                        IsRefreshing = false;
                        await Application.Current.MainPage.DisplayAlert("Warning", "Select Product", "ok");
                        return;
                    }
                    List<int> list2 = new List<int>();
                    list2.Add(Product.id);
                    if (IsToggle == true)
                    {
                        _searchRequest = new SearchCommercialProduct
                        {
                            order = "asc",
                            sortedBy = "code",
                            exchangeRate = 0.86,
                            productList = list2,
                            format = "XLSX"
                        };
                    }
                    else
                    {
                        _searchRequest = new SearchCommercialProduct
                        {
                            order = "asc",
                            sortedBy = "code",
                            productList = list2,
                            format = "XLSX"
                        };
                    }

                    var requestJson = JsonConvert.SerializeObject(_searchRequest);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(requestJson);
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/product/generate/list";
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

                    await DependencyService.Get<ISave>().SaveAndView("product-" + dateNow + ".xlsx", pdf.defaultExtention, stream);
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
        //Product
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
        public async Task<List<Product>> ListProductAutoComplete()
        {
            var _searchRequest = new SearchProduct
            {
                order = "asc",
                sortedBy = "code",
                maxResult = 200,
                isActive = true
            };
            var response = await apiService.SearchProduct<Product>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/search/projected?sortedBy=code&order=asc&maxResult=200",
                  _searchRequest);
            ProductAutoComplete = (List<Product>)response.Result;
            return ProductAutoComplete;
        }
        #endregion
    }
    #region Model
      public class SearchProduct
    {
        public string order { get; set; }
        public string sortedBy { get; set; }
        public int maxResult { get; set; }
        public bool isActive { get; set; }
    }
    public class SearchCommercialProduct
    {
        public List<int> productList { get; set; }
        public string order { get; set; }
        public string sortedBy { get; set; }
        public Double exchangeRate { get; set; }
        public string format { get; set; }
    }
    public class SearchExportAll
    {
        public string order { get; set; }
        public string sortedBy { get; set; }
        public Double exchangeRate { get; set; }
        public string format { get; set; }
    }
    #endregion
}
