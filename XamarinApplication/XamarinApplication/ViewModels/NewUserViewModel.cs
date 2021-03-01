using Rg.Plugins.Popup.Extensions;
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

namespace XamarinApplication.ViewModels
{
    public class NewUserViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        #endregion

        #region Constructor
        public NewUserViewModel()
        {
            SelectedRole = new List<RoleUser>();
            apiService = new ApiServices();

            roleCollection = new ObservableCollection<RoleUser>();
            roleCollection.Add(new RoleUser() { id = 1, name = "ROLE_ADMIN" });
            roleCollection.Add(new RoleUser() { id = 3, name = "ROLE_USER" });
            roleCollection.Add(new RoleUser() { id = 4, name = "ROLE_CLIENT" });
            roleCollection.Add(new RoleUser() { id = 5, name = "ROLE_AGENT" });
        }
        #endregion

        #region Properties
        public string Username { get; set; }
        public string Password { get; set; }
        private object selectedRole;
        public object SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
            }
        }
        private ObservableCollection<RoleUser> roleCollection;
        public ObservableCollection<RoleUser> RoleCollection
        {
            get { return roleCollection; }
            set { roleCollection = value; }
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
        public async void AddUser()
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
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                Value = true;
                return;
            }
            if(SelectedRole == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Warning,
                    "Role is required",
                    Languages.Ok);
                return;
            }

            var user = new AddUser
            {
                username = Username,
                password = Password,
                roles = SelectedRole
            };
            var response = await apiService.Save<AddUser>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/user",
                  user);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }

            Value = false;
            MessagingCenter.Send((App)Application.Current, "OnSaved");
            DependencyService.Get<INotification>().CreateNotification("Medial", "User Added");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand SaveUser
        {
            get
            {
                return new Command(() =>
                {
                    AddUser();
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

    }
    }
