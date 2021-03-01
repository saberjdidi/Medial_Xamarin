using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
   public class Events
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public string title { get; set; }
        public string note { get; set; }
        public Client client { get; set; }
        public TaskStatuss status { get; set; }
        public TaskType type { get; set; }
        public double startDate { get; set; }
        public double endDate { get; set; }
        public string colorPrimary { get; set; }
        public User createdBy { get; set; }
        #endregion

        #region Constructors
        public Events()
        {
            dialogService = new DialogService();
        }
        #endregion

        #region Commands
        public ICommand DeleteEvent
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }

        async void Delete()
        {
            var response = await dialogService.ShowConfirm(
                "Confirm",
                "Are you sure to delete this Event ?");
            if (!response)
            {
                return;
            }

            await UpdateClientViewModel.GetInstance().DeleteEvent(this);
        }
        #endregion
    }
}
