using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
   public class CalendarAgentsViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Client client;
        private ObservableCollection<User> agents;
        private List<User> agentsList;
        private bool isRefreshing;
        bool _isVisibleStatus;
        #endregion

        #region Constructors
        public CalendarAgentsViewModel()
        {
            GetAgents();
        }
        #endregion

        #region Properties
        public Client Client
        {
            get { return client; }
            set
            {
                client = value;
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
        public ObservableCollection<User> Agents
        {
            get { return agents; }
            set
            {
                agents = value;
                OnPropertyChanged();
            }
        }
        public bool IsRefreshing
        {
            get
            {
                return isRefreshing;
            }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public bool IsVisibleStatus
        {
            get { return _isVisibleStatus; }
            set
            {
                _isVisibleStatus = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Method
        public async void GetAgents()
        {
            IsRefreshing = true;
            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var search = new Checked
            {
                
            };
            var response = await apiService.PostAgents<User>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/user/agents",
                  search);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            agentsList = (List<User>)response.Result;
            Agents = new ObservableCollection<User>(agentsList);

            if (Agents.Count() == 0)
            {
                IsVisibleStatus = true;
            }
            else
            {
                IsVisibleStatus = false;
            }
            IsRefreshing = false;
        }
        #endregion

        #region Command
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetAgents);
            }
        }
        #endregion
    }
    #region Model
     public class Checked
    {
       // public bool checked {get; set;}
    }
    #endregion
}
