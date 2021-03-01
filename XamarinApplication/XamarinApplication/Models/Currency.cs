using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Currency
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public string entity { get; set; }
        public string currency { get; set; }
        public string alphabeticCode { get; set; }
        public string numericCode { get; set; }
        public string minorUnit { get; set; }
        public string withdrawalDate { get; set; }
        public string remark { get; set; }
        #endregion

        #region Constructors
        public Currency()
        {
            dialogService = new DialogService();
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
                "Are you sure to delete this Currency ?");
            if (!response)
            {
                return;
            }

            await CurruncyViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
