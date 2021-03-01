using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Offer
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        public int id { get; set; }
        public Client client { get; set; }
        public int number { get; set; }
        public string supplyCondition { get; set; }
        public string paymentCondition { get; set; }
        public string note { get; set; }
        public string name { get; set; }
        public string mimeType { get; set; }
        public DateTime date { get; set; }
        public User createdBy { get; set; }
        #endregion

        #region Constructors
        public Offer()
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
                "Are you sure to delete this Offer ?");
            if (!response)
            {
                return;
            }

            await OfferViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
