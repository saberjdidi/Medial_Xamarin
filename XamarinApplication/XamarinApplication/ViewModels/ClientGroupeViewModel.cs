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
    public class ClientGroupeViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<Groupe> groupes;
        private bool isRefreshing;
        private string filter;
        private List<Groupe> groupesList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<Groupe> Groupes
        {
            get { return groupes; }
            set
            {
                groupes = value;
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
        public ClientGroupeViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetClientGroupe();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetClientGroupe();
            });
        }
        #endregion

        #region Sigleton
        static ClientGroupeViewModel instance;
        public static ClientGroupeViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ClientGroupeViewModel();
            }

            return instance;
        }

        public void Update(Groupe groupe)
        {
            IsRefreshing = true;
            var oldgroupe = groupesList
                .Where(p => p.id == groupe.id)
                .FirstOrDefault();
            oldgroupe = groupe;
            Groupes = new ObservableCollection<Groupe>(groupesList);
            IsRefreshing = false;
        }
        public async Task Delete(Groupe groupe)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Groupe>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/client_groupe",
                groupe.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            groupesList.Remove(groupe);
            Groupes = new ObservableCollection<Groupe>(groupesList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetClientGroupe()
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

            var response = await apiService.Post<Groupe>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client_groupe/search?sortedBy=code&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            groupesList = (List<Groupe>)response.Result;
            Groupes = new ObservableCollection<Groupe>(groupesList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetClientGroupe);
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
                Groupes = new ObservableCollection<Groupe>(groupesList);
                IsVisibleStatus = false;
            }
            else
            {
                Groupes = new ObservableCollection<Groupe>(
                      groupesList.Where(
                          l => l.code.ToLower().StartsWith(Filter.ToLower()) ||
                               l.description.ToLower().StartsWith(Filter.ToLower())));

                if (Groupes.Count() == 0)
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
