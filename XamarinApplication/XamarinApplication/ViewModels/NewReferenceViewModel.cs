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
   public class NewReferenceViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Client _client;
        private bool value = false;
        #endregion

        #region Constructor
        public NewReferenceViewModel(Client client)
        {
            apiService = new ApiServices();
        }
        #endregion

        #region Properties
        public Client Client
        {
            get { return _client; }
            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; } 
        public string Note { get; set; }
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
        public async void AddReference()
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
            if (string.IsNullOrEmpty(FistName) || string.IsNullOrEmpty(LastName))
            {
                Value = true;
                return;
            }
            var reference = new AddReference
            {
                firstName = FistName,
                lastName = LastName,
                email = Email,
                phoneNumber = PhoneNumber,
                role = Role,
                note = Note
            };
            await apiService.SaveEvent<AddReference>(
                 "https://app.smart-path.it/",
                 "/md-core",
                 "/medial/client",
                 Client.id,
                 "/references",
                 reference);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Reference Added");
            await Navigation.PopPopupAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveReference
        {
            get
            {
                return new Command(() =>
                {
                    AddReference();
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
