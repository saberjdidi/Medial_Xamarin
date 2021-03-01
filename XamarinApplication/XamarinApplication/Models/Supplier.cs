using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Supplier
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
       // [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public Country country { get; set; }
        public string note { get; set; }
        public Containner container { get; set; }
        public CustomsDuty customsDuty { get; set; }
        public bool european { get; set; }
        public bool exporter { get; set; }
        public bool dutyFree { get; set; }
        #endregion

        #region Constructors
        public Supplier()
        {
            dialogService = new DialogService();
        }
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return id;
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
                "Are you sure to delete this Supplier ?");
            if (!response)
            {
                return;
            }

            await SuppliersViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
