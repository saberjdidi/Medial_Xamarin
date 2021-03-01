using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class ReportsViewModel : INotifyPropertyChanged
    {
        #region Services
        private ApiServices apiService;
        #endregion

        #region Attributes
        private ObservableCollection<PostedReport> reports;
        private bool isRefreshing;
        private List<PostedReport> reportsList;
        private bool isVisible;
        //private Command<object> changeItemsSource;
        #endregion

        #region Properties
        public ObservableCollection<PostedReport> Reports
        {
            get { return reports; }
            set
            {
                reports = value;
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

        /*public Command<object> ChangeItemsSource
        {
            get { return changeItemsSource; }
            set { this.changeItemsSource = value; }
        }*/
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                this.isVisible = value;
                this.RaisePropertyChanged("IsVisible");
                //OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public ReportsViewModel()
        {
            apiService = new ApiServices();
            //isVisible = true;
            //ChangeItemsSource = new Command<object>(OnChangeItemsSource);
            GetReports();
        }
        #endregion
        /*private void OnChangeItemsSource(object obj)
        {
            if (IsVisible)
            {
                IsVisible = false;
                GetReports();
            }
            else
            {
               // GetReports.Clear();
                IsVisible = true;
            }
        }
        public void GetListOrEmpty()
        {
            if (IsVisible)
            {
                IsVisible = true;
            }
            else
            {
                
                IsVisible = false;
                GetReports();
            }
        }
             */
        #region Methods
        public async void GetReports()
        {
            IsRefreshing = true;
           // IsVisible = true;
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

            var response = await apiService.GetList<PostedReport>(
                 "https://portalesp.smart-path.it",
                 "/Portalesp",
                 "/postedReports/getPostedReports");
            if (!response.IsSuccess)
            {
                IsVisible = true;
                IsRefreshing = false;
            //  await Application.Current.MainPage.DisplayAlert("Warning", "List is Empty", "ok");
                return;
            }
            reportsList = (List<PostedReport>)response.Result;
            Reports = new ObservableCollection<PostedReport>(reportsList);
            IsVisible = false;
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetReports);
            }
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void RaisePropertyChanged(String name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
