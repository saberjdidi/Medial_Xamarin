using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class TaskStatusViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<TaskStatuss> _taskStatus;
        private bool isRefreshing;
        private string filter;
        private List<TaskStatuss> taskStatusList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<TaskStatuss> TaskStatus
        {
            get { return _taskStatus; }
            set
            {
                _taskStatus = value;
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

        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
                Search();
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
        public bool ShowHide
        {
            get => _showHide;
            set
            {
                _showHide = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public TaskStatusViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetTaskStatus();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetTaskStatus();
            });
        }
        #endregion

        #region Sigleton
        static TaskStatusViewModel instance;
        public static TaskStatusViewModel GetInstance()
        {
            if (instance == null)
            {
                return new TaskStatusViewModel();
            }

            return instance;
        }

        public void Update(TaskStatuss taskType)
        {
            IsRefreshing = true;
            var oldtaskType = taskStatusList
                .Where(p => p.id == taskType.id)
                .FirstOrDefault();
            oldtaskType = taskType;
            TaskStatus = new ObservableCollection<TaskStatuss>(taskStatusList);
            IsRefreshing = false;
        }
        public async Task Delete(TaskStatuss taskType)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<TaskStatuss>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/taskStatus",
                taskType.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            taskStatusList.Remove(taskType);
            TaskStatus = new ObservableCollection<TaskStatuss>(taskStatusList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetTaskStatus()
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

            var _searchRequest = new SearchRequest
            {
                order = "asc",
                sortedBy = "description"
            };

            var response = await apiService.Post<TaskStatuss>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/taskStatus/search?sortedBy=description&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            taskStatusList = (List<TaskStatuss>)response.Result;
            TaskStatus = new ObservableCollection<TaskStatuss>(taskStatusList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetTaskStatus);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                ShowHide = false;
                return new RelayCommand(Search);
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(Filter))
            {
                TaskStatus = new ObservableCollection<TaskStatuss>(taskStatusList);
                IsVisibleStatus = false;
            }
            else
            {
                TaskStatus = new ObservableCollection<TaskStatuss>(
                      taskStatusList.Where(
                          l => l.code.ToLower().StartsWith(Filter.ToLower()) ||
                             l.description.ToLower().StartsWith(Filter.ToLower())));

                if (TaskStatus.Count() == 0)
                {
                    IsVisibleStatus = true;
                }
                else
                {
                    IsVisibleStatus = false;
                }
            }
        }

        public ICommand OpenSearchBar
        {
            get
            {
                return new Command(() =>
                {
                    if (ShowHide == false)
                    {
                        ShowHide = true;
                    }
                    else
                    {
                        ShowHide = false;
                    }
                });
            }
        }
        #endregion
    }
}
