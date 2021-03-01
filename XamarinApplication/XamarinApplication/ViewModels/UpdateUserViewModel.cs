using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class UpdateUserViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private User _user;
        #endregion

        #region Constructors
        public UpdateUserViewModel()
        {
             roleCollection = new ObservableCollection<RoleUser>();
             roleCollection.Add(new RoleUser() { id = 1, name = "ROLE_ADMIN" });
             roleCollection.Add(new RoleUser() { id = 3, name = "ROLE_USER" });
             roleCollection.Add(new RoleUser() { id = 4, name = "ROLE_CLIENT" });
             roleCollection.Add(new RoleUser() { id = 5, name = "ROLE_AGENT" });
           // ListRoleAutoComplete();
        }
        #endregion

        #region Properties
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
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
        public async void EditUser()
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

            if (string.IsNullOrEmpty(User.username))
            {
                Value = true;
                return;
            }

            var user = new User
            {
                id = User.id,
                username = User.username,
                password = User.password,
                roles = User.roles
            };
            var response = await apiService.Put<User>(
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
            UserViewModel.GetInstance().Update(user);
            MessagingCenter.Send((App)Application.Current, "OnSaved");
            DependencyService.Get<INotification>().CreateNotification("Medial", "User Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateUser
        {
            get
            {
                return new Command(() =>
                {
                    EditUser();
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
        //Role
        private List<RoleUser> _roleAutoComplete;
        public List<RoleUser> RoleAutoComplete
        {
            get { return _roleAutoComplete; }
            set
            {
                _roleAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<RoleUser>> ListRoleAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
                order = "asc",
                sortedBy = "name"
            };
            var response = await apiService.Post<RoleUser>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/role/search?sortedBy=name&order=asc",
                  _searchRequest);
            RoleAutoComplete = (List<RoleUser>)response.Result;
            return RoleAutoComplete;
        }
        #endregion
    }
}
