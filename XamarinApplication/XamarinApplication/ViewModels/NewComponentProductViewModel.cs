using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class NewComponentProductViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Properties
        private Product product;
        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public NewComponentProductViewModel()
        {
            ListProductAutoComplete();
        }
        #endregion

        #region Autocomplete
        //Product
        private List<Product> _productAutoComplete;
        public List<Product> ProductAutoComplete
        {
            get { return _productAutoComplete; }
            set
            {
                _productAutoComplete = value;
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
            ProductAutoComplete = (List<Product>)response.Result;
            return ProductAutoComplete;
        }
        #endregion
    }
}
