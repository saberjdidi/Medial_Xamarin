using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddProduct
    {
        public AddPurchaseCost purchaseCost { get; set; }
        public AddPackagingCost packagingCost { get; set; }
        public string code { get; set; }
        public string availability { get; set; }
        public string description { get; set; }
        public PackagingMethod packagingMethod { get; set; }
        public MeasureUnit measureUnit { get; set; }
        public decimal quantityPerPackage { get; set; }
        //public string quantityPerPackage { get; set; }
        public decimal importQuantity { get; set; }
        //public string importQuantity { get; set; }
        public decimal importVolume { get; set; }
        //public string importVolume { get; set; }
        public Supplier supplier { get; set; }
        //public Cost purchaseCost { get; set; }
        //public double cartonVolume { get; set; }
        public double cartonVolume { get; set; }
        public string note { get; set; }
        public CustomsDuty customsDuty { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal length { get; set; }
        public bool active { get; set; }
        public bool fob { get; set; }
        public DateTime updateCostDate { get; set; }
        ////public Cost supplierPurchaseCost { get; set; }
        public decimal costChange { get; set; }
        ////public bool useCarton { get; set; }
        public bool composed { get; set; }
        ////public bool component { get; set; }
    }
}
