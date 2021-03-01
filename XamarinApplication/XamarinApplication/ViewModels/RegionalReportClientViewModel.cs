using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
   public class RegionalReportClientViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Constructors
        public RegionalReportClientViewModel()
        {
            GetYears();
            Months = new ObservableCollection<MonthClass>();
            SelectedMonth = new List<int>();
        }
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<int> _years;
        private ObservableCollection<MonthClass> _months;
        private ObservableCollection<Reggion> _regions;
        #endregion

        #region Properties
        public ObservableCollection<int> Years
        {
            get { return _years; }
            set
            {
                _years = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MonthClass> Months
        {
            get { return _months; }
            set
            {
                _months = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Reggion> Regions
        {
            get { return _regions; }
            set
            {
                _regions = value;
                OnPropertyChanged();
            }
        }
        public int Year { get; set; }
        public int Month { get; set; }
        private object selectedMonth;
        public object SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                // RaisePropertyChanged("SelectedItem");
            }
        }
        public MonthClass MonthItem { get; set; }
        private Reggion _region = null;
        public Reggion Region
        {
            get { return _region; }
            set
            {
                _region = value;
                OnPropertyChanged();
            }
        }
        private bool _showMonths = false;
        public bool ShowMonths
        {
            get => _showMonths;
            set
            {
                _showMonths = value;
                OnPropertyChanged();
            }
        }
        private bool _showRegion = false;
        public bool ShowRegion
        {
            get => _showRegion;
            set
            {
                _showRegion = value;
                OnPropertyChanged();
            }
        }
        
        #endregion

        #region Methods
        public async void GetYears()
        {
            var httpClient = new HttpClient();
            var url = "https://app.smart-path.it/md-core/medial/client/export/year?category=-1&group=-1&province=-1&clientId=-1";
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("********result*************");
            Debug.WriteLine(result);
            if (!response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                return;
            }
            var listYears = JsonConvert.DeserializeObject<List<int>>(result);
            Years = new ObservableCollection<int>(listYears);
        }
        public async void GetMonths()
        {
            List<int> listRegion = new List<int>();
            listRegion.Add(1);
            var _search = new SearchMonth
            {
                clientCategory = -1,
                clientGroup = -1,
                clientId = -1,
                clientProvince = -1,
                year = Year,
                cumulSheet = false,
                regionIds = listRegion
            };
            var requestJson = JsonConvert.SerializeObject(_search);
            Debug.WriteLine("********request*************");
            Debug.WriteLine(requestJson);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            var url = "https://app.smart-path.it/md-core/medial/client/export/months";
            var response = await httpClient.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("********result*************");
            Debug.WriteLine(result);
            if (!response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                return;
            }
            var listMonths = JsonConvert.DeserializeObject<List<int>>(result);
            // Months = new ObservableCollection<int>(listMonths);
            if (listMonths.Count == 4)
            {
                MonthClass month1 = new MonthClass()
                {
                    value = "January",
                    key = listMonths[0]
                };
                Months.Add(month1);
                MonthClass month2 = new MonthClass()
                {
                    value = "Fubruary",
                    key = listMonths[1]
                };
                Months.Add(month2);
                MonthClass month3 = new MonthClass()
                {
                    value = "March",
                    key = listMonths[2]
                };
                Months.Add(month3);
                MonthClass month4 = new MonthClass()
                {
                    value = "April",
                    key = listMonths[3]
                };
                Months.Add(month4);
            }
            else {
              MonthClass month1 = new MonthClass()
              {
                  value = "January",
                  key = listMonths[0]
              };
              Months.Add(month1);
              MonthClass month2 = new MonthClass()
              {
                  value = "Fubruary",
                  key = listMonths[1]
              };
              Months.Add(month2);
              MonthClass month3 = new MonthClass()
              {
                  value = "March",
                  key = listMonths[2]
              };
              Months.Add(month3);
              MonthClass month4 = new MonthClass()
              {
                  value = "April",
                  key = listMonths[3]
              };
              Months.Add(month4);
              MonthClass month5 = new MonthClass()
              {
                  value = "May",
                  key = listMonths[4]
              };
              Months.Add(month5);
              MonthClass month6 = new MonthClass()
              {
                  value = "June",
                  key = listMonths[5]
              };
              Months.Add(month6);
              MonthClass month7 = new MonthClass()
              {
                  value = "July",
                  key = listMonths[6]
              };
              Months.Add(month7);
              MonthClass month8 = new MonthClass()
              {
                  value = "Aout",
                  key = listMonths[7]
              };
              Months.Add(month8);
              MonthClass month9 = new MonthClass()
              {
                  value = "September",
                  key = listMonths[8]
              };
              Months.Add(month9);
              MonthClass month10 = new MonthClass()
              {
                  value = "October",
                  key = listMonths[9]
              };
              Months.Add(month10);
              MonthClass month11 = new MonthClass()
              {
                  value = "November",
                  key = listMonths[10]
              };
              Months.Add(month11);
              MonthClass month12 = new MonthClass()
              {
                  value = "December",
                  key = listMonths[11]
              };
              Months.Add(month12);
            }
            Debug.WriteLine("********Months*************");
              Debug.WriteLine(Months);
        }
        public async void GetRegions()
        {
            List<int> listMonth = new List<int>();
            listMonth.Add(MonthItem.key);
            var _search = new SearchRegion
            {
                clientCategory = -1,
                clientGroup = -1,
                clientId = -1,
                clientProvince = -1,
                year = Year,
                cumulSheet = false,
                months = listMonth
            };
            var requestJson = JsonConvert.SerializeObject(_search);
            Debug.WriteLine("********request*************");
            Debug.WriteLine(requestJson);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            var url = "https://app.smart-path.it/md-core/medial/client/export/regions";
            var response = await httpClient.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("********result*************");
            Debug.WriteLine(result);
            if (!response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                return;
            }
            var listRegions = JsonConvert.DeserializeObject<List<Reggion>>(result);
            Regions = new ObservableCollection<Reggion>(listRegions);
        }
            #endregion

        #region Commands
        public ICommand GenerateReport
        {
            get
            {
                return new Command(async () =>
                {
                    var dateNow = DateTime.Now.ToString("dd-MM-yyyy");
                    if (Region == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Select Region", "ok");
                        return;
                    }
                    List<int> listRegion = new List<int>();
                    listRegion.Add(Region.id);
                    List<int> listMonth = new List<int>();
                    listMonth.Add(MonthItem.key);
                    var _search = new SearchReport
                    {
                        year = Year,
                        cumulSheet = false,
                        regionIds = listRegion,
                        months = listMonth
                    };
                    var requestJson = JsonConvert.SerializeObject(_search);
                    Debug.WriteLine("********request*************");
                    Debug.WriteLine(requestJson);
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/client/generate";
                    Debug.WriteLine("********url*************");
                    Debug.WriteLine(url);
                    var response = await httpClient.PostAsync(url, content);
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("********result*************");
                    Debug.WriteLine(result);
                    if (!response.IsSuccessStatusCode)
                    {
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

                    await DependencyService.Get<ISave>().SaveAndView("selling_data-" + dateNow + ".xlsx", pdf.defaultExtention, stream);
                });
            }
        }
        public Command ClosePopup
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PopPopupAsync();
                    //App.Current.MainPage.Navigation.PopPopupAsync(true);
                    Debug.WriteLine("********Close*************");
                });
            }
        }
        public ICommand MonthsCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Years == null)
                    {
                        Application.Current.MainPage.DisplayAlert("Alert", "Please Select Year", "ok");
                        return;
                    }
                    else
                    {
                        GetMonths();
                        ShowMonths = true;
                    }
                });
            }
        }
        public ICommand RegionCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (Months == null)
                    {
                        Application.Current.MainPage.DisplayAlert("Alert", "Please Select Month", "ok");
                        return;
                    }
                    else
                    {
                        GetRegions();
                        ShowRegion = true;
                    }
                });
            }
        }
        //Popup Region
        public ICommand ButtonClicked
        {
            get
            {
                return new Command(async() =>
                {
                    await Navigation.PopPopupAsync();

                    var result = Regions.Where(w => w.IsChecked == true).ToList();

                    string s = "";

                    int index = 0;
                    foreach (var model in result)
                    {
                        s = s + model.id;
                        if (index < result.Count - 1)
                        {
                            s = s + ",";
                        }
                        index++;
                    }
                    Debug.WriteLine("********s*************");
                    Debug.WriteLine(s);
                   // MessagingCenter.Send<object, string>(this, "Hi", s);
                });
            }
        }
        #endregion
    }
    #region Model
      public class SearchMonth
    {
        public int clientCategory { get; set; }
        public int clientGroup { get; set; }
        public int clientId { get; set; }
        public int clientProvince { get; set; }
        public int year { get; set; }
        public List<int> regionIds { get; set; }
        public bool cumulSheet { get; set; }
    }
    public class SearchRegion
    {
        public int clientCategory { get; set; }
        public int clientGroup { get; set; }
        public int clientId { get; set; }
        public int clientProvince { get; set; }
        public int year { get; set; }
        //public List<int> regionIds { get; set; }
        public object months { get; set; }
        public bool cumulSheet { get; set; }
    }
    public class SearchReport
    {
        public int year { get; set; }
        public List<int> regionIds { get; set; }
        public List<int> months { get; set; }
        public bool cumulSheet { get; set; }
    }
    public class MonthClass
    {
        public int key { get; set; }
        public string value { get; set; }
    }
    #endregion
}
