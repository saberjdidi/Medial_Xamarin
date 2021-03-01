using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class ClientProductsViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        private ObservableCollection<SellingDetails> sellingDetails;
        private List<SellingDetails> sellingDetailsList;
        private Client client;
        private bool isVisible;
        #endregion

        #region Properties
        public Client Client
        {
            get { return client; }
            set
            {
                client = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<SellingDetails> SellingDetails
        {
            get { return sellingDetails; }
            set
            {
                sellingDetails = value;
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
        public ClientProductsViewModel(Client client)
        {
            GetProducts();
        }
        #endregion

        #region Methods
        public async void GetProducts()
        {
            //IsVisible = true;
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
            var _searchRequest = new SearchRequestByClient
            {
                client = Client

            };

            var response = await apiService.ClientProducts<SellingDetails>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client",
                   Client.id,
                 "/selling_details/grouped?fromDate=null&toDate=null",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsVisible = true;
                //  await Application.Current.MainPage.DisplayAlert("Warning", "List is Empty", "ok");
                return;
            }
            sellingDetailsList = (List<SellingDetails>)response.Result;
            SellingDetails = new ObservableCollection<SellingDetails>(sellingDetailsList);
            IsVisible = false;

            if (SellingDetails.Count() == 0)
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
            }
        }
        #endregion
    }
}
