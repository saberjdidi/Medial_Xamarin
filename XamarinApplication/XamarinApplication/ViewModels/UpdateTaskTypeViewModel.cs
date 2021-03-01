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
    public class UpdateTaskTypeViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private TaskType _taskType;
        #endregion

        #region Constructors
        public UpdateTaskTypeViewModel()
        {

        }
        #endregion

        #region Properties
        public TaskType TaskType
        {
            get { return _taskType; }
            set
            {
                _taskType = value;
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
        public async void EditTaskType()
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

            if (string.IsNullOrEmpty(TaskType.code) || string.IsNullOrEmpty(TaskType.description))
            {
                Value = true;
                return;
            }

            var taskType = new TaskType
            {
                id = TaskType.id,
                code = TaskType.code,
                description = TaskType.description,
                businessCard = TaskType.businessCard,
                dfault = TaskType.dfault
            };
            var response = await apiService.Put<TaskType>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/taskType",
                  taskType);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            Value = false;
            TaskTypeViewModel.GetInstance().Update(taskType);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Task Type Updated");
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
                    EditTaskType();
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
