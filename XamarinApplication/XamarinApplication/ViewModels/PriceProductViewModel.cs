using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
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
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class PriceProductViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        private ObservableCollection<PriceProduct> product;
        private List<PriceProduct> productList;
        private bool isVisible;
        private SearchPriceProduct _searchRequest;
        #endregion

        #region Properties
        public ObservableCollection<PriceProduct> Product
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
        public List<int> SupplierValue { get; set; }
        List<Supplier> list1 = new List<Supplier>();
        private object selectedSupplier;
        public object SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                selectedSupplier = value;
                // RaisePropertyChanged("SelectedItem");
            }
        }
        private DateTime _validationTime = System.DateTime.Today;  //DateTime.Today.Date;
        public DateTime ValidationTime
        {
            get { return _validationTime; }
            set
            {
                _validationTime = value;
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
        #endregion

        #region Constructors
        public PriceProductViewModel()
        {
            SelectedSupplier = new List<Supplier>();
            //GetPrice();
            ListSupplierAutoComplete();
        }
        #endregion

        #region Methods
        public async void GetPrice()
        {
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
            
            if(Supplier == null)
            {
                  _searchRequest = new SearchPriceProduct
                  {
                      supplierList = list1.Select(x => x.id).ToList(),
                      validationTime = ValidationTime
                  };
            } else {
                List<int> list2 = new List<int>();
                list2.Add(Supplier.id);
                _searchRequest = new SearchPriceProduct
                {
                    supplierList = list2,
                    validationTime = ValidationTime
                };
            }
            var response = await apiService.PostPrice<PriceProduct>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/prices",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            productList = (List<PriceProduct>)response.Result;
            Product = new ObservableCollection<PriceProduct>(productList);
            if (Product.Count() == 0)
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
        public ICommand GetPriceData
        {
            get
            {
                return new Command(() =>
                {
                    GetPrice();
                });
            }
        }
        public Command DownloadPdf
        {
            get
            {
                return new Command(async () =>
                {
                    if (Supplier == null)
                    {
                        _searchRequest = new SearchPriceProduct
                        {
                            supplierList = list1.Select(x => x.id).ToList(),
                            validationTime = ValidationTime,
                            format = "PDF"
                        };
                    }
                    else
                    {
                        List<int> list2 = new List<int>();
                        list2.Add(Supplier.id);
                        _searchRequest = new SearchPriceProduct
                        {
                            supplierList = list2,
                            validationTime = ValidationTime,
                            format = "PDF"
                        };
                    }
                    var request = JsonConvert.SerializeObject(_searchRequest);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(request);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/product/prices_log";
                    Debug.WriteLine("********url*************");
                    Debug.WriteLine(url);
                    var response = await httpClient.PostAsync(url, content);

                    PopuPage page1 = new PopuPage();
                    await PopupNavigation.Instance.PushAsync(page1);
                    await Task.Delay(1000);

                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("********result*************");
                    Debug.WriteLine(result);
                    if (!response.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                        return;
                    }
                    var pdf = JsonConvert.DeserializeObject<PdfPriceProduct>(result);

                    byte[] bytes = Convert.FromBase64String(pdf.report);
                    MemoryStream stream = new MemoryStream(bytes);

                    if (stream == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Data is Empty", "ok");
                        return;
                    }

                    await DependencyService.Get<ISave>().SaveAndView("prices"+ DateTimeOffset.UtcNow.ToUnixTimeSeconds()+".pdf", "application/pdf", stream);
                });
            }
        }
        public Command DownloadExcel
        {
            get
            {
                return new Command(async () =>
                {
                    if (Supplier == null)
                    {
                        _searchRequest = new SearchPriceProduct
                        {
                            supplierList = list1.Select(x => x.id).ToList(),
                            validationTime = ValidationTime,
                            format = "XLSX"
                        };
                    }
                    else
                    {
                        List<int> list2 = new List<int>();
                        list2.Add(Supplier.id);
                        _searchRequest = new SearchPriceProduct
                        {
                            supplierList = list2,
                            validationTime = ValidationTime,
                            format = "XLSX"
                        };
                    }
                    var request = JsonConvert.SerializeObject(_searchRequest);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(request);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/product/prices_log";
                    Debug.WriteLine("********url*************");
                    Debug.WriteLine(url);
                    var response = await httpClient.PostAsync(url, content);

                    PopuPage page1 = new PopuPage();
                    await PopupNavigation.Instance.PushAsync(page1);
                    await Task.Delay(1000);

                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("********result*************");
                    Debug.WriteLine(result);
                    if (!response.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                        return;
                    }
                    var pdf = JsonConvert.DeserializeObject<PdfPriceProduct>(result);

                    byte[] bytes = Convert.FromBase64String(pdf.report);
                    MemoryStream stream = new MemoryStream(bytes);

                    if (stream == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Data is Empty", "ok");
                        return;
                    }

                    await DependencyService.Get<ISave>().SaveAndView("prices" + DateTimeOffset.UtcNow.ToUnixTimeSeconds() + ".xlsx", pdf.defaultExtention, stream);
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
                description = "",
                order = "asc",
                sortedBy = "description"
            };
            var response = await apiService.Post<Supplier>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/supplier/search?sortedBy=description&order=asc",
                  _searchRequest);
            SupplierAutoComplete = (List<Supplier>)response.Result;
            return SupplierAutoComplete;
        }
        #endregion
    }
}
