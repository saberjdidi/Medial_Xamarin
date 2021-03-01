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
    public class UpdateTaskStatusViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private TaskStatuss _taskStatus;
        #endregion

        #region Constructors
        public UpdateTaskStatusViewModel()
        {

        }
        #endregion

        #region Properties
        public TaskStatuss TaskStatus
        {
            get { return _taskStatus; }
            set
            {
                _taskStatus = value;
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
        public async void EditTaskStatus()
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

            if (string.IsNullOrEmpty(TaskStatus.code) || string.IsNullOrEmpty(TaskStatus.description))
            {
                Value = true;
                return;
            }

            var taskStatus = new TaskStatuss
            {
                id = TaskStatus.id,
                code = TaskStatus.code,
                description = TaskStatus.description,
                dfault = TaskStatus.dfault
            };
            var response = await apiService.Put<TaskStatuss>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/taskStatus",
                  taskStatus);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            TaskStatusViewModel.GetInstance().Update(taskStatus);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Task Status Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand Update
        {
            get
            {
                return new Command(() =>
                {
                    EditTaskStatus();
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
