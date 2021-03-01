using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using XamarinApplication.Services;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Models
{
    public class Product
    {
        #region Services
        DialogService dialogService;
        #endregion

        #region Properties
        private string s_description;
        public int id { get; set; }
        public string availability { get; set; }
        public string code { get; set; }
        //public string description { get; set; }
        public string description
        {
            get { return s_description; }
            set { this.s_description = value; }
        }
        public PackagingMethod packagingMethod { get; set; }
        public MeasureUnit measureUnit { get; set; }
        public decimal quantityPerPackage { get; set; }
        public decimal importQuantity { get; set; }
        public decimal importVolume { get; set; }
        public Supplier supplier { get; set; }
        //public Cost purchaseCost { get; set; }
        public PackagingCost packagingCost { get; set; }
        public double cartonVolume { get; set; }
        public Price price { get; set; }
        public string note { get; set; }
        public CustomsDuty customsDuty { get; set; }
        public PurchaseCost purchaseCost { get; set; }
        public decimal width { get; set; }
        public decimal length { get; set; }
        public decimal height { get; set; }
        public bool active { get; set; }
        public bool fob { get; set; }
        public DateTime? updateCostDate { get; set; }
        public Cost supplierPurchaseCost { get; set; }
        public decimal costChange { get; set; }
        public bool useCarton { get; set; }
        public bool composed { get; set; }
        public bool component { get; set; }
        #endregion

        #region Constructors
        public Product()
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
                "Are you sure to delete this Product ?");
            if (!response)
            {
                return;
            }

            await ProductsViewModel.GetInstance().Delete(this);
        }
        #endregion
    }
}
