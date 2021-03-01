using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
   public class Attachment
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public long creationDate { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string mimeType { get; set; }
        #endregion

        #region Constructors
        public Attachment()
        {
            dialogService = new DialogService();
        }
        #endregion

        #region Commands
        public ICommand DownloadReport
        {
            get
            {
                return new RelayCommand(download);
            }
        }

        async void download()
        {
           /* var response = await dialogService.ShowConfirm(
                "Confirm",
                "Are you sure to delete this Client ?");
            if (!response)
            {
                return;
            }*/

            await ClientReportsViewModel.GetInstance().Download(this);
        }
        #endregion
    }
}
