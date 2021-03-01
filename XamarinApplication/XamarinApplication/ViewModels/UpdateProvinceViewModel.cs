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
    public class UpdateProvinceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Province _province;
        #endregion

        #region Constructors
        public UpdateProvinceViewModel()
        {
            ListRegionAutoComplete();
        }
        #endregion

        #region Properties
        public Province Province
        {
            get { return _province; }
            set
            {
                _province = value;
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
        public async void EditProvince()
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

            if (string.IsNullOrEmpty(Province.description) || string.IsNullOrEmpty(Province.code))
            {
                Value = true;
                return;
            }
            var province = new Province
            {
                id = Province.id,
                code = Province.code,
                description = Province.description,
                region = Province.region
            };
            var response = await apiService.Put<Province>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/province",
                  province);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            ProvinceViewModel.GetInstance().Update(province);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Province Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateProvince
        {
            get
            {
                return new Command(() =>
                {
                    EditProvince();
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
        //Region
        private List<Reggion> _regionAutoComplete;
        public List<Reggion> RegionAutoComplete
        {
            get { return _regionAutoComplete; }
            set
            {
                _regionAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<Reggion>> ListRegionAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                order = "asc",
                sortedBy = "description"
            };
            var response = await apiService.Post<Reggion>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/region/search?sortedBy=description&order=asc",
                  _searchRequest);
            RegionAutoComplete = (List<Reggion>)response.Result;
            return RegionAutoComplete;
        }
        #endregion
    }
}
