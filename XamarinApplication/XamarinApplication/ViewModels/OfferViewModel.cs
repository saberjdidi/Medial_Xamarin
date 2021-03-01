using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class OfferViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<Offer> offers;
        private bool isRefreshing;
        private string filter;
        private List<Offer> offersList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        private SearchOffer _searchOffer;
        #endregion

        #region Properties
        public ObservableCollection<Offer> Offers
        {
            get { return offers; }
            set
            {
                offers = value;
                OnPropertyChanged();
            }
        }
        private User _user = null;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
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
        public OfferViewModel()
        {
            apiService = new ApiServices();
            instance = this;
            GetOffers();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetOffers();
            });
        }
        #endregion

        #region Sigleton
        static OfferViewModel instance;
        public static OfferViewModel GetInstance()
        {
            if (instance == null)
            {
                return new OfferViewModel();
            }

            return instance;
        }

        public void Update(Offer offer)
        {
            IsRefreshing = true;
            var oldOffer = offersList
                .Where(p => p.id == offer.id)
                .FirstOrDefault();
            oldOffer = offer;
            Offers = new ObservableCollection<Offer>(offersList);
            IsRefreshing = false;
        }
        public async Task Delete(Offer offer)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<Offer>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/offer",
                offer.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            offersList.Remove(offer);
            Offers = new ObservableCollection<Offer>(offersList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetOffers()
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();
            var Username = Settings.Username;
            User = JsonConvert.DeserializeObject<User>(Username);

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

            if (User.roles.Select(r => r.name).FirstOrDefault().Equals("ROLE_ADMIN"))
            {
                _searchOffer = new SearchOffer
                {
                    order = "asc",
                    sortedBy = "number"
                };
            }
            if (User.roles.Select(r => r.name).FirstOrDefault().Equals("ROLE_AGENT"))
            {
                _searchOffer = new SearchOffer
                {
                    order = "asc",
                    sortedBy = "number",
                    createdBy = User
                };
            }

            var response = await apiService.PostOffer<Offer>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/offer/search?sortedBy=number&order=asc",
                  _searchOffer);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            offersList = (List<Offer>)response.Result;
            Offers = new ObservableCollection<Offer>(offersList);
            IsRefreshing = false;
            if (Offers.Count() == 0)
            {
                IsVisibleStatus = true;
            }
            else
            {
                IsVisibleStatus = false;
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetOffers);
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
                Offers = new ObservableCollection<Offer>(offersList);
                IsVisibleStatus = false;
            }
            else
            {
                Offers = new ObservableCollection<Offer>(
                      offersList.Where(
                          l => l.supplyCondition.ToLower().StartsWith(Filter.ToLower()) ||
                             l.paymentCondition.ToLower().StartsWith(Filter.ToLower()))); 
            }
            if (Offers.Count() == 0)
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

    #region Model
      public class SearchOffer
    {
        public string order { get; set; }
        public string sortedBy { get; set; }
        public User createdBy { get; set; }
    }
    #endregion
}
