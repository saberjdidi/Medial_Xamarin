using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.Resources;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class MasterDetailViewModel : INotifyPropertyChanged
    {

        public INavigation Navigation { get; set; }
        private ObservableCollection<MasterMenu> _menuItems;
        private ObservableCollection<Carousel> _carousel;
        public MasterDetailViewModel()
        {
            PopulateMenu();
            GetCarousels();
        }

        public void PopulateMenu()
        {
            var Username = Settings.Username;
            User = JsonConvert.DeserializeObject<User>(Username);

            MenuItems = new ObservableCollection<MasterMenu>();

            if (User.roles.Select(r => r.name).FirstOrDefault().Equals("ROLE_ADMIN"))
            { 
                MenuItems.Add(new MasterMenu { MenuName = Languages.Supplier, MenuIcon = "supplier", TargetType = typeof(SuppliersPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Product, MenuIcon = "product", TargetType = typeof(ProductsPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Client, MenuIcon = "client", TargetType = typeof(ClientsPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.CommercialSupplier, MenuIcon = "supplier", TargetType = typeof(CommercialSupplierPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.CommercialProduct, MenuIcon = "product", TargetType = typeof(CommercialProductPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Offer, MenuIcon = "card_travel", TargetType = typeof(OfferPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Category, MenuIcon = "dashboard_color", TargetType = typeof(CategoryPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Container, MenuIcon = "supplier", TargetType = typeof(ContainerPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Province, MenuIcon = "setting", TargetType = typeof(ProvincePage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.CustomsDuty, MenuIcon = "public_langage", TargetType = typeof(CustomsDutyPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Currency, MenuIcon = "attach_money", TargetType = typeof(CurrencyPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Region, MenuIcon = "public_langage", TargetType = typeof(RegionPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.User, MenuIcon = "person", TargetType = typeof(UserPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Role, MenuIcon = "setting", TargetType = typeof(RolePage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.ClientGroupe, MenuIcon = "public_langage", TargetType = typeof(ClientGroupePage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.MeasureUnit, MenuIcon = "law", TargetType = typeof(MeasureUnitPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.PackagingMethod, MenuIcon = "dashboard_color", TargetType = typeof(PackagingMethodPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.TaskType, MenuIcon = "bookmark", TargetType = typeof(TaskTypePage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.TaskStatus, MenuIcon = "bookmark_border", TargetType = typeof(TaskStatusPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Country, MenuIcon = "public_langage", TargetType = typeof(CountryPage) });
            }
            else if (User.roles.Select(r => r.name).FirstOrDefault().Equals("ROLE_AGENT"))
            {
                MenuItems.Add(new MasterMenu { MenuName = Languages.Product, MenuIcon = "product", TargetType = typeof(ProductsAgentPage) });//ProductsPage 
                MenuItems.Add(new MasterMenu { MenuName = Languages.Client, MenuIcon = "client", TargetType = typeof(ClientsAgentPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Offer, MenuIcon = "card_travel", TargetType = typeof(OfferPage) });
               // MenuItems.Add(new MasterMenu { MenuName = "Commercial Supplier", MenuIcon = "client", TargetType = typeof(CommercialSupplierPage) });
            }
            else if (User.roles.Select(r => r.name).FirstOrDefault().Equals("ROLE_CLIENT"))
            { }
            else if (User.roles.Select(r => r.name).FirstOrDefault().Equals("ROLE_USER"))
            { }
            else if(User.username.Equals("admin"))
            {
                MenuItems.Add(new MasterMenu { MenuName = Languages.Supplier, MenuIcon = "supplier", TargetType = typeof(SuppliersPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Product, MenuIcon = "product", TargetType = typeof(ProductsPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Client, MenuIcon = "client", TargetType = typeof(ClientsPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.CommercialSupplier, MenuIcon = "client", TargetType = typeof(CommercialSupplierPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.CommercialProduct, MenuIcon = "client", TargetType = typeof(CommercialProductPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Offer, MenuIcon = "card_travel", TargetType = typeof(OfferPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Category, MenuIcon = "dashboard_color", TargetType = typeof(CategoryPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Container, MenuIcon = "supplier", TargetType = typeof(ContainerPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Province, MenuIcon = "setting", TargetType = typeof(ProvincePage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.CustomsDuty, MenuIcon = "public_langage", TargetType = typeof(CustomsDutyPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Currency, MenuIcon = "attach_money", TargetType = typeof(CurrencyPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Region, MenuIcon = "public_langage", TargetType = typeof(RegionPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.User, MenuIcon = "person", TargetType = typeof(UserPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Role, MenuIcon = "setting", TargetType = typeof(RolePage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.ClientGroupe, MenuIcon = "public_langage", TargetType = typeof(ClientGroupePage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.MeasureUnit, MenuIcon = "law", TargetType = typeof(MeasureUnitPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.PackagingMethod, MenuIcon = "dashboard_color", TargetType = typeof(PackagingMethodPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.TaskType, MenuIcon = "bookmark", TargetType = typeof(TaskTypePage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.TaskStatus, MenuIcon = "bookmark_border", TargetType = typeof(TaskStatusPage) });
                MenuItems.Add(new MasterMenu { MenuName = Languages.Country, MenuIcon = "public_langage", TargetType = typeof(CountryPage) });
            }
        }

        public void GetCarousels()
        {
            CarouselImage = new ObservableCollection<Carousel>();
            CarouselImage.Add(new Carousel { Heading = "Medial", Message = "Medial", Caption = "Medial", Image = "logo" });
            CarouselImage.Add(new Carousel { Heading = Languages.Products, Message = Languages.Products, Caption = "Medial", Image = "products" });
            CarouselImage.Add(new Carousel { Heading = Languages.Clients, Message = Languages.Clients, Caption = "Medial", Image = "clients" });
            CarouselImage.Add(new Carousel { Heading = Languages.Suppliers, Message = Languages.Suppliers, Caption = "Medial", Image = "suppliers" });

        }
        public ObservableCollection<MasterMenu> MenuItems
        {
            get
            {
                return _menuItems;
            }
            set
            {
                if (value != null)
                {
                    _menuItems = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Carousel> CarouselImage
        {
            get
            {
                return _carousel;
            }
            set
            {
                    _carousel = value;
                    OnPropertyChanged();
            }
        }

        MasterMenu _selectedMenu;
        public MasterMenu SelectedMenu
        {
            get
            {
                return _selectedMenu;
            }
            set
            {
                if (_selectedMenu != null)
                {
                    _selectedMenu.Selected = false;
                    _selectedMenu.MenuIcon = _selectedMenu.MenuIcon.Substring(0, _selectedMenu.MenuIcon.Length - 6);
                }


                _selectedMenu = value;

                if (_selectedMenu != null)
                {
                    _selectedMenu.Selected = true;
                    _selectedMenu.MenuIcon += "_color";
                    MessagingCenter.Send<MasterMenu>(_selectedMenu, "OpenMenu");
                }
            }
        }
        private User _user = null;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        /*public ICommand LogoutCommand
        {
            get
            {
                return new Command(async() =>
                {
                    
                    var confirm = Application.Current.MainPage.DisplayAlert("Exit", "Do you wan't to exit the App ?", "Yes", "No");
                    if (confirm.Equals("Yes"))
                    {
                        Settings.AccessToken = string.Empty;
                        Debug.WriteLine(Settings.Username);
                        Settings.Username = string.Empty;
                        Debug.WriteLine(Settings.Password);
                        Settings.Password = string.Empty;

                        await Navigation.PushModalAsync(new LoginPage());
                    }
                    else if(confirm.Equals("No"))
                    {
                        await Navigation.PushModalAsync(new MainPage());
                    }
                });
            }
        }*/

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
