using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Resources;
using XamarinApplication.Services;

namespace XamarinApplication.ViewModels
{
    public class SearchRequestViewModel : INotifyPropertyChanged
    {
        public SearchRequestViewModel()
        {
            apiService = new ApiServices();
            StatusList = GetStatus().OrderBy(t => t.name).ToList();

            Task.Run(async () =>
            {
                Requests = new InfiniteScrollCollection<Request>
                {
                    OnLoadMore = async () =>
                    {
                        //IsRefreshing = true;
                        // load the next page
                        var page = Requests.Count / PageSize;

                        var _searchModel = new SearchModel
                        {
                            maxResult = 200,
                            order = "desc",
                            sortedBy = "request_creation_date",
                            date = CheckDateFrom,
                            date1 = CheckDateTo,
                            status = SelectedStatus.name
                        };
                        var cookie = Settings.Cookie;
                        var res = cookie.Substring(11, 32);
                        var response = await apiService.PostRequest<Request>(
                    "https://portalesp.smart-path.it",
                    "/Portalesp",
                    "/request/searchRequest?mobile=mobile",
                    res,
                    _searchModel);
                        requestsList = (List<Request>)response.Result;
                        //IsRefreshing = false;
                        return requestsList;
                    },
                    OnCanLoadMore = () =>
                    {
                        return Requests.Count < 100 * PageSize;
                        // return Requests.Count < TotalCount;
                    }
                };
                // GetRequests();
                await Requests.LoadMoreAsync();
            });
        }

        public List<Status> StatusList { get; set; }
        private Status _selectedStatus { get; set; }
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    // Do whatever functionality you want...When a selectedItem is changed..
                    // write code here..
                    //Resource.Culture = new CultureInfo(_selectedStatus.name);
                }
            }
        }

        public List<Status> GetStatus()
        {
            var status = new List<Status>()
            {
                new Status(){name= ""},
                new Status(){name= "CH"},
                new Status(){name= "SV"},
                new Status(){name= "SE"},
                new Status(){name= "TC"},
                new Status(){name= "VL"},
                new Status(){name= "SI"},
                new Status(){name= "NS"}
            };

            return status;
        }

        #region Services
        private ApiServices apiService;
        #endregion
        public EventHandler<DialogResult> OnDialogClosed;
        private DateTime from = System.DateTime.Now;  //DateTime.Today.Date;
        private DateTime to = System.DateTime.Now;
        private ObservableCollection<Request> requestsObservable;
        private InfiniteScrollCollection<Request> requests;
        private List<Request> requestsList;
        private const int PageSize = 10;
        private bool isRefreshing;
        public DateTime CheckDateFrom {
            get { return from; }
            set
            {
                from = value;
                OnPropertyChanged();
            }
        }
        public DateTime CheckDateTo {
            get { return to; }
            set
            {
                to = value;
                OnPropertyChanged("to");
            }
        }

        public ObservableCollection<Request> RequestsObservable
        {
            get { return requestsObservable; }
            set
            {
                requestsObservable = value;
                OnPropertyChanged();
            }
        }
        public InfiniteScrollCollection<Request> Requests
        {
            get { return requests; }
            set
            {
                requests = value;
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
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(GetRequestsSearch);
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    GetRequestsSearch();
                });
            }
        }

        public async void GetRequestsSearch()
        {
           // IsRefreshing = true;
            var _searchModel = new SearchModel
            {
                maxResult = 200,
                order = "desc",
                sortedBy = "request_creation_date",
                date = CheckDateFrom,
                date1 = CheckDateTo,
                status = SelectedStatus.name
            };
            var cookie = Settings.Cookie;
            var res = cookie.Substring(11, 32);
            var response = await apiService.PostRequest<Request>(
            "https://portalesp.smart-path.it",
            "/Portalesp",
            "/request/searchRequest?mobile=mobile",
            res,
            _searchModel);
            requestsList = (List<Request>)response.Result;
            // RequestsObservable = new ObservableCollection<Request>(requestsList);
            Requests.AddRange(requestsList);
            //OnDialogClosed?.Invoke(this, new DialogResult { Success = true, Message = "send Data", RequestsPopup = Requests });
           // IsRefreshing = false;
            MessagingCenter.Send(new DialogResult() { RequestsPopup = Requests }, "PopUpData");
            await App.Current.MainPage.Navigation.PopPopupAsync(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Status
    {
        public string name { get; set; }
    }
    public class DialogResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public InfiniteScrollCollection<Request> RequestsPopup { get; set; }
    }
}
