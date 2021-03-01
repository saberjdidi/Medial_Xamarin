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
   public class RoleViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<RoleUser> _roles;
        private bool isRefreshing;
        private string filter;
        private List<RoleUser> rolesList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<RoleUser> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
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
        public RoleViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetList();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetList();
            });
        }
        #endregion

        #region Sigleton
        static RoleViewModel instance;
        public static RoleViewModel GetInstance()
        {
            if (instance == null)
            {
                return new RoleViewModel();
            }

            return instance;
        }

        public void Update(RoleUser role)
        {
            IsRefreshing = true;
            var oldrole = rolesList
                .Where(p => p.id == role.id)
                .FirstOrDefault();
            oldrole = role;
            Roles = new ObservableCollection<RoleUser>(rolesList);
            IsRefreshing = false;
        }
        public async Task Delete(RoleUser role)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<RoleUser>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/role",
                role.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            rolesList.Remove(role);
            Roles = new ObservableCollection<RoleUser>(rolesList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetList()
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
                sortedBy = "name"
            };

            var response = await apiService.Post<RoleUser>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/role/search?sortedBy=name&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            rolesList = (List<RoleUser>)response.Result;
            Roles = new ObservableCollection<RoleUser>(rolesList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetList);
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
                Roles = new ObservableCollection<RoleUser>(rolesList);
                IsVisibleStatus = false;
            }
            else
            {
                Roles = new ObservableCollection<RoleUser>(
                      rolesList.Where(l => l.name.ToLower().StartsWith(Filter.ToLower())));

                if (Roles.Count() == 0)
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
