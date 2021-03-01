using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
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
   public class UpdateEventViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Events _events;
        #endregion

        #region Constructors
        public UpdateEventViewModel()
        {
            ListStatusAutoComplete();
            ListTypeAutoComplete();
        }
        #endregion

        #region Properties
        public Events Events
        {
            get { return _events; }
            set
            {
                _events = value;
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
        public async void EditEvent()
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

            if (string.IsNullOrEmpty(Events.title))
            {
                Value = true;
                return;
            }
            var events = new Events
            {
                id = Events.id,
                title = Events.title,
                client = Events.client,
                colorPrimary = Events.colorPrimary,
                startDate = Events.startDate,
                endDate = Events.endDate,
                status = Events.status,
                type = Events.type,
                note = Events.note,
                createdBy = Events.createdBy
            };
            await apiService.PutEvent<Events>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client/" + Events.client.id + "/admin/events",
                  events);
           /* if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }*/
            Value = false;
            UpdateClientViewModel.GetInstance().Update(events);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Event Updated");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
        #endregion

        #region Commands
        public ICommand UpdateEvent
        {
            get
            {
                return new Command(() =>
                {
                    EditEvent();
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
        //Task Status
        private List<TaskStatuss> _statusAutoComplete;
        public List<TaskStatuss> StatusAutoComplete
        {
            get { return _statusAutoComplete; }
            set
            {
                _statusAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<TaskStatuss>> ListStatusAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
            };
            var response = await apiService.Post<TaskStatuss>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/taskStatus/search?sortedBy=description&order=asc",
                  _searchRequest);
            StatusAutoComplete = (List<TaskStatuss>)response.Result;
            return StatusAutoComplete;
        }
        //Task Type
        private List<TaskType> _typeAutoComplete;
        public List<TaskType> TypeAutoComplete
        {
            get { return _typeAutoComplete; }
            set
            {
                _typeAutoComplete = value;
                OnPropertyChanged();
            }
        }
        public async Task<List<TaskType>> ListTypeAutoComplete()
        {
            var _searchRequest = new SearchRequest
            {
            };
            var response = await apiService.Post<TaskType>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/taskType/search?sortedBy=description&order=asc",
                  _searchRequest);
            TypeAutoComplete = (List<TaskType>)response.Result;
            return TypeAutoComplete;
        }

        #endregion
    }
}
