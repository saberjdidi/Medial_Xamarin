using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class UpdateMeasureUnitViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private MeasureUnit _measureUnit;
        #endregion

        #region Constructors
        public UpdateMeasureUnitViewModel()
        {

        }
        #endregion

        #region Properties
        public MeasureUnit MeasureUnit
        {
            get { return _measureUnit; }
            set
            {
                _measureUnit = value;
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
        public async void EditMeasureUnit()
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

            if (string.IsNullOrEmpty(MeasureUnit.code) || string.IsNullOrEmpty(MeasureUnit.description))
            {
                Value = true;
                return;
            }

            var category = new MeasureUnit
            {
                id = MeasureUnit.id,
                code = MeasureUnit.code,
                description = MeasureUnit.description
            };
            var response = await apiService.Put<MeasureUnit>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/measureUnit",
                  category);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            MeasureUnitViewModel.GetInstance().Update(category);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Measure Unit Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateMeasureUnit
        {
            get
            {
                return new Command(() =>
                {
                    EditMeasureUnit();
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
                });
            }
        }
        #endregion
    }
}
