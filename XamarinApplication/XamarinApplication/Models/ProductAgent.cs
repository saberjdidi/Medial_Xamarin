using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
   public class ProductAgent
    {
        public int id { get; set; }
        //public string availability { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public PackagingMethod packagingMethod { get; set; }
        public MeasureUnit measureUnit { get; set; }
        //public decimal quantityPerPackage { get; set; }
        //public decimal importQuantity { get; set; }
        //public decimal importVolume { get; set; }
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
        public string updateCostDate { get; set; }
        public Cost supplierPurchaseCost { get; set; }
        public decimal costChange { get; set; }
        public bool useCarton { get; set; }
        public bool composed { get; set; }
        public bool component { get; set; }
    }
}
