using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Reference
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string role { get; set; }
        public string note { get; set; }
        public Client client { get; set; }
        #endregion

        #region Constructors
        public Reference()
        {
            dialogService = new DialogService();
        }
        #endregion

        #region Commands
        public ICommand DeleteReference
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
                "Are you sure to delete this Reference ?");
            if (!response)
            {
                return;
            }

            await UpdateClientViewModel.GetInstance().DeleteReference(this);
        }
        #endregion
    }
}
