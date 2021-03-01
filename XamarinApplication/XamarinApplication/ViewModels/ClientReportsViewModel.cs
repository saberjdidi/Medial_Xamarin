using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Models;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
   public class ClientReportsViewModel : BaseViewModel
    {
        #region Services
        private ApiServices apiService = new ApiServices();
        DialogService dialogService;
        #endregion

        #region Attributes
        public INavigation Navigation { get; set; }
        private Client client;
        private ObservableCollection<Attachment> attachments;
        private List<Attachment> attachmentsList;
        private bool isRefreshing;
        bool _isVisibleStatus;
        #endregion

        #region Constructors
        public ClientReportsViewModel()
        {
            instance = this;
            GetAttachments();
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
        public ObservableCollection<Attachment> Attachments
        {
            get { return attachments; }
            set
            {
                attachments = value;
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
        public async void GetAttachments()
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

            var response = await apiService.GetEvents<Attachment>(
                 "https://app.smart-path.it",
                 "/md-core",
                 "/medial/client",
                   Client.id,
                 "/attachmentsPerClient");
            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "ok");
                return;
            }
            attachmentsList = (List<Attachment>)response.Result;
            Attachments = new ObservableCollection<Attachment>(attachmentsList);

            if (Attachments.Count() == 0)
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

        #region Sigleton
        static ClientReportsViewModel instance;
        public static ClientReportsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ClientReportsViewModel();
            }

            return instance;
        }

        public async Task Download(Attachment attachment)
        {
            IsRefreshing = true;
            var httpClient = new HttpClient();
            var url = "https://app.smart-path.it/md-core/medial/client/" + Client.id + "/"+ attachment.id + "/attachement/downloadd";
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
            IsRefreshing = false;
        }
        #endregion

        #region Command
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetAttachments);
            }
        }
        #endregion
    }
}
