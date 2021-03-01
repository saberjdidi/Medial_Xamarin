using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Client
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string district { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string fiscalCode { get; set; }
        public string iva { get; set; }
        public string note { get; set; }
        public string clientTaskLog { get; set; }
        public Category category { get; set; }
        public Province province { get; set; }
        public Country country { get; set; }
        public Groupe groupe { get; set; }
        public CommercialDetails commercialDetails { get; set; }
        #endregion

        #region Constructors
        public Client()
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
                "Are you sure to delete this Client ?");
            if (!response)
            {
                return;
            }

            await ClientsViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
