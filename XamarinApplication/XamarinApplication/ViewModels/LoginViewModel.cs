using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApplication.Helpers;
using XamarinApplication.Interfaces;
using XamarinApplication.Resources;
using XamarinApplication.Services;
using XamarinApplication.Views;

namespace XamarinApplication.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private ApiServices _apiServices = new ApiServices();
        public INavigation Navigation { get; set; }
        public LoginViewModel(INavigation _navigation)
        {
            Navigation = _navigation;

        }
        private bool isVisible;
        private bool value = false;
        private bool isEnabled = false;
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                this.isVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                this.isEnabled = value;
                OnPropertyChanged();
            }
        }
        public bool Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //IsVisible = true;
                    Value = true;
                    //IsEnabled = true;
                    var client = new HttpClient();
                    var connection = await _apiServices.CheckConnection();

                    if (!connection.IsSuccess)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            Languages.Warning,
                            Languages.CheckConnection,
                            Languages.Ok);
                        return;
                    }
                     if (string.IsNullOrEmpty(Username))
                     {
                        //IsVisible = true;
                        Value = true;
                        //IsEnabled = true;
                        //await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.UsernameValidation, Languages.Ok);
                        return;
                     }
                     if (string.IsNullOrEmpty(Password))
                     {
                        //IsVisible = true;
                        Value = true;
                        //IsEnabled = true;
                        //await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PasswordValidation, Languages.Ok);
                        return;
                     }

                    //var response = await client.GetAsync("https://portalesp.smart-path.it/Portalesp/login/login?userName=" + Username + "&password=" + Password);
                    var response = await client.GetAsync("https://app.smart-path.it/md-core/medial/global/getLang?username=" + Username + "&password=" + Password);

                    var token = await response.Content.ReadAsStringAsync();
                    Settings.Username = token;
                    Debug.WriteLine("**************response****************");
                    Debug.WriteLine(response);
                    Debug.WriteLine("-------------------token-----------------");
                    Debug.WriteLine(token);
                    
                    // Settings.AccessToken = token;
                    //if (token.Contains("firstName"))
                    if (token.Contains("username"))
                    {
                        //IsVisible = false;
                        Value = false;
                       /* var cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                        Debug.WriteLine("********cookie*************");
                        Debug.WriteLine(cookie);
                        Settings.Cookie = cookie;*/

                        PopuPage page1 = new PopuPage();
                        await PopupNavigation.Instance.PushAsync(page1);
                        await Task.Delay(2000);
                        MessagingCenter.Send<LoginViewModel>(this, "Hi");

                        await Navigation.PushModalAsync(new MainPage());
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.FailedLogin, Languages.Ok);

                    }
                    
                });
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public LoginViewModel()
        {
            Username = Settings.Username;
            Password = Settings.Password;
        }
        
    }

    
}
