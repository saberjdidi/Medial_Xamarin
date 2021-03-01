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
    public class NewProvinceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public NewProvinceViewModel()
        {
            apiService = new ApiServices();
            ListRegionAutoComplete();
        }
        #endregion

        #region Properties
        public string Code { get; set; }
        public string Description { get; set; }
        private Reggion _region = null;
        public Reggion Region
        {
            get { return _region; }
            set
            {
                _region = value;
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
        public async void AddProvince()
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
            if (string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(Code))
            {
                Value = true;
                return;
            }
            var container = new AddProvince
            {
                code = Code,
                description = Description,
                region = Region
            };
            var response = await apiService.Save<AddProvince>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/province",
                  container);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            MessagingCenter.Send((App)Application.Current, "OnSaved");
            DependencyService.Get<INotification>().CreateNotification("Medial", "Province Added");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand SaveProvince
        {
            get
            {
                return new Command(() =>
                {
                    AddProvince();
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
