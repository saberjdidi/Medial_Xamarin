using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Models;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarEventsPage : ContentPage
    {
        public CalendarEventsPage(Client client)
        {
            InitializeComponent();
            Device.SetFlags(new[] { "Expander_Experimental" });
            var eventViewModel = new CalendarEventsViewModel(client);
            eventViewModel.Client = client;
            BindingContext = eventViewModel;
        }

        /* private async void Download_pdf(object sender, EventArgs e)
         {
             var mi = ((MenuItem)sender);
             var client = mi.CommandParameter as Client;
             var httpClient = new HttpClient();
             var url = "https://app.smart-path.it/md-core/medial/client/"+ client.id + "/eventsReport?format=pdf";
             Debug.WriteLine("********url*************");
             Debug.WriteLine(url);
             var response = await httpClient.GetAsync(url);
             var result = await response.Content.ReadAsStringAsync();
             Debug.WriteLine("********result*************");
             Debug.WriteLine(result);
             if (!response.IsSuccessStatusCode)
             {
                 await Application.Current.MainPage.DisplayAlert("Error", response.StatusCode.ToString(), "ok");
                 return;
             }
             var pdf = JsonConvert.DeserializeObject<PdfClient>(result);

             byte[] bytes = Convert.FromBase64String(pdf.report);
             MemoryStream stream = new MemoryStream(bytes);

             await DependencyService.Get<ISave>().SaveAndView(pdf.name, pdf.defaultExtention, stream);
         }

          protected override void OnAppearing()
         {
             (this.BindingContext as CalendarEventsViewModel).GetEvents();

         }*/
    }
}