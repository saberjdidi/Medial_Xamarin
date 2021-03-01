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
    public class TaskTypeViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private ObservableCollection<TaskType> taskTypes;
        private bool isRefreshing;
        private string filter;
        private List<TaskType> taskTypesList;
        bool _isVisibleStatus;
        private bool _showHide = false;
        #endregion

        #region Properties
        public ObservableCollection<TaskType> TaskTypes
        {
            get { return taskTypes; }
            set
            {
                taskTypes = value;
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
        public TaskTypeViewModel()
        {
            //Navigation = _navigation;
            apiService = new ApiServices();
            instance = this;
            GetTaskType();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnSaved", (sender) => {
                GetTaskType();
            });
        }
        #endregion

        #region Sigleton
        static TaskTypeViewModel instance;
        public static TaskTypeViewModel GetInstance()
        {
            if (instance == null)
            {
                return new TaskTypeViewModel();
            }

            return instance;
        }

        public void Update(TaskType taskType)
        {
            IsRefreshing = true;
            var oldtaskType = taskTypesList
                .Where(p => p.id == taskType.id)
                .FirstOrDefault();
            oldtaskType = taskType;
            TaskTypes = new ObservableCollection<TaskType>(taskTypesList);
            IsRefreshing = false;
        }
        public async Task Delete(TaskType taskType)
        {
            IsRefreshing = true;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            var response = await apiService.Delete<TaskType>(
                "https://app.smart-path.it",
                "/md-core",
                "/medial/taskType",
                taskType.id);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            taskTypesList.Remove(taskType);
            TaskTypes = new ObservableCollection<TaskType>(taskTypesList);

            IsRefreshing = false;
        }
        #endregion

        #region Methods
        public async void GetTaskType()
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

            var response = await apiService.Post<TaskType>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/taskType/search?sortedBy=description&order=asc",
                  _searchRequest);
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            taskTypesList = (List<TaskType>)response.Result;
            TaskTypes = new ObservableCollection<TaskType>(taskTypesList);
            IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetTaskType);
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
                TaskTypes = new ObservableCollection<TaskType>(taskTypesList);
                IsVisibleStatus = false;
            }
            else
            {
                TaskTypes = new ObservableCollection<TaskType>(
                      taskTypesList.Where(
                          l => l.code.ToLower().StartsWith(Filter.ToLower()) ||
                             l.description.ToLower().StartsWith(Filter.ToLower())));

                if (TaskTypes.Count() == 0)
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
