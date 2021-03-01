using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class ComponentProductViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        private ObservableCollection<Product> productObservableCollection;
        private List<Product> productList;
        private Product product;
        private bool isVisible;
        #endregion

        #region Properties
        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
                OnPropertyChanged();
            }
        }
        private Product productAutocomplete;
        public Product ProductAutocomplete
        {
            get { return productAutocomplete; }
            set
            {
                productAutocomplete = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Product> ProductObservableCollection
        {
            get { return productObservableCollection; }
            set
            {
                productObservableCollection = value;
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
        public ComponentProductViewModel()
        {
            GetComponent();

            ListProductAutoComplete();
        }
        #endregion

        #region Methods
        public async void GetComponent()
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

            var response = await apiService.GetComponentProduct<Product>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product",
                   Product.id,
                 "/components");
            if (!response.IsSuccess)
            {
                IsVisible = true;
                //  await Application.Current.MainPage.DisplayAlert("Warning", "List is Empty", "ok");
                return;
            }
            productList = (List<Product>)response.Result;
            ProductObservableCollection = new ObservableCollection<Product>(productList);
            IsVisible = false;

            if (ProductObservableCollection.Count() == 0)
            {
                IsVisible = true;
            }
            else
            {
                IsVisible = false;
            }
        }

        public async void AddComponent()
        {
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Warning,
                    Languages.CheckConnection,
                    Languages.Ok);
                return;
            }
            var response = await apiService.PutComponent<Product>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product",
                 Product.id,
                 "/components",
                  ProductObservableCollection);
            Debug.WriteLine("********responseIn ViewModel*************");
            Debug.WriteLine(response);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            DependencyService.Get<INotification>().CreateNotification("Medial", "Component updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand SaveComponent
        {
            get
            {
                return new Command(() =>
                {
                    AddComponent();
                });
            }
        }
        public Command AddComponentProduct
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new NewComponentProduct());
                });
            }
        }
        #endregion

        #region Autocomplete
        //Product
        private List<Product> _packagingMethodAutoComplete;
        public List<Product> PackagingMethodAutoComplete
        {
            get { return _packagingMethodAutoComplete; }
            set
            {
                _packagingMethodAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Product>> ListProductAutoComplete()
        {
            var _searchRequest = new SearchProductComponent
            {
               composed = false,
               isActive = true,
               maxResult = 100,
               order = "asc",
               sortedBy = "code"
            }; 
            var response = await apiService.PostComponent<Product>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/product/search/projected?sortedBy=code&order=asc&maxResult=100",
                  _searchRequest);
            PackagingMethodAutoComplete = (List<Product>)response.Result;
            return PackagingMethodAutoComplete;
        }
        #endregion
    }
}
