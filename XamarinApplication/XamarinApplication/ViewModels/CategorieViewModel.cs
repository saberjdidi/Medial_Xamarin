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
    public class CategorieViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<Category> categories;
        private bool isRefreshing;
        private string filter;
        private List<Category> categoriesList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
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
        public CategorieViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetCategory();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetCategory();
            });
        }
        #endregion

        #region Sigleton
        static CategorieViewModel instance;
        public static CategorieViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CategorieViewModel();
            }

            return instance;
        }

        public void Update(Category category)
        {
            IsRefreshing = true;
            var oldcategory = categoriesList
                .Where(p => p.id == category.id)
                .FirstOrDefault();
            oldcategory = category;
            Categories = new ObservableCollection<Category>(categoriesList);
            IsRefreshing = false;
        }
        public async Task Delete(Category category)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Category>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/category",
                category.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            categoriesList.Remove(category);
            Categories = new ObservableCollection<Category>(categoriesList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetCategory()
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

            var response = await apiService.Post<Category>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/category/search?sortedBy=code&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            categoriesList = (List<Category>)response.Result;
            Categories = new ObservableCollection<Category>(categoriesList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetCategory);
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
                Categories = new ObservableCollection<Category>(categoriesList);
                IsVisibleStatus = false;
            }
            else
            {
                Categories = new ObservableCollection<Category>(
                      categoriesList.Where(
                          l => l.code.ToLower().StartsWith(Filter.ToLower()) ||
                             l.description.ToLower().StartsWith(Filter.ToLower())));

            }
            if (Categories.Count() == 0)
            {
                IsVisibleStatus = true;
            }
            else
            {
                IsVisibleStatus = false;
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
