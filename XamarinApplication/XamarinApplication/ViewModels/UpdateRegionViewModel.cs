using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class UpdateRegionViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Reggion _region;
        #endregion

        #region Constructors
        public UpdateRegionViewModel ()
        {

        }
        #endregion

        #region Properties
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
        public async void EditRegion()
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

            if (string.IsNullOrEmpty(Region.code) || string.IsNullOrEmpty(Region.description))
            {
                Value = true;
                return;
            }

            var region = new Reggion
            {
                id = Region.id,
                code = Region.code,
                description = Region.description
            };
            var response = await apiService.Put<Reggion>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/region",
                  region);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            RegionViewModel.GetInstance().Update(region);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Region Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateRegion
        {
            get
            {
                return new Command(() =>
                {
                    EditRegion();
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
    }
}
