using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class NewEventViewModel : BaseViewModel
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
        public NewEventViewModel(Client client)
        {
            apiService = new ApiServices();

            ListStatusAutoComplete();
            ListTypeAutoComplete();
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
        private TaskStatuss _status = null;
        public TaskStatuss TaskStatus
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }
        private TaskType _type = null;
        public TaskType TaskType
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }
        public string Title { get; set; }
        public string Note { get; set; }
        private DateTime _startDate = System.DateTime.Today;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }
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
        public async void AddEvent()
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
            if (string.IsNullOrEmpty(Title))
            {
                Value = true;
                return;
            }
            if (TaskStatus == null || TaskType == null)
            {
                Value = true;
                return;
            }
            var addEvent = new AddEvent
            {
                title = Title,
                note = Note,
                startDate = StartDate,
                endDate = StartDate,
                status = TaskStatus,
                type = TaskType,
                colorPrimary = "#8093F1"
            };
            await apiService.SaveEvent<AddEvent>(
                 "https://app.smart-path.it/",
                 "/md-core",
                 "/medial/client",
                 Client.id,
                 "/null/events",
                 addEvent);

            DependencyService.Get<INotification>().CreateNotification("Medial", "Event Added");
            await Navigation.PopPopupAsync();
        }
        #endregion

        #region Commands
        public ICommand SaveEvent
        {
            get
            {
                return new Command(() =>
                {
                    AddEvent();
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
