using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class MeasureUnitViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<MeasureUnit>measureUnits;
        private bool isRefreshing;
        private string filter;
        private List<MeasureUnit> measureUnitsList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<MeasureUnit> MeasureUnits
        {
            get { return measureUnits; }
            set
            {
                measureUnits = value;
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
        public bool IsVisibleStatus
        {
            get { return _isVisibleStatus; }
            set
            {
                _isVisibleStatus = value;
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
        public MeasureUnitViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetMeasureUnits();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetMeasureUnits();
            });
        }
        #endregion

        #region Sigleton
        static MeasureUnitViewModel instance;
        public static MeasureUnitViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MeasureUnitViewModel();
            }

            return instance;
        }

        public void Update(MeasureUnit measureUnit)
        {
            IsRefreshing = true;
            var oldmeasureUnit = measureUnitsList
                .Where(p => p.id == measureUnit.id)
                .FirstOrDefault();
            oldmeasureUnit = measureUnit;
            MeasureUnits = new ObservableCollection<MeasureUnit>(measureUnitsList);
            IsRefreshing = false;
        }
        public async Task Delete(MeasureUnit measureUnit)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<MeasureUnit>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/measureUnit",
                measureUnit.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            measureUnitsList.Remove(measureUnit);
            MeasureUnits = new ObservableCollection<MeasureUnit>(measureUnitsList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetMeasureUnits()
        {
            IsRefreshing = true;
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

            var response = await apiService.Post<MeasureUnit>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/measureUnit/search?sortedBy=code&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            measureUnitsList = (List<MeasureUnit>)response.Result;
            MeasureUnits = new ObservableCollection<MeasureUnit>(measureUnitsList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetMeasureUnits);
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
                MeasureUnits = new ObservableCollection<MeasureUnit>(measureUnitsList);
                IsVisibleStatus = false;
            }
            else
            {
                MeasureUnits = new ObservableCollection<MeasureUnit>(
                      measureUnitsList.Where(
                          l => l.code.ToLower().StartsWith(Filter.ToLower()) ||
                             l.description.ToLower().StartsWith(Filter.ToLower())));

                if (MeasureUnits.Count() == 0)
                {
                    IsVisibleStatus = true;
                }
                else
                {
                    IsVisibleStatus = false;
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
        #endregion
    }
}
