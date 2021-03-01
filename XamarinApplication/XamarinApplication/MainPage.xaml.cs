using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Models;
using XamarinApplication.ViewModels;
using XamarinApplication.Views;

namespace XamarinApplication
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]


    public partial class MainPage : MasterDetailPage
    {
        //private const string URL = "http://phoneofficine.it/niini-gim/planning";
        //private HttpClient _client = new HttpClient();
        //private ObservableCollection<Planning> _plannings;
        private Timer timer;
        
        public MainPage()
        {
            InitializeComponent();
            Task.Run(AnimateBackground);
            this.Title = "master";
            BindingContext = new MasterDetailViewModel();
            MessagingCenter.Subscribe<MasterMenu>(this, "OpenMenu", (Menu) =>
            {
                this.Detail = new NavigationPage((Page)Activator.CreateInstance(Menu.TargetType));
                IsPresented = false;
            });

            //Name of XAML (label, Button, etc.)
            //Logout.Text = Languages.Logout;
        }
        private async void AnimateBackground()
        {
            Action<double> forward = input => bdGradient.AnchorY = input;
            Action<double> backward = input => bdGradient.AnchorY = input;

            while (true)
            {
                bdGradient.Animate(name: "forward", callback: forward, start: 0, end: 1, length: 5000, easing: Easing.SinIn);
                await Task.Delay(5000);
                bdGradient.Animate(name: "backward", callback: backward, start: 1, end: 0, length: 5000, easing: Easing.SinIn);
                await Task.Delay(5000);
            }
        }
        protected override void OnAppearing()
        {
            (this.BindingContext as MasterDetailViewModel).GetCarousels();
            timer = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds) { AutoReset = true, Enabled = true };
            timer.Elapsed += Timer_Elapsed;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            timer?.Dispose();
            base.OnDisappearing();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {

                if (cvCarousels.Position == 3)
                {
                    cvCarousels.Position = 0;
                    return;
                }

                cvCarousels.Position += 1;
            });
        }

        
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
          
            IsPresented = true;
        }

        public async void Sign_Out(object sender, EventArgs e)
        {
            
            var confirm = await DisplayAlert("Exit", Languages.Exit, Languages.Yes, Languages.No);
            if (confirm)
            {
                Settings.AccessToken = string.Empty;
                Debug.WriteLine(Settings.Username);
                Settings.Username = string.Empty;
                Debug.WriteLine(Settings.Password);
                Settings.Password = string.Empty;

                
            var client = new HttpClient();
            var response = await client.GetAsync("https://portalesp.smart-path.it/Portalesp/login/logOut");

            var token = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("-------------------token-----------------");
                Debug.WriteLine(token);
                if (token.Contains("logOut"))
                {
                    await Navigation.PushModalAsync(new LoginPage());
                }

            }
            else
            {
                await Navigation.PushModalAsync(new MainPage());
            }
        }
        private void Popup_Language(object o, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new LanguagePopupPage());
        }

    }

}
