using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Reggion
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public bool IsChecked { get; set; }
        #endregion

        #region Constructors
        public Reggion()
        {
            dialogService = new DialogService();

            IsChecked = false;
        }
        #endregion

        #region Commands
        public ICommand DeleteCommand
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
                "Are you sure to delete this Region ?");
            if (!response)
            {
                return;
            }

            await RegionViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
