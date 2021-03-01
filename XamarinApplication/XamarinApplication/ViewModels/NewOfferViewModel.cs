using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class NewOfferViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public NewOfferViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            apiService = new ApiServices();

            ListClientAutoComplete();
        }
        #endregion

        #region Properties
        public string SupplyCondition { get; set; }
        public string PaymentCondition { get; set; }
        public string Note { get; set; }
        private Client _client = null;
        public Client Client
        {
            get { return _client; }
            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }
        private DateTime _date = System.DateTime.Today;  //DateTime.Today.Date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
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
        #endregion

        #region Methods
        public async void AddOffer()
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
            if (string.IsNullOrEmpty(SupplyCondition) || string.IsNullOrEmpty(PaymentCondition) || string.IsNullOrEmpty(Note))
            {
                Value = true;
                return;
            }
            if (Client == null)
            {
                Value = true;
                return;
            }

            var offer = new AddOffer
            {
                client = Client,
                date = Date,
                supplyCondition = SupplyCondition,
                paymentCondition = PaymentCondition,
                note = Note
            };
            var response = await apiService.Save<AddOffer>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/offer",
                  offer);
           /* if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }*/

            Value = false;
            MessagingCenter.Send((App)Application.Current, "OnSaved");
            DependencyService.Get<INotification>().CreateNotification("Medial", "Offer Added");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand SaveOffer
        {
            get
            {
                return new Command(() =>
                {
                    AddOffer();
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
        #endregion

        #region Autocomplete
        //Client
        public string Description { get; set; } 
        private List<Client> _clientAutoComplete;
        public List<Client> ClientAutoComplete
        {
            get { return _clientAutoComplete; }
            set
            {
                _clientAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Client>> ListClientAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                description = "a"
            };
            Debug.WriteLine("********_searchRequest*************");
            Debug.WriteLine(_searchRequest);
            var response = await apiService.Post<Client>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client/search?sortedBy=code&order=asc",
                  _searchRequest);
            ClientAutoComplete = (List<Client>)response.Result;
            return ClientAutoComplete;
        }
        #endregion
    }
}
