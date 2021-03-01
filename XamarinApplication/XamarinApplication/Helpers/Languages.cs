using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApplication.Interfaces;
using XamarinApplication.Resources;

namespace XamarinApplication.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }
        public static string Error
        {
            get { return Resource.Error; }
        }
        public static string Ok
        {
            get { return Resource.Ok; }
        }
        public static string UsernameValidation
        {
            get { return Resource.UsernameValidation; }
        }
        public static string PasswordValidation
        {
            get { return Resource.PasswordValidation; }
        }
        public static string Username
        {
            get { return Resource.Username; }
        }
        public static string Password
        {
            get { return Resource.Password; }
        }
        public static string Warning
        {
            get { return Resource.Warning; }
        }
        public static string CheckConnection
        {
            get { return Resource.CheckConnection; }
        }
        public static string FailedLogin
        {
            get { return Resource.FailedLogin; }
        }
        public static string Exit
        {
            get { return Resource.Exit; }
        }
        public static string Yes
        {
            get { return Resource.Yes; }
        }
        public static string No
        {
            get { return Resource.No; }
        }
        public static string European
        {
            get { return Resource.European; }
        }
        public static string Country
        {
            get { return Resource.Country; }
        }
        public static string PurchaseCost
        {
            get { return Resource.PurchaseCost; }
        }
        public static string SearchSupplier
        {
            get { return Resource.SearchSupplier; }
        }
        public static string Note
        {
            get { return Resource.Note; }
        }
        public static string Supplier
        {
            get { return Resource.Supplier; }
        }
        public static string Exporter
        {
            get { return Resource.Exporter; }
        }
        public static string Product
        {
            get { return Resource.Product; }
        }
        public static string Client
        {
            get { return Resource.Client; }
        }
        public static string ShowAll
        {
            get { return Resource.ShowAll; }
        }
        public static string Logout
        {
            get { return Resource.Logout; }
        }
        public static string Date
        {
            get { return Resource.Date; }
        }
        public static string Informations
        {
            get { return Resource.Informations; }
        }
        public static string Suppliers
        {
            get { return Resource.Suppliers; }
        }
        public static string CustomsDuty
        {
            get { return Resource.CustomsDuty; }
        }
        public static string Products
        {
            get { return Resource.Products; }
        }
        public static string Code
        {
            get { return Resource.Code; }
        }
        public static string Name
        {
            get { return Resource.Name; }
        }
        public static string Description
        {
            get { return Resource.Description; }
        }
        public static string Close
        {
            get { return Resource.Close; }
        }
        public static string District
        {
            get { return Resource.District; }
        }
        public static string ClientDetails
        {
            get { return Resource.ClientDetails; }
        }
        public static string ProductDetails
        {
            get { return Resource.ProductDetails; }
        }
        public static string Number
        {
            get { return Resource.Number; }
        }
        public static string StartDate
        {
            get { return Resource.StartDate; }
        }
        public static string EndDate
        {
            get { return Resource.EndDate; }
        }
        public static string PackagingMethod
        {
            get { return Resource.PackagingMethod; }
        }
        public static string MeasureUnit
        {
            get { return Resource.MeasureUnit; }
        }
        public static string SupplierDetails
        {
            get { return Resource.SupplierDetails; }
        }
        public static string Clients
        {
            get { return Resource.Clients; }
        }
        public static string Address
        {
            get { return Resource.Address; }
        }
        public static string PhoneNumber
        {
            get { return Resource.PhoneNumber; }
        }
        public static string FiscalCode
        {
            get { return Resource.FiscalCode; }
        }
        public static string Category
        {
            get { return Resource.Category; }
        }
        public static string Province
        {
            get { return Resource.Province; }
        }
        public static string Loading
        {
            get { return Resource.Loading; }
        }
        public static string Search
        {
            get { return Resource.Search; }
        }
        public static string UsernameRequired
        {
            get { return Resource.UsernameRequired; }
        }
        public static string PasswordRequired
        {
            get { return Resource.PasswordRequired; }
        }
        public static string CommercialSupplier
        {
            get { return Resource.CommercialSupplier; }
        }
        public static string Continue
        {
            get { return Resource.Continue; }
        }
        public static string CommercialProduct
        {
            get { return Resource.CommercialProduct; }
        }
        public static string SimulateChange
        {
            get { return Resource.SimulateChange; }
        }
        public static string ExportAll
        {
            get { return Resource.ExportAll; }
        }
        public static string NoResult
        {
            get { return Resource.NoResult; }
        }
        public static string ExportExcel
        {
            get { return Resource.ExportExcel; }
        }
        public static string Offer
        {
            get { return Resource.Offer; }
        }
        public static string Container
        {
            get { return Resource.Container; }
        }
        public static string Currency
        {
            get { return Resource.Currency; }
        }
        public static string Region
        {
            get { return Resource.Region; }
        }
        public static string User
        {
            get { return Resource.User; }
        }
        public static string Role
        {
            get { return Resource.Role; }
        }
        public static string ClientGroupe
        {
            get { return Resource.ClientGroupe; }
        }
        public static string TaskType
        {
            get { return Resource.TaskType; }
        }
        public static string TaskStatus
        {
            get { return Resource.TaskStatus; }
        }
        public static string New
        {
            get { return Resource.New; }
        }
        public static string Save
        {
            get { return Resource.Save; }
        }
        public static string City
        {
            get { return Resource.City; }
        }
        public static string PaymentCondition
        {
            get { return Resource.PaymentCondition; }
        }
        public static string TransportationCost
        {
            get { return Resource.TransportationCost; }
        }
        public static string Entity
        {
            get { return Resource.Entity; }
        }
        public static string AlphabeticCode
        {
            get { return Resource.AlphabeticCode; }
        }
        public static string NumericCode
        {
            get { return Resource.NumericCode; }
        }
        public static string MinorUnit
        {
            get { return Resource.MinorUnit; }
        }
        public static string Percentage
        {
            get { return Resource.Percentage; }
        }
        public static string Event
        {
            get { return Resource.Event; }
        }
        public static string Title
        {
            get { return Resource.Title; }
        }
        public static string SupplyCondition
        {
            get { return Resource.SupplyCondition; }
        }
        public static string Active
        {
            get { return Resource.Active; }
        }
        public static string Composed
        {
            get { return Resource.Composed; }
        }
        public static string UpdateCostDate
        {
            get { return Resource.UpdateCostDate; }
        }
        public static string Value
        {
            get { return Resource.Value; }
        }
        public static string Change
        {
            get { return Resource.Change; }
        }
        public static string PackagingCost
        {
            get { return Resource.PackagingCost; }
        }
        public static string ImportVolume
        {
            get { return Resource.ImportVolume; }
        }
        public static string Quantity
        {
            get { return Resource.Quantity; }
        }
        public static string Width
        {
            get { return Resource.Width; }
        }
        public static string Height
        {
            get { return Resource.Height; }
        }
        public static string Length
        {
            get { return Resource.Length; }
        }
        public static string Reference
        {
            get { return Resource.Reference; }
        }
        public static string FirstName
        {
            get { return Resource.FirstName; }
        }
        public static string LastName
        {
            get { return Resource.LastName; }
        }
        public static string BusinessCard
        {
            get { return Resource.BusinessCard; }
        }
        public static string Roles
        {
            get { return Resource.Roles; }
        }
        public static string Update
        {
            get { return Resource.Update; }
        }
        public static string QuantityPackage
        {
            get { return Resource.QuantityPackage; }
        }
        public static string Price
        {
            get { return Resource.Price; }
        }
        public static string ValidationTime
        {
            get { return Resource.ValidationTime; }
        }
    }
}
