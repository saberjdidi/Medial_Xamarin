using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class CalendarEventsViewModel : INotifyPropertyChanged
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        #endregion

        #region Attributes
        private ObservableCollection<Event> eventCollection;
        private List<Event> eventsList;
        private Client client;
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
        public ObservableCollection<Event> EventCollection
        {
            get { return eventCollection; }
            set
            {
                eventCollection = value;
                OnPropertyChanged();
            }
        }
        public CalendarEventCollection CalendarInlineEvents { get; set; } = new CalendarEventCollection();
        private bool _enabled = true;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public CalendarEventsViewModel(Client client)
        {
            GetEvents();
        }
        #endregion

        #region Methods
        public async void GetEvents()
        {
            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var response = await apiService.GetEvents<Event>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client",
                   Client.id, 
                 "/eventsDto");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            eventsList = (List<Event>)response.Result;
            // EventCollection = new ObservableCollection<Event>(eventsList);
            // CalendarInlineEvents.Add(new CalendarInlineEvent() { Subject = Event.title, StartTime = DateTime.Today.AddHours(9), EndTime = DateTime.Today.AddHours(10) });
            if (eventsList != null)
                for (int i = 0; i < eventsList.Count; i++)
            CalendarInlineEvents.Add(new CalendarInlineEvent()
            {
                Subject = eventsList[i].title,
                StartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(eventsList[i].startsAt),
                EndTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddMilliseconds(eventsList[i].endsAt),
                Color = Color.FromHex(eventsList[i].color.primary)
            });

        }
        #endregion

        #region Commands
        public Command AddEvent
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new NewEventPage(Client));
                });
            }
        }
        public Command Refrerences
        {
            get
            {
                return new Command(() =>
                {
                     PopupNavigation.Instance.PushAsync(new ClientReferencePage(Client));
                });
            }
        }
        public Command Reports
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new ClientReportPage(Client));
                });
            }
        }
        public Command Agents
        {
            get
            {
                return new Command(() =>
                {
                    PopupNavigation.Instance.PushAsync(new CalendarAgentsPage(Client));
                });
            }
        }
        public Command DownloadPdf
        {
            get
            {
                if (eventsList == null)
                {
                    Enabled = false;
                }
                return new Command(async () =>
                {
                   
                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/client/" + Client.id + "/eventsReport?format=pdf";
                    Debug.WriteLine("********url*************");
                    Debug.WriteLine(url);
                    var response = await httpClient.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("********result*************");
                    Debug.WriteLine(result);
                    if (!response.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                        return;
                    }
                    var pdf = JsonConvert.DeserializeObject<PdfClient>(result);

                    byte[] bytes = Convert.FromBase64String(pdf.report);
                    MemoryStream stream = new MemoryStream(bytes);

                    if(stream == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Data is Empty", "ok");
                        return;
                    }

                    await DependencyService.Get<ISave>().SaveAndView(pdf.name, "application/pdf", stream);
                });
            }
        }
        public Command DownloadExcel
        {
            get
            {
                if (eventsList == null)
                {
                    Enabled = false;
                }
                return new Command(async () =>
                {

                    var httpClient = new HttpClient();
                    var url = "https://app.smart-path.it/md-core/medial/client/" + Client.id + "/eventsReport?format=excel";
                    Debug.WriteLine("********url*************");
                    Debug.WriteLine(url);
                    var response = await httpClient.GetAsync(url);
                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("********result*************");
                    Debug.WriteLine(result);
                    if (!response.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                        return;
                    }
                    var pdf = JsonConvert.DeserializeObject<PdfClient>(result);

                    byte[] bytes = Convert.FromBase64String(pdf.report);
                    MemoryStream stream = new MemoryStream(bytes);

                    if (stream == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Data is Empty", "ok");
                        return;
                    }

                    await DependencyService.Get<ISave>().SaveAndView(pdf.name, pdf.defaultExtention, stream);
                });
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region EventStatic
        // Create events  //use in constructor
        /*
        CalendarInlineEvent event1 = new CalendarInlineEvent()
        {
            StartTime = DateTime.Today.AddHours(9),
            EndTime = DateTime.Today.AddHours(10),
            Subject = "Meeting",
            Color = Color.Green
        };
        CalendarInlineEvents.Add(event1);
            */
        #endregion

        public CalendarEventsViewModel()
        {
        }
    }
}
