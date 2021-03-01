using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class SupplierProductsViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        private ObservableCollection<Product> products;
        private List<Product> productsList;
        private Supplier supplier;
        private bool isVisible;
        #endregion

        #region Properties
        public Supplier Supplier
        {
            get { return supplier; }
            set
            {
                supplier = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
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
        public SupplierProductsViewModel(Supplier supplier)
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
            var _searchRequest = new SearchRequestBySupplier
            {
                supplier = Supplier
            };

            var response = await apiService.SearchProductBySupplier<Product>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/search?sortedBy=code&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsVisible = true;
                //  await Application.Current.MainPage.DisplayAlert("Warning", "List is Empty", "ok");
                return;
            }
            productsList = (List<Product>)response.Result;
            Products = new ObservableCollection<Product>(productsList);
            IsVisible = false;

            if (Products.Count() == 0)
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
