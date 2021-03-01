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
    public class UpdateClientGroupeViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Groupe _groupe; 
        #endregion

        #region Constructors
        public UpdateClientGroupeViewModel()
        {

        }
        #endregion

        #region Properties
        public Groupe Groupe
        {
            get { return _groupe; }
            set
            {
                _groupe = value;
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
        public async void EditGroupe()
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

            if (string.IsNullOrEmpty(Groupe.code) || string.IsNullOrEmpty(Groupe.description))
            {
                Value = true;
                return;
            }

            var groupe = new Groupe
            {
                id = Groupe.id,
                code = Groupe.code,
                description = Groupe.description
            };
            var response = await apiService.Put<Groupe>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client_groupe",
                  groupe);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            ClientGroupeViewModel.GetInstance().Update(groupe);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Groupe Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateGroupe
        {
            get
            {
                return new Command(() =>
                {
                    EditGroupe();
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
