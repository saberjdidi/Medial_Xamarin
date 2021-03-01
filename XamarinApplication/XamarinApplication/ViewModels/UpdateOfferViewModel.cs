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
    public class UpdateOfferViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Offer _offer;
        #endregion

        #region Constructors
        public UpdateOfferViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

            ListClientAutoComplete();
        }
        #endregion

        #region Properties
        public Offer Offer
        {
            get { return _offer; }
            set
            {
                _offer = value;
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
        public async void EditOffer()
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

            if (string.IsNullOrEmpty(Offer.supplyCondition) || string.IsNullOrEmpty(Offer.paymentCondition) || string.IsNullOrEmpty(Offer.note))
            {
                Value = true;
                return;
            }
            if (Offer.client == null)
            {
                Value = true;
                return;
            }

            var offer = new Offer
            {
                id = Offer.id,
                number = Offer.number,
                client = Offer.client,
                supplyCondition = Offer.supplyCondition,
                paymentCondition = Offer.paymentCondition,
                date = Offer.date,
                note = Offer.note
            };
            var response = await apiService.Put<Offer>(
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
            OfferViewModel.GetInstance().Update(offer);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Offer Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateOffer
        {
            get
            {
                return new Command(() =>
                {
                    EditOffer();
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
